using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.CodeDom.Compiler;

namespace Pie02.Tests
{
    [TestClass]
    public class NamespaceTests
    {
        public static class foo
        {

        }

        [TestMethod]
        public void NestedNamespaces()
        {
            string source = @"
namespace foo:
    namespace bar:
        type Cow:
";
            string[] sources = { source };

            CompilerParameters parameters = new CompilerParameters();
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.ReferencedAssemblies.Add("System.Core.dll");
            parameters.GenerateInMemory = true;

            CodeDomProvider p = Binding.GetCompiler();

            CompilerResults results = p.CompileAssemblyFromSource(parameters, sources);
            foreach (var e in results.Errors)
                Console.WriteLine(e);

            var cow = results.CompiledAssembly.GetType("foo.bar.Cow");

            Assert.IsNotNull(cow);

            Console.WriteLine("done");
        }
    }
}
