using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;

using System.CodeDom.Compiler;

namespace Pie02.Tests
{
    static class Binding
    {
        public static CodeDomProvider GetCompiler()
        {
            return new Pie02CS.PieCodeProvider();

           /* ScriptEngine engine = Python.CreateEngine();
            var s = engine.GetSearchPaths();
            s.Add("C:\\Users\\owner\\Documents\\Visual Studio 2015\\Projects\\Pie02\\Pie02IronPython");
            engine.SetSearchPaths(s);
            ScriptSource script = engine.CreateScriptSourceFromFile("C:\\Users\\owner\\Documents\\Visual Studio 2015\\Projects\\Pie02\\Pie02IronPython\\PieCodeProvider.py");
            ScriptScope scope = engine.CreateScope();

            script.Execute(scope);

            var providertype = scope.GetVariable("PieCodeProvider");
            dynamic p = providertype();

            return p;*/
        }
    }
}
