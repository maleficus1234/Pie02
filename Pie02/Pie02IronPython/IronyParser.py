import clr
clr.AddReferenceToFileAndPath("Irony.dll")

from Irony import *
from Irony.Parsing import *

from Rules import PieGrammar
from Expressions import *
from Expressions.QualifiedImport import *
from Expressions.NamespaceDeclaration import *
from Expressions.ClassDeclaration import *
from Expressions.Assignment import *
from Expressions.MethodDeclaration import *
from Expressions.GenericList import *
from Expressions.Literal import *

class IronyParser(object):

    def __init__(self):
        self.Parser = Parser(PieGrammar())
        self.Builders = { 'qualified_import' : QualifiedImport.Build, 
                         'namespace_declaration' : NamespaceDeclaration.Build,
                         'class_declaration' : ClassDeclaration.Build,
                         'assignment' : Assignment.Build,
                         'method_declaration' : MethodDeclaration.Build,
                         'generic_list' : GenericList.Build,
                         'string_literal' : Literal.Build
                         }

       
    def Parse(self, source):
        root = Expression(None)
        tree = self.Parser.Parse(source)
        if tree.ParserMessages.Count > 0:
            for error in tree.ParserMessages:
                print error
            return None
        else:
            self.ConsumeParseTree(root, tree.Root)
        return root

    def ConsumeParseTree(self, parent, node):
        for childNode in node.ChildNodes:
            key = childNode.Term.ToString()
            if self.Builders.ContainsKey(key):
                self.Builders[key](self, parent, childNode)
            else:
                self.ConsumeParseTree(parent, childNode)