from Expression import Expression

class Literal(Expression):
    """description of class"""

    def __init__(self, parentExpression):
        super(self.__class__, self).__init__(parentExpression)

    @staticmethod
    def Build(parser, parent, node):
        print "build literal"
        l = Literal(parent)
        parent.Children.append(l)
        print "value: " + node.Token.Value
        l.Value = node.Token.Value

    def Emit(self, source):
        source += " " + self.Value + " "
        return source