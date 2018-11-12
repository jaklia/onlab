using Robot.Model;
using System.Collections.Generic;

namespace Robot.Commands
{
    public class CommandList
    {
        private int repeatCnt;
        private List<CommandBase> commands;

        //private Game gameRef;

        private int doIteration = 1;
        private int undoIteration = 1;
        private int doIndex = 0;
        private int undoIndex = -1;

        public CommandList(/*Game game,*/ List<CommandBase> commands, int repeatCnt = 1)
        {
            this.repeatCnt = repeatCnt;
            this.commands = new List<CommandBase>(commands);
            //this.gameRef = game;
        }

        public void Do()
        {
            if (doIteration == repeatCnt && doIndex == commands.Count)
            {
                return;
            }
            if (doIteration < repeatCnt && doIndex == commands.Count)
            {
                doIteration++;
                doIndex = 0;
                // re-init all contained cmd-s as if they were undone
                //   so they can be executed in the next iteration
                foreach (var cmd in commands)
                {
                    cmd.InitUndone();
                }
            }
            commands[doIndex].Do();
            undoIteration = doIteration;
            undoIndex = doIndex;
            //if (commands[doIndex].Done)
            //{
            doIndex++;
            //}
        }

        public void Undo()
        {
            if (undoIteration == 1 && undoIndex < 0)
            {
                return;
            }
            if (undoIteration > 1 && undoIndex < 0)
            {
                undoIteration--;
                undoIndex = commands.Count - 1;
                // re-init all contained cmd-s as if they were done
                //   so they can be undone in the next (previous) iteration
                foreach (var cmd in commands)
                {
                    cmd.InitDone();
                }
            }
            commands[undoIndex].Undo();
            doIteration = undoIteration;
            doIndex = undoIndex;
            //if (commands[undoIndex].Undone)
            //{
            undoIndex--;
            //}
        }

        public void DoAll()
        {
            for (; doIteration <= repeatCnt; doIteration++)
            {
                for (; doIndex < commands.Count; doIndex++)
                {
                    commands[doIndex].DoAll();
                }
                doIndex = 0;
                if (doIteration < repeatCnt)
                {
                    foreach (var cmd in commands)
                    {
                        cmd.InitUndone();
                    }
                }
            }
            doIteration = repeatCnt;
            doIndex = commands.Count;
            undoIteration = repeatCnt;
            undoIndex = commands.Count - 1;
        }

        public void UndoAll()
        {
            for (; undoIteration > 0; undoIteration--)
            {
                for (; undoIndex >= 0; undoIndex--)
                {
                    commands[undoIndex].UndoAll();
                }
                undoIndex = commands.Count - 1;
                if (undoIteration > 1)
                {
                    foreach (var cmd in commands)
                    {
                        cmd.InitDone();
                    }
                }
            }
            undoIteration = 1;
            undoIndex = -1;
            doIteration = 1;
            doIndex = 0;
        }

        /* set the list to "undone"
         *   so it could be executed again in the next iteration
         */
        public void SetUndone()
        {
            undoIteration = 1;
            undoIndex = -1;
            doIteration = 1;
            doIndex = 0;
            foreach (var cmd in commands)
            {
                cmd.InitUndone();
            }
        }

        /*  set the list to "done" */
        public void SetDone()
        {
            doIteration = repeatCnt;
            doIndex = commands.Count;
            undoIteration = repeatCnt;
            undoIndex = commands.Count - 1;
            foreach (var cmd in commands)
            {
                cmd.InitDone();
            }
        }

        //public CommandBase getNext()
        //{
        //    if (commands[index] is ICommandList)
        //    {
        //        return ((ICommandList)commands[index]).nextCmd();
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        //public CommandBase getPrev()
        //{
        //    if (commands[index - 1] is ICommandList)
        //    {
        //        return ((ICommandList)commands[index]).prevCmd();
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        public bool AllDone()
        {
            return doIteration == repeatCnt && doIndex == commands.Count;
        }

        public bool AllUndone()
        {
            return undoIteration == 1 && undoIndex < 0;
        }
    }
}
