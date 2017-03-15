using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony.Parsing;

namespace Pie02CS.Expressions
{
    class ReturnStatement
        : Expression
    {
        public ReturnStatement(Expression parentExpression)
            : base(parentExpression)
        {

        }

        public static void Build(IronyParser parser, Expression parentExpression, ParseTreeNode node)
        {
            var r = new ReturnStatement(parentExpression);
            parentExpression.Children.Add(r);

            if (node.ChildNodes.Count > 1)
                parser.ConsumeParseTree(r, node.ChildNodes[1]);
        }

        public override void Emit(ref string source)
        {
            source += "return ";

            base.Emit(ref source);

            source += ";\n";
        }
    }
}