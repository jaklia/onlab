using Robot.Model;

namespace Robot.Commands
{
    class MoveCommand : SimpleCommand
    {
        private Game gameRef;

        private int totalAmount;
        private int steps;

        public MoveCommand(Game game, int amount)
        {
            gameRef = game;
            totalAmount = amount;
            steps = 0;
        }

        public override void Do()
        {
            steps = gameRef.MoveRobot(totalAmount);
        }

        public override void Undo()
        {
            gameRef.TurnRobot(TurnDir.BACK);
            gameRef.MoveRobot(steps);
            gameRef.TurnRobot(TurnDir.BACK);
        }
    }
}
