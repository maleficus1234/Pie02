using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace Pie02CS.Expressions
{
    class ForLoop
        : Expression
    {
        Expression Initializer;
        Expression Conditional;
        Expression Step;

        public ForLoop(Expression parent)
            : base(parent)
        {
            Initializer = new Expression(parent);
            Conditional = new Expression(parent);
            Step = new Expression(parent);
        }

        public static void Build(IronyParser parser, Expression parentExpression, ParseTreeNode node)
        {
            var f = new ForLoop(parentExpression);
            parentExpression.Children.Add(f);
            parser.ConsumeParseTree(f.Initializer, node.ChildNodes[1]);
            parser.ConsumeParseTree(f.Conditional, node.ChildNodes[2]);
            parser.ConsumeParseTree(f.Step, node.ChildNodes[3]);

            parser.ConsumeParseTree(f, node.ChildNodes[4]);
        }

        public override void Validate()
        {

            ScopeStack.Push(Scope);
            Initializer.Validate();
            Conditional.Validate();
            Step.Validate();
            base.Validate();
            ScopeStack.Pop(Scope);
        }

        public override void Emit(ref string source)
        {
            source += "for(";
            Initializer.Emit(ref source);
            Conditional.Emit(ref source);
            Step.Emit(ref source);
            source = source.Substring(0, source.Length - 2);
            source += ")\n";
            source += "{\n";
            foreach (var child in Children)
                child.Emit(ref source);
            source += "}\n";
        }
    }
}
