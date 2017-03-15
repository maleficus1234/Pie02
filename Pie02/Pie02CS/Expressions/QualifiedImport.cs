using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using Irony.Parsing;

namespace Pie02CS.Expressions
{
    public class QualifiedImport
        : Expression
    {
        List<string> Identifiers = new List<string>();
        public bool IsType = false;

        public QualifiedImport(Expression parentExpression)
            : base(parentExpression)
        {

        }

        public static void Build(IronyParser parser, Expression parentExpression, ParseTreeNode node)
        {
            var i = new QualifiedImport(parentExpression);
            parentExpression.Children.Add(i);
            foreach (var childNode in node.ChildNodes[0].ChildNodes)
                i.Identifiers.Add(childNode.FindTokenAndGetText());
        }

        public override void Emit(ref string source)
        {
            source += " using ";
            if (IsType)
                source += " static ";
            source += Identifiers[0];
            for(int i = 1; i < Identifiers.Count; i++)
            {
                source += ".";
                source += Identifiers[i];
            }
            source += ";\n";
        }

        public override string GetFullName()
        {
            var name = Identifiers[0];
            for(int i = 1; i < Identifiers.Count; i++)
            {
                name += "." + Identifiers[i];
            }
            return name;
        }

        public override void Validate()
        {
            var n = GetFullName();
            Type t = Type.GetType(GetFullName());
            if(t != null)
            {
                IsType = true;
                return;
            }

            foreach(var at in ScopeOld.AllTypes)
            {
                if(at is ClassDeclaration)
                    if((at as ClassDeclaration).GetFullName() == GetFullName())
                    {
                        IsType = true;
                        return;
                    }
            }
        }

        public bool ImportsType(string name)
        {
            var n = GetFullName() + "." + name;
            // Type t = Type.GetType(n + ", System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
            if (FindType(n))
                return true;
            else
                return false;
           // if (t != null) return true;
           // return false;
        }

        public bool FindType(string fullName)
        {


            var t = Type.GetType(fullName);
            if (t != null)
                return true;



                foreach(var ass in PieCodeProvider.referencedAssembly)
                {
                    t = ass.GetType(fullName);
                    if (t != null) return true;
                }

            return false;
        }
    }
}
