using Robot.Grammar;
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
        }

        // undo the last command (step by step if it's not a simple command)
        public void UndoCommand()
        {
            if (currentCommandList.AllUndone() && contextStack.Count > 1)
            {
                contextStack.Pop();
            }
            currentCommandList.Undo();
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
        }

        // run the next command (run all contained commands if it's not simple)
        public void DoAll ()
        {
            if (currentCommandList.AllDone() && contextStack.Count > 1)
            {
                contextStack.Pop();
            }
            currentCommandList.DoAll();
        }

        // undo the last command (undo all contained command if it's not simple)
        public void UndoAll()
        {
            if (currentCommandList.AllUndone() && contextStack.Count > 1)
            {
                contextStack.Pop();
            }
            currentCommandList.UndoAll();
        }

        public void AddCommand(CommandBase cmd)
        {
            _cmdList.Add(cmd);

        }

    }
}
