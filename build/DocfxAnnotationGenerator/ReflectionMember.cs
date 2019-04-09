﻿using Mono.Cecil;
using Mono.Collections.Generic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ICustomAttributeProvider = Mono.Cecil.ICustomAttributeProvider;

namespace DocfxAnnotationGenerator
{
    /// <summary>
    /// A member loaded via reflection.
    /// </summary>
    public class ReflectionMember
    {
        /// <summary>
        /// The UID Docfx would use for this member. See
        /// https://dotnet.github.io/docfx/spec/metadata_dotnet_spec.html
        /// </summary>
        public string DocfxUid { get; }

        // Never null, but only actually relevant for methods, properties and indexers.
        public IEnumerable<string> NotNullParameters { get; } = Enumerable.Empty<string>();

        // Methods, indexers, properties.
        public bool NotNullReturn { get; }

        // Only relevant for function members.
        public bool IsOverride { get; }

        public static IEnumerable<ReflectionMember> Load(Stream stream) =>
            ModuleDefinition.ReadModule(stream).Types.Where(t => t.IsPublic).SelectMany(GetMembers);

        private static IEnumerable<ReflectionMember> GetMembers(TypeDefinition type)
        {
            yield return new ReflectionMember(type);

            if (type.BaseType?.FullName == "System.MulticastDelegate")
            {
                yield break;
            }

            foreach (var property in type.Properties.Where(p => IsAccessible(p.GetMethod) || IsAccessible(p.SetMethod)))
            {
                yield return new ReflectionMember(property);
            }

            // TODO: Do we want protected methods?
            foreach (var method in type.Methods.Where(m => !m.IsGetter && !m.IsSetter && IsAccessible(m)))
            {
                yield return new ReflectionMember(method);
            }

            foreach (var field in type.Fields.Where(m => m.IsPublic && !m.IsSpecialName))
            {
                yield return new ReflectionMember(field);
            }

            foreach (var nestedMember in type.NestedTypes.Where(t => t.IsNestedPublic).SelectMany(GetMembers))
            {
                yield return nestedMember;
            }
        }

        private static bool IsAccessible(MethodDefinition method)
        {
            // This is pretty crude at the moment...
            // Handy for properties
            if (method == null)
            {
                return false;
            }
            // Overrides is used to check for explicit interface implementation
            return method.IsPublic || method.IsFamily || method.Overrides.Count != 0;
        }

        // TODO: More specifically, is it an explicit interface implementation for a public interface?
        // (Doesn't seem to affect us right now...)

        private ReflectionMember(TypeDefinition type)
        {
            DocfxUid = GetUid(type);
        }

        private ReflectionMember(PropertyDefinition property)
        {
            // property.Name will be Type.Name for explicit interface implementations, e.g. NodaTime.IClock.Now
            DocfxUid = $"{GetUid(property.DeclaringType)}.{property.Name.Replace('.', '#')}{GetParameterNames(property.Parameters)}";
            NotNullReturn = IsNonNullable(property.PropertyType, property);

            // For indexers...
            NotNullParameters = property.Parameters
                .Where(p => IsNonNullable(p.ParameterType, p) && !p.IsOut)
                .Select(p => p.Name)
                .ToList();
        }

        private ReflectionMember(MethodDefinition method)
        {
            DocfxUid = GetUid(method);
            IsOverride = method.Overrides.Count > 0;
            NotNullReturn = !method.IsConstructor && IsNonNullable(method.ReturnType, method);
            NotNullParameters = method.Parameters
                .Where(p => IsNonNullable(p.ParameterType, p) && !p.IsOut)
                .Select(p => p.Name)
                .ToList();
        }

        private ReflectionMember(FieldDefinition field)
        {
            DocfxUid = $"{GetUid(field.DeclaringType)}.{field.Name}";
        }

        private string GetUid(MethodDefinition method)
        {
            if (method.Overrides.Count == 1)
            {
                var interfaceMethod = method.Overrides[0];
                var interfaceMethodNameUid = $"{GetUid(interfaceMethod.DeclaringType)}.{interfaceMethod.Name}".Replace('.', '#');
                return $"{GetUid(method.DeclaringType)}.{interfaceMethodNameUid}{GetParameterNames(method.Parameters)}";
            }
            string name = method.Name.Replace('.', '#');
            string arity = method.HasGenericParameters ? $"``{method.GenericParameters.Count}" : "";
            return $"{GetUid(method.DeclaringType)}.{name}{arity}{GetParameterNames(method.Parameters)}";
        }

        private string GetUid(TypeDefinition type)
        {
            return GetUid(type, false);
        }

        private string GetUid(TypeReference type)
        {
            return GetUid(type, true);
        }

        private string GetUid(TypeReference type, bool useTypeArgumentNames)
        {
            string name = type.FullName.Replace('/', '.');
            // TODO: Check whether this handles nested types involving generics
            switch (type)
            {
                case ByReferenceType brt: return $"{GetUid(brt.ElementType, useTypeArgumentNames)}@";
                case GenericParameter gp when useTypeArgumentNames: return gp.Name;
                case GenericParameter gp when gp.DeclaringType != null: return $"`{gp.Position}";
                case GenericParameter gp when gp.DeclaringMethod != null: return $"``{gp.Position}";
                case GenericParameter _: throw new InvalidOperationException("Unhandled generic parameter");
                case GenericInstanceType git: return $"{RemoveArity(name)}{GetGenericArgumentNames(git.GenericArguments, useTypeArgumentNames)}";
                default: return name;
            }
        }

        private string GetGenericArgumentNames(Collection<TypeReference> arguments, bool useTypeArgumentNames) =>
            arguments.Count == 0 ? "" : $"{{{string.Join(",", arguments.Select(arg => GetUid(arg, useTypeArgumentNames)))}}}";

        private string GetParameterNames(Collection<ParameterDefinition> parameters) =>
            parameters.Count == 0 ? "" : $"({string.Join(",", parameters.Select(p => GetUid(p.ParameterType, false)))})";

        private string RemoveArity(string name)
        {
            // TODO: Nested types, e.g. Foo`1.Bar or Foo`1.Bar`1
            int index = name.IndexOf('`');
            return index == -1 ? name : name.Substring(0, index);
        }

        private static bool IsNonNullable(TypeReference type, ICustomAttributeProvider provider)
        {
            if (type.IsValueType || type.FullName == "System.Void")
            {
                return false;
            }
            var nullable = provider.CustomAttributes.FirstOrDefault(ca => ca.AttributeType.FullName == "System.Runtime.CompilerServices.NullableAttribute");
            if (nullable == null)
            {
                return HasAttribute(provider, "JetBrains.Annotations.NotNullAttribute");
            }
            // We treat SpecialNullHandling as if it's nullable, because we don't want to add
            // the ArgumentNullException handling to those parameters.
            if (HasAttribute(provider, "NodaTime.Annotations.SpecialNullHandlingAttribute"))
            {
                return false;
            }
            var value = nullable.ConstructorArguments[0].Value;
            // We only care about the outermost nullability - List<string> and List<string?> are both non-nullable.
            return value switch
            {
                byte b => b == 1,
                IEnumerable<CustomAttributeArgument> arguments => (byte) (arguments.First().Value) == 1,
                _ => throw new InvalidOperationException($"Unexpected argument type: {value.GetType()}")
            };
        }

        private static bool HasAttribute(ICustomAttributeProvider provider, string name) =>
            provider != null &&
            provider.HasCustomAttributes &&
            provider.CustomAttributes.Any(attr => attr.AttributeType.FullName == name);
    }
}
