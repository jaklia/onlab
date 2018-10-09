using Robot.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Robot.Model.Robot;

namespace Robot.Commands
{
    class TurnCommand : CommandBase
    {
        Game gameRef;
        TurnDir turnDir, reverseDir;

        public TurnCommand(Game game, TurnDir dir)
        {
            gameRef = game;
            turnDir = dir;
            reverseDir = (dir == TurnDir.RIGHT) 
                 ? TurnDir.LEFT 
                 : dir == TurnDir.LEFT ? TurnDir.RIGHT : TurnDir.BACK;
        }

        public override void Do()
        {
            gameRef.TurnRobot(turnDir);
        }

        public override void Undo()
        {
            gameRef.TurnRobot(reverseDir);
        }
    }
}
