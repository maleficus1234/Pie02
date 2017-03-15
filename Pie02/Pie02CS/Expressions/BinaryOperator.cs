using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony.Parsing;

namespace Pie02CS.Expressions
{
    class BinaryOperator
        : Expression
    {
        public Expression Left;
        public Expression Right;
        public string OperatorType = "";

        public BinaryOperator(Expression parentExpression)
            : base(parentExpression)
        {
            Left = new Expression(this);
            Right = new Expression(this);
        }

        public static void Build(IronyParser parser, Expression parentExpression, ParseTreeNode node)
        {
            var c = new BinaryOperator(parentExpression);
            parentExpression.Children.Add(c);
            parser.ConsumeParseTree(c.Left, node.ChildNodes[0]);
            c.OperatorType = node.ChildNodes[1].FindTokenAndGetText();
            parser.ConsumeParseTree(c.Right, node.ChildNodes[2]);
        }

        public override void Emit(ref string source)
        {

                Left.Emit(ref source);
                source += " " + OperatorType + " ";
                Right.Emit(ref source);

        }

        public override void Validate()
        {
            Left.Validate();
            Right.Validate();
        }
    }
}
