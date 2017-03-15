from Scope import *
from QualifiedImport import *

class Expression(object):
    
    def __init__(self, parentExpression):
        self.ParentExpression = parentExpression
        self.Children = []


   # def Validate(self):
    #    for child in self.Children:
       #     child.Validate()

    def Emit(self, source):
        for child in self.Children:
            source = child.Emit(source)
        return source
    
    def Match(self, ex):
        return None

    def MatchChildren(self, ex):
        for child in self.Children:
            match = child.Match(ex)
            if match is not None:
                return match
        return None

    @staticmethod   
    def MergeTrees(source, target):
        for tree in source:
            Expression.MergeTree(tree, target)

    @staticmethod
    def MergeTree(source, target):
        for child in source.Children:
            match = target.MatchChildren(child)
            if match is None:
                target.Children.append(child)
                child.ParentExpression = target
            else:
                Expression.MergeTree(child, match)


    #def PropagateImports(self):
     #   for child in self.Children:
       #     if type(child) is QualifiedImport:
      #          child.Validate()