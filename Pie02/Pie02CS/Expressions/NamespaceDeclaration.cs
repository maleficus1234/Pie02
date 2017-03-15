using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony.Parsing;

namespace Pie02CS.Expressions
{
    class NamespaceDeclaration
        :Expression
    {
        public NamespaceDeclaration(Expression parentExpression)
            : base(parentExpression)
        {

        }

        public static void Build(IronyParser parser, Expression parentExpresion, ParseTreeNode node)
        {
            var lastEx = parentExpresion;
            foreach(var name in node.ChildNodes[1].ChildNodes)
            {
                var n = new NamespaceDeclaration(lastEx);
                n.Name = name.FindTokenAndGetText();
                lastEx.Children.Add(n);
                lastEx = n;
            }
            parser.ConsumeParseTree(lastEx, node.ChildNodes[2]);
        }

        public override void Emit(ref string source)
        {
            source += " namespace ";
            source += Name;
            source += "\n{\n";

            base.Emit(ref source);

            source += "}\n";
        
        }

        public override List<dynamic> Match(Expression ex)
        {
            if (ex is NamespaceDeclaration)
                if (ex.GetFullName() == GetFullName())
                    return new List<dynamic>() { this };
            return base.Match(ex);
        }

        public override string GetFullName()
        {
            if (ParentExpression == null)
                return Name;
            else
            {
                if (ParentExpression is NamespaceDeclaration)
                {
                    return ParentExpression.GetFullName() + "." + Name;
                }
                else return Name;
            }
        }

        public override void Validate()
        {


            base.Validate();
        }
    }
}
