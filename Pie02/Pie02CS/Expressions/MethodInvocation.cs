using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony.Parsing;

namespace Pie02CS.Expressions
{
    class MethodInvocation
        : Expression
    {
        Expression Arguments;
        public bool IsType;

        public MethodInvocation(Expression parentExpression)
            : base(parentExpression)
        {
            Arguments = new Expression(this);
        }

        public static void Build(IronyParser parser, Expression parentExpression, ParseTreeNode node)
        {
            var m = new MethodInvocation(parentExpression);
            parentExpression.Children.Add(m);

            m.Name = node.ChildNodes[0].FindTokenAndGetText();
            foreach( var n in node.ChildNodes[1].ChildNodes)
                parser.ConsumeParseTree(m.Arguments, n);
        }

        public override void Emit(ref string source)
        {
            if (IsType && !(ParentExpression is AccessedExpression))
                source += " new ";
            source += Name;
            source += "(";
            if(Arguments.Children.Count > 0)
            {
                Arguments.Children[0].Emit(ref source);
                if(Name == "base" || IsBaseCall)
                    source += " as object ";
                for (int i = 1; i < Arguments.Children.Count; i++)
                {
                    source += ", ";
                    Arguments.Children[i].Emit(ref source);
                    if (Name == "base" || IsBaseCall)
                        source += " as object ";
                }
            }
            source += ")";
        }

        public override void Validate()
        {
            string n = this.Name;
            if (ScopeStack.TypeInScope(n))
            {
                IsType = true;
            }

            switch(Name)
            {
                case "list":
                    this.Name = "System.Collections.Generic.List<dynamic>";
                    IsType = true;
                    break;
                case "map":
                    this.Name = "System.Collections.Generic.Dictionary<dynamic, dynamic>";
                    IsType = true;
                    break;
                case "queue":
                    this.Name = "System.Collections.Generic.Queue<dynamic>";
                    IsType = true;
                    break;
                case "stack":
                    this.Name = "System.Collections.Generic.Stack<queue>";
                    IsType = true;
                    break;
            }

            foreach (var e in Arguments.Children)
                e.Validate();

            base.Validate();
        }

        public override void PropagateImport(QualifiedImport i)
        {
            foreach (var e in Arguments.Children)
                e.PropagateImport(i);
            base.PropagateImport(i);
        }
    }
}