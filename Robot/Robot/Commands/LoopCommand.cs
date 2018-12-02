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

        private CommandList cmdList;
        
        public event Action<CommandList> ListContextEntered;
        public event Action<CommandList> ListContextExited;  // ez valszeg nem kell itt

        //private LoopManager loopManager;

        public override bool Done { get { return cmdList.AllDone(); } }
        public override bool Undone { get { return cmdList.AllUndone(); } }

        public LoopCommand(Game game, int repeatCnt, List<CommandBase> commands)
        {
            //this.repeatCnt = repeatCnt;
            //this.commands = new List<CommandBase>(commands);
            //this.gameRef = game;
            this.cmdList = new CommandList(/*game,*/ commands, repeatCnt);
            //this.loopManager = new LoopManager(game, repeatCnt, commands);
        }

        public override void Do()
        {
            ListContextEntered?.Invoke(cmdList);
            cmdList.Do();
        }

        public override void Undo()
        {
            ListContextEntered?.Invoke(cmdList);
            cmdList.Undo();
        }

        public override void DoAll()
        {
            if (Undone)
            {
                ListContextEntered?.Invoke(cmdList);
            }
            cmdList.DoAll();
            ListContextExited?.Invoke(cmdList);
        }

        public override void UndoAll()
        {
            if (Done)
            {
                ListContextEntered?.Invoke(cmdList);
            }
            cmdList.UndoAll();
            ListContextExited?.Invoke(cmdList);
        }

        public override void InitDone()
        {
            cmdList.SetDone();
        }

        public override void InitUndone()
        {
            cmdList.SetUndone();
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
