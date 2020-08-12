using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dusk.CodeAnalysis.Syntax
{
    public enum SyntaxKind
    {
        NumberToken,
        WhiteSpaceToken,
        PlusToken,
        MinusToken,
        ClosedParenthesisToken,
        StarToken,
        SlashToken,
        OpenParenthesisToken,
        BadToken,
        EndOfFileToken,
        NumberExpression,
        BinaryExpression,
        ParenthesizedExpression
    }
}
