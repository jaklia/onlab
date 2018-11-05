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
        private int index;

        private CommandBase nextCmd { get;  set; }

        public CommandManager()
        {
            //Reset();
            commandList = new List<CommandBase>();
            index = 0;
        }

        public void Reset()
        {
            commandList = new List<CommandBase>();
            index = 0;
        }

        public void DoCommand()
        {
            if (commandList.Count > 0)
            {
                if (index == commandList.Count)
                {
                    return;
                }
                commandList[index].Do();
                if (commandList[index].Done)
                {
                    index++;
                }
            }
        }

        public void UndoCommand()
        {
            if (commandList.Count > 0)
            {
                if (index <= 0)
                {
                    return;
                }
                commandList[index - 1].Undo();
                if (commandList[index - 1].Undone)
                {
                    index--;
                }
            }
        }

        public void RunProg()  /// start btn
        {
            while (index < commandList.Count)
            {
                commandList[index].DoAll();
                index++;
            }
        }

        public void UndoProg() // reset btn
        {
            while (index > 0)
            {
                commandList[index - 1].UndoAll();
                index--;
            }
        }

        public void DoAll ()
        {
            if (commandList.Count > 0)
            {
                if (index == commandList.Count)
                {
                    return;
                }
                commandList[index].DoAll();
                if (commandList[index].Done)
                {
                    index++;
                }
            }
        }

        public void UndoAll()
        {
            if (commandList.Count > 0)
            {
                if (index <= 0)
                {
                    return;
                }
                commandList[index - 1].UndoAll();
                if (commandList[index - 1].Undone)
                {
                    index--;
                }
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
