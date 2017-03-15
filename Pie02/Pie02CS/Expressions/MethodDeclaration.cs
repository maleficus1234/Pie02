using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony.Parsing;

namespace Pie02CS.Expressions
{
    class MethodDeclaration
        : Expression
    {
        List<string> Modifiers = new List<string>();
        string MethodType = "";
        Expression Parameters;
        public bool Constructor = false;

        public MethodDeclaration(Expression parentExpression)
            : base(parentExpression)
        {
            Parameters = new Expression(this);
        }

        public static void Build(IronyParser parser, Expression parentExpression,ParseTreeNode node)
        {
            var m = new MethodDeclaration(parentExpression);
            parentExpression.Children.Add(m);

            foreach (var modifier in node.ChildNodes[0].ChildNodes)
                m.Modifiers.Add(modifier.FindTokenAndGetText());

            m.MethodType = node.ChildNodes[1].Term.ToString();

            if (m.MethodType != "new")
            {
                m.Name = node.ChildNodes[2].FindTokenAndGetText();

                parser.ConsumeParseTree(m.Parameters, node.ChildNodes[3]);

                parser.ConsumeParseTree(m, node.ChildNodes[4]);
            }
            else
            {
                m.Name = parentExpression.Name;
                parser.ConsumeParseTree(m.Parameters, node.ChildNodes[2]);

                parser.ConsumeParseTree(m, node.ChildNodes[3]);
            }


        }

        public override void Validate()
        {
            ScopeStack.Push(Scope);

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

            if(ParentExpression is ClassDeclaration)
            {
                var c = ParentExpression as ClassDeclaration;
                if (c.ClassType == "module" && !Modifiers.Contains("static"))
                    Modifiers.Add("static");
            }

            if (!Modifiers.Contains("internal") && !Modifiers.Contains("public") && !Modifiers.Contains("private")) Modifiers.Add("public");

            foreach(var c in Parameters.Children)
            {
                var p = c as Parameter;
                var v = new Variable();
                v.FullName = p.Identifier;
                //PropagateVariable(v);
                Scope.Variables.Add(v);
            }
            base.Validate();

            ScopeStack.Pop(Scope);
        }

        public override void Emit(ref string source)
        {
            foreach(var m in Modifiers)
            {
                source += m + " ";
            }

            if(MethodType != "new")
                if (MethodType == "act")
                    source += " void ";
                else
                    source += " dynamic ";

            source += Name;

            source += "(";

            if(Parameters.Children.Count > 0)
            {
                Parameters.Children[0].Emit(ref source);
                for(int i= 1; i < Parameters.Children.Count; i++)
                {
                    source += ", ";
                    Parameters.Children[i].Emit(ref source);
                }
            }

            source += ")";

            if(Children.Count > 0)
            {
                if(Children[0].Children[0] is MethodInvocation)
                {
                    if(Children[0].Children[0].Name == "base")
                    {
                        source += " : ";
                        Children[0].Children[0].Emit(ref source);
                    }
                }
            }

            source += "\n{\n";

            foreach(var c in Children)
            {
                if(c is Statement)
                {
                    var m = (c as Statement).Children[0];
                    if(m is MethodInvocation)
                    {
                        if (m.Name == "base") continue;
                    }
                }
               // else
              //  {
                    c.Emit(ref source);
                    source += ";\n";
               // }
            }

            source += "}\n";

        }
    }
}
