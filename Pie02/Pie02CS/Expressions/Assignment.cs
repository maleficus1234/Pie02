using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony.Parsing;

namespace Pie02CS.Expressions
{
    class Assignment
        : Expression
    {
        Expression Left;
        Expression Right;
        public bool Init = false;
        string AssignmentType = "";

        public Assignment(Expression parentExpression)
            : base(parentExpression)
        {
            Left = new Expression(this);
            Right = new Expression(this);
        }

        public static void Build(IronyParser parser, Expression parentExpression, ParseTreeNode node)
        {
            var c = new Assignment(parentExpression);
            parentExpression.Children.Add(c);
            parser.ConsumeParseTree(c.Left, node.ChildNodes[0]);
            c.AssignmentType = node.ChildNodes[1].FindTokenAndGetText();
            parser.ConsumeParseTree(c.Right, node.ChildNodes[2]);
        }

        public override void Emit(ref string source)
        {
            if (Init)
                source += "dynamic ";
            Left.Emit(ref source);
            source += AssignmentType;
            Right.Emit(ref source);
           // source += "\n";
        }

        public override void Validate()
        {

            var q = Left.Children[0] as Identifier;
            if (q != null)
            {
                if(!ScopeStack.VariableInScope(q.Name))
                {
                    Init = true;
                    var v = new Variable();
                    v.FullName = q.Name;
                    //Scope.Variables.Add(v);
                    ScopeStack.Scopes.Last().Variables.Add(v);
                }
            }

            Right.Validate();
            Left.Validate();
            base.Validate();
        }

        public override void PropagateImport(QualifiedImport i)
        {
            Left.PropagateImport(i);
            Right.PropagateImport(i);
            base.PropagateImport(i);
        }
    }
}
