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
            commandList = new List<CommandBase>();
            lastExecuted = -1;
        }

        public void DoCommand()
        {
            if (commandList.Count > 0)
            {
                if (lastExecuted == commandList.Count - 1)
                {
                    // all commands are executed
                    // TODO: disable the |>| btn
                    return;
                }
                lastExecuted++;
                commandList[lastExecuted].Do();
            }
            // else: no cmds to execute
        }

        public void UndoCommand()
        {
            if (commandList.Count > 0)
            {
                if (lastExecuted < 0)
                {
                    // all command are undone
                    // TODO: disable the |<| btn
                    return;
                }
                commandList[lastExecuted].Undo();
                lastExecuted--;
            }
        }

    }
}
