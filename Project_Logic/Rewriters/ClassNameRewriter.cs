using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using Obfuskator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return Visit(node);
        }

        public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            string originalClassName = node.Identifier.Text;
            string encryptedClassName = _encryptor.Encrypt(originalClassName);

            var newIdentifierToken = SyntaxFactory.Identifier(encryptedClassName);

            return node.WithIdentifier(newIdentifierToken).WithTriviaFrom(node);
        }
    }
}
