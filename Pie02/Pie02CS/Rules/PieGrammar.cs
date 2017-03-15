using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Irony.Parsing;

namespace Pie02CS.Rules
{
    public class PieGrammar
        : Grammar
    {
        public Terminal Abstract;
        
        public Terminal Comma;
        public Terminal Dot;
        public Terminal LParens;
        public Terminal GThan;
        public Terminal LThan;
        public Terminal RParens;
        public Terminal Identifier;
        public NonTerminal Parameter;
        public NonTerminal ParameterList;
        public NonTerminal AccessedExpression;
        public NonTerminal IdentifierList;
        public NonTerminal QualifiedIdentifier;
        public NonTerminal ParameterDirection;
        public NonTerminal Modifier;
       // public NonTerminal GenericList;
        //public NonTerminal GenericListOpt;
        public NonTerminal CompileUnit;
        public NonTerminal ModifierList;
        public NonTerminal Expression;
        public NonTerminal StatementExpression;
        public NonTerminal ReturnStatement;
        public NonTerminal DirectionedIdentifier;
        public NonTerminal WithStatement;
        public NonTerminal Indexer;
        public NonTerminal IndexedIdentifier;
        public NonTerminal ParenedExpression;
        public NonTerminal VariableInit;
        //public NonTerminal AccessChain;
        public NonTerminal ReservedExpression;

        public LiteralRules LiteralRules;
        public ImportRules ImportRules;
        public OperatorRules OperatorRules;
        public MethodDeclarationRules MethodDeclarationRules;
        public ClassRules ClassRules;
        public NamespaceRules NamespaceRules;
        public FieldDeclarationRules FieldDeclarationRules;
        public MethodInvocationRules MethodInvocationRules;
        public TestRules TestRules;
        public IfConditionalRules IfConditionalRules;
        public ForLoopRules ForLoopRules;
        public AnonymousMethodRules AnonymousMethodRules;
        public SwitchBlockRules SwitchBlockRules;

        public NonTerminal ExpressionList;
        public NonTerminal Statement;
        public NonTerminal MethodMemberList;
        public NonTerminal MethodBody;
        public NonTerminal MethodType;

        public NonTerminal AsCast;

        public Terminal foo;

        public PieGrammar()
        {

            MarkReservedWords("import", "namespace", "type", "module", "interface", "func", "act", "return", "test", "assert", "if", "else",
                "private", "public", "internal", "virtual", "override", "final", "shared", "for", "in", "is", "out", "ref", 
                "while", "new", "base", "switch", "case", "break", "continue", "default", "true", "false");
            MarkReservedWords("byte", "sbyte", "short", "ushort", "int", "uint", "long", "ulong", "float", "double", "decimal", "string", "this", "base");

            MethodType = new NonTerminal("method_type");
            MethodType.Rule = ToTerm("func") | ToTerm("act");
            MarkTransient(MethodType);

            //Colon = new KeyTerm(":", ":");
            Comma = new KeyTerm(",", ",");
            Dot = new KeyTerm(".", ".");

            LParens = new KeyTerm("(", "(");
            GThan = new KeyTerm(">", ">");
            LThan = new KeyTerm("<", "<");
            RParens = new KeyTerm(")", ")");

            Statement = new NonTerminal("statement");


            MethodMemberList = new NonTerminal("method_member_list");
            MethodMemberList.Rule = MakeStarRule(MethodMemberList, Statement);

            MethodBody = new NonTerminal("method_body");
            MethodBody.Rule = Indent + MethodMemberList + Dedent;
            MethodBody.Rule |= Empty;

            Identifier = new IdentifierTerminal("identifier");
            IdentifierList = new NonTerminal("identifier_list");
            

            ParameterDirection = new NonTerminal("parameter_direction");
            ParameterDirection.Rule = ToTerm("ref");
            ParameterDirection.Rule |= ToTerm("out");
            ParameterDirection.Rule |= Empty;

            AsCast = new NonTerminal("as_cast");
            AsCast.Rule = ToTerm("as") + Identifier;
            AsCast.Rule |= Empty;

            Parameter = new NonTerminal("parameter");
            Parameter.Rule = ParameterDirection + Identifier + AsCast;
            
        

            ParameterList = new NonTerminal("parameter_list");
            ParameterList.Rule = MakeStarRule(ParameterList, Comma, Parameter);

            QualifiedIdentifier = new NonTerminal("qualified_identifier");
            QualifiedIdentifier.Rule = MakePlusRule(QualifiedIdentifier, Dot, Identifier);

            DirectionedIdentifier = new NonTerminal("directioned_identifier");
            DirectionedIdentifier.Rule = ToTerm("ref") + QualifiedIdentifier;
            DirectionedIdentifier.Rule |= ToTerm("out") + QualifiedIdentifier;

            IdentifierList.Rule = MakeStarRule(IdentifierList, Comma, QualifiedIdentifier);

            Modifier = new NonTerminal("modifier");
            Modifier.Rule = ToTerm("internal") | "private" | "public" | "shared" | "final" | "virtual" | "override" | "protected" | "debug";
            ModifierList = new NonTerminal("modifier_list");
            ModifierList.Rule = MakeStarRule(ModifierList, Modifier);


            LiteralRules = new LiteralRules(this);

            ImportRules = new ImportRules(this);

            Expression = new NonTerminal("expression");
            Expression.Rule = Identifier;
            Expression.Rule |= LiteralRules.Literal;
           // Expression.Rule |= LParens + Expression + RParens;
            Expression.Rule |= DirectionedIdentifier;
            //Expression.Rule |= ToTerm("base");
            //Expression.Rule |= ToTerm("this");

            ReservedExpression = new NonTerminal("reserved_expression");
            ReservedExpression.Rule = ToTerm("true") | "false" | "byte" | "sbyte" | "short" | "ushort" | "int" | "uint" | "long" | "ulong" | "float" | "double" | "decimal" | "string" | "this" | "base" | "null" | "continue";
            Expression.Rule |= ReservedExpression;

            ParenedExpression = new NonTerminal("parened_expression");
            ParenedExpression.Rule = LParens + Expression + RParens;
            Expression.Rule |= ParenedExpression;

            AccessedExpression = new NonTerminal("accessed_expression");
            AccessedExpression.Rule = Expression + "." + Expression;
            Expression.Rule |= AccessedExpression;

            ExpressionList = new NonTerminal("expression_list");
            ExpressionList.Rule = MakeStarRule(ExpressionList, Comma, Expression);

            Indexer = new NonTerminal("indexer");
            Indexer.Rule = ToTerm("[") + ExpressionList + "]";

            IndexedIdentifier = new NonTerminal("indexed_identifier");
            IndexedIdentifier.Rule = Identifier + Indexer;
            Expression.Rule |= IndexedIdentifier;

            MethodInvocationRules = new MethodInvocationRules(this);
            Expression.Rule |= MethodInvocationRules.MethodInvocation;
            //Expression.Rule |= MethodInvocationRules.AccessedMethodInvocation;

            VariableInit = new NonTerminal("variable_init");
            VariableInit.Rule = ToTerm("var") + Identifier;
            //Expression.Rule |= VariableInit;

            ReturnStatement = new NonTerminal("return_statement");
            ReturnStatement.Rule = ToTerm("return") + Eos;
            ReturnStatement.Rule |= ToTerm("return") + Expression + Eos;

           // AccessChain = new NonTerminal("access_chain");

            OperatorRules = new OperatorRules(this);
            Expression.Rule |= OperatorRules.BinaryOperation;
            Expression.Rule |= OperatorRules.UnaryOperation;
            Expression.Rule |= OperatorRules.Assignment;

            StatementExpression = new NonTerminal("statement_expression");
            StatementExpression.Rule = Expression + Eos;
            StatementExpression.Rule |= Expression + ";";
            StatementExpression.Rule |= Expression;
            //StatementExpression.Rule |= Expression + Empty;
            //StatementExpression.Rule |= OperatorRules.Assignment;





            WithStatement = new NonTerminal("with_statement");
            WithStatement.Rule = ToTerm("with") + Expression + ":" + Eos + MethodBody;

            IfConditionalRules = new IfConditionalRules(this);

            ForLoopRules = new ForLoopRules(this);

            AnonymousMethodRules = new AnonymousMethodRules(this);
            Expression.Rule |= AnonymousMethodRules.AnonymousMethod;

            Statement.Rule = StatementExpression;
            //Statement.Rule = AccessChain + Eos;
            Statement.Rule |= ReturnStatement;
            Statement.Rule |= WithStatement;
            Statement.Rule |= IfConditionalRules.IfConditional;
            Statement.Rule |= ForLoopRules.ForEachLoop;
            Statement.Rule |= ForLoopRules.WhileLoop;
            Statement.Rule |= ForLoopRules.ForLoop;

            SwitchBlockRules = new SwitchBlockRules(this);
            Statement.Rule |= SwitchBlockRules.SwitchBlock;

            MethodDeclarationRules = new MethodDeclarationRules(this);

            FieldDeclarationRules = new FieldDeclarationRules(this);

            TestRules = new TestRules(this);

            ClassRules = new ClassRules(this);

            NamespaceRules = new NamespaceRules(this);

            var chainables = new NonTerminal("chainables");
            chainables.Rule = QualifiedIdentifier;
            chainables.Rule |= MethodInvocationRules.MethodInvocation; 
            chainables.Rule |= IndexedIdentifier;

            
           // AccessChain.Rule = MakePlusRule(AccessChain, ToTerm("."), chainables);
            //Expression.Rule |= AccessChain;

            // Comments defining comment tokens.
            var singleLineComment = new CommentTerminal("SingleLineComment", "//", "\r", "\n", "\u2085", "\u2028", "\u2029");
            var delimitedComment = new CommentTerminal("DelimitedComment", "/*", "*/");

            NonGrammarTerminals.Add(singleLineComment);
            NonGrammarTerminals.Add(delimitedComment);
            NonGrammarTerminals.Add(ToTerm(@"\"));

            RegisterOperators(0, Comma);
            RegisterOperators(0, Associativity.Right, "as");
            RegisterOperators(30, Associativity.Left, ".");
            RegisterOperators(1, Associativity.Left, "||");
            RegisterOperators(2, Associativity.Left, "&&");
            RegisterOperators(3, Associativity.Left, "|");
            RegisterOperators(4, Associativity.Left, "^");
            RegisterOperators(5, Associativity.Left, "&");
            RegisterOperators(6, Associativity.Left, "==", "!=");
            RegisterOperators(7, Associativity.Left, "<", ">", "<=", ">=", "is", "as");
            RegisterOperators(8, Associativity.Left, "<<", ">>");
            RegisterOperators(9, Associativity.Left, "+", "-");
            RegisterOperators(10, Associativity.Left, "*", "/", "%");
            RegisterOperators(11, Associativity.Right, "++", "--", "~", "!");
            RegisterOperators(12, Associativity.Left, "=", "+=", "-=", "*=", "/=", "%=", "&=", "|=","^=", "<<=", ">>=");

            RegisterBracePair("(", ")");

            MarkPunctuation(LParens, RParens, ToTerm(","), ToTerm(":"));
            

            //RegisterOperators(-3, Assoc"=", "+=", "-=", "*=", "/=", "%=", "&=", "|=", "^=", "<<=", ">>=");

            CompileUnit = new NonTerminal("compile_unit");
            CompileUnit.Rule = NamespaceRules.NamespaceMemberList;
            CompileUnit.Rule |= Eos;

            AddToNoReportGroup(Eos);
            this.LanguageFlags = LanguageFlags.NewLineBeforeEOF | LanguageFlags.SupportsBigInt;

            Root = CompileUnit;
        }

        public override void CreateTokenFilters(LanguageData language, TokenFilterList filters)
        {
            var outlineFilter = new CodeOutlineFilter(language.GrammarData, OutlineOptions.ProduceIndents | OutlineOptions.CheckOperator | OutlineOptions.CheckBraces, ToTerm("_"));
            filters.Add(outlineFilter);
        }

        /*



    def CreateTokenFilters(self, language, filters):
        outlineFilter = CodeOutlineFilter(language.GrammarData, OutlineOptions.ProduceIndents | OutlineOptions.CheckOperator | OutlineOptions.CheckBraces, ToTerm("_"))
        filters.Add(outlineFilter)*/
    }
}
