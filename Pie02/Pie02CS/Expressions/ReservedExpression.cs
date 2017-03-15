using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony.Parsing;

namespace Pie02CS.Expressions
{
    class ReservedExpression
        : Expression
    {
        public ReservedExpression(Expression parentExpression)
            : base(parentExpression)
        {

        }

        public static void Build(IronyParser parser, Expression parentExpression, ParseTreeNode node)
        {
            var r = new ReservedExpression(parentExpression);
            parentExpression.Children.Add(r);

            r.Name = node.ChildNodes[0].FindTokenAndGetText();
        }

        public override void Emit(ref string source)
        {
            if (Name == "this")
                source += "(" + Name + " as dynamic)";
            else
                source += " " + Name + " ";
        }
    }
}
