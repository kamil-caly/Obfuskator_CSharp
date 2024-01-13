using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using Project_Logic.Obfuscators;

namespace Project_Logic.Rewriters
{
    public class MethodNameRewriter : CSharpSyntaxRewriter
    {
        public MethodNameRewriter(Encryptor encryptor)
        {
            _encryptor = encryptor;
        }

        private readonly Encryptor _encryptor;

        public SyntaxNode Rewrite(CompilationUnitSyntax node)
        {
            return base.Visit(node); 
        }

        public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            if (node.Identifier.Text != "Main")
            {
                return ReplaceMethodName((MethodDeclarationSyntax)base.VisitMethodDeclaration(node)!);
            }

            return base.VisitMethodDeclaration(node)!;
        }

        private SyntaxNode ReplaceMethodName(MethodDeclarationSyntax methodNode)
        {
            string originalMethodName = methodNode.Identifier.Text;
            string encryptedMethodName = _encryptor.Encrypt(originalMethodName);
            CodeObfuscator.EcryptedObjects.TryAdd(originalMethodName, encryptedMethodName);

            var newIdentifierToken = SyntaxFactory.Identifier(encryptedMethodName);
            return methodNode.WithIdentifier(newIdentifierToken).WithTriviaFrom(methodNode);
        }
    }
}
