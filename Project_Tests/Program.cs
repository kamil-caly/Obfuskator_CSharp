// See https://aka.ms/new-console-template for more information
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Obfuskator;
using Project_Logic;
using Project_Logic.Rewriters;

class Program
{
    static void Main()
    {
        string text = "To_jest_przykladowy_tekst";
        Encryptor encryptor = new Encryptor();

        string encryptedText = encryptor.Encrypt(text);
        Console.WriteLine($"Encrypted text: {encryptedText}");

        string decryptedText = encryptor.Decrypt(encryptedText);
        Console.WriteLine($"Decrypted text: {decryptedText}");

        // Code analysis ////////////////////////////////////////////
        var code = @"
        public class Example 
        { 
            private void testMethod33(){}

            public void testMethod(int b)
            {
                if(true)
                {
                    Console.WriteLine(""Inside if"");
                }

                for(int i = 0; i < 10; i++)
                {
                    for(int g = 0; g < 1; g++)
                    {
                        for(int f = 0; f < 1; f++)
                        {
                            break;
                        }
                    }

                    Console.WriteLine(i);
                    break;
                }

                float zmienna = 10.5;
                int j = 0;

                while(j < 10)
                {
                    Console.WriteLine(j);
                    j++;
                }
            } 
        }
        public class Example2{}";

        //Console.WriteLine("Wprowadź kod źródłowy (naciśnij Ctrl+Z, a następnie Enter, aby zakończyć):");

        //var codeBuilder = new StringBuilder();
        //string line;
        //while ((line = Console.ReadLine()) != null)
        //{
        //    codeBuilder.AppendLine(line);
        //}

        //var code = codeBuilder.ToString();

        CodeAnalysis codeAnalysis = new(code);
        codeAnalysis.Analysis();

        // Rewriters  ////////////////////////////////////////////
        var tree = CSharpSyntaxTree.ParseText(code);
        var root = (CompilationUnitSyntax)tree.GetRoot();

        //Encryptor encryptor = new Encryptor();

        var syntaxRewriter = new SyntaxElementRewriter(encryptor);
        var newRoot = (CompilationUnitSyntax)syntaxRewriter.Rewrite(root);

        var methodNameRewriter = new MethodNameRewriter(encryptor);
        var newRoot2 = (CompilationUnitSyntax)methodNameRewriter.Rewrite(newRoot);

        var methodArgumentRewriter = new MethodArgumentRewriter(encryptor);
        var newRoot3 = (CompilationUnitSyntax)methodArgumentRewriter.Rewrite(newRoot2);

        var typeRewriter = new VariableTypeRewriter(encryptor);
        var newRoot5 = (CompilationUnitSyntax)typeRewriter.Rewrite(newRoot3);

        var variableNameRewriter = new VariableNameRewriter(encryptor);
        var newRoot6 = (CompilationUnitSyntax)variableNameRewriter.Rewrite(newRoot5);

        Console.WriteLine(newRoot6.ToFullString());

        Console.ReadKey();
    }
}
