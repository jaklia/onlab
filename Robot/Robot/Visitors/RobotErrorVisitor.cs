using Robot.Grammar;
using System.Collections.Generic;
using Antlr4.Runtime.Tree;
using Robot.Errors;
using Robot.Model;
using Robot.Utility;
using Antlr4.Runtime.Misc;
using System;

namespace Robot.Visitors
{
    class RobotErrorVisitor : RobotGrammarBaseVisitor<object>
    {
        private Map map;
        private Dictionary<string, List<Parameter>> functionParams;
        private string currentFunction = "";
        private string calledFunction = "";

        private List<IErrorNode> antlrErrorList = new List<IErrorNode>();
        public List<ErrorLogItem> errorList { get; } = new List<ErrorLogItem>();

        public  RobotErrorVisitor(Map map, Dictionary<string, List<Parameter>> functionParams)
        {
            this.map = new Map(map);
            this.functionParams = functionParams;
        }

        public override object VisitErrorNode(IErrorNode node)
        {
            errorList.Add(new ErrorLogItem(node.GetText(), "ANTLR", "Robot", node.Symbol.Line, node.Symbol.Column));
            antlrErrorList.Add(node);
            return base.VisitErrorNode(node);
        }

        public override object VisitFunctionDef([NotNull] RobotGrammarParser.FunctionDefContext context)
        {
            currentFunction = context.functionName().GetText();
            var res = base.VisitFunctionDef(context);
            currentFunction = "";
            return res;
        }

        public override object VisitItemId([NotNull] RobotGrammarParser.ItemIdContext context)
        {
            var res = base.VisitItemId(context);
            try
            {
                var itemId = int.Parse(context.GetText());
            }
            catch (Exception)
            {
                errorList.Add(new ErrorLogItem($"Invalid value for itemId: {context.GetText()}", "Custom", "Robot", context.Start.Line, context.Start.Column));
            }
            return res;
        }

        public override object VisitMoveAmount([NotNull] RobotGrammarParser.MoveAmountContext context)
        {
            var res = base.VisitMoveAmount(context);

            if (context.parameterName() != null) // moveAmount is given with a parameter
            {
                var name = context.parameterName().GetText();
                if (currentFunction == "") 
                {
                    // not in a function
                    errorList.Add(new ErrorLogItem($"No parameter with the name '{name}' in the current context", "Custom", "Robot", context.Start.Line, context.Start.Column));
                } else if (!functionParams[currentFunction].Exists(param => param.Name == name))
                {
                    // invalid parameter name (there is no parameter with the given name)
                    errorList.Add(new ErrorLogItem($"No parameter with the name '{name}' in the current context", "Custom", "Robot", context.Start.Line, context.Start.Column));
                }
                else if (functionParams[currentFunction].Find(param => param.Name == name).Type != "int")
                {
                    // invalid parameter type
                    errorList.Add(new ErrorLogItem($"Invalid type for moveAmount: {name}, expected int, got {functionParams[currentFunction].Find(param => param.Name == name).Type}", "Custom", "Robot", context.Start.Line, context.Start.Column));
                }
            } else
            {
                try
                {
                    // check if moveAmount is a number
                    var moveAmount = int.Parse(context.GetText());
                    if (moveAmount < 1)
                    {
                        errorList.Add(new ErrorLogItem("Value of moveAmount must be greater than 0", "Custom", "Robot", context.Start.Line, context.Start.Column));
                    }
                    else if (moveAmount > Math.Max(map.Height, map.Width))
                    {
                        errorList.Add(new ErrorLogItem($"Value of moveAmount must be less than {Math.Max(map.Height, map.Width)}", "Custom", "Robot", context.Start.Line, context.Start.Column));
                    }
                }
                catch (Exception)
                {
                    errorList.Add(new ErrorLogItem($"Invalid value for moveAmount: {context.GetText()}", "Custom", "Robot", context.Start.Line, context.Start.Column));
                }
            }
            
            return res;
        }

