import clr
from System import Console

from Rules import PieGrammar

clr.AddReferenceToFileAndPath("C:\Users\owner\Documents\Visual Studio 2015\Projects\Pie02\Pie02IronPython\Irony.dll")

#clr.CompileModules("Pie.dll", "PieGrammar.py")

p = PieGrammar()


Console.ReadKey()