using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Reflection;

using Irony.Parsing;

namespace Pie02CS
{
    public class PieCodeProvider
        : CodeDomProvider
    {

        IronyParser parser;
        public static List<Assembly> referencedAssembly = new List<Assembly>();

        public PieCodeProvider()
        {
            parser = new IronyParser();
        }

        [Obsolete]
        public override ICodeCompiler CreateCompiler()
        {
            throw new NotImplementedException();
        }

        [Obsolete]
        public override ICodeGenerator CreateGenerator()
        {
            throw new NotImplementedException();
        }

        public override CompilerResults CompileAssemblyFromFile(CompilerParameters options, params string[] fileNames)
        {
            var sources = new List<string>();
            foreach (var filename in fileNames)
                sources.Add(System.IO.File.ReadAllText(filename));

            return CompileAssemblyFromSource(options, sources.ToArray());
        }

        

        public override CompilerResults CompileAssemblyFromSource(CompilerParameters options, params string[] sources)
        {
            referencedAssembly.Clear();
            foreach(var a in options.ReferencedAssemblies)
            {
                var ass = Assembly.LoadFile(a);
                if (ass == null) throw new NullReferenceException(a);
                referencedAssembly.Add(ass);
            }

            Pie02CS.Expressions.ScopeOld.AllTypes.Clear();
            var trees = new List<Pie02CS.Expressions.Expression>();
            var csSources = new List<string>();
            foreach(var source in sources)
            {
                var t = parser.Parse(source);
                
                if(t != null) trees.Add(t);
            }

            if (trees.Count > 0)
            {
                //var merged = new Expressions.Expression(null);
                var merged = Pie02CS.Expressions.Expression.MergeTrees(trees);
                merged.Validate();
                var cssource = "#pragma warning disable 0105\n";
                merged.Emit(ref cssource);

                cssource += @"
static class PieTestzkyz_Assert
{
    public static void Assert(bool condition, string message = """")
    {
        if(!condition)
        {
            System.Console.WriteLine(""assert failed with message: "" + message);
            throw new System.Exception(message);
        }
    }
}

";

               // Console.WriteLine(cssource);
                csSources.Add(cssource);



                var csprovider = new Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider();
                return csprovider.CompileAssemblyFromSource(options, csSources.ToArray());
            }
            return null;
        }
    }
}
