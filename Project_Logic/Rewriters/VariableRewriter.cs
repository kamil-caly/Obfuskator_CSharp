using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Project_Logic.Obfuscators;
using System.Runtime.CompilerServices;

namespace Project_Logic.Rewriters
{
    public class VariableRewriter : CSharpSyntaxRewriter
    {
        private readonly Encryptor _encryptor;
        private Dictionary<string, string> variableNameMap;

        public VariableRewriter(Encryptor encryptor)
        {
            _encryptor = encryptor;
            variableNameMap = new Dictionary<string, string>();
        }

        public SyntaxNode Rewrite(CompilationUnitSyntax node)
        {
            return Visit(node);
        }

        public override SyntaxNode VisitVariableDeclarator(VariableDeclaratorSyntax node)
        {
            var originalVariableName = node.Identifier.Text;
            string encryptedVariableName = _encryptor.Encrypt(originalVariableName);
            variableNameMap[originalVariableName] = encryptedVariableName;

            // sprawdzam czy jest to zmienna typu klasowego
            if (node.Parent is VariableDeclarationSyntax variableDeclaration &&
            variableDeclaration.Type is IdentifierNameSyntax)
            {
                CodeObfuscator.EcryptedObjects.TryAdd(originalVariableName, encryptedVariableName);
            }

            var newIdentifierToken = SyntaxFactory.Identifier(encryptedVariableName);
            return node.WithIdentifier(newIdentifierToken).WithTriviaFrom(node);
        }

        public override SyntaxNode VisitParameter(ParameterSyntax node)
        {
            var originalParameterName = node.Identifier.Text;

            // Obfuskuj nazwę argumentu
            string encryptedParameterName = _encryptor.Encrypt(originalParameterName);
            variableNameMap[originalParameterName] = encryptedParameterName;

            // Utwórz nowy identyfikator dla argumentu
            var newIdentifierToken = SyntaxFactory.Identifier(encryptedParameterName);
            var newIdentifier = SyntaxFactory.Identifier(newIdentifierToken.ToString());

            // Zwróć parametr z nowym identyfikatorem
            return node.WithIdentifier(newIdentifier).WithTriviaFrom(node);
        }

        public override SyntaxNode VisitObjectCreationExpression(ObjectCreationExpressionSyntax node)
        {
            if (node.Type is IdentifierNameSyntax identifierName)
            {
                // Obsługa tworzenia obiektu klasy
                var originalClassName = identifierName.Identifier.Text;
                if (originalClassName != "Program")
                {
                    string encryptedClassName = _encryptor.Encrypt(originalClassName);
                    var newIdentifierToken = SyntaxFactory.Identifier(encryptedClassName);
                    var newIdentifierName = identifierName.WithIdentifier(newIdentifierToken);
                    return node.WithType(newIdentifierName).WithTriviaFrom(node);
                }
            }

            return base.VisitObjectCreationExpression(node)!;
        }

        public override SyntaxNode VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
        {
            // Obsługa wyrażeń z dostępem do składowych, np. example.testMethod(5)
            if (node.Expression is IdentifierNameSyntax identifierName)
            {
                var originalMemberName = node.Name.Identifier.Text;
                string encryptedMemberName = _encryptor.Encrypt(originalMemberName);
                var newIdentifierToken = SyntaxFactory.Identifier(encryptedMemberName);
                var newIdentifierName = SyntaxFactory.IdentifierName(newIdentifierToken);
                return node.WithName(newIdentifierName).WithTriviaFrom(node);
            }

            return base.VisitMemberAccessExpression(node)!;
        }

        public override SyntaxNode VisitIdentifierName(IdentifierNameSyntax node)
        {
            // Obsługa referencji do zmiennych
            var originalVariableName = node.Identifier.Text;

            if (variableNameMap.TryGetValue(originalVariableName, out var encryptedVariableName))
            {
                var newIdentifierToken = SyntaxFactory.Identifier(encryptedVariableName);
                return node.WithIdentifier(newIdentifierToken);
            }

            return base.VisitIdentifierName(node)!;
        }

        public override SyntaxNode VisitReturnStatement(ReturnStatementSyntax node)
        {
            var expression = node.Expression;

            if (expression is IdentifierNameSyntax identifierName)
            {
                // Obsługa zmiany nazwy zmiennej zwracanej
                var originalReturnVariableName = identifierName.Identifier.Text;
                if (variableNameMap.TryGetValue(originalReturnVariableName, out var encryptedReturnVariableName))
                {
                    var newIdentifierToken = SyntaxFactory.Identifier(encryptedReturnVariableName);
                    var newIdentifierName = identifierName.WithIdentifier(newIdentifierToken);
                    return node.WithExpression(newIdentifierName).WithTriviaFrom(node);
                }
            }

            return base.VisitReturnStatement(node)!;
        }
    }

}
