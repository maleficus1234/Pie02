using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using System.CodeDom.Compiler;

namespace Pie02.WorkingTests
{
    delegate void d(dynamic t);

    class Program
    {
        static void Main(string [] args)
        {
            //var an = new AssemblyName();
            Assembly.Load("System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
            Assembly.Load("System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
            Assembly.Load("Microsoft.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
            Assembly.Load("Irony, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ca48ace7223ead47");
        }
    }
}
