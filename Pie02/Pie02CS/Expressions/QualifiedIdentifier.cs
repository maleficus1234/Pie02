using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony.Parsing;

namespace Pie02CS.Expressions
{
    class QualifiedIdentifier
        : Expression
    {
        //public List<string> Identifiers = new List<string>();
        public QualifiedIdentifier(Expression parentExpression)
            : base(parentExpression)
        {

        }

        public static void Build(IronyParser parser, Expression parentExpression, ParseTreeNode node)
        {
            var i = new QualifiedIdentifier(parentExpression);
            parentExpression.Children.Add(i);
            foreach (var n in node.ChildNodes)
                parser.ConsumeParseTree(i, n);
               // i.Identifiers.Add(n.FindTokenAndGetText());
        }

        public override void Validate()
        {
         /*   foreach(var c in Children)
            {
                var i = c as Identifier;
                if(i.Name == )
            }
           /* for(int i = 0; i < Identifiers.Count; i++)
            {
                if (Identifiers[i] == "this")
                    Identifiers[i] = "(this as dynamic)";
            }*/
        }

        public override void Emit(ref string source)
        {
            source += Children[0].Name;
            for (int i = 1; i < Children.Count; i++)
                source += "." + Children[i].Name;
        }

        public override string GetFullName()
        {
            var name = Children[0].Name;
            for (int i = 1; i < Children.Count; i++)
            {
                name += "." + Children[i].Name;
            }
            return name;
        }


    }
}
