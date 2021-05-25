using Dusk.CodeAnalysis.ExpressionSyntax;
using Dusk.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dusk.CodeAnalysis
{

    internal sealed class Parser
    {
        private SyntaxToken[] _tokens;
        private int _position;
        private List<string> _diagnostics = new List<string>();

        public Parser(string text)
        {
            var tokens = new List<SyntaxToken>();

            var lexer = new Lexer(text);

            SyntaxToken token;
            do
            {
                token = lexer.Lex();

                if (token.SyntaxKind != SyntaxKind.WhiteSpaceToken && token.SyntaxKind != SyntaxKind.BadToken)
                {
                    tokens.Add(token);
                }
            } while (token.SyntaxKind != SyntaxKind.EndOfFileToken);

            _tokens = tokens.ToArray();
            _diagnostics.AddRange(lexer.Diagnostics);
        }

        public IEnumerable<string> Diagnostics => _diagnostics;

        private SyntaxToken Peek(int offSet)
        {
            var index = _position + offSet;
            if (index >= _tokens.Length)
                return _tokens[_tokens.Length - 1];

            return _tokens[index];
        }

        private SyntaxToken Current => Peek(0);

        private SyntaxToken NextToken()
        {
            var current = Current;
            _position++;
            return current;
        }

        private SyntaxToken MatchToken(SyntaxKind syntaxKind)
        {
            if (Current.SyntaxKind == syntaxKind)
                return NextToken();

            _diagnostics.Add($"Error: Unexpected token <{Current.SyntaxKind}>, expected <{syntaxKind}>");
            return new SyntaxToken(syntaxKind, Current.Position, null, null);
        }

        public SyntaxTree Parse()
        {

            var expression = ParseExpression();
            var endOfFile = MatchToken(SyntaxKind.EndOfFileToken);

            return new SyntaxTree(_diagnostics, expression, endOfFile);
        }


        private ExpressionSyntax.ExpressionSyntax ParseExpression(int parentPrecedence = 0)
        {
            var left = ParsePrimaryExpression();

            while (true)
            {
                var precedence = GetBinaryOperatorPrecedence(Current.SyntaxKind);
                if (precedence == 0 || precedence <= parentPrecedence)
                    break;

                var operatorToken = NextToken();
                var right = ParseExpression(precedence);
                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }
            return left;
        }

        private static int GetBinaryOperatorPrecedence(SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    return 1;

                case SyntaxKind.StarToken:
                case SyntaxKind.SlashToken:
                    return 2;

                default:
                    return 0;
            }
        }


        private ExpressionSyntax.ExpressionSyntax ParsePrimaryExpression()
        {
            if (Current.SyntaxKind == SyntaxKind.OpenParenthesisToken)
            {
                var left = NextToken();
                var expression = ParseExpression();
                var right = MatchToken(SyntaxKind.ClosedParenthesisToken);
                return new ParenthesizedExpressionSytax(left, expression, right);
            }

            var numberToken = MatchToken(SyntaxKind.NumberToken);
            return new LiteralExpressionSyntax(numberToken);
        }
    }
}
