using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony.Parsing;

namespace Pie02CS.Rules
{
    public class OperatorRules
    {
        public NonTerminal AssignmentOperator;
        public NonTerminal Assignment;
        public NonTerminal BinaryOperators;
        public NonTerminal BinaryOperation;
        public NonTerminal PreUnaryOperator;
        public NonTerminal PostUnaryOperator;
        public NonTerminal UnaryOperation;
        public NonTerminal PreUnaryOperation;
        public NonTerminal PostUnaryOperation;

        public OperatorRules(PieGrammar grammar)
        {
            AssignmentOperator = new NonTerminal("assignment_operator");
            AssignmentOperator.Rule = grammar.ToTerm("=") | ":=" | "+=" | "-=" | "*=" | "/=" | "%=" | "&=" | "|=" | "^=" | "<<=" | ">>=";
            grammar.MarkTransient(AssignmentOperator);

            Assignment = new NonTerminal("assignment");
            Assignment.Rule = grammar.Expression + AssignmentOperator + grammar.Expression;

            BinaryOperators = new NonTerminal("binary_operators");
            BinaryOperators.Rule = grammar.ToTerm("<")
                | "||" | "&&" | "|" | "^" | "&" | "==" | "!=" | ">" | "<=" | ">=" | "<<" | ">>" | "+" | "-" | "*" | "/" | "%"
                | "is" | "as";
            grammar.MarkTransient(BinaryOperators);

            BinaryOperation = new NonTerminal("binary_operation");
            BinaryOperation.Rule = grammar.Expression + BinaryOperators + grammar.Expression;
            //BinaryOperation.Rule |= grammar.Expression + "->" + grammar.MethodMemberList;

            PreUnaryOperator = new NonTerminal("pre_unary_operator");
            PreUnaryOperator.Rule = grammar.ToTerm("!") | "-" | "--" | "++" | "~";
            grammar.MarkTransient(PreUnaryOperator);

            PostUnaryOperator = new NonTerminal("post_unary_operator");
            PostUnaryOperator.Rule = grammar.ToTerm("++") | "--";
            grammar.MarkTransient(PostUnaryOperator);

            PostUnaryOperation = new NonTerminal("post_unary_operation");
            PostUnaryOperation.Rule = grammar.Expression + PostUnaryOperator;

            PreUnaryOperation = new NonTerminal("pre_unary_operation");
            PreUnaryOperation.Rule = PreUnaryOperator + grammar.Expression;

            UnaryOperation = new NonTerminal("unary_operation");
            UnaryOperation.Rule = PostUnaryOperation;
            UnaryOperation.Rule |= PreUnaryOperation;
        }
    }
}
