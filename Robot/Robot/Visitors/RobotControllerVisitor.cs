using Robot.Grammar;
using System.Collections.Generic;
using Antlr4.Runtime.Misc;
using Robot.Model;
using Robot.Commands;
using System;

namespace Robot.Visitors
{
    class RobotControllerVisitor : RobotGrammarBaseVisitor<object>
    {
        Game Game;
        //CommandManager cmdManager;
       // List<CommandBase> commands;
        private Dictionary<string, List<CommandBase>> declaredFunctions;
        private Action<CommandList> onEnterContext;
        private Action<CommandList> onExitContext;
        
        public RobotControllerVisitor(Game game, 
            Dictionary<string, List<CommandBase>> declaredFunctions, 
            Action<CommandList> onEnterContext, Action<CommandList> onExitContext)
        {
            Game = game;
            //this.cmdManager = cmdManager;
            
            this.declaredFunctions = declaredFunctions;
            this.onEnterContext = onEnterContext;
            this.onExitContext = onExitContext;
        }

        public override object VisitProgram([NotNull] RobotGrammarParser.ProgramContext context)
        {
            List<CommandBase> commands = new List<CommandBase>();
            foreach (var item in context.progInstructionSet().progInstruction())
            {
                if (item.instruction() != null)
                {
                    commands.Add((CommandBase)VisitInstruction(item.instruction()));
                }
            }
            return commands;
        }

        public override object VisitInstructionSet([NotNull] RobotGrammarParser.InstructionSetContext context)
        {
            List<CommandBase> cmdList = new List<CommandBase>();
            foreach (var instruction in context.instruction())
            {
                cmdList.Add((CommandBase)VisitInstruction(instruction));
            }
            return cmdList;
        }

        public override object VisitInstruction([NotNull] RobotGrammarParser.InstructionContext context)
        {
            if (context.loopInstruction() != null)
                return VisitLoopInstruction(context.loopInstruction());
            else if (context.dropInstruction() != null)
                return VisitDropInstruction(context.dropInstruction());
            else if (context.moveInstruction() != null)
                return VisitMoveInstruction(context.moveInstruction());
            else if (context.pickUpInstruction() != null)
                return VisitPickUpInstruction(context.pickUpInstruction());
            else if (context.turnInstruction() != null)
                return VisitTurnInstruction(context.turnInstruction());
            else if (context.functionCall() != null)
                return VisitFunctionCall(context.functionCall());
            return null;
        }

        public override object VisitLoopInstruction([NotNull] RobotGrammarParser.LoopInstructionContext context)
        {
            int cnt = int.Parse(VisitRepeatCnt(context.repeatCnt()).ToString());
            List<CommandBase> cmdList = new List<CommandBase>();
            foreach (var instruction in context.instructionSet().instruction())
            {
                cmdList.Add((CommandBase)VisitInstruction(instruction));
            }
            LoopCommand loopCmd = new LoopCommand(Game, cnt, cmdList);
            ((ICommandList)loopCmd).ListContextEntered += onEnterContext;
            ((ICommandList)loopCmd).ListContextExited += onExitContext;
            return loopCmd;
        }


        public override object VisitFunctionCall([NotNull] RobotGrammarParser.FunctionCallContext context)
        {
            
            string name = context.functionName().GetText();
            FunctionCommand functionCmd = new FunctionCommand(Game, declaredFunctions[name]);
            ((ICommandList)functionCmd).ListContextEntered += onEnterContext;
            ((ICommandList)functionCmd).ListContextExited += onExitContext;
            return functionCmd;
        }

      
        public override object VisitDropInstruction([NotNull] RobotGrammarParser.DropInstructionContext context)
        {
            int itemId = int.Parse(VisitItemId(context.itemId()).ToString());
            DropCommand dropCmd = new DropCommand(Game, itemId);

            //cmdManager.AddCommand(new DropCommand(Game, itemId));
            //Game.DropItem(itemId);
            return dropCmd;
        }

