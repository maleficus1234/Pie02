from Irony import *
from Irony.Parsing import *

from ClassRules import ClassRules
from ImportRules import ImportRules
from NamespaceRules import NamespaceRules
from MethodDeclarationRules import *
from OperatorRules import *
from LiteralRules import *

class PieGrammar(Grammar):
    """description of class"""

    def __init__(self):

        self.Class = KeyTerm("class", "class")
        self.Colon = KeyTerm(":", ":")
        self.Comma = KeyTerm(",", ",")
        self.Dot = KeyTerm(".", ".")
        self.Import = KeyTerm("import", "import")
        self.Interface = KeyTerm("interface", "interface")
        self.Internal = KeyTerm("internal", "internal")
        self.Final = KeyTerm("final", "final")
        self.Namespace = KeyTerm("namespace", "namespace")
        self.Module = KeyTerm("module", "module")
        self.Private = KeyTerm("private", "private")
        self.Public = KeyTerm("public", "public")
        self.Shared = KeyTerm("shared", "shared")
        self.Struct = KeyTerm("struct", "struct")
        self.Void = KeyTerm("void", "void")

        self.LParens = KeyTerm("(", "(")
        self.GThan = KeyTerm(">", ">")
        self.LThan = KeyTerm("<", "<")
        self.RParens = KeyTerm(")", ")")

        self.Identifier = IdentifierTerminal("identifier")
        self.IdentifierList = NonTerminal("identifier_list")
        self.IdentifierList.Rule = self.MakeStarRule(self.IdentifierList, self.Comma, self.Identifier)
      
        self.QualifiedIdentifier = NonTerminal("qualified_identifier")
        self.QualifiedIdentifier.Rule = self.MakePlusRule(self.QualifiedIdentifier, self.Dot, self.Identifier)

        self.Modifier = NonTerminal("modifier")
        self.Modifier.Rule = self.Internal | self.Private | self.Public | self.Shared | self.Final
        self.ModifierList = NonTerminal("modifier_list")
        self.ModifierList.Rule = self.MakeStarRule(self.ModifierList, self.Modifier)

        self.GenericList = NonTerminal("generic_list")
        self.GenericList.Rule = self.LThan + self.IdentifierList + self.GThan
        self.GenericListOpt = NonTerminal("generic_list_opt")
        self.GenericListOpt.Rule = self.GenericList | self.Empty

        self.LiteralRules = LiteralRules(self)

        self.ImportRules = ImportRules(self)

        self.Expression = NonTerminal("expression")
        self.Expression.Rule = self.QualifiedIdentifier
        self.Expression.Rule |= self.LiteralRules.Literal

        self.OperatorRules = OperatorRules(self)
        self.StatementExpression = NonTerminal("statement_expression")
        self.StatementExpression.Rule = self.OperatorRules.Assignment + self.Eos

        self.MethodDeclarationRules = MethodDeclarationRules(self)

        self.ClassRules = ClassRules(self)

        self.NamespaceRules = NamespaceRules(self)



        self.CompileUnit = NonTerminal("compile_unit")
        self.CompileUnit.Rule = self.NamespaceRules.NamespaceMemberList



        self.Root = self.CompileUnit

    
    def CreateTokenFilters(self, language, filters):
        outlineFilter = CodeOutlineFilter(language.GrammarData, OutlineOptions.ProduceIndents | OutlineOptions.CheckOperator | OutlineOptions.CheckBraces, self.ToTerm("_"))
        filters.Add(outlineFilter)