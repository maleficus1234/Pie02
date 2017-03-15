using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony.Parsing;

namespace Pie02CS.Expressions
{
    class Parameter
        : Expression
    {
        public string Direction = "";
        public string Identifier = "";
        public string AsCast = "dynamic";

        public Parameter(Expression parentExpression)
            : base(parentExpression)
        {

        }

        public static void Build(IronyParser parser, Expression parentExpression, ParseTreeNode node)
        {
            var p = new Parameter(parentExpression);
            parentExpression.Children.Add(p);
            if (node.ChildNodes[0].ChildNodes.Count > 0)
                p.Direction = node.ChildNodes[0].ChildNodes[0].Term.ToString();
            p.Identifier = node.ChildNodes[1].FindTokenAndGetText();
            if (p.Identifier == "args") p.AsCast = " System.String [] ";
            if (node.ChildNodes[2].ChildNodes.Count > 0)
                p.AsCast = node.ChildNodes[2].ChildNodes[1].FindTokenAndGetText();
        }

        public override void Emit(ref string source)
        {
            source += Direction + " " + AsCast + " ";
            source += Identifier;
        }
    }
}
