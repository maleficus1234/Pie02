using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony.Parsing;

namespace Pie02CS.Expressions
{
    public class ClassDeclaration
        : Expression
    {
        List<string> Modifiers = new List<string>();
        public string ClassType;
        Expression TypeNames;
        bool MakeDynamic = false;

        public ClassDeclaration(Expression parentExpression)
            : base(parentExpression)
        {
            TypeNames = new Expression(this);
            //Scope.EnclosingType = this;
        }

        public override List<dynamic> Match(Expression ex)
        {
            if(ex is ClassDeclaration)
            {
                if (ex.GetFullName() == this.GetFullName())
                    return new List<dynamic>() { this };
            }
            return base.Match(ex);
        }

        public static void Build(IronyParser parser, Expression parentExpression, ParseTreeNode node)
        {
            var c = new ClassDeclaration(parentExpression);
            // ScopeOld.AllTypes.Add(c);
            ScopeStack.ClassDeclarations.Add(c);
            foreach (var m in node.ChildNodes[0].ChildNodes)
                c.Modifiers.Add(m.FindTokenAndGetText());
            c.ClassType = node.ChildNodes[1].Term.ToString();
            c.Name = node.ChildNodes[2].FindTokenAndGetText();
            parentExpression.Children.Add(c);

            if (node.ChildNodes[3].ChildNodes.Count > 0)
                parser.ConsumeParseTree(c.TypeNames, node.ChildNodes[3].ChildNodes[0]);

            parser.ConsumeParseTree(c, node.ChildNodes[4]);
        }

        public override void Emit(ref string source)
        {
           foreach(var m in Modifiers)
            {
                if (m != "debug")
                    source += m + " ";
            }
            if (ClassType == "module")
                source += " static ";
            source += " class ";
            source += Name + "\n";
            if (TypeNames.Children.Count > 0)
            {
                source += " : ";
                TypeNames.Children[0].Emit(ref source);
                for (int i = 1; i < TypeNames.Children.Count; i++)
                    TypeNames.Children[i].Emit(ref source);
                source += "\n";
            }

            if(MakeDynamic == true)
            {
                source += " : System.Dynamic.DynamicObject\n";
            }

            source += "{\n";
            base.Emit(ref source);

            if(MakeDynamic)
            {
                source += @"
// The inner dictionary.
        System.Collections.Generic.Dictionary<string, object> pie_member_dictionary
            = new System.Collections.Generic.Dictionary<string, object>();

        // If you try to get a value of a property 
        // not defined in the class, this method is called.
        public override bool TryGetMember(
            System.Dynamic.GetMemberBinder binder, out object result)
        {
            // Converting the property name to lowercase
            // so that property names become case-insensitive.
            //string name = binder.Name.ToLower();

            // If the property name is found in a dictionary,
            // set the result parameter to the property value and return true.
            // Otherwise, return false.
            return pie_member_dictionary.TryGetValue(binder.Name, out result);
        }

        // If you try to set a value of a property that is
        // not defined in the class, this method is called.
        public override bool TrySetMember(
            System.Dynamic.SetMemberBinder binder, object value)
        {
            // Converting the property name to lowercase
            // so that property names become case-insensitive.
            pie_member_dictionary[binder.Name] = value;

            // You can always add a value to a dictionary,
            // so this method always returns true.
            return true;
        }

";
            }

            source += "}\n";
     
        }

        public override string GetFullName()
        {
            if (ParentExpression == null)
                return Name;
            else
            {
                if (ParentExpression is ClassDeclaration)
                    return ParentExpression.GetFullName() + "." + Name;
                else
                    return Name;
            }
        }

        public override void Validate()
        {
            ScopeStack.Push(Scope);
            foreach(var c in Children)
                if (c is FieldDeclaration)
                {
                    var v = new Variable();
                    v.FullName = (c as FieldDeclaration).Name;
                    Scope.Variables.Add(v);
                }
            for(int i = 0; i < Modifiers.Count; i++)
            {
                switch(Modifiers[i])
                {
                    case "final":
                        Modifiers[i] = "sealed";
                        break;
                }
            }
            if (!Modifiers.Contains("internal") && !Modifiers.Contains("public") && !Modifiers.Contains("private")) Modifiers.Add("public");

            if (TypeNames.Children.Count == 0 && ClassType == "type")
                MakeDynamic = true;

            base.Validate();
            ScopeStack.Pop(Scope);
        }
    }
}
