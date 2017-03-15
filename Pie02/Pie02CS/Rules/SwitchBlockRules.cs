using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony.Parsing;

namespace Pie02CS.Rules
{
    public class SwitchBlockRules
    {
        public NonTerminal SwitchBlock;
        public NonTerminal CaseBlock;
        public NonTerminal CaseBlockList;
        public NonTerminal SwitchBody;
        public SwitchBlockRules(PieGrammar grammar)
        {
            CaseBlock = new NonTerminal("case_block");
            CaseBlock.Rule = grammar.ToTerm("case") + grammar.Expression + ":" + grammar.Eos + grammar.MethodBody;
            CaseBlock.Rule |= grammar.ToTerm("else") + ":" + grammar.Eos + grammar.MethodBody;

            CaseBlockList = new NonTerminal("case_block_list");
            CaseBlockList.Rule = grammar.MakePlusRule(CaseBlockList, CaseBlock);

            SwitchBody = new NonTerminal("switch_body");
            SwitchBody.Rule = grammar.Indent + CaseBlockList + grammar.Dedent;

            SwitchBlock = new NonTerminal("switch_block");
            SwitchBlock.Rule = grammar.ToTerm("switch") + grammar.Expression + ":" + grammar.Eos + SwitchBody;


        }
    }
}
