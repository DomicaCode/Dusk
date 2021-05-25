using Dusk.CodeAnalysis.ExpressionSyntax;
using Dusk.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dusk.CodeAnalysis
{
    public sealed class Evaluator
    {
        private readonly ExpressionSyntax.ExpressionSyntax _root;

        public Evaluator(ExpressionSyntax.ExpressionSyntax root)
        {
            _root = root;
        }

        public int Evaluate()
        {
            return EvaluateExpression(_root);
        }

        private int EvaluateExpression(ExpressionSyntax.ExpressionSyntax node)
        {
            if (node is LiteralExpressionSyntax numberExpression)
                return (int)numberExpression.LiteralToken.Value;

            if (node is BinaryExpressionSyntax binaryExpression)
            {
                var left = EvaluateExpression(binaryExpression.Left);
                var right = EvaluateExpression(binaryExpression.Right);

                if (binaryExpression.OperatorToken.SyntaxKind == SyntaxKind.PlusToken)
                    return left + right;
                else if (binaryExpression.OperatorToken.SyntaxKind == SyntaxKind.MinusToken)
                    return left - right;
                else if (binaryExpression.OperatorToken.SyntaxKind == SyntaxKind.StarToken)
                    return left * right;
                else if (binaryExpression.OperatorToken.SyntaxKind == SyntaxKind.SlashToken)
                    return left / right;
                else
                    throw new Exception($"Unexpected binary operator {binaryExpression.OperatorToken.SyntaxKind}");
            }

            if (node is ParenthesizedExpressionSytax parenthesizedExpression)
            {
                return EvaluateExpression(parenthesizedExpression.Expression);
            }
            else
                throw new Exception($"Unexpected node {node.SyntaxKind}");

        }
    }
}
