
namespace Robot.Commands
{
    public abstract class CommandBase
    {

        public abstract bool Done { get;  }
        public abstract bool Undone { get;  }


        public abstract void Do();

        public abstract void Undo();

        public abstract void DoAll();

        public abstract void UndoAll();

        public abstract void InitDone();

        public abstract void InitUndone();
    }
}
