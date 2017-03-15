using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pie02CS.Expressions
{
    public class Expression
    {
        public Expression ParentExpression;
        public string Name;
        public List<dynamic> Children;
        public bool IsBaseCall = false;
        public Scope Scope = new Scope();

        public Expression(Expression parentExpression)
        {
            this.ParentExpression = parentExpression;
            this.Children = new List<dynamic>();
        }

        public virtual string GetFullName()
        {
            if (ParentExpression == null)
                return Name;
            else
            {
                if (ParentExpression is Expression)
                    return Name;
                else
                    return ParentExpression.Name + "." + Name;
            }
        }

        public virtual void Validate()
        {
           // if(ParentExpression != null) Scope = ParentExpression.Scope.Copy();
            foreach (var c in Children)
                if (c is ImportDeclaration)
                {
                    foreach (var i in c.QualifiedIdentifiers.Children)
                        if (i is QualifiedImport)
                            PropagateImport(i);
                }


            foreach (var child in Children)
                child.Validate();
        }

        public virtual void Emit(ref string source)
        {
            foreach (var child in Children)
                child.Emit(ref source);
        }

        public virtual List<dynamic> Match(Expression ex)
        {
            return new List<dynamic>();
        }

        public Expression MatchChildren(Expression ex)
        {
            foreach(var child in this.Children)
            {
                var match = child.Match(ex);
                if (match.Count > 0)
                    return match[0];
            }
            return null;
        }

        public static Expression MergeTrees(List<Expression> trees)
        {
            var target = new Expression(null);
            foreach(var tree in trees)
            {
                Expression.MergeTree(tree, target);
            }
            return target;
        }

        public static void MergeTree(Expression source, Expression target)
        {
            foreach(var child in source.Children)
            {
                var match = target.MatchChildren(child);
                if (match == null)
                {
                    if (child is ImportDeclaration)
                        target.Children.Insert(0, child);
                    else
                        target.Children.Add(child);
                    child.ParentExpression = target;
                }
                else Expression.MergeTree(child, match);
            }
        }

        public virtual void PropagateImport(QualifiedImport i)
        {
            
            Scope.ImportDeclarations.Add(i);
            foreach (var c in Children)
                c.PropagateImport(i);
        }

        public void PropagateIsBaseCall()
        {
            IsBaseCall = true;
            foreach (var c in Children)
                c.PropagateIsBaseCall();
        }
    }
}