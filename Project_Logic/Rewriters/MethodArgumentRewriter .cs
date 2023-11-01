using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Obfuskator;

namespace Project_Logic.Rewriters
{
    public class MethodArgumentRewriter : CSharpSyntaxRewriter
    {
        private readonly Encryptor _encryptor;

        public MethodArgumentRewriter(Encryptor encryptor)
        {
            _encryptor = encryptor;
        }

        public SyntaxNode Rewrite(CompilationUnitSyntax node)
        {
            return base.Visit(node);
        }

        public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            var modifiedMethod = (MethodDeclarationSyntax)base.VisitMethodDeclaration(node)!;

            var modifiedParameters = new SeparatedSyntaxList<ParameterSyntax>();
            foreach (var parameter in modifiedMethod.ParameterList.Parameters)
            {
                var encryptedParameterName = _encryptor.Encrypt(parameter.Identifier.Text);
                var newParameter = parameter.WithIdentifier(SyntaxFactory.Identifier(encryptedParameterName));
                modifiedParameters = modifiedParameters.Add(newParameter);
            }

            return modifiedMethod.WithParameterList(modifiedMethod.ParameterList.WithParameters(modifiedParameters));
        }
    }
}
