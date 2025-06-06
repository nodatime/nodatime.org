﻿// Copyright 2017 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SnippetExtractor
{
    public sealed class SnippetRewriter
    {
        // Additional imports, for Console.WriteLine and to make
        // up for the fact that the script isn't in a namespace
        // (and new versions of NUnit).
        private static readonly string[] ExtraUsings =
        {
             "using System;",
             "using NodaTime;",
             "using NodaTime.Demo;",
             "using Assert = NUnit.Framework.Legacy.ClassicAssert;"
        };

        private readonly ScriptOptions buildOptions;
        private readonly ScriptOptions executeOptions;

        public SnippetRewriter(Project project)
        {
            var nodaTimeFile = Path.Combine(Path.GetDirectoryName(project.OutputFilePath), "NodaTime.dll");
            var nodaTimeReference = MetadataReference.CreateFromFile(nodaTimeFile);
            var demoReference = MetadataReference.CreateFromFile(project.OutputFilePath);

            buildOptions = ScriptOptions.Default
                .AddReferences(project.MetadataReferences)
                .AddReferences(new[] { nodaTimeReference, demoReference });

            // Remove any unresolved metadata references. I don't know why we have them, but it appears we don't need them...
            buildOptions = buildOptions.WithReferences(buildOptions.MetadataReferences.Where(mr => mr is not UnresolvedMetadataReference));

            executeOptions = ScriptOptions.Default.AddReferences(nodaTimeReference);
        }

        public async Task<RewrittenSnippet> RewriteSnippetAsync(SourceSnippet snippet)
        {
            var usings = snippet.Usings.Concat(ExtraUsings).Distinct().OrderBy(x => x.TrimEnd(';'));
            var text = string.Join("\r\n", usings.Concat(new[] { "" }).Concat(Trim(snippet.Lines)));
            var tree = CSharpSyntaxTree.ParseText(text, CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            Compilation compilation = CSharpCompilation.Create("Foo", new[] { tree }, buildOptions.MetadataReferences)
                .CheckSuccessful();
            // TODO: Replace var with explicit declarations?
            compilation = RewriteInvocations(compilation).CheckSuccessful();
            compilation = RemoveUnusedImports(compilation).CheckSuccessful();

            // Now we should only need the NodaTime reference.
            var script = CSharpScript.Create(compilation.SyntaxTrees.Single().ToString(), executeOptions);
            var output = await RunScriptAsync(script);
            return new RewrittenSnippet(script.Code, output, snippet.Uid);
        }

        private static Compilation RewriteInvocations(Compilation compilation)
        {
            ExpressionSyntax consoleWriteLineExpression =
                SyntaxFactory.MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    SyntaxFactory.IdentifierName("Console"),
                    SyntaxFactory.IdentifierName("WriteLine"));
            var tree = compilation.SyntaxTrees.Single();
            var model = compilation.GetSemanticModel(tree);

            var assertType = model.Compilation.GetTypeByMetadataName("NUnit.Framework.Legacy.ClassicAssert");
            var snippetType = model.Compilation.GetTypeByMetadataName("NodaTime.Demo.Snippet");
            var root = tree.GetRoot();

            var newRoot = root.ReplaceNodes(root.DescendantNodes(), ReplaceNode);
            newRoot = Formatter.Format(newRoot, new AdhocWorkspace());

            // Force it back to have a kind of Script... not sure why this is required.
            var newTree = newRoot.SyntaxTree.WithRootAndOptions(newRoot, tree.Options);
            return compilation.ReplaceSyntaxTree(tree, newTree);

            SyntaxNode ReplaceNode(SyntaxNode oldNode, SyntaxNode newNode)
            {
                return oldNode switch
                {
                    InvocationExpressionSyntax invocation => ReplaceInvocation(invocation, (InvocationExpressionSyntax) newNode),
                    GlobalStatementSyntax
                    { 
                        Statement: ExpressionStatementSyntax { Expression: InvocationExpressionSyntax invocation }
                    } => ShouldRemoveStatementInvocation(invocation) ? null : newNode,
                    _ => newNode
                };

                SyntaxNode ReplaceInvocation(InvocationExpressionSyntax oldNode, InvocationExpressionSyntax newNode)
                {
                    var symbol = model.GetSymbolInfo(oldNode).Symbol;
                    return symbol switch
                    {
                        IMethodSymbol method when SymbolEqualityComparer.Default.Equals(method.ContainingType, assertType) => ReplaceAssert(method, newNode),
                        IMethodSymbol method when SymbolEqualityComparer.Default.Equals(method.ContainingType, snippetType) => ReplaceSnippetHelper(method, newNode),
                        _ => newNode
                    };

                    SyntaxNode ReplaceAssert(IMethodSymbol method, InvocationExpressionSyntax newNode) =>
                        method switch
                        {
                            // Assert.AreEqual(x, y) => Console.WriteLine(y)
                            { Name: "AreEqual" } =>
                                newNode.WithExpression(consoleWriteLineExpression)
                                    .WithArgumentList(SyntaxFactory.ArgumentList().AddArguments(newNode.ArgumentList.Arguments[1]))
                                    .WithTriviaFrom(newNode),
                            // Assert.True(x) => Console.WriteLine(x)
                            { Name: string name } when name == "True" || name == "False" || name == "IsTrue" || name == "IsFalse" =>
                                newNode.WithExpression(consoleWriteLineExpression)
                                    .WithArgumentList(SyntaxFactory.ArgumentList().AddArguments(newNode.ArgumentList.Arguments[0]))
                                    .WithTriviaFrom(newNode),
                            // Assert.Less(x, 0) and Assert.Greater(x, 0) => Console.WriteLine(x)
                            { Name: string name } when (name == "Less" || name == "Greater") && newNode.ArgumentList.Arguments[1].ToString() == "0" =>
                                newNode.WithExpression(consoleWriteLineExpression)
                                    .WithArgumentList(SyntaxFactory.ArgumentList().AddArguments(newNode.ArgumentList.Arguments[0]))
                                    .WithTriviaFrom(newNode),
                            _ => throw new ArgumentException($"Unhandled Assert method: {method.Name}")
                        };

                    SyntaxNode ReplaceSnippetHelper(IMethodSymbol method, InvocationExpressionSyntax newNode) =>
                        method switch
                        {
                            { Name: "For" } => newNode.ArgumentList.Arguments[0].Expression,
                            { Name: "ForAction" } => ((LambdaExpressionSyntax) newNode.ArgumentList.Arguments[0].Expression).Body,
                            // This will be removed at the statement level
                            { Name: "SilentForAction" } => newNode,
                            _ => throw new ArgumentException($"Unhandled Snippet method: {method.Name}")
                        };
                }

                bool ShouldRemoveStatementInvocation(InvocationExpressionSyntax invocation) =>
                    model.GetSymbolInfo(invocation).Symbol is IMethodSymbol method &&
                        SymbolEqualityComparer.Default.Equals(method.ContainingType, snippetType) && method.Name == "SilentForAction";
            }
        }

        private static Compilation RemoveUnusedImports(Compilation compilation)
        {
            // TODO: See if there's a better way of doing this.
            // There's Document.GetLanguageService<IOrganizeImportsService>, but that
            // requires a Document.
            var tree = compilation.SyntaxTrees.Single();
            var root = tree.GetRoot();
            var unusedImportNodes = compilation.GetDiagnostics()
                .Where(d => d.Id == "CS8019")
                .Where(d => d.Location?.SourceTree == tree)
                .Select(d => root.FindNode(d.Location.SourceSpan))
                .ToList();
            var newRoot = root.RemoveNodes(unusedImportNodes, SyntaxRemoveOptions.KeepNoTrivia);
            // Force it back to have a kind of Script... not sure why this is required. 
            var newTree = newRoot.SyntaxTree.WithRootAndOptions(newRoot, tree.Options);
            return compilation.ReplaceSyntaxTree(tree, newTree);
        }

        /// <summary>
        /// Trims common leading whitespace from the given lines. Whitespace-only lines
        /// are not considered when determining how much to trim.
        /// </summary>
        private static List<string> Trim(IEnumerable<string> lines)
        {
            // TODO: Use Microsoft.CodeAnalysis.Formatting instead? That requires a workspace...
            // which we do have, admittedly, but the script we want to format isn't in that workspace.
            // Also, it means reformatting the code, whereas we may want it exactly as written in the snippet.

            // Trim leading and trailing empty lines
            var list = lines
                .SkipWhile(string.IsNullOrWhiteSpace)
                .Reverse()
                .SkipWhile(string.IsNullOrWhiteSpace)
                .Reverse()
                .ToList();

            var leadingWhitespace = list
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Min(line => line.Length - line.TrimStart().Length);
            for (int i = 0; i < list.Count; i++)
            {
                list[i] = string.IsNullOrWhiteSpace(list[i]) ? "" : list[i].Substring(leadingWhitespace);
            }
            return list;
        }

        /// <summary>
        /// Runs the given script, capturing its console output.
        /// </summary>
        private static async Task<string> RunScriptAsync(Script script)
        {
            var outputWriter = new StringWriter();
            var originalOutput = Console.Out;
            Console.SetOut(outputWriter);
            try
            {
                await script.RunAsync();
            }
            finally
            {
                Console.SetOut(originalOutput);
            }
            return outputWriter.ToString();
        }
    }
}
