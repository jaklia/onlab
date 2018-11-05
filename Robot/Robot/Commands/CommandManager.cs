using Robot.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Commands
{
    class CommandManager
    {
        private List<CommandBase> commandList;
        private int doIndex;
        private int undoIndex;

        private CommandBase nextCmd { get;  set; }

        public CommandManager()
        {
            //Reset();
            commandList = new List<CommandBase>();
            doIndex = 0;
            undoIndex = -1;
        }

        public void Reset()
        {
            commandList = new List<CommandBase>();
            doIndex = 0;
            undoIndex = -1;
        }

        // run the next command (run step by step if it's not a simple command)
        public void DoCommand()
        {
            if (commandList.Count > 0)
            {
                if (doIndex == commandList.Count)
                {
                    return;
                }
                commandList[doIndex].Do();
                undoIndex = doIndex;
                if (commandList[doIndex].Done)
                {
                    //undoIndex = doIndex;
                    doIndex++;
                } /*else if (!commandList[doIndex].Done && !commandList[doIndex].Undone)
                {

                }*/
            }
        }

        // undo the last command (step by step if it's not a simple command)
        public void UndoCommand()
        {
            if (commandList.Count > 0)
            {
                if (undoIndex < 0)
                {
                    return;
                }
                commandList[undoIndex].Undo();
                doIndex = undoIndex;
                if (commandList[undoIndex].Undone)
                {
                    undoIndex--;
                }
            }
        }
        
        //  run the whole program (starting after the last executed command)
        public void RunProg()  
        {
            while (doIndex < commandList.Count)
            {
                commandList[doIndex].DoAll();
                doIndex++;
            }
            undoIndex = doIndex - 1;
            
        }

        // undo the whole program (starting with the last executed command)
        public void UndoProg() 
        {
            while (undoIndex >= 0)
            {
                commandList[undoIndex].UndoAll();
                undoIndex--;
            }
            doIndex = undoIndex + 1;
        }

        // run the next command (run all contained commands if it's not simple)
        public void DoAll ()
        {
            if (commandList.Count > 0)
            {
                if (doIndex == commandList.Count)
                {
                    return;
                }
                commandList[doIndex].DoAll();
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
            if (commandList.Count > 0)
            {
                if (undoIndex < 0)
                {
                    return;
                }
                commandList[undoIndex].UndoAll();
                doIndex = undoIndex;
                //if (commandList[undoIndex].Undone)
                //{
                undoIndex--;
                //}
            }
        }

        public void AddCommand(CommandBase cmd)
        {
            commandList.Add(cmd);

            if (commandList.Count == 1)
            {
                if (commandList[0] is ICommandList)
                {
                    nextCmd = ((ICommandList)commandList[0]).nextCmd();
                } else
                {
                    nextCmd = commandList[0];
                }
            }
        }

    }
}
