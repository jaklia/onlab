using Robot.Model;
using System.Collections.Generic;
using System;

namespace Robot.Commands
{
    class LoopCommand : CommandBase, ICommandList
    {
        //private int repeatCnt;
        //private List<CommandBase> commands;

        //private Game gameRef;
        private LoopManager loopManager;

        public override bool Done { get { return loopManager.AllDone(); } }
        public override bool Undone { get { return loopManager.AllUndone(); } }

        public LoopCommand(Game game, int repeatCnt, List<CommandBase> commands)
        {
            //this.repeatCnt = repeatCnt;
            //this.commands = new List<CommandBase>(commands);
            //this.gameRef = game;
            this.loopManager = new LoopManager(game, repeatCnt, commands);
        }

        //  classes: LoopManager, FunctionManager      (CommandManager)
        // cmdlist context   interface:  do / undo / doAll / undoAll    
        //      --> ezeket a context izeket egy stack-be kell rakni
        //       es akk az aktualis context fuggvenyeit kell kikotni a gombokra

        //   add  doAll / undoAll  to every cmd   -->  minden egyszerubb lesz valszeg
        //       ha a kov cmd-nel nincs ertelme akk le kell tiltani a gombokat es kesz
        // esetleg a loopCmd-nak kell adni vmi interface-t

        // valszeg igy is kelleni fog az a context-es cucc
        // a kov cmd egy loopCmd, akk uj context-et kell hozzaadni
        // a cmdManagernek kell vmi fv, amivel ertesiteni lehet a context valtozasarol
        //    hogy tudja h melyik do / doAll  stb fv-t kell hivni

       

        public override void Do()
        {
            loopManager.Do();
            //if(loopManager.AllDone())
            //{
            //    OnDoAllCompleted();
            //}
        }

        public override void Undo()
        {
            loopManager.Undo();
            //if (loopManager.AllUndone())
            //{
            //    OnUndoAllCompleted();
            //}
        }

        public override void DoAll()
        {
            loopManager.DoAll();
            //OnDoAllCompleted();
        }

        public override void UndoAll()
        {
            loopManager.UndoAll();
            //OnUndoAllCompleted();
        }

        public CommandBase nextCmd()
        {
            return loopManager.getNext();
        }

        public CommandBase prevCmd()
        {
            return loopManager.getPrev();
        }

      
    }
}
