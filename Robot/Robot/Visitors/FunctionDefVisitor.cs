using Antlr4.Runtime.Tree;
using Robot.Errors;
using Robot.Grammar;
using System.Collections.Generic;
using Antlr4.Runtime.Misc;
using Robot.Commands;
using Robot.Utility;

namespace Robot.Visitors
{
    public class FunctionDefVisitor : RobotGrammarBaseVisitor<object>
    {
        public Dictionary<string, List<Parameter>> functionParameters { get; }
        public List<ErrorLogItem> errorList { get; } = new List<ErrorLogItem>();

        private List<IErrorNode> antlrErrorList = new List<IErrorNode>();

        private List<CommandBase> cmdList;
        private List<Parameter> paramList;

        private HashSet<string> reservedNames = new HashSet<string>()
            { "int", "dir", "left", "right", "end", "loop", "function", 
            "wall", "free", "drop", "pickup", "turn", "move", "not", "or", "and" };

        public FunctionDefVisitor()
        {
            functionParameters = new Dictionary<string, List<Parameter>>();
        }
        

        public override object VisitFunctionDef([NotNull] RobotGrammarParser.FunctionDefContext context)
        {
            cmdList = new List<CommandBase>();
            paramList = new List<Parameter>();
            var res = base.VisitFunctionDef(context);
            var name = context.functionName().GetText();
            if (functionParameters.ContainsKey(name))
            {
                errorList.Add(new ErrorLogItem($"Function already declared with the name: {context.GetText()}",
                    "Custom", "Robot", context.Start.Line, context.Start.Column));
            } else if (reservedNames.Contains(name))
            {
                errorList.Add(new ErrorLogItem($"Invalid function name: {context.GetText()}",
                    "Custom", "Robot", context.Start.Line, context.Start.Column));
            } else
            {
                functionParameters[name] = new List<Parameter>();
                paramList.ForEach(item => functionParameters[name].Add(item));
            }
            return res;
        }

        public override object VisitParameterDef([NotNull] RobotGrammarParser.ParameterDefContext context)
        {
            var name = context.parameterName().GetText();
            var type = context.paramType().GetText();
            var param = new Parameter(name, type);
            if (paramList.Contains(param))
            {
                errorList.Add(new ErrorLogItem($"Parameter already declared with the name: {context.GetText()}",
                    "Custom", "Robot", context.Start.Line, context.Start.Column));
            } else if (reservedNames.Contains(name))
            {
                errorList.Add(new ErrorLogItem($"Invalid parameter name: {context.GetText()}",
                    "Custom", "Robot", context.Start.Line, context.Start.Column));
            } else
            {
                paramList.Add(param);
            }
            return base.VisitParameterDef(context);
        }

        
    }
}
