using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Logic.Obfuscators;

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

            if (originalClassName != "Program")
            {
                string encryptedClassName = _encryptor.Encrypt(originalClassName);
                CodeObfuscator.EcryptedObjects.TryAdd(originalClassName, encryptedClassName);
                var newIdentifierToken = SyntaxFactory.Identifier(encryptedClassName);
                return node.WithIdentifier(newIdentifierToken).WithTriviaFrom(node);
            }

            return base.VisitClassDeclaration(node)!;
        }

        public override SyntaxNode VisitVariableDeclaration(VariableDeclarationSyntax node)
        {
            var originalTypeName = node.Type.ToString();

            if (originalTypeName != "Program" && node.Type is not PredefinedTypeSyntax)
            {
                string encryptedTypeName = _encryptor.Encrypt(originalTypeName) + " ";
                var newIdentifierName = SyntaxFactory.IdentifierName(encryptedTypeName);
                return node.WithType(newIdentifierName).WithTriviaFrom(node);
            }

            return base.VisitVariableDeclaration(node)!;
        }
    }
}
