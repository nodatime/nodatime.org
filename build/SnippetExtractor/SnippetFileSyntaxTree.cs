// Copyright 2017 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnippetExtractor
{
    /// <summary>
    /// Wrapper around a Roslyn syntax tree representing a file within a snippet project.
    /// </summary>
    public sealed class SnippetFileSyntaxTree
    {
        private readonly SyntaxTree tree;
        private readonly SemanticModel model;
        private readonly INamedTypeSymbol snippetType;

        public SnippetFileSyntaxTree(SyntaxTree tree, SemanticModel model)
        {
            this.tree = tree;
            this.model = model;
            snippetType = model.Compilation.GetTypeByMetadataName("NodaTime.Demo.Snippet");
        }

        public IEnumerable<SourceSnippet> GetSnippets() =>
            tree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>().SelectMany(GetSnippets);

        private IEnumerable<SourceSnippet> GetSnippets(MethodDeclarationSyntax method)
        {
            // Note: this won't get using directives in namespace declarations, but hey...
            var usings = method.SyntaxTree.GetCompilationUnitRoot().Usings.Select(uds => uds.ToString());
            var invocations = method.DescendantNodes().OfType<InvocationExpressionSyntax>();
            foreach (var invocation in invocations)
            {
                var expression = GetSnippetInvocationExpression(invocation);
                if (expression is null)
                {
                    continue;
                }
                var targetSymbol = model.GetSymbolInfo(expression).Symbol;
                if (targetSymbol == null)
                {
                    throw new Exception($"Couldn't get a symbol for Snippet.For argument: {invocation}");
                }
                // docfx UIDs don't have the M: (etc) prefix.
                var uid = targetSymbol.GetDocumentationCommentId().Substring(2);
                var block = invocation.Ancestors().OfType<BlockSyntax>().First();
                yield return new SourceSnippet(uid, block.GetLines(), usings);
            }
        }

        private ExpressionSyntax GetSnippetInvocationExpression(InvocationExpressionSyntax syntax)
        {
            var symbol = model.GetSymbolInfo(syntax).Symbol;
            if (!snippetType.Equals(symbol?.ContainingType) || syntax.ArgumentList.Arguments.Count == 0)
            {
                return null;
            }
            var firstArgument = syntax.ArgumentList.Arguments[0].Expression;
            return symbol.Name switch
            {
                "For" => firstArgument,
                "ForAction" => GetLambda(),
                "SilentForAction" => GetLambda(),
                _ => null
            };
            
            ExpressionSyntax GetLambda()
            {
                if (firstArgument is LambdaExpressionSyntax lambda)
                {
                    if (lambda.Body is ExpressionSyntax expression)
                    {
                        return expression;
                    }
                    throw new Exception($"Expected expression-bodied lambda");
                }
                throw new Exception($"Expected lambda expression syntax; was {firstArgument.GetType()}");
            }
        }

        private bool IsSnippetMethod(InvocationExpressionSyntax syntax, string name)
        {
            var symbol = model.GetSymbolInfo(syntax).Symbol;
            return snippetType.Equals(symbol?.ContainingType) && symbol.Name == name;
        }

        public static async Task<SnippetFileSyntaxTree> CreateAsync(Document document)
        {
            var tree = await document.GetSyntaxTreeAsync().ConfigureAwait(false);
            var model = await document.GetSemanticModelAsync().ConfigureAwait(false);
            return new SnippetFileSyntaxTree(tree, model);
        }
    }
}
