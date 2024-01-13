using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Project_Logic.Rewriters;

namespace Project_Logic.Obfuscators
{
    public class CodeObfuscator
    {
        private readonly Encryptor _encryptor;
        public static Dictionary<string, string> EcryptedObjects = new Dictionary<string, string>();

        public CodeObfuscator()
        {
            _encryptor = new Encryptor();
        }

        public string Obfuscate(string code)
        {
            var tree = CSharpSyntaxTree.ParseText(code);
            var root = (CompilationUnitSyntax)tree.GetRoot();

            root = (CompilationUnitSyntax)new CommentRemover().RemoveComments(root);

            root = (CompilationUnitSyntax)new VariableRewriter(_encryptor).Rewrite(root);

            string obfuscatedCode = root.ToFullString();

            foreach (var item in EcryptedObjects)
            {
                obfuscatedCode = obfuscatedCode.Replace(item.Key, item.Value);   
            }

            return obfuscatedCode;
        }
    }
}
