using Dusk.CodeAnalysis.Syntax;
using System.Collections.Generic;

namespace Dusk.CodeAnalysis.ExpressionSyntax
{
    public sealed class ParenthesizedExpressionSytax : ExpressionSyntax
    {
        public ParenthesizedExpressionSytax(SyntaxToken openParenthesisToken, ExpressionSyntax expression, SyntaxToken closedParenthesisToken)
        {
            ClosedParenthesisToken = closedParenthesisToken;
            OpenParenthesisToken = openParenthesisToken;
            Expression = expression;
        }

        public override SyntaxKind SyntaxKind => SyntaxKind.ParenthesizedExpression;
        public SyntaxToken ClosedParenthesisToken { get; }
        public SyntaxToken OpenParenthesisToken { get; }
        public ExpressionSyntax Expression { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return OpenParenthesisToken;
            yield return Expression;
            yield return ClosedParenthesisToken;
        }
    }
}