        public override object VisitRepeatCnt([NotNull] RobotGrammarParser.RepeatCntContext context)
        {
            var res = base.VisitRepeatCnt(context);
            
            if (context.parameterName() != null) // repeatCnt given with a parameter
            {
                var name = context.parameterName().GetText();
                if (currentFunction == "")
                {
                    // not in a function
                    errorList.Add(new ErrorLogItem($"No parameter with the name '{name}' in the current context", "Custom", "Robot", context.Start.Line, context.Start.Column));
                } else if (!functionParams[currentFunction].Exists(param => param.Name == name))
                {
                    // invalid parameter name (there is no parameter with the given name)
                    errorList.Add(new ErrorLogItem($"No parameter with the name '{name}' in the current context", "Custom", "Robot", context.Start.Line, context.Start.Column));
                } else if (functionParams[currentFunction].Find(param => param.Name == name).Type != "int")
                {
                    // invalid parameter type
                    errorList.Add(new ErrorLogItem($"Invalid type for repeatCnt: {name}, expected int, got {functionParams[currentFunction].Find(param => param.Name == name).Type}", "Custom", "Robot", context.Start.Line, context.Start.Column));
                }
            } else
            {
                try
                {
                    var repeatCnt = int.Parse(context.GetText());
                    if (repeatCnt < 1)
                    {
                        errorList.Add(new ErrorLogItem("Value of repeatCnt must be greater than 0", "Custom", "Robot", context.Start.Line, context.Start.Column));
                    }
                    else if (repeatCnt > 100)
                    {
                        errorList.Add(new ErrorLogItem($"Value of repeatCnt must be less than {100}", "Custom", "Robot", context.Start.Line, context.Start.Column));
                    }
                }
                catch (Exception)
                {
                    errorList.Add(new ErrorLogItem($"Invalid value for repeatCnt: {context.GetText()}", "Custom", "Robot", context.Start.Line, context.Start.Column));
                }
            }
            return res;
        }
        
        public override object VisitDir([NotNull] RobotGrammarParser.DirContext context)
        {
            if (context.leftDir() != null)
            {
                var dir = context.leftDir().GetText();
                if (dir != "left" && dir != "-")
                {
                    errorList.Add(new ErrorLogItem($"Invalid value for left direction: {context.leftDir().GetText()}", "Custom", "Robot", context.Start.Line, context.Start.Column));
                }
            } else if (context.rightDir() != null)
            {
                var dir = context.rightDir().GetText();
                if (dir != "right" && dir != "+")
                {
                    errorList.Add(new ErrorLogItem($"Invalid value for right direction: {context.rightDir().GetText()}", "Custom", "Robot", context.Start.Line, context.Start.Column));
                }
            } else if (context.parameterName() != null)
            {
                var name = context.parameterName().GetText();
                if (currentFunction == "")
                {
                    errorList.Add(new ErrorLogItem($"No parameter with the name '{name}' in the current context", "Custom", "Robot", context.Start.Line, context.Start.Column));
                } else if (!functionParams[currentFunction].Exists(param => param.Name == name))
                {
                    errorList.Add(new ErrorLogItem($"No parameter with the name '{name}' in the current context", "Custom", "Robot", context.Start.Line, context.Start.Column));
                } else if (functionParams[currentFunction].Find(param => param.Name == name).Type != "dir")
                {
                    errorList.Add(new ErrorLogItem($"Invalid type for direction: {name}, expected dir got {functionParams[currentFunction].Find(param => param.Name == name).Type}", "Custom", "Robot", context.Start.Line, context.Start.Column));
                }
            } else
            {
                errorList.Add(new ErrorLogItem($"Invalid value for direction: {context.GetText()}", "Custom", "Robot", context.Start.Line, context.Start.Column));
            }
            return base.VisitDir(context);
        }

        public override object VisitFunctionCall([NotNull] RobotGrammarParser.FunctionCallContext context)
        {
            var name = context.functionName().GetText();
            if (functionParams.ContainsKey(name))
            {
                calledFunction = name;
            } else
            {
                errorList.Add(new ErrorLogItem($"Invalid function name: {name}", "Custom", "Robot", context.Start.Line, context.Start.Column));
                calledFunction = "-";
            }
            var res = base.VisitFunctionCall(context);
            calledFunction = "";
            return res;
        }

