using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pie02CS.Expressions
{
    public class Scope
    {
        public List<Variable> Variables = new List<Variable>();
        public List<TypeReference> TypeReferences = new List<TypeReference>();
        public List<QualifiedImport> ImportDeclarations = new List<QualifiedImport>();
        
    }

    public abstract class TypeReference
    {
        public abstract string GetFullName();
        public abstract string GetName();
    }

    public static class ScopeStack
    {
        public static List<Scope> Scopes = new List<Scope>();
        public static List<ClassDeclaration> ClassDeclarations = new List<Expressions.ClassDeclaration>();

        public static void Push(Scope scope)
        {
            Scopes.Add(scope);
        }

        public static void Pop(Scope scope)
        {
            int i = Scopes.IndexOf(scope);
            Scopes.RemoveRange(i, Scopes.Count - i);
        }

        public static bool VariableInScope(string name)
        {
            foreach (var scope in Scopes)
                foreach (var v in scope.Variables)
                    if (v.FullName == name) return true;
            return false;
        }

        public static bool TypeInScope(string name)
        {
            foreach(var c in ClassDeclarations)
            {
                if (name == c.Name)
                    return true;
            }
            foreach (var scope in Scopes)
                foreach (var i in scope.ImportDeclarations)
                    if (i.ImportsType(name))
                        return true;
            return false;
        }
    }
}
