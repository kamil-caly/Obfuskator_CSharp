using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Project_Logic.Rewriters;

namespace Project_Logic.Obfuscators
{
    public class CodeObfuscator
    {
        private readonly Encryptor _encryptor;

        public CodeObfuscator()
        {
            _encryptor = new Encryptor();
        }

        public string Obfuscate(string code)
        {
            var tree = CSharpSyntaxTree.ParseText(code);
            var root = (CompilationUnitSyntax)tree.GetRoot();

            root = (CompilationUnitSyntax)new CommentRemover().RemoveComments(root);

            root = (CompilationUnitSyntax)new ClassNameRewriter(_encryptor).Rewrite(root);

            root = (CompilationUnitSyntax)new MethodNameRewriter(_encryptor).Rewrite(root);

            root = (CompilationUnitSyntax)new MethodArgumentRewriter(_encryptor).Rewrite(root);

            // TODO: Też do poprawy, bo nie wszystkie nazwy są zamieniane
            root = (CompilationUnitSyntax)new VariableNameRewriter(_encryptor).Rewrite(root);

            return root.ToFullString();
        }
    }
}
