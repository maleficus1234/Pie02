from Irony.Parsing import *

class MethodDeclarationRules(object):
    """description of class"""

    def __init__(self, grammar):
        self.MethodMember = NonTerminal("method_member")
        self.MethodMember.Rule = grammar.StatementExpression

        self.MethodMemberList = NonTerminal("method_member_list")
        self.MethodMemberList.Rule = grammar.MakeStarRule(self.MethodMemberList, self.MethodMember)

        self.MethodBody = NonTerminal("method_body")
        self.MethodBody.Rule = grammar.Indent + self.MethodMemberList + grammar.Dedent
        self.MethodBody.Rule |= grammar.Empty

        self.MethodDeclaration = NonTerminal("method_declaration")
        self.MethodDeclaration.Rule = grammar.ModifierList + grammar.Void + grammar.Identifier + grammar.GenericListOpt + grammar.LParens + grammar.RParens + grammar.Colon + grammar.Eos + self.MethodBody

        