using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony.Parsing;

namespace Pie02CS.Expressions
{
    class ParenedExpression
        : Expression
    {
        public ParenedExpression(Expression parent)
            : base(parent)
        {

        }

        public static void Build(IronyParser parser, Expression parentExpression, ParseTreeNode node)
        {
            var p = new ParenedExpression(parentExpression);
            parentExpression.Children.Add(p);
            parser.ConsumeParseTree(p, node.ChildNodes[0]);
        }

        public override void Emit(ref string source)
        {
            source += "(";
            Children[0].Emit(ref source);
            source += ")";
        }
    }
}
