using Robot.Grammar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace Robot.Visitors
{
    class RobotErrorVisitor : RobotGrammarBaseVisitor<object>
    {
        public List<IErrorNode> errorList { get; } = new List<IErrorNode>();

        public override object VisitErrorNode(IErrorNode node)
        {
            errorList.Add(node);
            return base.VisitErrorNode(node);
        }

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

        //public object VisitDir([NotNull] RobotGrammarParser.DirContext context)
        //{
        //    throw new NotImplementedException();
        //}

        //public object VisitDropInstruction([NotNull] RobotGrammarParser.DropInstructionContext context)
        //{
        //    throw new NotImplementedException();
        //}

        //public object VisitFunctionCall([NotNull] RobotGrammarParser.FunctionCallContext context)
        //{
        //    throw new NotImplementedException();
        //}

        //public object VisitFunctionDef([NotNull] RobotGrammarParser.FunctionDefContext context)
        //{
        //    throw new NotImplementedException();
        //}

        //public object VisitFunctionDefinitions([NotNull] RobotGrammarParser.FunctionDefinitionsContext context)
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

        //public object VisitItemId([NotNull] RobotGrammarParser.ItemIdContext context)
        //{
        //    throw new NotImplementedException();
        //}

        //public object VisitLeftDir([NotNull] RobotGrammarParser.LeftDirContext context)
        //{
        //    throw new NotImplementedException();
        //}

        //public object VisitLoopInstruction([NotNull] RobotGrammarParser.LoopInstructionContext context)
        //{
        //    throw new NotImplementedException();
        //}

        //public object VisitLoopWhileInstruction([NotNull] RobotGrammarParser.LoopWhileInstructionContext context)
        //{
        //    throw new NotImplementedException();
        //}

        //public object VisitMoveAmount([NotNull] RobotGrammarParser.MoveAmountContext context)
        //{
        //    throw new NotImplementedException();
        //}

        //public object VisitMoveInstruction([NotNull] RobotGrammarParser.MoveInstructionContext context)
        //{
        //    throw new NotImplementedException();
        //}

        //public object VisitParameter([NotNull] RobotGrammarParser.ParameterContext context)
        //{
        //    throw new NotImplementedException();
        //}

        //public object VisitParameterDef([NotNull] RobotGrammarParser.ParameterDefContext context)
        //{
        //    throw new NotImplementedException();
        //}

        //public object VisitParameterList([NotNull] RobotGrammarParser.ParameterListContext context)
        //{
        //    throw new NotImplementedException();
        //}

        //public object VisitPickUpInstruction([NotNull] RobotGrammarParser.PickUpInstructionContext context)
        //{
        //    throw new NotImplementedException();
        //}

        //public object VisitProgram([NotNull] RobotGrammarParser.ProgramContext context)
        //{
        //    throw new NotImplementedException();
        //}

        //public object VisitRepeatCnt([NotNull] RobotGrammarParser.RepeatCntContext context)
        //{
        //    throw new NotImplementedException();
        //}

        //public object VisitRightDir([NotNull] RobotGrammarParser.RightDirContext context)
        //{
        //    throw new NotImplementedException();
        //}

        //public object VisitTerminal(ITerminalNode node)
        //{
        //    throw new NotImplementedException();
        //}

        //public object VisitTurnInstruction([NotNull] RobotGrammarParser.TurnInstructionContext context)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