        public override object VisitMoveInstruction([NotNull] RobotGrammarParser.MoveInstructionContext context)
        {
            int amount = int.Parse(VisitMoveAmount(context.moveAmount()).ToString());
            MoveCommand moveCmd = new MoveCommand(Game, amount);

            //cmdManager.AddCommand(new MoveCommand(Game, amount));
            //Game.MoveRobot(amount);
            return moveCmd;
        }

        public override object VisitPickUpInstruction([NotNull] RobotGrammarParser.PickUpInstructionContext context)
        {
            PickUpCommand pickUpCmd = new PickUpCommand(Game);

            //cmdManager.AddCommand(new PickUpCommand(Game));
            //Game.PickUpItem();
            return pickUpCmd;
        }

        public override object VisitTurnInstruction([NotNull] RobotGrammarParser.TurnInstructionContext context)
        {
            TurnDir dir = (TurnDir)VisitDir(context.dir());
            TurnCommand turnCmd = new TurnCommand(Game, dir);

            //cmdManager.AddCommand(new TurnCommand(Game, dir));
            //Game.TurnRobot(dir);
            return turnCmd;
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
            }
            else 
            {
                return VisitRightDir(context.rightDir());
            }
        }

        public override object VisitLeftDir([NotNull] RobotGrammarParser.LeftDirContext context)
        {
            return TurnDir.LEFT;
        }

        public override object VisitRightDir([NotNull] RobotGrammarParser.RightDirContext context)
        {
            return TurnDir.RIGHT;
        }

        public override object VisitMoveAmount([NotNull] RobotGrammarParser.MoveAmountContext context)
        {
            return context.GetText();
        }

        public override object VisitRepeatCnt([NotNull] RobotGrammarParser.RepeatCntContext context)
        {
            return context.GetText();
        }

        //public override object VisitCondition([NotNull] RobotGrammarParser.ConditionContext context)
        //{
        //    return base.VisitCondition(context);
        //}

    }
}






//public override object VisitProgram([NotNull] RobotGrammarParser.ProgramContext context)
//{
//    VisitFunctionDefinitions(context.functionDefinitions());
//    List<CommandBase> commands = new List<CommandBase>();
//    foreach (var instruction in context.instructionSet().instruction())
//    {
//        //cmdManager.AddCommand((CommandBase)VisitInstruction(instruction));
//        commands.Add((CommandBase)VisitInstruction(instruction));
//    }

//    //VisitInstructionSet(context.instructionSet());
//    return commands;
//}

//public override object VisitInstructionSet([NotNull] RobotGrammarParser.InstructionSetContext context)
//{
//    List<CommandBase> cmdList = new List<CommandBase>();
//    foreach (var instruction in context.instruction())
//    {
//        cmdList.Add((CommandBase)VisitInstruction(instruction));
//    }
//    return cmdList;
//}

//public override object VisitFunctionDefinitions([NotNull] RobotGrammarParser.FunctionDefinitionsContext context)
//{
//    foreach (var function in context.functionDef())
//    {
//          VisitFunctionDef(function);
//    }
//    return 0;
//}

//public override object VisitInstruction([NotNull] RobotGrammarParser.InstructionContext context)
//{
//    if (context.loopInstruction() != null)
//        return VisitLoopInstruction(context.loopInstruction());
//    else if (context.loopWhileInstruction() != null)
//        return VisitLoopWhileInstruction(context.loopWhileInstruction());
//    else if (context.dropInstruction() != null)
//        return VisitDropInstruction(context.dropInstruction());
//    else if (context.moveInstruction() != null)
//        return VisitMoveInstruction(context.moveInstruction());
//    else if (context.pickUpInstruction() != null)
//        return VisitPickUpInstruction(context.pickUpInstruction());
//    else if (context.turnInstruction() != null)
//        return VisitTurnInstruction(context.turnInstruction());
//    else if (context.functionCall() != null)
//        return VisitFunctionCall(context.functionCall());
//    return null;
//}

