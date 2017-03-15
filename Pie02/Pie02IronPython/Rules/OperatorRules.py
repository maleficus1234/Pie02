from Irony.Parsing import *

class OperatorRules(object):
    """description of class"""

    def __init__(self, grammar):
        self.AssignmentOperator = NonTerminal("assignment_operator")
        self.AssignmentOperator.Rule = grammar.ToTerm("=") | "+=" | "-=" | "*=" | "/=" | "%=" | "&=" | "|=" | "^=" | "<<=" | ">>="


        self.Assignment = NonTerminal("assignment")
        self.Assignment.Rule = grammar.QualifiedIdentifier + self.AssignmentOperator + grammar.Expression