using Dusk.CodeAnalysis.Syntax;
using System.Collections.Generic;

namespace Dusk.CodeAnalysis.ExpressionSyntax
{
    public sealed class LiteralExpressionSyntax : ExpressionSyntax
    {
        public LiteralExpressionSyntax(SyntaxToken literalToken)
        {
            LiteralToken = literalToken;
        }

        public override SyntaxKind SyntaxKind => SyntaxKind.LiteralExpression;

        public SyntaxToken LiteralToken { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return LiteralToken;
        }
    }
}
