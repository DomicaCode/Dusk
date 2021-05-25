using Dusk.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dusk.CodeAnalysis
{

    internal sealed class Lexer
    {
        private readonly string _text;

        private int _position;

        private List<string> _diagnostics = new List<string>();

        public Lexer(string text)
        {
            _text = text;
        }

        public IEnumerable<string> Diagnostics => _diagnostics;

        private char Current
        {
            get
            {
                if (_position >= _text.Length)
                    return '\0';
                return _text[_position];
            }
        }

        private void Next()
        {
            _position++;
        }

        public SyntaxToken NextToken()
        {

            if (_position >= _text.Length)
            {
                return new SyntaxToken(SyntaxKind.EndOfFileToken, _position, "\0", null);
            }

            if (char.IsDigit(Current))
            {
                var startPos = _position;

                while (char.IsDigit(Current))
                {
                    Next();
                }

                var length = _position - startPos;
                var text = _text.Substring(startPos, length);

                if (!int.TryParse(text, out var value))
                {
                    _diagnostics.Add($"The number {_text} isnt an Int32");
                }


                return new SyntaxToken(SyntaxKind.NumberToken, startPos, text, value);
            }

            if (char.IsWhiteSpace(Current))
            {
                var startPos = _position;

                while (char.IsWhiteSpace(Current))
                {
                    Next();
                }

                var length = _position - startPos;
                var text = _text.Substring(startPos, length);

                return new SyntaxToken(SyntaxKind.WhiteSpaceToken, startPos, text, null);
            }

            if (Current == '+')
                return new SyntaxToken(SyntaxKind.PlusToken, _position++, "+", null);
            else if (Current == '-')
                return new SyntaxToken(SyntaxKind.MinusToken, _position++, "-", null);
            else if (Current == '*')
                return new SyntaxToken(SyntaxKind.StarToken, _position++, "*", null);
            else if (Current == '/')
                return new SyntaxToken(SyntaxKind.SlashToken, _position++, "/", null);
            else if (Current == '(')
                return new SyntaxToken(SyntaxKind.OpenParenthesisToken, _position++, "(", null);
            else if (Current == ')')
                return new SyntaxToken(SyntaxKind.ClosedParenthesisToken, _position++, ")", null);

            //If nothing matches what we can read
            _diagnostics.Add($"Error: bad character input: '{Current}'");
            return new SyntaxToken(SyntaxKind.BadToken, _position++, _text.Substring(_position - 1, 1), null);
        }
    }
}
