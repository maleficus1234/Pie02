from Expression import Expression

class Assignment(Expression):
    """description of class"""

    def __init__(self, parentExpression):
        super(self.__class__, self).__init__(parentExpression)
        self.Left = Expression(self)
        self.Right = Expression(self)

    @staticmethod
    def Build(parser, parent, node):
        print "build assignment"
        c = Assignment(parent)
        parent.Children.append(c)   
        c.Left = node.ChildNodes[0].FindTokenAndGetText()

        parser.ConsumeParseTree(c.Right, node.ChildNodes[2])
       # c.Right = node.ChildNodes[2].FindTokenAndGetText()
        
        #parser.ConsumeParseTree(c, node.ChildNodes[4])  

    def Emit(self, source):
        source += self.Left
        source += "="
        source = self.Right.Emit(source)
        source += ";\n"
        return source