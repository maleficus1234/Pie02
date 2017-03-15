from Irony.Parsing import *

class LiteralRules(object):
    """description of class"""

    def __init__(self, grammar):
        self.StringLiteral = StringLiteral("string_literal", "\"", StringOptions.AllowsAllEscapes)

        self.Literal = NonTerminal("literal")
        self.Literal.Rule = self.StringLiteral
        grammar.MarkTransient(self.Literal)