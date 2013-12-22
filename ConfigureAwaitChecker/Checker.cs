using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roslyn.Compilers.CSharp;

namespace ConfigureAwaitChecker
{
    public sealed class Checker
    {
        static readonly ParseOptions ParseOptions = new ParseOptions(
                languageVersion: LanguageVersion.CSharp5,
                parseDocumentationComments: false);

        SyntaxTree _tree;

        public Checker(string file)
        {
            _tree = SyntaxTree.ParseFile(file, ParseOptions);
        }

        public string DebugListTree()
        {
            return DebugListNodes(_tree.GetRoot().ChildNodes());
        }

        public IEnumerable<CheckerResult> Check()
        {
            foreach (var item in _tree.GetRoot().DescendantNodes())
            {
                if (item.Kind == SyntaxKind.IdentifierName && item.ToString().Equals("await", StringComparison.Ordinal))
                {
                    var node = FindInterestingNode(item);
                    var check = CheckConfigureAwait(node);
                    var positionStart = node.GetLocation().GetLineSpan(true).StartLinePosition;
                    var positionEnd = node.GetLocation().GetLineSpan(true).EndLinePosition;
                    var line = positionEnd.Line + 1;
                    var column = positionEnd.Character + 1;
                    yield return new CheckerResult(check, line, column);
                }
            }
        }

        static SyntaxNode FindInterestingNode(SyntaxNode node)
        {
            return FindArgumentInteresting(node)
                ?? FindLocalInteresting(node)
                ?? FindExpressionInteresting(node);
        }

        static SyntaxNode FindLocalStart(SyntaxNode node)
        {
            if (node is LocalDeclarationStatementSyntax)
                return node;
            if (node.Parent != null)
                return FindLocalStart(node.Parent);
            return null;
        }
        static SyntaxNode FindLocalInteresting(SyntaxNode node)
        {
            var startNode = FindLocalStart(node);
            if (startNode != null)
            {
                return startNode.Parent.ChildNodes().SkipWhile(n => n != startNode).Skip(1).First();
            }
            return null;
        }

        static SyntaxNode FindExpressionInterestingStart(SyntaxNode node)
        {
            if (node is StatementSyntax)
                return node;
            if (node.Parent != null)
                return FindExpressionInterestingStart(node.Parent);
            return null;
        }
        static SyntaxNode FindExpressionInteresting(SyntaxNode node)
        {
            var startNode = FindExpressionInterestingStart(node);
            return startNode;
        }

        static SyntaxNode FindArgumentInterestingStart(SyntaxNode node)
        {
            if (node is ArgumentSyntax)
                return node;
            if (node.Parent != null)
                return FindArgumentInterestingStart(node.Parent);
            return null;
        }
        static SyntaxNode FindArgumentInteresting(SyntaxNode node)
        {
            var startNode = FindArgumentInterestingStart(node);
            if (startNode != null)
            {
                return startNode.Parent.ChildNodes().SkipWhile(n => n != startNode).Skip(1).First();
            }
            return null;
        }

        static bool CheckConfigureAwait(SyntaxNode node)
        {
            var enumerator = node.DescendantNodes().GetEnumerator();
            while (enumerator.MoveNext())
            {
                var item = enumerator.Current;
                if (item.Kind == SyntaxKind.IdentifierName && item.ToString().Equals("ConfigureAwait", StringComparison.Ordinal))
                {
                    if (enumerator.MoveNext())
                    {
                        var item2 = enumerator.Current;
                        if (item2.Kind == SyntaxKind.ArgumentList)
                        {
                            if (enumerator.MoveNext())
                            {
                                var item3 = enumerator.Current;
                                if (item3.Kind == SyntaxKind.Argument)
                                {
                                    if (enumerator.MoveNext())
                                    {
                                        var item4 = enumerator.Current;
                                        if (item4.Kind == SyntaxKind.FalseLiteralExpression)
                                        {
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        static string DebugListNodes(IEnumerable<SyntaxNode> nodes, string indent = "")
        {
            var result = new StringBuilder();
            foreach (var node in nodes)
            {
                result.AppendFormat("{0}{1}:{2}", indent, node.Kind, (node.Kind == SyntaxKind.IdentifierName || node.Kind == SyntaxKind.ExpressionStatement ? node.ToString() : string.Empty));
                result.AppendLine();
                result.Append(DebugListNodes(node.ChildNodes(), indent + "  "));
            }
            return result.ToString();
        }
    }
}
