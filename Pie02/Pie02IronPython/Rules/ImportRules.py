from Irony.Parsing import *

class ImportRules(object):
    
    def __init__(self, grammar):
        self.ImportedName = NonTerminal("qualified_import")
        self.ImportedName.Rule = grammar.QualifiedIdentifier
        self.ImportList = NonTerminal("import_list")
        self.ImportList.Rule = grammar.MakePlusRule(self.ImportList, grammar.Comma, self.ImportedName)
        self.ImportDeclaration = NonTerminal("import_declaration")
        self.ImportDeclaration.Rule = grammar.Import + self.ImportList + grammar.Eos
        