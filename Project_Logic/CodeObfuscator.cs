using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Obfuskator;
using Project_Logic.Rewriters;

namespace Project_Logic
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
            root = (CompilationUnitSyntax)new SyntaxElementRewriter(_encryptor).Rewrite(root);
            root = (CompilationUnitSyntax)new MethodNameRewriter(_encryptor).Rewrite(root);
            root = (CompilationUnitSyntax)new MethodArgumentRewriter(_encryptor).Rewrite(root);
            root = (CompilationUnitSyntax)new VariableTypeRewriter(_encryptor).Rewrite(root);
            root = (CompilationUnitSyntax)new VariableNameRewriter(_encryptor).Rewrite(root);

            return root.ToFullString();
        }

        public string Deobfuscate(string obfuscateCode)
        {
            return _encryptor.DecryptText(obfuscateCode);
        }
    }
}
