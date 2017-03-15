using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony.Parsing;

namespace Pie02CS.Expressions
{
    public class AssertStatement
        : Expression
    {
        public AssertStatement(Expression parent)
            : base(parent)
        {

        }

        public static void Build(IronyParser parser, Expression parentExpression, ParseTreeNode node)
        {
            var a = new AssertStatement(parentExpression);
            parentExpression.Children.Add(a);

            parser.ConsumeParseTree(a, node.ChildNodes[1]);
        }

        public override void Emit(ref string source)
        {
            source += "PieTestzkyz_Assert.Assert(";
            Children[0].Emit(ref source);
            source += ");";
        }
    }
}
