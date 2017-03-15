using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony.Parsing;

namespace Pie02CS.Rules
{
    public class LiteralRules
    {
        public StringLiteral StringLiteral;
        public NumberLiteral NumberLiteral;
        public NonTerminal Literal;

        public LiteralRules(PieGrammar grammar)
        {
            StringLiteral = new StringLiteral("string_literal", "\"", StringOptions.AllowsAllEscapes | StringOptions.AllowsDoubledQuote);
            StringLiteral.AddPrefix("@", StringOptions.AllowsLineBreak | StringOptions.AllowsAllEscapes | StringOptions.AllowsDoubledQuote);
            this.Literal = new NonTerminal("literal");
            this.Literal.Rule = this.StringLiteral;
            //grammar.MarkTransient(this.Literal);

            NumberLiteral = new NumberLiteral("number_literal", NumberOptions.AllowStartEndDot)
            {
                // Define types to assign if the parser can't determine.
                DefaultIntTypes = new[] { TypeCode.Int32 },
                DefaultFloatType = TypeCode.Double
            };

            // The pre and post fixes for different number types and formats.
            NumberLiteral.AddPrefix("0x", NumberOptions.Hex);
            NumberLiteral.AddSuffix("b", TypeCode.Byte);
            NumberLiteral.AddSuffix("sb", TypeCode.SByte);
            NumberLiteral.AddSuffix("s", TypeCode.Int16);
            NumberLiteral.AddSuffix("ui", TypeCode.UInt32);
            NumberLiteral.AddSuffix("i", TypeCode.Int32);
            NumberLiteral.AddSuffix("us", TypeCode.UInt16);
            NumberLiteral.AddSuffix("L", TypeCode.Int64);
            NumberLiteral.AddSuffix("ul", TypeCode.UInt64);
            NumberLiteral.AddSuffix("f", TypeCode.Single);
            NumberLiteral.AddSuffix("d", TypeCode.Double);
            NumberLiteral.AddSuffix("m", TypeCode.Decimal);

            this.Literal.Rule |= NumberLiteral;
        }

    }
}
