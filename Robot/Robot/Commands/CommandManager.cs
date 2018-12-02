﻿using Robot.Grammar;
using Robot.Model;
using Robot.Utility;
using Robot.Visitors;
using System.Collections.Generic;

namespace Robot.Commands
{
    class CommandManager
    {
        public Dictionary<string, List<CommandBase>> declaredFunctions;

        private Stack<CommandList> contextStack;

        private CommandList currentCommandList { get { return contextStack?.Peek(); } }
        private CommandList progCmdList;

        private List<CommandBase> _cmdList;
        private int doIndex;
        private int undoIndex;

        private CommandBase nextCmd { get;  set; }

        public CommandManager(Game game, 
            Dictionary<string, List<Parameter>> functionParameters, 
            RobotGrammarParser.ProgramContext ctx)
        {
            
            contextStack = new Stack<CommandList>();

            var functionVisitor = new FunctionVisitor(game, functionParameters, 
                (CommandList cmdList) =>
                {
                    contextStack.Push(cmdList);
                },
                (CommandList cmdList) => contextStack.Pop());
            functionVisitor.VisitProgram(ctx);
            declaredFunctions = functionVisitor.declaredFunctions;
            RobotControllerVisitor robotControllerVisitor = new RobotControllerVisitor(game,
                declaredFunctions,
                (CommandList cmdList) =>
                {
                    contextStack.Push(cmdList);
                },
                (CommandList cmdList) => contextStack.Pop());
            List<CommandBase> commands = (List<CommandBase>)robotControllerVisitor.VisitProgram(ctx);

            _cmdList = commands;
            progCmdList = new CommandList(commands);
            contextStack.Push(progCmdList);
            doIndex = 0;
            undoIndex = -1;
        }

      
        // run the next command (run step by step if it's not a simple command)
        public void DoCommand()
        {
            if (currentCommandList.AllDone() && contextStack.Count > 1)
            {
                contextStack.Pop();
            }
            currentCommandList.Do();
            //if (_cmdList.Count > 0)
            //{
            //    if (doIndex == _cmdList.Count)
            //    {
            //        return;
            //    }
            //    _cmdList[doIndex].Do();
            //    undoIndex = doIndex;
            //    if (_cmdList[doIndex].Done)
            //    {
            //        doIndex++;
            //    }
            //}
        }

        // undo the last command (step by step if it's not a simple command)
        public void UndoCommand()
        {
            if (currentCommandList.AllUndone() && contextStack.Count > 1)
            {
                contextStack.Pop();
            }
            currentCommandList.Undo();
            //if (_cmdList.Count > 0)
            //{
            //    if (undoIndex < 0)
            //    {
            //        return;
            //    }
            //    _cmdList[undoIndex].Undo();
            //    doIndex = undoIndex;
            //    if (_cmdList[undoIndex].Undone)
            //    {
            //        undoIndex--;
            //    }
            //}
        }

        //  run the whole program (starting after the last executed command)
        public void RunProg()  
        {
            while (contextStack.Count > 1)
            {
                currentCommandList.DoAll();
                if (currentCommandList.AllDone())
                {
                    contextStack.Pop();
                }
            }
            progCmdList.DoAll();
            //while (doIndex < _cmdList.Count)
            //{
            //    _cmdList[doIndex].DoAll();
            //    doIndex++;
            //}
            //undoIndex = doIndex - 1;

        }

        // undo the whole program (starting with the last executed command)
        public void UndoProg() 
        {
            while (contextStack.Count > 1)
            {
                currentCommandList.UndoAll();
                if (currentCommandList.AllUndone())
                {
                    contextStack.Pop();
                }
            }
            progCmdList.UndoAll();
            //while (undoIndex >= 0)
            //{
            //    _cmdList[undoIndex].UndoAll();
            //    undoIndex--;
            //}
            //doIndex = undoIndex + 1;
        }

        // run the next command (run all contained commands if it's not simple)
        public void DoAll ()
        {
            if (currentCommandList.AllDone() && contextStack.Count > 1)
            {
                contextStack.Pop();
            }
            currentCommandList.DoAll();
            //if (_cmdList.Count > 0)
            //{
            //    if (doIndex == _cmdList.Count)
            //    {
            //        return;
            //    }
            //    _cmdList[doIndex].DoAll();
            //    undoIndex = doIndex;
            //    //if (commandList[index].Done)
            //    //{
            //    doIndex++;
            //    //}
            //}
        }

        // undo the last command (undo all contained command if it's not simple)
        public void UndoAll()
        {
            if (currentCommandList.AllUndone() && contextStack.Count > 1)
            {
                contextStack.Pop();
            }
            currentCommandList.UndoAll();
            //if (_cmdList.Count > 0)
            //{
            //    if (undoIndex < 0)
            //    {
            //        return;
            //    }
            //    _cmdList[undoIndex].UndoAll();
            //    doIndex = undoIndex;
            //    //if (commandList[undoIndex].Undone)
            //    //{
            //    undoIndex--;
            //    //}
            //}
        }

        public void AddCommand(CommandBase cmd)
        {
            _cmdList.Add(cmd);

            //if (commandList.Count == 1)
            //{
            //    if (commandList[0] is ICommandList)
            //    {
            //        nextCmd = ((ICommandList)commandList[0]).nextCmd();
            //    } else
            //    {
            //        nextCmd = commandList[0];
            //    }
            //}
        }

    }
}
