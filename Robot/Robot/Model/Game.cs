
namespace Robot.Model
{
    public class Game  // deep copy
    {
        public Robot Player;
        public Board Board;

        public Game (int boardHeight, int boardWith)
        {
            Board = new Board(boardHeight, boardWith);
            Player = new Robot(Board, MoveDir.RIGHT);
            //Board.Init1();
        }

        public Game (Game other)
        {
            Board = new Board(other.Board);
            Player = new Robot(Board, other.Player);
        }

        // return int instead of void
        public int MoveRobot (int moveAmount)
        {
            return Player.Move(moveAmount);
        }

        public void TurnRobot (TurnDir turnDir)
        {
            Player.Turn(turnDir);
        }

        // return item id
        public int PickUpItem ()
        {
            return Player.PickUp();
        }

        public void DropItem (int itemId)
        {
            Player.Drop(itemId);
        }

        // get deep copy of the instance
        public Game Clone()
        {
            return new Game(this);
        }
    }
}
