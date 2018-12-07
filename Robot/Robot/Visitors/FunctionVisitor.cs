using Antlr4.Runtime.Misc;
using Robot.Commands;
using Robot.Grammar;
using Robot.Model;
using Robot.Utility;
using System;
using System.Collections.Generic;

namespace Robot.Visitors
{
    public class FunctionVisitor : RobotGrammarBaseVisitor<object>
    {
        Game Game;

        private Dictionary<string, List<Parameter>> functionParameters;

        public Dictionary<string, List<CommandBase>> declaredFunctions;
        private Action<CommandList> onEnterContext;
        private Action<CommandList> onExitContext;

        private string currentFunction = "";
      //  private List<CommandBase> currentCmdList;

        public FunctionVisitor(Game game, Dictionary<string, List<Parameter>> functionParameters, Action<CommandList> onEnterContext, Action<CommandList> onExitContext)
        {
            Game = game;

            this.functionParameters = functionParameters;
            declaredFunctions = new Dictionary<string, List<CommandBase>>();
            foreach (var item in functionParameters)
            {
                declaredFunctions[item.Key] = new List<CommandBase>();
            }

            //this.cmdManager = cmdManager;

            this.onEnterContext = onEnterContext;
            this.onExitContext = onExitContext;
        }

        public override object VisitProgram([NotNull] RobotGrammarParser.ProgramContext context)
        {

            foreach (var item in context.progInstructionSet().progInstruction())
            {
                if (item.functionDef() != null)
                {
                    VisitFunctionDef(item.functionDef());
                }
            }
            
            return 0;
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
            //else if (context.loopWhileInstruction() != null)
            //    return VisitLoopWhileInstruction(context.loopWhileInstruction());
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

        public override object VisitFunctionDef([NotNull] RobotGrammarParser.FunctionDefContext context)
        {
            string name = context.functionName().GetText();
            List<CommandBase> cmdList = new List<CommandBase>();
            currentFunction = name;
            cmdList = (List<CommandBase>)VisitInstructionSet(context.instructionSet());
            //declaredFunctions[name] = cmdList;
            declaredFunctions[name].AddRange(cmdList);
            currentFunction = "";
            return 0;
        }

        public override object VisitFunctionCall([NotNull] RobotGrammarParser.FunctionCallContext context)
        {
            string name = context.functionName().GetText();
            foreach (var param in context.parameterList().parameter())
            {
                
            }
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
        
        public override object VisitTurnInstruction([NotNull] RobotGrammarParser.TurnInstructionContext context)
        {
            TurnDir dir = (TurnDir)VisitDir(context.dir());
            TurnCommand turnCmd = new TurnCommand(Game, dir);
            
            return turnCmd;
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
        
        public override object VisitMoveAmount([NotNull] RobotGrammarParser.MoveAmountContext context)
        {
            return context.GetText();
        }

        public override object VisitRepeatCnt([NotNull] RobotGrammarParser.RepeatCntContext context)
        {
            return context.GetText();
        }

        public override object VisitPickUpInstruction([NotNull] RobotGrammarParser.PickUpInstructionContext context)
        {
            PickUpCommand pickUpCmd = new PickUpCommand(Game);

            return pickUpCmd;
        }

        public override object VisitItemId([NotNull] RobotGrammarParser.ItemIdContext context)
        {
            return context.GetText();
        }

        public override object VisitLeftDir([NotNull] RobotGrammarParser.LeftDirContext context)
        {
            return TurnDir.LEFT;
        }

        public override object VisitRightDir([NotNull] RobotGrammarParser.RightDirContext context)
        {
            return TurnDir.RIGHT;
        }

    }
}





/*
 using Antlr4.Runtime.Misc;
using Robot.Commands;
using Robot.Grammar;
using Robot.Model;
using Robot.Utility;
using System;
using System.Collections.Generic;

namespace Robot.Visitors
{
    public class FunctionVisitor : RobotGrammarBaseVisitor<object>
    {
        Game Game;

        private Dictionary<string, List<Parameter>> functionParameters;

        private Dictionary<string, List<CommandBase>> declaredFunctions;
        private Action<CommandList> onEnterContext;
        private Action<CommandList> onExitContext;

        private string currentFunction = "";
      //  private List<CommandBase> currentCmdList;

        public FunctionVisitor(Game game, Dictionary<string, List<Parameter>> functionParameters, Action<CommandList> onEnterContext, Action<CommandList> onExitContext)
        {
            Game = game;

            this.functionParameters = functionParameters;
            declaredFunctions = new Dictionary<string, List<CommandBase>>();
            foreach (var item in functionParameters)
            {
                declaredFunctions[item.Key] = new List<CommandBase>();
            }

            //this.cmdManager = cmdManager;

            declaredFunctions = new Dictionary<string, List<CommandBase>>();
            this.onEnterContext = onEnterContext;
            this.onExitContext = onExitContext;
        }


        public override object VisitFunctionDef([NotNull] RobotGrammarParser.FunctionDefContext context)
        {
            currentFunction = context.functionName().GetText();
            var res = base.VisitFunctionDef(context);
            currentFunction = "";
            return res;
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
            if (currentFunction != "")  // we are in a function
            {
                int itemId = int.Parse(context.itemId().GetText());
                DropCommand dropCmd = new DropCommand(Game, itemId);
                declaredFunctions[currentFunction].Add(dropCmd);
            }
            return base.VisitDropInstruction(context);
        }

        public override object VisitPickUpInstruction([NotNull] RobotGrammarParser.PickUpInstructionContext context)
        {
            if (currentFunction != "")
            {
                PickUpCommand pickUpCmd = new PickUpCommand(Game);
                declaredFunctions[currentFunction].Add(pickUpCmd);
            }
            return base.VisitPickUpInstruction(context);
        }

        public override object VisitMoveInstruction([NotNull] RobotGrammarParser.MoveInstructionContext context)
        {
            if (currentFunction != "")
            {
                MoveCommand moveCmd;
                int amount;
                if (context.moveAmount().parameterName() != null)
                {
                    amount = 0;
                } else
                {
                    amount = int.Parse(context.moveAmount().GetText());
                }
                
                moveCmd = new MoveCommand(Game, amount);
                declaredFunctions[currentFunction].Add(moveCmd);
            }
            return base.VisitMoveInstruction(context);
        }
        
        public override object VisitTurnInstruction([NotNull] RobotGrammarParser.TurnInstructionContext context)
        {
            if (currentFunction != "")
            {
                TurnDir dir = (TurnDir)VisitDir(context.dir());
                TurnCommand turnCmd = new TurnCommand(Game, dir);

            }
            
            //cmdManager.AddCommand(new TurnCommand(Game, dir));
            //Game.TurnRobot(dir);
            return base.VisitTurnInstruction(context);
        }

        public override object VisitLoopInstruction([NotNull] RobotGrammarParser.LoopInstructionContext context)
        {
            if (currentFunction != "")
            {
                if (context.repeatCnt().parameterName() != null)
                {

                } else
                {

                }
            }
            //    int cnt = int.Parse(VisitRepeatCnt(context.repeatCnt()).ToString());
            //    List<CommandBase> cmdList = new List<CommandBase>();
            //    foreach (var instruction in context.instructionSet().instruction())
            //    {
            //        cmdList.Add((CommandBase)VisitInstruction(instruction));
            //    }
            //    LoopCommand loopCmd = new LoopCommand(Game, cnt, cmdList);
            //    ((ICommandList)loopCmd).ListContextEntered += onEnterContext;
            //    ((ICommandList)loopCmd).ListContextExited += onExitContext;

            return base.VisitLoopInstruction(context);
        }
        
    }
}



     */
