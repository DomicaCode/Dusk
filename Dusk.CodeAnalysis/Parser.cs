using Dusk.CodeAnalysis.ExpressionSyntax;
using Dusk.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dusk.CodeAnalysis
{

    class Parser
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
                token = lexer.NextToken();

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

        private SyntaxToken Match(SyntaxKind syntaxKind)
        {
            if (Current.SyntaxKind == syntaxKind)
                return NextToken();

            _diagnostics.Add($"Error: Unexpected token <{Current.SyntaxKind}>, expected <{syntaxKind}>");
            return new SyntaxToken(syntaxKind, Current.Position, null, null);
        }

        private ExpressionSyntax.ExpressionSyntax ParseExpression()
        {
            return ParseTerm();
        }

        public SyntaxTree Parse()
        {

            var expression = ParseTerm();
            var endOfFile = Match(SyntaxKind.EndOfFileToken);

            return new SyntaxTree(_diagnostics, expression, endOfFile);
        }

        /// <summary>
        /// Parses term integers -- + & -
        /// </summary>
        /// <returns></returns>
        private ExpressionSyntax.ExpressionSyntax ParseTerm()
        {
            var left = ParseFactor();

            while (Current.SyntaxKind == SyntaxKind.PlusToken ||
                   Current.SyntaxKind == SyntaxKind.MinusToken)
            {
                var operatorToken = NextToken();
                var right = ParseFactor();
                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }

            return left;
        }

        /// <summary>
        /// Parse multiplacitive integers -- * & /
        /// </summary>
        /// <returns></returns>
        private ExpressionSyntax.ExpressionSyntax ParseFactor()
        {
            var left = ParsePrimaryExpression();

            while (Current.SyntaxKind == SyntaxKind.StarToken ||
                   Current.SyntaxKind == SyntaxKind.SlashToken)
            {
                var operatorToken = NextToken();
                var right = ParsePrimaryExpression();
                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }

            return left;
        }

        private ExpressionSyntax.ExpressionSyntax ParsePrimaryExpression()
        {
            if (Current.SyntaxKind == SyntaxKind.OpenParenthesisToken)
            {
                var left = NextToken();
                var expression = ParseExpression();
                var right = Match(SyntaxKind.ClosedParenthesisToken);
                return new ParenthesizedExpressionSytax(left, expression, right);
            }

            var numberToken = Match(SyntaxKind.NumberToken);
            return new NumberExpressionSyntax(numberToken);
        }
    }
}
