using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony.Parsing;

namespace Pie02CS.Expressions
{
    public class AnonymousMethod
        : Expression
    {
        bool hasReturn = false;
        Expression Parameters;

        public AnonymousMethod(Expression parent)
            : base(parent)
        {
            Parameters = new Expression(parent);
        }

        public static void Build(IronyParser parser, Expression parentExpression, ParseTreeNode node)
        {
            var a = new AnonymousMethod(parentExpression);
            parentExpression.Children.Add(a);
            if (node.ChildNodes[0].FindTokenAndGetText() == "func")
                a.hasReturn = true;
            parser.ConsumeParseTree(a.Parameters, node.ChildNodes[1]);

            parser.ConsumeParseTree(a, node.ChildNodes[3]);
        }


        // var z = (Func<dynamic, dynamic>)((x) => x* x);
        // Console.WriteLine(z(9));

        public override void Emit(ref string source)
        {

            source += "(";
            if (hasReturn)
            {
                source += "System.Func<dynamic";
                if (Parameters.Children.Count > 0)
                    source += ", ";
            }
            else
            {
                source += "System.Action";
                if (Parameters.Children.Count > 0)
                    source += "<";
            }

            if (Parameters.Children.Count > 0)
                source += "dynamic";
            for (int i = 1; i < Parameters.Children.Count; i++)
                source += ", dynamic";

            if (hasReturn || (!hasReturn && Parameters.Children.Count > 0))
                source += ">";

            source += ")";
            source += "((";
            if (Parameters.Children.Count > 0)
                Parameters.Children[0].Emit(ref source);
            for (int i = 1; i < Parameters.Children.Count; i++)
            {
                source += ",";
                Parameters.Children[i].Emit(ref source);
            }
            source += ") => ";


           // if((!hasReturn))
           //     source += "{";
            if(Children.Count == 1)
            {
                Children[0].Emit(ref source);
            }

            //if ((!hasReturn))
           //     source += "}";
            source += ")";
        }

        public override void Validate()
        {
            //this.Scope = ParentExpression.Scope.Copy();
            foreach (var c in Children)
                c.Validate();
        }
    }
}
