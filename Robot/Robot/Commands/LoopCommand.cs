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
        public event Action<CommandList> ListContextExited;  // ez valszeg nem kell itt

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
            ListContextEntered?.Invoke(loopManager);
            loopManager.Do();
        }

        public override void Undo()
        {
            ListContextEntered?.Invoke(loopManager);
            loopManager.Undo();
        }

        public override void DoAll()
        {
            ListContextEntered?.Invoke(loopManager);
            loopManager.DoAll();
            ListContextExited?.Invoke(loopManager);
        }

        public override void UndoAll()
        {
            ListContextEntered?.Invoke(loopManager);
            loopManager.UndoAll();
            ListContextExited?.Invoke(loopManager);
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
