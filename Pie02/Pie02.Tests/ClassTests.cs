using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.CodeDom.Compiler;

namespace Pie02.Tests
{
    [TestClass]
    public class ClassTests
    {
        [TestMethod]
        public void SimpleClass()
        {
            string source = @"
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

            var cow = results.CompiledAssembly.GetType("Cow");

            Assert.IsNotNull(cow);
            Assert.IsTrue(cow.IsPublic);

            dynamic cowi = Activator.CreateInstance(cow);
            Assert.IsTrue(cowi is System.Dynamic.DynamicObject);

            Console.WriteLine("done");
        }

        [TestMethod]
        public void ClassInheritance()
        {
            string source = @"
type Dog:
type Cow(Dog):
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

            var cow = results.CompiledAssembly.GetType("Cow");

            Assert.IsNotNull(cow);
            Assert.IsTrue(cow.IsPublic);

            Console.WriteLine("done");
        }

        [TestMethod]
        public void PublicSimpleClass()
        {
            string source = @"
public type Cow:
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

            var cow = results.CompiledAssembly.GetType("Cow");

            Assert.IsNotNull(cow);
            Assert.IsTrue(cow.IsPublic);

            Console.WriteLine("done");
        }

        [TestMethod]
        public void InternalSimpleClass()
        {
            string source = @"
internal type Cow:
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

            var cow = results.CompiledAssembly.GetType("Cow");

            Assert.IsNotNull(cow);
            Assert.IsFalse(cow.IsPublic);

            Console.WriteLine("done");
        }

        [TestMethod]
        public void ModifierSimpleClass()
        {
            string source = @"
public final type Cow:
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

            var cow = results.CompiledAssembly.GetType("Cow");
            Assert.IsNotNull(cow);
            Assert.IsTrue(cow.IsSealed);
            Assert.IsTrue(cow.IsPublic);
        }


        [TestMethod]
        public void NamespaceSimpleClass()
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

        [TestMethod]
        public void NamespaceSimpleModule()
        {
            string source = @"
namespace foo:
    module m:
namespace foo:
    module m:
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

            var cow = results.CompiledAssembly.GetType("foo.m");

            Assert.IsNotNull(cow);

            Console.WriteLine("done");
        }
    }
}
