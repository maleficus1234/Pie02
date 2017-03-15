using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony.Parsing;

namespace Pie02CS.Expressions
{
    class AccessedExpression
        : Expression
    {
        public bool IsType = false;

        public AccessedExpression(Expression parent)
            : base(parent)
        {

        }

        public static void Build(IronyParser parser, Expression parentExpression, ParseTreeNode node)
        {
            var a = new AccessedExpression(parentExpression);
            parentExpression.Children.Add(a);
            parser.ConsumeParseTree(a, node.ChildNodes[0]);
            parser.ConsumeParseTree(a, node.ChildNodes[2]);
        }

        public override void Validate()
        {
            if (Children[0].Name == "base")
                PropagateIsBaseCall();
            else
            {

            }
            base.Validate();
        }

        public override void Emit(ref string source)
        {
            if (!(ParentExpression is AccessedExpression))
                if (IsType)
                    source += " new ";

            Children[0].Emit(ref source);

                source += ".";
                Children[1].Emit(ref source);

        }

        public override string GetFullName()
        {
            return base.GetFullName();
        }

        public void PropagateIsType()
        {
            IsType = true;
            if (ParentExpression is AccessedExpression)
                (ParentExpression as AccessedExpression).PropagateIsType();
        }
    }
}