using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony.Parsing;

namespace Pie02CS.Expressions
{
    public class Identifier
        : Expression
    {
        public Identifier(Expression parent)
            : base(parent)
        {

        }

        public static void Build(IronyParser parser, Expression parentExpression, ParseTreeNode node)
        {
            var i = new Identifier(parentExpression);
            parentExpression.Children.Add(i);
            i.Name = node.FindTokenAndGetText();
            if (i.Name == "this") i.Name = "(this as dynamic)";
        }

        public override void Emit(ref string source)
        {
            source += this.Name;
        }
    }
}
