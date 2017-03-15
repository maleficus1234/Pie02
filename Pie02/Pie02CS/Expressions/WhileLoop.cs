using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony.Parsing;

namespace Pie02CS.Expressions
{
    class WhileLoop
        : Expression
    {
        public Expression Expression;

        public WhileLoop(Expression parent)
            : base(parent)
        {
            Expression = new Expression(parent);
        }

        public static void Build(IronyParser parser, Expression parentExpression, ParseTreeNode node)
        {
            var w = new WhileLoop(parentExpression);
            parentExpression.Children.Add(w);

            parser.ConsumeParseTree(w.Expression, node.ChildNodes[1]);

           // if(node.ChildNodes.Count == 3)
                parser.ConsumeParseTree(w, node.ChildNodes[2]);
          //  else
           //     parser.ConsumeParseTree(w, node.ChildNodes)
        }

        public override void Emit(ref string source)
        {
            source += "while(";
            Expression.Emit(ref source);
            source += ") {\n";
            base.Emit(ref source);
            source += "}\n";
        }
    }
}
