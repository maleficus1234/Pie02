using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace Pie02CS.Expressions
{
    public class Statement
        : Expression
    {
        public Statement(Expression parent)
            : base(parent)
        {
        }

        public static void Build(IronyParser parser, Expression parentExpression, ParseTreeNode node)
        {
            var s = new Statement(parentExpression);
            parentExpression.Children.Add(s);
            parser.ConsumeParseTree(s, node.ChildNodes[0]);
        }

        public override void Validate()
        {
            //Scope = ParentExpression.Scope.Copy();
            base.Validate();
        }

        public override void Emit(ref string source)
        {
            var v = this;
            base.Emit(ref source);
            source += ";\n";
        }


    }
}
