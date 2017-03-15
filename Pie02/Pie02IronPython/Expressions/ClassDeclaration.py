from Expression import Expression
from Scope import *

class ClassDeclaration(Expression):
    """description of class"""
    def __init__(self, parentExpression):
        super(self.__class__, self).__init__(parentExpression)
        self.Modifiers = []

    def Match(self, ex):
        if type(ex) is ClassDeclaration:
            print "validating namespace " + self.Name + " " + ex.Name
            if ex.GetFullName() == self.GetFullName():
                return self
        return super(self.__class__, self).Match(ex)

    @staticmethod
    def Build(parser, parent, node):
        c = ClassDeclaration(parent)
        for m in node.ChildNodes[0].ChildNodes:
            c.Modifiers.append(m.FindTokenAndGetText())
        c.ClassType = node.ChildNodes[1].Term.ToString()
        c.Name = node.ChildNodes[2].FindTokenAndGetText()
        #Scope.AllTypes.append(c)
        parent.Children.append(c)   
        parser.ConsumeParseTree(c, node.ChildNodes[4])    
    
    def Validate(self):
        # Validate modifiers
        self.Modifiers = [modifier.replace('final', 'sealed') for modifier in self.Modifiers]
        if self.Modifiers.count('internal') == 0 and self.Modifiers.count('public') == 0 and self.Modifiers.count('private') == 0: self.Modifiers.append('public')  
        super(self.__class__, self).Validate()     

    def Emit(self, source):
        for m in self.Modifiers:
            source += m
            source += " "
        if self.ClassType == "module":
            source += " static "
        source += " class "
        source += self.Name
        source += "\n{\n"

        source = super(self.__class__, self).Emit(source)

        source += "}\n"
        return source

    def GetFullName(self):
        if self.ParentExpression is None:
            return self.Name
        else:
            if type(self.ParentExpression) is Expression:
                return self.Name
            else:
                return self.ParentExpression.Name + "." + self.Name