using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using Obfuskator;
using System.Xml.Linq;
using System.Security.Cryptography;
using System.Text;
using System;

namespace Project_Logic.Rewriters
{
    public class SyntaxElementRewriter : CSharpSyntaxRewriter
    {
        private readonly Dictionary<SyntaxKind, string> SyntaxKindToKeyword = new Dictionary<SyntaxKind, string>
        {
            { SyntaxKind.IfKeyword, "if" },
            { SyntaxKind.ForKeyword, "for" },
            { SyntaxKind.WhileKeyword, "while" },
            { SyntaxKind.ElseKeyword, "else" },
            { SyntaxKind.BreakKeyword, "break" },
            { SyntaxKind.ContinueKeyword, "continue" },
            { SyntaxKind.SwitchKeyword, "switch" }
        };

        private readonly Encryptor _encryptor;

        public SyntaxElementRewriter(Encryptor encryptor)
        {
            _encryptor = encryptor;
        }

        public SyntaxNode Rewrite(CompilationUnitSyntax node)
        {
            return base.Visit(node);
        }

        public override SyntaxNode VisitBreakStatement(BreakStatementSyntax node) => 
            ReplaceSyntaxNode((BreakStatementSyntax)base.VisitBreakStatement(node)!, SyntaxKindToKeyword[SyntaxKind.BreakKeyword]);
        public override SyntaxNode VisitIfStatement(IfStatementSyntax node) =>
            ReplaceSyntaxNode((IfStatementSyntax)base.VisitIfStatement(node)!, SyntaxKindToKeyword[SyntaxKind.IfKeyword]);
        public override SyntaxNode VisitForStatement(ForStatementSyntax node) =>
            ReplaceSyntaxNode((ForStatementSyntax)base.VisitForStatement(node)!, SyntaxKindToKeyword[SyntaxKind.ForKeyword]);
        public override SyntaxNode VisitWhileStatement(WhileStatementSyntax node) =>
            ReplaceSyntaxNode((WhileStatementSyntax)base.VisitWhileStatement(node)!, SyntaxKindToKeyword[SyntaxKind.WhileKeyword]);
        public override SyntaxNode VisitElseClause(ElseClauseSyntax node) =>
            ReplaceSyntaxNode((ElseClauseSyntax)base.VisitElseClause(node)!, SyntaxKindToKeyword[SyntaxKind.ElseKeyword]);
        public override SyntaxNode VisitContinueStatement(ContinueStatementSyntax node) =>
            ReplaceSyntaxNode((ContinueStatementSyntax)base.VisitContinueStatement(node)!, SyntaxKindToKeyword[SyntaxKind.ContinueKeyword]);
        public override SyntaxNode VisitSwitchStatement(SwitchStatementSyntax node) =>
            ReplaceSyntaxNode((SwitchStatementSyntax)base.VisitSwitchStatement(node)!, SyntaxKindToKeyword[SyntaxKind.SwitchKeyword]);

        private SyntaxNode ReplaceSyntaxNode(SyntaxNode node, string keyword)
        {
            string encryptedText = _encryptor.Encrypt(keyword);
            SyntaxKind currentKeyword = SyntaxKindToKeyword.FirstOrDefault(v => v.Value == keyword).Key;
            var newKeywordToken = SyntaxFactory.Token(SyntaxTriviaList.Empty, currentKeyword, encryptedText, encryptedText, SyntaxTriviaList.Empty);
            return currentKeyword switch
            {
                SyntaxKind.IfKeyword => ((IfStatementSyntax)node).WithIfKeyword(newKeywordToken).WithTriviaFrom(node),
                SyntaxKind.ForKeyword => ((ForStatementSyntax)node).WithForKeyword(newKeywordToken).WithTriviaFrom(node),
                SyntaxKind.WhileKeyword => ((WhileStatementSyntax)node).WithWhileKeyword(newKeywordToken).WithTriviaFrom(node),
                SyntaxKind.ElseKeyword => ((ElseClauseSyntax)node).WithElseKeyword(newKeywordToken).WithTriviaFrom(node),
                SyntaxKind.BreakKeyword => ((BreakStatementSyntax)node).WithBreakKeyword(newKeywordToken).WithTriviaFrom(node),
                SyntaxKind.ContinueKeyword => ((ContinueStatementSyntax)node).WithContinueKeyword(newKeywordToken).WithTriviaFrom(node),
                SyntaxKind.SwitchKeyword => ((SwitchStatementSyntax)node).WithSwitchKeyword(newKeywordToken).WithTriviaFrom(node),
                SyntaxKind.ClassKeyword => ((ClassDeclarationSyntax)node).WithKeyword(newKeywordToken).WithTriviaFrom(node),
                _ => node
            };
        }
    }
}
