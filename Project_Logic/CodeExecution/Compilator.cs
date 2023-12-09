using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using Project_Logic.Entities;
using Microsoft.CodeAnalysis.Emit;

namespace Project_Logic.CodeExecution
{
    public class Compilator
    {
        public static CompilationInfo Compile(string code)
        {
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
                    IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);

                    List<CompilationFailure> compFailures = new();

                    foreach (Diagnostic diagnostic in failures)
                    {
                        CompilationFailure compFailure = new();

                        compFailure.ErrorId = diagnostic.Id;
                        compFailure.Message = diagnostic.GetMessage();
                        compFailure.Severity = diagnostic.Severity.ToString();

                        var location = diagnostic.Location;
                        if (location != null)
                        {
                            compFailure.StartLine = location.GetLineSpan().StartLinePosition.Line;
                            compFailure.StartColumn = location.GetLineSpan().StartLinePosition.Character;
                        }

                        compFailures.Add(compFailure);
                    }

                    return new CompilationInfo { CompilationMS = ms, CompilationFailures = compFailures };
                }
                
                return new CompilationInfo { CompilationMS = ms, CompilationFailures = null };
            }
        }
    }
}
