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
        private int lastExecuted;

        public CommandManager()
        {
            Reset();
        }

        public void Reset()
        {
            commandList = new List<CommandBase>();
            lastExecuted = -1;
        }

        public bool DoCommand()
        {
            //System.Windows.MessageBox.Show(lastExecuted.ToString());
            if (commandList.Count > 0)
            {
                if (lastExecuted == commandList.Count - 1)
                {
                    // all commands are executed
                    // TODO: disable the |>| btn
                    return false;
                }
                lastExecuted++;
                commandList[lastExecuted].Do();
                return true;
            }
            // else: no cmds to execute
            return false;
        }

        public bool UndoCommand()
        {
            if (commandList.Count > 0)
            {
                if (lastExecuted < 0)
                {
                    // all command are undone
                    // TODO: disable the |<| btn
                    return false;
                }
                commandList[lastExecuted].Undo();
                lastExecuted--;
                return true;
            }
            return false;
        }

        public void DoAll()  /// start btn
        {
            if (commandList.Count > 0)
            {
                while (lastExecuted < commandList.Count - 1)
                {
                    lastExecuted++;
                    commandList[lastExecuted].Do();
                }
            }
        }

        public void UndoAll() // reset btn
        {
            if (commandList.Count > 0)
            {
                while(lastExecuted >= 0)
                {
                    commandList[lastExecuted].Undo();
                    lastExecuted--;
                }
            }
        }

        public void AddCommand(CommandBase cmd)
        {
            commandList.Add(cmd);
        }

    }
}
