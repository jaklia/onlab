﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Commands
{
    public abstract class SimpleCommand : CommandBase
    {

        protected bool _done = false;

        public override bool Done { get { return _done; } }
        public override bool Undone { get {return  !_done; } }

        // simple command:  DoAll must be the same as Do()

        public sealed override void DoAll()
        {
            Do();
        }
        
        public sealed override void UndoAll()
        {
            Undo();
        }
        

    }
}
