using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony.Parsing;

namespace Pie02CS.Expressions
{
    public class UnaryOperation
        : Expression
    {
        public string OperatorType = "";
        public bool Pre = false;

        public UnaryOperation(Expression parent)
            : base(parent)
        {

        }

        public static void Build(IronyParser parser, Expression parentExpression, ParseTreeNode node)
        {
            var u = new UnaryOperation(parentExpression);
            parentExpression.Children.Add(u);

            if(node.Term.ToString() == "pre_unary_operation")
            {
                u.Pre = true;
                u.OperatorType = node.ChildNodes[0].FindTokenAndGetText();
                parser.ConsumeParseTree(u, node.ChildNodes[1]);
            }
            else
            {
                u.Pre = false;
                u.OperatorType = node.ChildNodes[1].FindTokenAndGetText();
                parser.ConsumeParseTree(u, node.ChildNodes[0]);
            }
        }

        public override void Emit(ref string source)
        {
            if (Pre)
                source += OperatorType;

            Children[0].Emit(ref source);

            if (!Pre)
                source += OperatorType;
        }
    }
}
