using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Model
{
    public class Game  // deep copy
    {
        public Robot Player;
        public Board Board;

        public Game (int boardWith, int boardHeight)
        {
            Board = new Board(boardWith, boardHeight);
            Player = new Robot(Board, Robot.MoveDir.RIGHT);
            //Board.Init1();
        }

        public Game (Game other)
        {
            Board = new Board(other.Board);
            Player = new Robot(Board, other.Player);
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

        // get deep copy of the instance
        public Game Clone()
        {
            //Game gm = new Game(0, 0);
            //gm.Board = 
            return new Game(this);
        }
    }
}
