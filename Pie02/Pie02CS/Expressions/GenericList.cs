using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony.Parsing;

namespace Pie02CS.Expressions
{
    class GenericList
        : Expression
    {
        List<string> Names = new List<string>();

        public GenericList(Expression parentExpression)
            : base(parentExpression)
        {

        }

        public static void Build(IronyParser parser, Expression parentExpression, ParseTreeNode node)
        {
            var g = new GenericList(parentExpression);
            parentExpression.Children.Add(g);
            foreach (var n in node.ChildNodes[1].ChildNodes)
                g.Names.Add(n.FindTokenAndGetText());
        }

        public override void Emit(ref string source)
        {
            source += "<";
            if(Names.Count > 0)
            {
                source += Names[0];
                for (int i = 1; i < Names.Count; i++)
                    source += "," + Names[i];
            }
            source += ">";
        }


        /*
        def __init__(self, parentExpression):
        super(self.__class__, self).__init__(parentExpression)
        self.Names = []

        @staticmethod
    def Build(parser, parent, node):
        g = GenericList(parent)
        parent.Children.append(g)
        for n in node.ChildNodes[1].ChildNodes:
            g.Names.append(n.FindTokenAndGetText())

    def Emit(self, source):
        source += "<"
        if len(self.Names) > 0:
            source += self.Names[0]
            for n in range(1, len(self.Names)):
                source += "," + self.Names[n]
# for n in self.Names:
# source += n + ","
        source += ">"
        return source*/
    }
}
