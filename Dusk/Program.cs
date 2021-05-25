using Dusk.CodeAnalysis;
using Dusk.CodeAnalysis.Syntax;
using System;
using System.Linq;

namespace Dusk
{
    internal static class Program
    {
        #region Methods

        private static void Main()
        {
            bool showTree = false;

            while (true)
            {
                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    return;

                if (line == "#showTree")
                {
                    showTree = !showTree;
                    Console.WriteLine($"Showing trees: {showTree}");
                    continue;
                }
                else if (line == "#cls")
                {
                    Console.Clear();
                    continue;
                }

                var syntaxTree = SyntaxTree.Parse(line);

                if (showTree)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;

                    Print(syntaxTree.Root);

                    Console.ResetColor();
                }

                if (syntaxTree.Diagnostics.Count == 0)
                {
                    var e = new Evaluator(syntaxTree.Root);
                    var result = e.Evaluate();

                    Console.WriteLine(result);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;

                    foreach (var diagnostic in syntaxTree.Diagnostics)
                    {
                        Console.WriteLine(diagnostic);
                    }

                    Console.ResetColor();
                }
            }
        }

        private static void Print(SyntaxNode node, string indent = "", bool isLast = true)
        {
            var marker = isLast ? "└──" : "├──";

            //└──" : "├──

            Console.Write(indent);
            Console.Write(marker);
            Console.Write(node.SyntaxKind);

            if (node is SyntaxToken t && t.Value != null)
            {
                Console.Write("");
                Console.Write(t.Value);
            }

            Console.WriteLine();

            indent += isLast ? "   " : "│    ";

            var lastChild = node.GetChildren().LastOrDefault();

            foreach (var child in node.GetChildren())
            {
                Print(child, indent, child == lastChild);
            }
        }

        #endregion Methods
    }
}