using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony.Parsing;

namespace Pie02CS.Expressions
{
    class DirectionedIdentifier
        : Expression
    {
        public string Direction;

        public DirectionedIdentifier(Expression parent)
            : base(parent)
        {

        }

        public static void Build(IronyParser parser, Expression parentExpression, ParseTreeNode node)
        {
            var d = new DirectionedIdentifier(parentExpression);
            parentExpression.Children.Add(d);
            d.Direction = node.ChildNodes[0].FindTokenAndGetText();
            parser.ConsumeParseTree(d, node.ChildNodes[1]);
        }

        public override void Emit(ref string source)
        {
            source += " " + Direction + " ";
            base.Emit(ref source);
        }
    }
}
