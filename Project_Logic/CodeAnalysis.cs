using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;

namespace Project_Logic
{
    public class CodeAnalysis
    {
        public readonly string CodeToAnalysis;
        public CodeAnalysis(string code)
        {
            CodeToAnalysis = code;  
        }

        public void Analysis()
        {
            var tree = CSharpSyntaxTree.ParseText(CodeToAnalysis);
            var root = (CompilationUnitSyntax)tree.GetRoot();

            foreach (var member in root.Members)
            {
                if (member is ClassDeclarationSyntax classDeclaration)
                {
                    Console.WriteLine($"Znaleziono klasę o nazwie: {classDeclaration.Identifier.Text}");

                    // Znajduje metody w klasie
                    foreach (var method in classDeclaration.Members.OfType<MethodDeclarationSyntax>())
                    {
                        Console.WriteLine($"Znaleziono metodę o nazwie: {method.Identifier.Text}");

                        // Znajduje instrukcje if
                        foreach (var ifStatement in method.Body.Statements.OfType<IfStatementSyntax>())
                        {
                            Console.WriteLine($"Znaleziono instrukcję if");
                        }

                        // Znajduje pętle for
                        foreach (var forLoop in method.Body.Statements.OfType<ForStatementSyntax>())
                        {
                            Console.WriteLine("Znaleziono pętlę for");
                        }

                        // Znajduje pętle while
                        foreach (var whileLoop in method.Body.Statements.OfType<WhileStatementSyntax>())
                        {
                            Console.WriteLine("Znaleziono pętlę while");
                        }
                    }
                }
            }
        }
    }
}
