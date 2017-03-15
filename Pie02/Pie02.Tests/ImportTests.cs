using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.CodeDom.Compiler;

namespace Pie02.Tests
{
    [TestClass]
    public class ImportTests
    {
        [TestMethod]
        public void NamespaceImport()
        {
            string source = @"
import System

type Cow:
    func foo(x):
        return x

    func Moo(x):
        return foo(x)
";
            string[] sources = { source };

            CompilerParameters parameters = new CompilerParameters();
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.ReferencedAssemblies.Add("System.Core.dll");
            parameters.ReferencedAssemblies.Add("Microsoft.CSharp.dll");
            parameters.GenerateInMemory = true;

            CodeDomProvider p = Binding.GetCompiler();

            CompilerResults results = p.CompileAssemblyFromSource(parameters, sources);
            foreach (var e in results.Errors)
                Console.WriteLine(e);

            var cowType = results.CompiledAssembly.GetType("Cow");

            Assert.IsNotNull(cowType);
            Assert.IsTrue(cowType.IsPublic);

            dynamic cow = Activator.CreateInstance(cowType);

            dynamic moo = cow.Moo(2);
            Assert.AreEqual(2, moo);

            Console.WriteLine(moo);

            Console.WriteLine("done");
        }
    }
}
