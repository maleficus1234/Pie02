using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.CodeDom.Compiler;

namespace Pie02.Tests
{
    [TestClass]
    public class MethodTests
    {
        [TestMethod]
        public void SimpleAction()
        {
            string source = @"
type Cow:
    act Moo():
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

            var cowType = results.CompiledAssembly.GetType("Cow");

            Assert.IsNotNull(cowType);
            Assert.IsTrue(cowType.IsPublic);

            dynamic cow = Activator.CreateInstance(cowType);

            cow.Moo();

            Console.WriteLine("done");
        }

        [TestMethod]
        public void SimpleFunction()
        {
            string source = @"
type Cow:
    func Moo():
        return ""moo""
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

            var cowType = results.CompiledAssembly.GetType("Cow");

            Assert.IsNotNull(cowType);
            Assert.IsTrue(cowType.IsPublic);

            dynamic cow = Activator.CreateInstance(cowType);

            dynamic moo = cow.Moo();
            Assert.AreEqual("moo", moo);

            Console.WriteLine(moo);

            Console.WriteLine("done");
        }

        [TestMethod]
        public void SimpleFunction2()
        {
            string source = @"
type Cow:
    func Moo( x ):
        return x
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

            var cowType = results.CompiledAssembly.GetType("Cow");

            Assert.IsNotNull(cowType);
            Assert.IsTrue(cowType.IsPublic);

            dynamic cow = Activator.CreateInstance(cowType);

            dynamic moo = cow.Moo("moo");
            Assert.AreEqual("moo", moo);

            Console.WriteLine(moo);

            Console.WriteLine("done");
        }

        [TestMethod]
        public void SimpleInvocation()
        {
            string source = @"
type Cow:
    func foo():
        return ""moo"" 

    func Moo():
        return foo()
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

            var cowType = results.CompiledAssembly.GetType("Cow");

            Assert.IsNotNull(cowType);
            Assert.IsTrue(cowType.IsPublic);

            dynamic cow = Activator.CreateInstance(cowType);

            dynamic moo = cow.Moo();
            Assert.AreEqual("moo", moo);

            Console.WriteLine(moo);

            Console.WriteLine("done");
        }

        [TestMethod]
        public void SimpleInvocation2()
        {
            string source = @"
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

        [TestMethod]
        public void QualifiedInvocation()
        {
            string source = @"
type Cow:
    act foo():
        System.Console.WriteLine(""qualified test"")
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

            cow.foo();

            Console.WriteLine("done");
        }
    }
}
