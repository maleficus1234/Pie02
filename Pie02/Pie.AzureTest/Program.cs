using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.CodeDom.Compiler;
using System.Reflection;

namespace Pie.AzureTest
{
    class Program
    {
        static void Main(string[] args)
        {
            CompilerParameters parameters = new CompilerParameters();
            //parameters.ReferencedAssemblies.Add("System.dll");
            parameters.ReferencedAssemblies.Add("System.Core.dll");
            parameters.ReferencedAssemblies.Add("Microsoft.CSharp.dll");
            parameters.ReferencedAssemblies.Add("Irony.dll");
            parameters.ReferencedAssemblies.Add("Microsoft.CodeDom.Providers.DotNetCompilerPlatform.dll");
            parameters.ReferencedAssemblies.Add("Microsoft.ServiceBus.dll");
            parameters.ReferencedAssemblies.Add("Microsoft.WindowsAzure.Configuration.dll");

            parameters.GenerateInMemory = true;

            var sources = new List<string>()
            {
               "Source/Program.pie"
            };

            var p = new Pie02CS.PieCodeProvider();
            CompilerResults results = p.CompileAssemblyFromFile(parameters, sources.ToArray());

            if (results != null)
            {
                if(results.Errors.Count > 0)
                    foreach (CompilerError e in results.Errors)
                        Console.WriteLine(e.Line + " " + e.FileName + ": " + e.ErrorText);
                else
                {
                    Console.WriteLine("Starting Pie");
                    Type t = results.CompiledAssembly.GetType("Program");
                    t.GetMethod("Main").Invoke(null, null);
                }
            }
            


            Console.ReadKey();
        }
    }
}
