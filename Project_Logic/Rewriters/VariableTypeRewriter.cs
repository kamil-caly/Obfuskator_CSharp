using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Obfuskator;

namespace Project_Logic.Rewriters
{
    public class VariableTypeRewriter : CSharpSyntaxRewriter
    {
        private readonly Encryptor _encryptor;

        public VariableTypeRewriter(Encryptor encryptor)
        {
            _encryptor = encryptor;
        }

        public SyntaxNode Rewrite(CompilationUnitSyntax node)
        {
            return base.Visit(node).NormalizeWhitespace();
        }

        public override SyntaxNode VisitVariableDeclaration(VariableDeclarationSyntax node)
        {
            string originalType = node.Type.ToString();
            string encryptedTypeName = _encryptor.Encrypt(originalType);

            var modifiedNode = node.WithType(SyntaxFactory.ParseTypeName(encryptedTypeName));

            return base.VisitVariableDeclaration(modifiedNode)!;
        }
    }
}
