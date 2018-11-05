using Robot.Model;
using System.Collections.Generic;

namespace Robot.Commands
{
    public class LoopManager 
    {
        private int repeatCnt;
        private List<CommandBase> commands;

        private Game gameRef;

        private int iteration = 1;
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
            if (iteration < repeatCnt && index == commands.Count)
            {
                iteration++;
                index = 0;
            }
            commands[index].Do();
            index++;
        }

        public void Undo()
        {
            if (iteration == 1 && index == 0)
            {
                return;
            }
            if (iteration > 1 && index == 0)
            {
                iteration--;
                index = commands.Count;
            }
            commands[index - 1].Undo();
            index--;
        }

        public void DoAll()
        {
            //while(iteration < repeatCnt || index != commands.Count)
            //{
            //    Do();
            //}
            for (; iteration<=repeatCnt; iteration++)
            {
                for (; index < commands.Count; index++)
                {
                    commands[index].DoAll();
                }
                index = 0;
            }
            iteration = repeatCnt;
            index = commands.Count;
        }

        public void UndoAll()
        {
            //while (iteration > 1 || index > 0)
            //{
            //    Undo();
            //}
            for (; iteration>0; iteration--)
            {
                for (; index>0; index--)
                {
                    commands[index - 1].UndoAll();
                }
                index = commands.Count;
            }
            iteration = 1;
            index = 0;
        }

        public CommandBase getNext()
        {
            if (commands[index] is ICommandList)
            {
                return ((ICommandList)commands[index]).nextCmd();
            } else {
                return null;
            }
        }

        public CommandBase getPrev()
        {
            if (commands[index - 1] is ICommandList)
            {
                return ((ICommandList)commands[index]).prevCmd();
            } else {
                return null;
            }
        }

        public bool AllDone()
        {
            return iteration == repeatCnt && index == commands.Count;
        }

        public bool AllUndone()
        {
            return iteration == 1 && index == 0;
        }

    }
}
