using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony.Parsing;

namespace Pie02CS.Expressions
{
    public class TestDeclaration
        : Expression
    {
        public TestDeclaration(Expression parent)
            : base(parent)
        {

        }

        public static void Build(IronyParser parser, Expression parentExpression, ParseTreeNode node)
        {
            var t = new TestDeclaration(parentExpression);
            parentExpression.Children.Add(t);

            t.Name = node.ChildNodes[1].FindTokenAndGetText();
            parser.ConsumeParseTree(t, node.ChildNodes[2]);
        }

        public override void Emit(ref string source)
        {
            source += "public static void PieTestzkyz_" + Name + "()\n";
            source += "{\n";
            foreach (var child in Children)
                child.Emit(ref source);
            source += "}\n";
        }

        public override void Validate()
        {
            ScopeStack.Push(Scope);
            base.Validate();
            ScopeStack.Pop(Scope);
        }
    }
}
