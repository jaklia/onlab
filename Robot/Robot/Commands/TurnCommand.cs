using Robot.Model;

namespace Robot.Commands
{
    class TurnCommand : SimpleCommand
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
            _done = true;
        }

        public override void Undo()
        {
            gameRef.TurnRobot(reverseDir);
            _done = false;
        }
    }
}
