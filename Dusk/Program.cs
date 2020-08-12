using Dusk.CodeAnalysis;
using Dusk.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dusk
{
    class Program
    {
        static void Main(string[] args)
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
                    var color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.DarkGray;

                    Print(syntaxTree.Root);

                    Console.ForegroundColor = color;
                }




                if (syntaxTree.Diagnostics.Count == 0)
                {
                    var e = new Evaluator(syntaxTree.Root);
                    var result = e.Evaluate();

                    Console.WriteLine(result);
                }
                else
                {
                    var color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;

                    foreach (var diagnostic in syntaxTree.Diagnostics)
                    {
                        Console.WriteLine(diagnostic);
                    }

                    Console.ForegroundColor = color;
                }

            }
        }

        static void Print(SyntaxNode node, string indent = "", bool isLast = true)
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

            indent += isLast ? "    " : "│    ";

            var lastChild = node.GetChildren().LastOrDefault();

            foreach (var child in node.GetChildren())
            {
                Print(child, indent, child == lastChild);
            }
        }
    }

}
