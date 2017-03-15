using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony.Parsing;

namespace Pie02CS.Expressions
{
    class Literal
        : Expression
    {
        object Value;
        bool AllowLineBreaks = false;

        public Literal(Expression parentExpression)
            : base(parentExpression)
        {

        }

        public static void Build(IronyParser parser, Expression parent, ParseTreeNode node)
        {
            var l = new Literal(parent);
            parent.Children.Add(l);
            l.Value = node.Token.Value;
            if(node.Token.Details != null)
                if (((Irony.Parsing.CompoundTerminalBase.CompoundTokenDetails)node.Token.Details).Prefix == "@")
                    l.AllowLineBreaks = true;
        }

        public override void Emit(ref string source)
        {

            source += " ";
            if (Value is string)
            {
                if (AllowLineBreaks)
                    source += "@";
                source += '"';
            }
            if (Value is byte)
                source += "(byte)";
            if (Value is sbyte)
                source += "(sbyte)";
            if (Value is short)
                source += "(short)";
            if (Value is int)
                source += "(int)";
            if (Value is long)
                source += "(long)";
            if (Value is ushort)
                source += "(ushort)";
            if (Value is uint)
                source += "(uint)";
            if (Value is ulong)
                source += "(ulong)";
            if (Value is float)
                source += "(float)";
            if (Value is double)
                source += "(double)";
            if (Value is decimal)
                source += "(decimal)";
            
            source += Value;

            if (Value is string)
                source += '"';
        }
    }
}
