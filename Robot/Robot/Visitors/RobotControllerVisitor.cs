using Robot.Grammar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System.Windows.Controls;
using Robot.Model;
using System.Windows.Media.Imaging;
using Robot.Commands;

namespace Robot.Visitors
{
    class RobotControllerVisitor : RobotGrammarBaseVisitor<object>
    {
        Game Game;

        public RobotControllerVisitor(Game game)
        {
            Game = game;
        }

        public override object VisitProgram([NotNull] RobotGrammarParser.ProgramContext context)
        {
            VisitInstructionSet(context.instructionSet());
            return 0;
        }

        public override object VisitInstructionSet([NotNull] RobotGrammarParser.InstructionSetContext context)
        {
            foreach (var instruction in context.instruction())
            {
                VisitInstruction(instruction);
            }
            return 0;
        }

        public override object VisitInstruction([NotNull] RobotGrammarParser.InstructionContext context)
        {
            if (context.loopInstruction() != null)
                VisitLoopInstruction(context.loopInstruction());
            else if (context.loopWhileInstruction() != null)
                VisitLoopWhileInstruction(context.loopWhileInstruction());
            else if (context.dropInstruction() != null)
                VisitDropInstruction(context.dropInstruction());
            else if (context.moveInstruction() != null)
                VisitMoveInstruction(context.moveInstruction());
            else if (context.pickUpInstruction() != null)
                VisitPickUpInstruction(context.pickUpInstruction());
            else if (context.turnInstruction() != null)
                VisitTurnInstruction(context.turnInstruction());
            return 0;
        }

        public override object VisitLoopInstruction([NotNull] RobotGrammarParser.LoopInstructionContext context)
        {
            int cnt = int.Parse(VisitRepeatCnt(context.repeatCnt()).ToString());
            for (int i=0; i<cnt; i++)
            {
                VisitInstructionSet(context.instructionSet());
            }
            return 0;
        }

        public override object VisitLoopWhileInstruction([NotNull] RobotGrammarParser.LoopWhileInstructionContext context)
        {
            /*  !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
             */
            return base.VisitLoopWhileInstruction(context);
        }

        public override object VisitDropInstruction([NotNull] RobotGrammarParser.DropInstructionContext context)
        {
            int itemId = int.Parse(VisitItemId(context.itemId()).ToString());
            Game.DropItem(itemId);
            return 0;
        }

        public override object VisitMoveInstruction([NotNull] RobotGrammarParser.MoveInstructionContext context)
        {
            int amount = int.Parse(VisitMoveAmount(context.moveAmount()).ToString());
            
            // just to test if it works / moveCmd.Do() won't be executed here
             
            MoveCommand moveCmd = new MoveCommand(Game, amount);
            moveCmd.Do();  
            //Game.MoveRobot(amount);
            return 0;
        }

        public override object VisitPickUpInstruction([NotNull] RobotGrammarParser.PickUpInstructionContext context)
        {
            Game.PickUpItem();
            return 0;
        }

        public override object VisitTurnInstruction([NotNull] RobotGrammarParser.TurnInstructionContext context)
        {
            Model.Robot.TurnDir dir = (Model.Robot.TurnDir)VisitDir(context.dir());
            Game.TurnRobot(dir);
            return 0;
        }

        public override object VisitItemId([NotNull] RobotGrammarParser.ItemIdContext context)
        {
            return context.GetText();
        }

        public override object VisitDir([NotNull] RobotGrammarParser.DirContext context)
        {
            if (context.leftDir() != null)
            {
                return VisitLeftDir(context.leftDir());
            } else
            {
                return VisitRightDir(context.rightDir());
            }
        }

        public override object VisitLeftDir([NotNull] RobotGrammarParser.LeftDirContext context)
        {
            return Model.Robot.TurnDir.LEFT;
        }

        public override object VisitRightDir([NotNull] RobotGrammarParser.RightDirContext context)
        {
            return Model.Robot.TurnDir.RIGHT;
        }

        public override object VisitMoveAmount([NotNull] RobotGrammarParser.MoveAmountContext context)
        {
            return context.GetText();
        }

        public override object VisitRepeatCnt([NotNull] RobotGrammarParser.RepeatCntContext context)
        {
            return context.GetText();
        }

        public override object VisitCondition([NotNull] RobotGrammarParser.ConditionContext context)
        {
            return base.VisitCondition(context);
        }

        //public object Visit(IParseTree tree)
        //{
        //    return base.Visit(tree);
        //}

        //public object VisitTerminal(ITerminalNode node)
        //{
        //    throw new NotImplementedException();
        //}

        //public object VisitChildren(IRuleNode node)
        //{
        //    throw new NotImplementedException();
        //}

        //public object VisitErrorNode(IErrorNode node)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
