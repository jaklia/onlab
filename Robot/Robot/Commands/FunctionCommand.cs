using Robot.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Commands
{
    class FunctionCommand : CommandBase
    {
        private List<CommandBase> commands;
        private Game gameRef;

        public FunctionCommand(Game game, List<CommandBase> commands)
        {
            gameRef = game;
            this.commands = new List<CommandBase>(commands);
        }

        public override void Do()
        {
            foreach (var cmd in commands)
            {
                cmd.Do();
            }
        }

        public override void Undo()
        {
            for (int i=commands.Count-1; i>=0; i--)
            {
                commands[i].Undo();
            }
        }
    }
}