//public override object VisitLoopInstruction([NotNull] RobotGrammarParser.LoopInstructionContext context)
//{
//    int cnt = int.Parse(VisitRepeatCnt(context.repeatCnt()).ToString());
//    List<CommandBase> cmdList = new List<CommandBase>();
//    foreach (var instruction in context.instructionSet().instruction())
//    {
//        cmdList.Add((CommandBase)VisitInstruction(instruction));
//    }
//    LoopCommand loopCmd = new LoopCommand(Game, cnt, cmdList);
//    ((ICommandList)loopCmd).ListContextEntered += onEnterContext;
//    ((ICommandList)loopCmd).ListContextExited += onExitContext;
//    return loopCmd;
//}

//public override object VisitFunctionDef([NotNull] RobotGrammarParser.FunctionDefContext context)
//{
//    string name = context.functionName().GetText();
//    List<CommandBase> cmdList = new List<CommandBase>();
//    cmdList = (List<CommandBase>) VisitInstructionSet(context.instructionSet());
//    declaredFunctions[name] = cmdList;
//    return 0;
//}

//public override object VisitFunctionCall([NotNull] RobotGrammarParser.FunctionCallContext context)
//{
//    string name = context.functionName().GetText();
//    FunctionCommand functionCmd = new FunctionCommand(Game, declaredFunctions[name]);
//    ((ICommandList)functionCmd).ListContextEntered += onEnterContext;
//    ((ICommandList)functionCmd).ListContextExited += onExitContext;
//    return functionCmd;
//}

//public override object VisitLoopWhileInstruction([NotNull] RobotGrammarParser.LoopWhileInstructionContext context)
//{
//    /*  !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
//     */
//    return base.VisitLoopWhileInstruction(context);
//}

//public override object VisitDropInstruction([NotNull] RobotGrammarParser.DropInstructionContext context)
//{
//    int itemId = int.Parse(VisitItemId(context.itemId()).ToString());
//    DropCommand dropCmd = new DropCommand(Game, itemId);

//    //cmdManager.AddCommand(new DropCommand(Game, itemId));
//    //Game.DropItem(itemId);
//    return dropCmd;
//}

//public override object VisitMoveInstruction([NotNull] RobotGrammarParser.MoveInstructionContext context)
//{
//    int amount = int.Parse(VisitMoveAmount(context.moveAmount()).ToString());
//    MoveCommand moveCmd = new MoveCommand(Game, amount);

//    //cmdManager.AddCommand(new MoveCommand(Game, amount));
//    //Game.MoveRobot(amount);
//    return moveCmd;
//}

//public override object VisitPickUpInstruction([NotNull] RobotGrammarParser.PickUpInstructionContext context)
//{
//    PickUpCommand pickUpCmd = new PickUpCommand(Game);

//    //cmdManager.AddCommand(new PickUpCommand(Game));
//    //Game.PickUpItem();
//    return pickUpCmd;
//}

//public override object VisitTurnInstruction([NotNull] RobotGrammarParser.TurnInstructionContext context)
//{
//    TurnDir dir = (TurnDir)VisitDir(context.dir());
//    TurnCommand turnCmd = new TurnCommand(Game, dir);

//    //cmdManager.AddCommand(new TurnCommand(Game, dir));
//    //Game.TurnRobot(dir);
//    return turnCmd;
//}

//public override object VisitItemId([NotNull] RobotGrammarParser.ItemIdContext context)
//{
//    return context.GetText();
//}

//public override object VisitDir([NotNull] RobotGrammarParser.DirContext context)
//{
//    if (context.leftDir() != null)
//    {
//        return VisitLeftDir(context.leftDir());
//    } else
//    {
//        return VisitRightDir(context.rightDir());
//    }
//}

//public override object VisitLeftDir([NotNull] RobotGrammarParser.LeftDirContext context)
//{
//    return TurnDir.LEFT;
//}

//public override object VisitRightDir([NotNull] RobotGrammarParser.RightDirContext context)
//{
//    return TurnDir.RIGHT;
//}

//public override object VisitMoveAmount([NotNull] RobotGrammarParser.MoveAmountContext context)
//{
//    return context.GetText();
//}

//public override object VisitRepeatCnt([NotNull] RobotGrammarParser.RepeatCntContext context)
//{
//    return context.GetText();
//}

//public override object VisitCondition([NotNull] RobotGrammarParser.ConditionContext context)
//{
//    return base.VisitCondition(context);
//}