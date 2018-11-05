using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Commands
{
    public abstract class SimpleCommand : CommandBase
    {
        // valszeg nem a legjobb megoldas
        public override bool Done { get;  } = true; 
        public override bool Undone { get;  } = true;

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
