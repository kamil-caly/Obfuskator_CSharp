using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Obfuskator;

namespace Project_Logic.Rewriters
{
    public class VariableNameRewriter : CSharpSyntaxRewriter
    {
        private readonly Encryptor _encryptor;

        public VariableNameRewriter(Encryptor encryptor)
        {
            _encryptor = encryptor;
        }

        public SyntaxNode Rewrite(CompilationUnitSyntax node)
        {
            return base.Visit(node);
        }

        public override SyntaxNode VisitVariableDeclarator(VariableDeclaratorSyntax node)
        {
            string originalVariableName = node.Identifier.Text;
            string encryptedVariableName = _encryptor.Encrypt(originalVariableName);

            var newIdentifierToken = SyntaxFactory.Identifier(encryptedVariableName);

            return node.WithIdentifier(newIdentifierToken).WithTriviaFrom(node);
        }
    }
}
