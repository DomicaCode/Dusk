using Dusk.CodeAnalysis.ExpressionSyntax;
using System.Collections.Generic;
using System.Linq;

namespace Dusk.CodeAnalysis.Syntax
{
    public sealed class SyntaxTree
    {
        public SyntaxTree(IEnumerable<string> diagnostics, ExpressionSyntax.ExpressionSyntax root, SyntaxToken endOfFileToken)
        {
            EndOfFileToken = endOfFileToken;
            Root = root;
            Diagnostics = diagnostics.ToArray();
        }

        public SyntaxToken EndOfFileToken { get; }
        public ExpressionSyntax.ExpressionSyntax Root { get; }
        public IReadOnlyList<string> Diagnostics { get; }

        public static SyntaxTree Parse(string text)
        {
            var parser = new Parser(text);

            return parser.Parse();
        }
    }
}
