using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony.Parsing;

namespace Pie02CS.Expressions
{
    class IfConditional
        : Expression
    {
        public Expression Conditional;
        public Expression IfBlock;
        public Expression ElseBlock;

        public IfConditional(Expression parent)
            : base(parent)
        {
            Conditional = new Expression(this);
            IfBlock = new Expression(this);
            ElseBlock = new Expression(this);
        }

        public static void Build(IronyParser parser, Expression parentExpression, ParseTreeNode node)
        {
            var i = new IfConditional(parentExpression);
            parentExpression.Children.Add(i);


            parser.ConsumeParseTree(i.Conditional, node.ChildNodes[1]);

            parser.ConsumeParseTree(i.IfBlock, node.ChildNodes[2]);

            if(node.ChildNodes.Count > 3)
                parser.ConsumeParseTree(i.ElseBlock, node.ChildNodes[4]);
        }

        public override void Validate()
        {
            ScopeStack.Push(Scope);
            Conditional.Validate();
            IfBlock.Validate();
            ScopeStack.Pop(Scope);
            ScopeStack.Push(ElseBlock.Scope);
            ElseBlock.Validate();
            ScopeStack.Pop(ElseBlock.Scope);
            //base.Validate();
            
        }

        public override void PropagateImport(QualifiedImport i)
        {
            Conditional.PropagateImport(i);
            IfBlock.PropagateImport(i);
            ElseBlock.PropagateImport(i);
            base.PropagateImport(i);
        }

        public override void Emit(ref string source)
        {
            source += " if(";
            Conditional.Emit(ref source);
            source += ")\n";
            source += "{\n";
            IfBlock.Emit(ref source);
            source += "}\n";

            if(ElseBlock.Children.Count > 0)
            {
                source += "else\n";
                source += "{\n";
                ElseBlock.Emit(ref source);
                source += "}\n";
            }
        }

      /*  public override void PropagateVariable(Variable variable)
        {
            IfBlock.PropagateVariable(variable);
            ElseBlock.PropagateVariable(variable);
        }*/
    }
}
