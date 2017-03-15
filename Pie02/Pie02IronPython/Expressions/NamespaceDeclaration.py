from Expression import Expression

class NamespaceDeclaration(Expression):
    """description of class"""
    def __init__(self, parentExpression):
        super(self.__class__, self).__init__(parentExpression)



    @staticmethod
    def Build(parser, parent, node):
        lastEx = parent
        for name in node.ChildNodes[1].ChildNodes:
            n = NamespaceDeclaration(parent)
            n.Name = name.FindTokenAndGetText()
            lastEx.Children.append(n)
            lastEx = n
        parser.ConsumeParseTree(lastEx, node.ChildNodes[3])    
        
          

    def Emit(self, source):
        source += "namespace "
        source += self.Name
        source += "\n{\n"

        source = super(self.__class__, self).Emit(source)

        source += "}\n"
        return source

    def Match(self, ex):
        print isinstance(ex, NamespaceDeclaration)
        if type(ex) is NamespaceDeclaration:
            print "validating namespace " + self.Name + " " + ex.Name
            if ex.GetFullName() == self.GetFullName():
                return self
        return super(self.__class__, self).Match(ex)

    def GetFullName(self):
        if self.ParentExpression is None:
            return self.Name
        else:
            if type(self.ParentExpression) is Expression:
                return self.Name
            else:
                return self.ParentExpression.Name + "." + self.Name