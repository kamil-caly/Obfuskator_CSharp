// See https://aka.ms/new-console-template for more information
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Obfuskator;
using Project_Logic;
using Project_Logic.Rewriters;
using Microsoft.CodeAnalysis;
using System.Reflection;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CSharp;
using System.CodeDom.Compiler;

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
                using System.Diagnostics;

                namespace RoslynCompileSample
                {
                    class Program
                    {
                        public static string Main(string message) 
                        {
                            int a = 2;
                            Debug.WriteLine(""sfks"");
                            string final = test();
                            var testClass = new TestClass();
                            return message + final + testClass.test2();
                        }

                        public static string test() 
                        {
                            return ""abc"";
                        }
                    }

                    class TestClass
                    {
                        public string test2() 
                        {
                            return ""TestClass"";
                        }
                    }
                }";

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
                    Console.Error.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                }
            }
            else
            {
                ms.Seek(0, SeekOrigin.Begin);
                Assembly assembly = Assembly.Load(ms.ToArray());

                Type type = assembly.GetType("RoslynCompileSample.Program")!;
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
                    if (code.Contains(method.Name))
                    {
                        try
                        {
                            var result2 = method.Invoke(obj, new object[] { "Hello World" });
                        }
                        catch (Exception)
                        {
                            // nic nie robimy, bo możemy
                        }
                    }
                }

                string aaa = "sss";
            }
        }
    }

    static void ObfuskateTests()
    {
        string text = "To_jest_przykladowy_tekst";
        Encryptor encryptor = new Encryptor();

        string encryptedText = encryptor.Encrypt(text);
        Console.WriteLine($"Encrypted text: {encryptedText}");

        string decryptedText = encryptor.DecryptText(encryptedText);
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

        // Obfucator  ////////////////////////////////////////////

        CodeObfuscator codeObfuscator = new CodeObfuscator();
        var obfucateCode = codeObfuscator.Obfuscate(code);
        var deobfuscateCode = codeObfuscator.Deobfuscate(obfucateCode);

        Console.WriteLine(obfucateCode);

        Console.WriteLine("--------------------------------------------------------------");

        Console.WriteLine(deobfuscateCode);

        Console.ReadKey();
    }
}
