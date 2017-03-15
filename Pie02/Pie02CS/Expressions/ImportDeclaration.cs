using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace Pie02CS.Expressions
{
    public class ImportDeclaration
        : Expression
    {
        public Expression QualifiedIdentifiers;


        public ImportDeclaration(Expression parentExpression)
            : base(parentExpression)
        {
            QualifiedIdentifiers = new Expression(this);
        }

        public static void Build(IronyParser parser, Expression parent, ParseTreeNode node)
        {
            var i = new ImportDeclaration(parent);
            parent.Children.Add(i);

            foreach(var n in node.ChildNodes[1].ChildNodes)
            {
                parser.ConsumeParseTree(i.QualifiedIdentifiers, n);
            }
        }

        public override void Emit(ref string source)
        {
            foreach (var c in QualifiedIdentifiers.Children)
            {



                c.Emit(ref source);

            }
        }

        public override void Validate()
        {
            QualifiedIdentifiers.Validate();
            base.Validate();
        }
    }
}
