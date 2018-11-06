
namespace Robot.Commands
{
    public abstract class CommandBase
    {

        public abstract bool Done { get;  }
        public abstract bool Undone { get;  }

        //public delegate void Completed();
        //public event Completed DoAllCompleted;
        //public event Completed UndoAllCompleted;

        //protected void OnDoAllCompleted()
        //{
        //    DoAllCompleted?.Invoke();
        //}

        //protected void OnUndoAllCompleted()
        //{
        //    UndoAllCompleted?.Invoke();
        //}

        public abstract void Do();

        public abstract void Undo();

        public abstract void DoAll();

        public abstract void UndoAll();
    }
}
