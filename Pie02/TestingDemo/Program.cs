using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.CodeDom.Compiler;
using System.Reflection;

using Pie;

namespace TestingDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            if (File.Exists("AllSensors.txt"))
                File.Delete("AllSensors.txt");

            var w = File.CreateText("AllSensors.txt");
            w.Close();

            CompilerParameters parameters = new CompilerParameters();
            parameters.ReferencedAssemblies.Add("C:\\Program Files (x86)\\Reference Assemblies\\Microsoft\\Framework\\.NETFramework\\v4.5.2\\System.Core.dll");
            parameters.ReferencedAssemblies.Add("C:\\Program Files (x86)\\Reference Assemblies\\Microsoft\\Framework\\.NETFramework\\v4.5.2\\Microsoft.CSharp.dll");
            parameters.ReferencedAssemblies.Add("C:\\Program Files (x86)\\Reference Assemblies\\Microsoft\\Framework\\.NETFramework\\v4.5.2\\System.dll");
            parameters.ReferencedAssemblies.Add(System.IO.Directory.GetCurrentDirectory() + "\\" + "Irony.dll");
            parameters.GenerateInMemory = false;
            parameters.GenerateExecutable = false;
            parameters.OutputAssembly = "TestingDemo2.dll";

            var sourceFiles = new List<string>()
            {
                "Demo.pie"
            };

            var sources = new List<string>();
            sources.Add(File.ReadAllText(sourceFiles[0]));

            Pie.PieCompiler.trackSensors = true;
            var p = new Pie.PieCompiler();
            
            CompilerResults results = p.CompileAssemblyFromSource(parameters, sources.ToArray());



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

                    double failedTests = 0.0;
                    double passedTests = 0.0;
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
