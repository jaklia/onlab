﻿using Robot.Model;
using System;
using System.Collections.Generic;

namespace Robot.Commands
{
    public class FunctionCommand : CommandBase, ICommandList
    {
        //private List<CommandBase> commands;
        //private Game gameRef;

        private CommandList cmdList;

        public event Action<CommandList> ListContextEntered;
        public event Action<CommandList> ListContextExited;

        public override bool Done { get { return cmdList.AllDone(); } }
        public override bool Undone { get { return cmdList.AllUndone(); } }

        public FunctionCommand(Game game, List<CommandBase> commands)
        {
            //gameRef = game;
            //this.commands = new List<CommandBase>(commands);
            cmdList = new CommandList(commands);
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
            ListContextEntered?.Invoke(cmdList);
            cmdList.DoAll();
            ListContextExited?.Invoke(cmdList);
        }
        
        public override void UndoAll()
        {
            ListContextEntered?.Invoke(cmdList);
            cmdList.UndoAll();
            ListContextExited?.Invoke(cmdList);
        }
        
    }
}
