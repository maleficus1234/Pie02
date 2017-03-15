from Microsoft.CSharp import CSharpCodeProvider
from System.CodeDom import *
from System.CodeDom.Compiler import *
from IronyParser import *
from System import *
from System.Collections import *

class PieCodeProvider(CodeDomProvider):
    """description of class"""

    def __init__(self):
        self.Parser = IronyParser()

    def Compile(self, options, sources):
        trees = []
        cssources = []
        for source in sources:
            root = self.Parser.Parse(source)
            trees.append(root)

        merged = Expression(None)
        Expression.MergeTrees(trees, merged)

        #for root in trees:
        merged.Validate()
        cssource = String()
        cssource = merged.Emit(cssource)
        print cssource
        cssources.append(cssource)

        arr = Array[String]([x for x in cssources])
        csprovider = CSharpCodeProvider()
        return csprovider.CompileAssemblyFromSource(options, arr)