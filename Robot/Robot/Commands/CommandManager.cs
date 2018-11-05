using System.Collections.Generic;

namespace Robot.Commands
{
    class CommandManager
    {

        private Stack<CommandList> contextStack;

        private CommandList currentCommandList { get { return contextStack?.Peek(); } }
        private CommandList progCmdList;

        private List<CommandBase> _cmdList;
        private int doIndex;
        private int undoIndex;

        private CommandBase nextCmd { get;  set; }

        public CommandManager(List<CommandBase> commands)
        {
            //Reset();
            //_cmdList = new List<CommandBase>();
            _cmdList = commands;
            contextStack = new Stack<CommandList>();
            progCmdList = new CommandList(commands);
            contextStack.Push(progCmdList);
            doIndex = 0;
            undoIndex = -1;
        }

        //public void Reset()
        //{
        //    _cmdList = new List<CommandBase>();
        //    doIndex = 0;
        //    undoIndex = -1;
        //}

        // run the next command (run step by step if it's not a simple command)
        public void DoCommand()
        {
            //currentCommandList.Do();
            if (_cmdList.Count > 0)
            {
                if (doIndex == _cmdList.Count)
                {
                    return;
                }
                _cmdList[doIndex].Do();
                undoIndex = doIndex;
                if (_cmdList[doIndex].Done)
                {
                    doIndex++;
                }
            }
        }

        // undo the last command (step by step if it's not a simple command)
        public void UndoCommand()
        {
            //currentCommandList.Undo();
            if (_cmdList.Count > 0)
            {
                if (undoIndex < 0)
                {
                    return;
                }
                _cmdList[undoIndex].Undo();
                doIndex = undoIndex;
                if (_cmdList[undoIndex].Undone)
                {
                    undoIndex--;
                }
            }
        }

        //  run the whole program (starting after the last executed command)
        public void RunProg()  
        {
            //progCmdList.DoAll();
            while (doIndex < _cmdList.Count)
            {
                _cmdList[doIndex].DoAll();
                doIndex++;
            }
            undoIndex = doIndex - 1;

        }

        // undo the whole program (starting with the last executed command)
        public void UndoProg() 
        {
            //progCmdList.UndoAll();
            while (undoIndex >= 0)
            {
                _cmdList[undoIndex].UndoAll();
                undoIndex--;
            }
            doIndex = undoIndex + 1;
        }

        // run the next command (run all contained commands if it's not simple)
        public void DoAll ()
        {
            //currentCommandList.DoAll();
            if (_cmdList.Count > 0)
            {
                if (doIndex == _cmdList.Count)
                {
                    return;
                }
                _cmdList[doIndex].DoAll();
                undoIndex = doIndex;
                //if (commandList[index].Done)
                //{
                doIndex++;
                //}
            }
        }

        // undo the last command (undo all contained command if it's not simple)
        public void UndoAll()
        {
            //currentCommandList.UndoAll();
            if (_cmdList.Count > 0)
            {
                if (undoIndex < 0)
                {
                    return;
                }
                _cmdList[undoIndex].UndoAll();
                doIndex = undoIndex;
                //if (commandList[undoIndex].Undone)
                //{
                undoIndex--;
                //}
            }
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
