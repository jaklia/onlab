using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Model
{
    public class Game
    {
        public Robot Player;
        Board Board;

        public Game (int boardWith, int boardHeight)
        {
            Board = new Board(boardWith, boardHeight);
            Player = new Robot(Board, Robot.MoveDir.RIGHT);
        }

        public void MoveRobot (int moveAmount)
        {
            Player.Move(moveAmount);
        }

        public void TurnRobot (Robot.TurnDir turnDir)
        {
            Player.Turn(turnDir);
        }

        public void PickUpItem ()
        {
            Player.PickUp();
        }

        public void DropItem (int itemId)
        {
            Player.Drop(itemId);
        }

    }
}
