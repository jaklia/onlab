using Robot.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Commands
{
    class MoveCommand : CommandBase
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
            gameRef.TurnRobot(Model.Robot.TurnDir.BACK);
            gameRef.MoveRobot(steps);
            gameRef.TurnRobot(Model.Robot.TurnDir.BACK);
        }
    }
}
