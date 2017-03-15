using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace Pie02CS.Expressions
{
    class CaseBlock
        : Expression
    {
        Expression Expression;
        bool First = false;
        bool Default = false;

        public CaseBlock(Expression parent)
            : base(parent)
        {
            Expression = new Expression(this);
        }

        public static void Build(IronyParser parser, Expression parentExpression, ParseTreeNode node)
        {
            var c = new CaseBlock(parentExpression);
            parentExpression.Children.Add(c);

            parser.ConsumeParseTree(c.Expression, node.ChildNodes[1]);

            if(node.ChildNodes[0].Term.ToString() == "else")
                foreach (var child in node.ChildNodes[1].ChildNodes)
                    parser.ConsumeParseTree(c, child);
            else
                foreach (var child in node.ChildNodes[2].ChildNodes)
                    parser.ConsumeParseTree(c, child);

            if (parentExpression.Children.Count == 1)
                c.First = true;
            if (node.ChildNodes[0].Term.ToString() == "else")
                c.Default = true;
        }

        public override void Emit(ref string source)
        {
            if (First)
            {
                source += "if(";
                (ParentExpression as SwitchBlock).Expression.Emit(ref source);
                source += "==";

                Expression.Emit(ref source);
                source += ")\n";
            }
            else
            {
                if (Default)
                    source += "else\n";
                else
                {
                    source += "else if(";
                    (ParentExpression as SwitchBlock).Expression.Emit(ref source);
                    source += "==";

                    Expression.Emit(ref source);
                    source += ")\n";
                }
            }

            
            source += "{\n";
            foreach (var child in Children)
                child.Emit(ref source);
            source += "}\n";
        }

        public override void Validate()
        {
            ScopeStack.Push(Scope);
            Expression.Validate();
            base.Validate();
            ScopeStack.Pop(Scope);
        }
    }
}
