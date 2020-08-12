using System.Collections.Generic;
using System.Linq;

namespace Dusk.CodeAnalysis.Syntax
{
    public class SyntaxToken : SyntaxNode
    {
        public SyntaxToken(
            SyntaxKind syntaxKind,
            int position,
            string text,
            object value)
        {
            SyntaxKind = syntaxKind;
            Position = position;
            Text = text;
            Value = value;
        }

        public override SyntaxKind SyntaxKind { get; }
        public int Position { get; }
        public string Text { get; }
        public object Value { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            return Enumerable.Empty<SyntaxNode>();
        }
    }
}
