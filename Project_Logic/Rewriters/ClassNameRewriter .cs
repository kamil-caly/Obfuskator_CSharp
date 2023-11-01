using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Obfuskator;

namespace Project_Logic.Rewriters
{
    public class ClassNameRewriter : CSharpSyntaxRewriter
    {
        private readonly Encryptor _encryptor;

        public ClassNameRewriter(Encryptor encryptor)
        {
            _encryptor = encryptor;
        }

        public SyntaxNode Rewrite(CompilationUnitSyntax node)
        {
            return base.Visit(node);
        }

        public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            var modifiedClass = (ClassDeclarationSyntax)base.VisitClassDeclaration(node)!;

            string originalClassName = node.Identifier.Text;
            string encryptedClassName = _encryptor.Encrypt(originalClassName);

            var newIdentifierToken = SyntaxFactory.Identifier(encryptedClassName);
            return modifiedClass.WithIdentifier(newIdentifierToken);
        }
    }
}
