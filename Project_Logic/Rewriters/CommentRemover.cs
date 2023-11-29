using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Logic.Rewriters
{
    public class CommentRemover : CSharpSyntaxRewriter
    {
        public SyntaxNode RemoveComments(CompilationUnitSyntax node)
        {
            return Visit(node);
        }

        public override SyntaxTrivia VisitTrivia(SyntaxTrivia trivia)
        {
            if (trivia.IsKind(SyntaxKind.SingleLineCommentTrivia) || trivia.IsKind(SyntaxKind.MultiLineCommentTrivia))
            {
                return default(SyntaxTrivia);
            }

            return base.VisitTrivia(trivia);
        }
    }
}
