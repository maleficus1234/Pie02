using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony.Parsing;

namespace Pie02CS.Expressions
{
    class IndexedIdentifier
        : Expression
    {
        //Expression Identifier;

        public IndexedIdentifier(Expression parent)
            : base(parent)
        {
           // Identifier = new Expression(this);
        }

        public static void Build(IronyParser parser, Expression parent, ParseTreeNode node)
        {
            var i = new IndexedIdentifier(parent);
            parent.Children.Add(i);

            //parser.ConsumeParseTree(i.Identifier, node.ChildNodes[0]);
            i.Name = node.ChildNodes[0].FindTokenAndGetText();
            foreach(var n in node.ChildNodes[1].ChildNodes[1].ChildNodes)
                parser.ConsumeParseTree(i, n);

        }

        public override void Emit(ref string source)
        {
            source += Name;
            source += "[";
            if(Children.Count > 0)
            {
                Children[0].Emit(ref source);
                for(int i = 1; i < Children.Count; i++)
                {
                    source += ",";
                    Children[i].Emit(ref source);
                }
            }
            source += "]";
        }
    }
}
