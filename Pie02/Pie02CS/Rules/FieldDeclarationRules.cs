using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony.Parsing;

namespace Pie02CS.Rules
{
    public class FieldDeclarationRules
    {
        public NonTerminal FieldDeclaration;

        public FieldDeclarationRules(PieGrammar grammar)
        {
            FieldDeclaration = new NonTerminal("field_declaration");
            FieldDeclaration.Rule = grammar.ModifierList + grammar.Identifier + grammar.Eos;
            FieldDeclaration.Rule |= grammar.ModifierList + grammar.Identifier + "=" + grammar.Expression + grammar.Eos;
        }
    }
}
