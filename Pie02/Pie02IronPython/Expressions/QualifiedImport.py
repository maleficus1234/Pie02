from Expressions.Expression import *

class QualifiedImport(Expression):
    """description of class"""

    def __init__(self, parentExpression):
        super(self.__class__, self).__init__(parentExpression)
        self.Identifiers = []

    @staticmethod
    def Build(parser, parent, node):
        i = QualifiedImport(parent)
        parent.Children.append(i)
        for childNode in node.ChildNodes[0].ChildNodes:
            i.Identifiers.append(childNode.FindTokenAndGetText())

    def Emit(self, source):
        source += "using "
        source += self.Identifiers[0]
        for i in range(1, len(self.Identifiers)):
            source += '.'
            source += self.Identifiers[i]   
        source += ';\n'
        return source