from Expression import Expression

class MethodDeclaration(Expression):
    """description of class"""

    def __init__(self, parentExpression):
        super(self.__class__, self).__init__(parentExpression)
        self.Modifiers = []
        self.ReturnType = None
        self.GenericList = None

    @staticmethod
    def Build(parser, parent, node):
        m = MethodDeclaration(parent)
        parent.Children.append(m)

        for modifier in node.ChildNodes[0].ChildNodes:
            m.Modifiers.append(modifier.FindTokenAndGetText())

        if node.ChildNodes[1].FindTokenAndGetText() != "void":
            m.ReturnType = Expression(m)
            parser.ConsumeParseTree(m.ReturnType, node.ChildNodes[1])

        m.Name = node.ChildNodes[2].FindTokenAndGetText()

        if node.ChildNodes[3].ChildNodes.Count > 0:
            m.GenericList = Expression(m)
            parser.ConsumeParseTree(m.GenericList, node.ChildNodes[3])

        parser.ConsumeParseTree(m, node.ChildNodes[7]);


    def Validate(self):
        # Validate modifiers
        self.Modifiers = [modifier.replace('final', 'sealed') for modifier in self.Modifiers]
        self.Modifiers = [modifier.replace('shared', 'static') for modifier in self.Modifiers]
        if self.Modifiers.count('internal') == 0 and self.Modifiers.count('public') == 0 and self.Modifiers.count('private') == 0: self.Modifiers.append('public')  
        super(self.__class__, self).Validate()    


    def Emit(self, source):
        for m in self.Modifiers:
            source += m
            source += " "

        if self.ReturnType == None:
            source += " void "
        else:
            source = self.ReturnType.Emit(source)

        source += self.Name
        if self.GenericList != None:
            source = self.GenericList.Emit(source)

        source += "("

        source += ")"

        source += "\n{\n"

        source = super(self.__class__, self).Emit(source)

        source += "}\n"
        return source

