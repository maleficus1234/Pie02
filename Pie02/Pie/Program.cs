using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.CodeDom.Compiler;
using System.Reflection;
using System.IO;

namespace Pie
{
    public class Program
    {
        public static void Main()
        {
            if (File.Exists("AllSensors.txt"))
                File.Delete("AllSensors.txt");

            var w = File.CreateText("AllSensors.txt");
            w.Close();

            CompilerParameters parameters = new CompilerParameters();
            parameters.ReferencedAssemblies.Add("C:\\Program Files (x86)\\Reference Assemblies\\Microsoft\\Framework\\.NETFramework\\v4.5.2\\System.Core.dll");
            parameters.ReferencedAssemblies.Add("C:\\Program Files (x86)\\Reference Assemblies\\Microsoft\\Framework\\.NETFramework\\v4.5.2\\Microsoft.CSharp.dll");
            parameters.ReferencedAssemblies.Add("C:\\Program Files (x86)\\Reference Assemblies\\Microsoft\\Framework\\.NETFramework\\v4.5.2\\System.dll");
            parameters.ReferencedAssemblies.Add(System.IO.Directory.GetCurrentDirectory() + "\\"+ "Irony.dll");
            parameters.GenerateInMemory = false;
            parameters.GenerateExecutable = true;
            parameters.OutputAssembly = "piec.exe";
            
            var sources = new List<string>()
            {
                "Pie/Expressions/PieGrammar.pie",
                "Pie/IronyParser.pie",
                "Pie/Expressions/Scope.pie",
                "Pie/Expressions/ScopeStack.pie",
               
                "Pie/Expressions/QualifiedImport.pie",
                "Pie/Expressions/Expression.pie",
                "Pie/Expressions/NamespaceDeclaration.pie",
                "Pie/Expressions/QualifiedIdentifier.pie",
                "Pie/Expressions/Identifier.pie",
                "Pie/Expressions/TypeDeclaration.pie",
                "Pie/PieCompiler.pie",
                "Pie/Expressions/MethodDeclaration.pie",
                "Pie/Expressions/ImportDeclaration.pie",
                "Pie/Expressions/ReturnStatement.pie",
                "Pie/Expressions/Literal.pie",
                "Pie/Expressions/TestHelper.pie",
                "Pie/Expressions/Parameter.pie",
                "Pie/Expressions/Assignment.pie",
                "Pie/Expressions/BinaryOperation.pie",
                "Pie/Expressions/ReservedExpression.pie",
                "Pie/Expressions/IfConditional.pie",
                "Pie/Expressions/ForLoop.pie",
                "Pie/Expressions/Statement.pie",
                "Pie/Expressions/ForEachLoop.pie",
                "Pie/Expressions/MethodInvocation.pie",
                "Pie/Expressions/AccessedExpression.pie",
                "Pie/Expressions/WhileLoop.pie",
                "Pie/Expressions/IndexedIdentifier.pie",
                "Pie/Expressions/FieldDeclaration.pie",
                "Pie/Expressions/SwitchBlock.pie",
                "Pie/Expressions/CaseBlock.pie",
                "Pie/Expressions/UnaryOperation.pie",
                "Pie/Expressions/TestDeclaration.pie",
                "Pie/Expressions/AssertStatement.pie",
                "Pie/Program.pie",
                "Pie/Expressions/ParenedExpression.pie",
                "Pie/Expressions/ExceptionBlock.pie",
                "Pie/Expressions/ThrowStatement.pie",
            };

            var p = new Pie02CS.PieCodeProvider();
            CompilerResults results = p.CompileAssemblyFromFile(parameters, sources.ToArray());

            

            if (results != null)
            {

                foreach (CompilerError e in results.Errors)
                    Console.WriteLine(e.Line + " " + e.FileName + ": " + e.ErrorText);

                if (results.Errors.Count == 0)
                {
                    System.Console.WriteLine("Running tests");

                    if (File.Exists("PassedSensors.txt"))
                        File.Delete("PassedSensors.txt");
                    w = File.CreateText("PassedSensors.txt");
                    w.Close();

                    int failedTests = 0;
                    int passedTests = 0;
                    // var tests = new Dictionary<string, MethodInfo>();
                    var types = results.CompiledAssembly.GetTypes();
                    foreach (var type in types)
                    {
                        var methods = type.GetMethods();
                        foreach (var method in methods)
                        {
                            if (method.IsStatic && method.Name.StartsWith("PieTestzkyz_"))
                            {
                                var name = method.Name.Substring(12);
                                try
                                {
                                    method.Invoke(null, null);
                                    passedTests++;
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("Test " + type.FullName + "." + name + " failed");
                                    Console.WriteLine(e.InnerException.Message);
                                    failedTests++;
                                }
                            }
                        }
                    }
                    Console.WriteLine(passedTests + "/" + (failedTests + passedTests) + " tests passed");
                }
            }

            var allSensorGuids = File.ReadAllLines("AllSensors.txt");
            var allSensors = new Dictionary<string, bool>();
            foreach (var s in allSensorGuids)
                allSensors.Add(s, false);

            var allPassedGuids = File.ReadAllLines("PassedSensors.txt");
            foreach (var s in allPassedGuids)
                allSensors[s] = true;

            double passedCount = 0;
            foreach (var v in allSensors.Values)
                if (v)
                    passedCount++;

            Console.WriteLine("Test coverage: " + passedCount / (double)allSensors.Count);

            Console.WriteLine("done");
            Console.ReadKey();
        }
    }
}
