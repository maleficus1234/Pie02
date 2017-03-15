from Expression import Expression

class GenericList(Expression):
    """description of class"""

    def __init__(self, parentExpression):
        super(self.__class__, self).__init__(parentExpression)
        self.Names = []

    @staticmethod
    def Build(parser, parent, node):
        g = GenericList(parent)
        parent.Children.append(g)
        for n in node.ChildNodes[1].ChildNodes:
            g.Names.append(n.FindTokenAndGetText())

    def Emit(self, source):
        source += "<"
        if len(self.Names) > 0:
            source += self.Names[0]
            for n in range(1, len(self.Names)):
                source += "," + self.Names[n]
       # for n in self.Names:
         #   source += n + ","
        source += ">"
        return source