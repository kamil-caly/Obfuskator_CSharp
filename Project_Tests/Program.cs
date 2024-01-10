// See https://aka.ms/new-console-template for more information
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Project_Logic;
using Project_Logic.Rewriters;
using Microsoft.CodeAnalysis;
using System.Reflection;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using Project_Logic.Obfuscators;

class Program
{
    static void Main()
    {
        CodeEvalTests();

        Console.ReadLine(); 
    }

    static void CodeEvalTests()
    {
        string code = @"
class Program
{
    public static string Main()
    {
        int[] tablica = { 5, 2, 8, 1, 4 };

        SortowanieBabelkowe(tablica);

        // Zwracamy posortowaną tablicę
        return string.Join("" "", tablica);
    }

    static void SortowanieBabelkowe(int[] tablica)
    {
        int n = tablica.Length;

        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - 1 - i; j++)
            {
                if (tablica[j] > tablica[j + 1])
                {
                    int temp = tablica[j];
                    tablica[j] = tablica[j + 1];
                    tablica[j + 1] = temp;
                }
            }
        }
    }
}

                    ";

        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);

        string assemblyName = Path.GetRandomFileName();
        MetadataReference[] references = new MetadataReference[]
        {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location)
        };

        CSharpCompilation compilation = CSharpCompilation.Create(
            assemblyName,
            syntaxTrees: new[] { syntaxTree },
            references: references,
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        using (var ms = new MemoryStream())
        {
            EmitResult result = compilation.Emit(ms);

            if (!result.Success)
            {
                // handle exceptions
                IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                    diagnostic.IsWarningAsError ||
                    diagnostic.Severity == DiagnosticSeverity.Error);

                foreach (Diagnostic diagnostic in failures)
                {
                    // Informację, wyjątki, błędy podczas kompilacji danego kodu
                    Console.Error.WriteLine("---------------------- Compilation Error ----------------------");
                    Console.Error.WriteLine($"Error ID: {diagnostic.Id}");
                    Console.Error.WriteLine($"Message: {diagnostic.GetMessage()}");
                    Console.Error.WriteLine($"Severity: {diagnostic.Severity}");

                    // Miejsce w kodzie, gdzie błąd wystąpił
                    var location = diagnostic.Location;
                    if (location != null)
                    {
                        Console.Error.WriteLine($"Start Line: {location.GetLineSpan().StartLinePosition.Line}");
                        Console.Error.WriteLine($"Start Column: {location.GetLineSpan().StartLinePosition.Character}");
                    }

                    Console.WriteLine();
                }
            }
            else
            {
                ms.Seek(0, SeekOrigin.Begin);
                Assembly assembly = Assembly.Load(ms.ToArray());

                Type type = assembly.GetType("Program")!;
                object obj = Activator.CreateInstance(type)!;

                //var test = type.InvokeMember("Main",
                //        BindingFlags.Default | BindingFlags.InvokeMethod,
                //        null,
                //        obj,
                //        new object[] { "Hello World" });

                // Inne rozwiązanie
                MethodInfo[] methods = type.GetMethods();

                foreach (MethodInfo method in methods)
                {
                    if (code.Contains(method.Name) && method.Name == "Main")
                    {
                        try
                        {
                            var result2 = method.Invoke(obj, null);
                            Console.WriteLine("Code result: " + result2);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine("---------------------- Runtime Error ----------------------");
                            // Wyjątki podczas wykonywania danego kodu
                            if (ex is TargetInvocationException targetEx && targetEx.InnerException != null)
                            {
                                Console.Error.WriteLine($"InnerException: {targetEx.InnerException.GetType().FullName}");
                                Console.Error.WriteLine($"InnerException Message: {targetEx.InnerException.Message}");
                                Console.Error.WriteLine($"InnerException StackTrace: {targetEx.InnerException.StackTrace}");
                            }
                            else 
                            {
                                Console.Error.WriteLine($"Wystąpił wyjątek: {ex.GetType().FullName}");
                                Console.Error.WriteLine($"Opis: {ex.Message}");
                                Console.Error.WriteLine($"StackTrace: {ex.StackTrace}");
                            }

                            Console.WriteLine();
                        }
                    }
                }
            }
        }
    }

    static void ObfuskateTests()
    {
        string text = "To_jest_przykladowy_tekst";
        Encryptor encryptor = new Encryptor();

        string encryptedText = encryptor.Encrypt(text);
        Console.WriteLine($"Encrypted text: {encryptedText}");

        //string decryptedText = encryptor.DecryptText(encryptedText);
        //Console.WriteLine($"Decrypted text: {decryptedText}");

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

        // Obfucator  ////////////////////////////////////////////

        CodeObfuscator codeObfuscator = new CodeObfuscator();
        var obfucateCode = codeObfuscator.Obfuscate(code);
        //var deobfuscateCode = codeObfuscator.Deobfuscate(obfucateCode);

        Console.WriteLine(obfucateCode);

        Console.WriteLine("--------------------------------------------------------------");

        //Console.WriteLine(deobfuscateCode);

        Console.ReadKey();
    }
}
