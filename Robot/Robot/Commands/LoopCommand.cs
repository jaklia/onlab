using Robot.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Commands
{
    class LoopCommand : CommandBase
    {
        private int repeatCnt;
        private List<CommandBase> commands;

        private Game gameRef;

        public LoopCommand(Game game, int repeatCnt, List<CommandBase> commands)
        {
            this.repeatCnt = repeatCnt;
            this.commands = new List<CommandBase>(commands);
            this.gameRef = game;
        }

        public override void Do()
        {
            for (int i=0; i<repeatCnt; i++)
            {
                foreach (var cmd in commands)
                {
                    cmd.Do();
                }
            }
        }

        public override void Undo()
        {
            for (int i=repeatCnt; i>0; i--)
            {
                for (int j=commands.Count - 1; j>=0; j--)
                {
                    commands[j].Undo();
                }
            }
        }
    }
}
