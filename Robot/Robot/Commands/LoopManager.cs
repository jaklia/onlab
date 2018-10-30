using Robot.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Commands
{
    public class LoopManager
    {
        private int repeatCnt;
        private List<CommandBase> commands;

        private Game gameRef;

        private int iteration = 0;
        private int index = 0;

        public LoopManager(Game game, int repeatCnt, List<CommandBase> commands)
        {
            this.repeatCnt = repeatCnt;
            this.commands = new List<CommandBase>(commands);
            this.gameRef = game;
        }

        public void Do()
        {
            if (iteration == repeatCnt && index == commands.Count)
            {
                return;
            }
            if (iteration < repeatCnt - 1 && index == commands.Count)
            {
                iteration++;
                index = 0;
            }
            commands[index].Do();
            index++;
        }

        public void Undo()
        {
            if (iteration == 0 && index == 0)
            {
                return;
            }
            if (iteration > 0 && index == 0)
            {
                iteration--;
                index = commands.Count;
            }
            commands[index - 1].Undo();
            index--;
        }

        public void DoAll()
        {
            while(iteration < repeatCnt || index != commands.Count)
            {
                Do();
            }
        }

        public void UndoAll()
        {
            while (iteration > 0 || index > 0)
            {
                Undo();
            }
        }

    }
}
