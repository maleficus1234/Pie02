using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pie02CS.Expressions
{
    public class Variable
    {
        public string FullName = "";
    }

    public class ScopeOld
    {
        public List<Variable> Variables = new List<Variable>();
        public static List<dynamic> AllTypes = new List<dynamic>();
        public List<string> LocalVariables = new List<string>();
        public List<QualifiedImport> ImportDeclarations = new List<QualifiedImport>();
        public Expression EnclosingType;

        public ScopeOld()
        {

        }

        public string FindVariable(string name)
        {
            foreach (var lv in LocalVariables)
                if (lv == name)
                    return lv;
            
            if(EnclosingType != null)
            {
                foreach(var ex in EnclosingType.Children)
                {
                    if(ex is FieldDeclaration)
                    {
                        if ((ex as FieldDeclaration).Name == name)
                            return name;
                    }
                }
            }

            return null;
        }    
        
        public ScopeOld Copy()
        {
            var s = new ScopeOld();
            s.LocalVariables = new List<string>(LocalVariables);
            s.EnclosingType = EnclosingType;
            return s;
        } 

        public bool IsVisibleType(string name)
        {
            foreach(var i in ImportDeclarations)
            {
                if (i.IsType)
                {
                    if (i.GetFullName().EndsWith("." + name) || i.GetFullName() == name)
                        return true;
                }
                else
                {
                    if (i.ImportsType(name)) return true;
                }
            }

            foreach(var t in AllTypes)
            {
                if (t.GetFullName() == name || t.Name == name)
                    return true;
            }

            return false;
        }

        public bool VariableInScope(string name)
        {
            foreach (var v in Variables)
                if (v.FullName == name) return true;

            return false;
        }
    }
}
