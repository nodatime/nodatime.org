using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DocfxYamlLoader
{
    public class DocfxMember
    {
        // Operator names used to be just "Multiply(Duration, Int64)" etc, instead of "operator *(Duration, Int64").
        // These are translated into the symbolic forms in DisplayName.
        private static readonly Dictionary<string, string> OperatorNames = new Dictionary<string, string>
        {
            { "GreaterThan", ">" },
            { "LessThan", "<" },
            { "GreaterThanOrEqual", ">=" },
            { "LessThanOrEqual", "<=" },
            { "Inequality", "!=" },
            { "Equality", "==" },
            { "Addition", "+" },
            { "Subtraction", "-" },
            { "UnaryNegation", "-" },
            { "Multiply", "*" },
            { "Division", "/" },
            // TODO: Conversions, unary addition, true/false. Anything else?
        };

        public string YamlFile { get; set; }
        public string Uid { get; set; }
        public string Parent { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public DocfxMember ParentMember { get; set; }
        public TypeKind Type { get; set; }
        public List<DocfxAttribute> Attributes { get; set; }

        public bool Obsolete => Attributes?.Any(attr => attr.Type == "System.ObsoleteAttribute") ?? false;

        // TODO: This is far from elegant...
        public string DisplayName
        {
            get
            {
                if (!IsTypeMember)
                {
                    return FullName ?? Uid;
                }
                if (Type == TypeKind.Operator)
                {
                    // Translation for old-style operator names. If the name
                    // isn't in the dictionary, we just use name as it is (which is fine
                    // for modern builds).
                    var op = Name.Split('(').First();
                    if (OperatorNames.TryGetValue(op, out string symbolicOp))
                    {
                        return $"operator {symbolicOp}{Name.Substring(op.Length)}";
                    }
                }
                return Name;
            }
        }

        public bool IsTypeMember =>
            Type == TypeKind.Property ||
            Type == TypeKind.Constructor ||
            Type == TypeKind.Method ||
            Type == TypeKind.Operator ||
            Type == TypeKind.Field;

        public enum TypeKind
        {
            Class,
            Enum,
            Interface,
            Property,
            Constructor,
            Method,
            Operator,
            Delegate,
            Field,
            Namespace,
            Struct
        }

        public class DocfxAttribute
        {
            public string Type { get; set; }
            // Add extra properties if necessary...
        }
    }
}
