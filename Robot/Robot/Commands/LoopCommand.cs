using Robot.Model;
using System.Collections.Generic;
using System;

namespace Robot.Commands
{
    class LoopCommand : CommandBase , ICommandList
    {
        //private int repeatCnt;
        //private List<CommandBase> commands;
        //private Game gameRef;

        private CommandList loopManager;
        
        public event Action<CommandList> ListContextEntered;
        public event Action<CommandList> ListContextExited;

        //private LoopManager loopManager;

        public override bool Done { get { return loopManager.AllDone(); } }
        public override bool Undone { get { return loopManager.AllUndone(); } }

        public LoopCommand(Game game, int repeatCnt, List<CommandBase> commands)
        {
            //this.repeatCnt = repeatCnt;
            //this.commands = new List<CommandBase>(commands);
            //this.gameRef = game;
            this.loopManager = new CommandList(/*game,*/ commands, repeatCnt);
            //this.loopManager = new LoopManager(game, repeatCnt, commands);
        }

        public override void Do()
        {
            loopManager.Do();
        }

        public override void Undo()
        {
            loopManager.Undo();
        }

        public override void DoAll()
        {
            loopManager.DoAll();
        }

        public override void UndoAll()
        {
            loopManager.UndoAll();
        }

        //public CommandBase nextCmd()
        //{
        //    return loopManager.getNext();
        //}

        //public CommandBase prevCmd()
        //{
        //    return loopManager.getPrev();
        //}

      
    }
}
