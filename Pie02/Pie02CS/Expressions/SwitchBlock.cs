using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace Pie02CS.Expressions
{
    class SwitchBlock
        : Expression
    {
        public Expression Expression;

        public SwitchBlock(Expression expression)
            : base(expression)
        {
            Expression = new Expression(this);
        }

        public static void Build(IronyParser parser, Expression parentExpression, ParseTreeNode node)
        {
            var sw = new SwitchBlock(parentExpression);
            parentExpression.Children.Add(sw);
            parser.ConsumeParseTree(sw.Expression, node.ChildNodes[1]);
            foreach(var child in node.ChildNodes[2].ChildNodes[0].ChildNodes)
            {
                parser.ConsumeParseTree(sw, child);
            }
        }

        public override void Validate()
        {
            ScopeStack.Push(Scope);
            Expression.Validate();
            base.Validate();
            ScopeStack.Pop(Scope);
        }

        public override void Emit(ref string source)
        {
            foreach (var child in Children)
                child.Emit(ref source);
        }
    }
}
