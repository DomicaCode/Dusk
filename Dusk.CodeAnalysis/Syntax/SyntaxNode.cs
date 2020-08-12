using Dusk.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dusk.CodeAnalysis
{
    public abstract class SyntaxNode
    {
        public abstract SyntaxKind SyntaxKind { get; }

        public abstract IEnumerable<SyntaxNode> GetChildren();
    }
}
