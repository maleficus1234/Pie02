using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony.Parsing;

namespace Pie02CS.Expressions
{
    class FieldDeclaration
        : Expression
    {
        List<string> Modifiers = new List<string>();

        public FieldDeclaration(Expression parentExpression)
            : base(parentExpression)
        {

        }

        public static void Build(IronyParser parser, Expression parentExpression, ParseTreeNode node)
        {
            var f = new FieldDeclaration(parentExpression);
            parentExpression.Children.Add(f);

            foreach (var modifier in node.ChildNodes[0].ChildNodes)
                f.Modifiers.Add(modifier.FindTokenAndGetText());

            f.Name = node.ChildNodes[1].FindTokenAndGetText();

            if(node.ChildNodes.Count > 2)
            {
                parser.ConsumeParseTree(f, node.ChildNodes[3]);
            }

        }

        public override void Validate()
        {

            for (int i = 0; i < Modifiers.Count; i++)
            {
                switch (Modifiers[i])
                {
                    case "final":
                        Modifiers[i] = "sealed";
                        break;
                    case "shared":
                        Modifiers[i] = "static";
                        break;
                }
            }
            if (!Modifiers.Contains("internal") && !Modifiers.Contains("public") && !Modifiers.Contains("private") && !Modifiers.Contains("protected")) Modifiers.Add("private");

            base.Validate();
        }

        public override void Emit(ref string source)
        {
            foreach (var m in Modifiers)
            {
                source += m + " ";
            }

            source += " dynamic " + Name;
                
            if(Children.Count > 0)
            {
                source += " = ";
                Children[0].Emit(ref source);
            }
                
                source += ";\n";
        }
    }
}
