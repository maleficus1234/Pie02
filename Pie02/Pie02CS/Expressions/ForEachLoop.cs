using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony.Parsing;

namespace Pie02CS.Expressions
{
    class ForEachLoop
        : Expression
    {
        public bool createVar = false;
        public Expression Init;
        public Expression Condition;

        public ForEachLoop(Expression parent)
            : base(parent)
        {
            Init = new Expression(this);
            Condition = new Expression(this);
        }

        public static void Build(IronyParser parser, Expression parentExpression, ParseTreeNode node)
        {
            var f = new ForEachLoop(parentExpression);
            parentExpression.Children.Add(f);
            parser.ConsumeParseTree(f.Init, node.ChildNodes[1]);
            parser.ConsumeParseTree(f.Condition, node.ChildNodes[3]);

            parser.ConsumeParseTree(f, node.ChildNodes[4]);
        }

        public override void PropagateImport(QualifiedImport i)
        {
            ScopeStack.Push(Scope);
            Init.PropagateImport(i);
            Condition.PropagateImport(i);
            base.PropagateImport(i);
            ScopeStack.Pop(Scope);
        }

        public override void Validate()
        {
            var q = Init.Children[0] as QualifiedIdentifier;
            Init.Validate();
            Condition.Validate();
            base.Validate();
        }

        public override void Emit(ref string source)
        {
            source += "foreach(";
         //   if (createVar)
                source += " dynamic ";
            Init.Emit(ref source);
            source += " in ";
            Condition.Emit(ref source);
            source += ") {\n";

            base.Emit(ref source);

            source += "}\n";
        }
    }
}
