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
                    var check = node != null && CheckConfigureAwait(node);
                    var positionStart = item.GetLocation().GetLineSpan(true).StartLinePosition;
                    var positionEnd = item.GetLocation().GetLineSpan(true).EndLinePosition;
                    var line = positionEnd.Line + 1;
                    var column = positionEnd.Character + 1;
                    yield return new CheckerResult(check, line, column);
                }
            }
        }

        static SyntaxNode FindInterestingNode(SyntaxNode node)
        {
            return FindNodeRec(node.Parent, node);
        }
        static SyntaxNode FindNodeRec(SyntaxNode node, SyntaxNode start)
        {
            if (node is StatementSyntax)
            {
                if (node is ExpressionStatementSyntax)
                    return node;
                var nodes = node.Parent.DescendantNodes().ToArray();
                var index = Array.FindIndex(nodes, n => n == start);
                var result = nodes.Skip(index + 1).First(n => n is ExpressionStatementSyntax);
                return result;
            }
            else if (node.Parent != null)
            {
                return FindNodeRec(node.Parent, start);
            }
            else
            {
                return null;
            }
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
                result.AppendFormat("{0}{1}:[{3}]|{2}",
                    indent,
                    node.Kind,
                    node.ToString(),
                    node.Span);
                result.AppendLine();
                result.Append(DebugListNodes(node.ChildNodes(), indent + "  "));
            }
            return result.ToString();
        }
    }
}