        public override object VisitParameterList([NotNull] RobotGrammarParser.ParameterListContext context)
        {
            if (calledFunction == "")
                // something is wrong with the called function name, don't need to deal with the parameter list
            {
                return base.VisitParameterList(context);
            }
            int i = 0;
            foreach (var param in context.parameter())
            {
                if (param.intParameter() != null)
                {
                    if (functionParams[calledFunction][i].Type != "int")
                    {
                        errorList.Add(new ErrorLogItem($"Type error, expected: {functionParams[calledFunction][i].Type}, got int", "Custom", "Robot", context.Start.Line, context.Start.Column));
                    } else
                    {
                        try
                        {
                            var value = int.Parse(param.intParameter().GetText());

                        }
                        catch (Exception)
                        {
                            errorList.Add(new ErrorLogItem($"Invalid value for int parameter: {param.intParameter().GetText()}", "Custom", "Robot", context.Start.Line, context.Start.Column));
                        }
                    }
                } else if (param.dirParameter() != null)
                {
                    var value = param.dirParameter().GetText();
                    if (functionParams[calledFunction][i].Type != "dir")
                    {
                        errorList.Add(new ErrorLogItem($"Type error, expected: {functionParams[calledFunction][i].Type}, got dir", "Custom", "Robot", context.Start.Line, context.Start.Column));
                    } else if (value != "right" && value != "+" && value != "left" && value != "-")
                    {
                        errorList.Add(new ErrorLogItem($"Invalid value for dir parameter: {param.intParameter().GetText()}", "Custom", "Robot", context.Start.Line, context.Start.Column));
                    }
                } else if (param.parameterName() != null)
                { 
                    var name = param.parameterName().GetText();
                    
                    if (currentFunction == "")
                    {
                        errorList.Add(new ErrorLogItem($"No parameter with the name '{name}' in the current context", "Custom", "Robot", context.Start.Line, context.Start.Column));
                    } else if (!functionParams[currentFunction].Exists(item => item.Name == name))
                    {
                        errorList.Add(new ErrorLogItem($"No parameter with the name '{name}' in the current context", "Custom", "Robot", context.Start.Line, context.Start.Column));
                    } else if (functionParams[calledFunction][i].Type !=
                        functionParams[currentFunction].Find(item => item.Name == name).Type)
                    {
                        errorList.Add(new ErrorLogItem($"Invalid type: expected {functionParams[calledFunction][i].Type} in {calledFunction}, got {functionParams[currentFunction].Find(item => item.Name == name).Type}",
                            "Custom", "Robot", context.Start.Line, context.Start.Column));
                    }
                }
                i++;
            }
            return base.VisitParameterList(context);
        }


        //public override object VisitParameter([NotNull] RobotGrammarParser.ParameterContext context)
        //{
        //    throw new NotImplementedException();
        //}



        

        //public object VisitLeftDir([NotNull] RobotGrammarParser.LeftDirContext context)
        //{
        //    throw new NotImplementedException();
        //}

        //public object VisitRightDir([NotNull] RobotGrammarParser.RightDirContext context)
        //{
        //    throw new NotImplementedException();
        //}

        //public override object VisitDropInstruction([NotNull] RobotGrammarParser.DropInstructionContext context)
        //{
        //    throw new NotImplementedException();
        //}

        //public object VisitFunctionName([NotNull] RobotGrammarParser.FunctionNameContext context)
        //{
        //    throw new NotImplementedException();
        //}

        //public object VisitInstruction([NotNull] RobotGrammarParser.InstructionContext context)
        //{
        //    throw new NotImplementedException();
        //}

        //public object VisitInstructionSet([NotNull] RobotGrammarParser.InstructionSetContext context)
        //{
        //    throw new NotImplementedException();
        //}

        //public object VisitLoopInstruction([NotNull] RobotGrammarParser.LoopInstructionContext context)
        //{
        //    throw new NotImplementedException();
        //}

        //public object VisitMoveInstruction([NotNull] RobotGrammarParser.MoveInstructionContext context)
        //{
        //    throw new NotImplementedException();
        //}

        

        //public object VisitPickUpInstruction([NotNull] RobotGrammarParser.PickUpInstructionContext context)
        //{
        //    throw new NotImplementedException();
        //}



        //public object VisitTurnInstruction([NotNull] RobotGrammarParser.TurnInstructionContext context)
        //{
        //    throw new NotImplementedException();
        //}


        /* PROBABLY DON'T NEED THIS */

        //public object VisitParameterDef([NotNull] RobotGrammarParser.ParameterDefContext context)
        //{
        //    throw new NotImplementedException();
        //}





        /* 100%  DON'T NEED THIS */

        //public object VisitProgram([NotNull] RobotGrammarParser.ProgramContext context)
        //{
        //    throw new NotImplementedException();
        //}

        //public object VisitTerminal(ITerminalNode node)
        //{
        //    throw new NotImplementedException();
        //}

        //public object Visit(IParseTree tree)
        //{
        //    throw new NotImplementedException();
        //}

        //public object VisitChildren(IRuleNode node)
        //{
        //    throw new NotImplementedException();
        //}

        //public object VisitCondition([NotNull] RobotGrammarParser.ConditionContext context)
        //{
        //    throw new NotImplementedException();
        //}

        //public object VisitLoopWhileInstruction([NotNull] RobotGrammarParser.LoopWhileInstructionContext context)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
