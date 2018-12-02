
namespace Robot.Model
{
    public class Game  // deep copy
    {
        public Robot Player;
        public Map map;

        public Game (int boardHeight, int boardWith)
        {
            map = new Map(boardHeight, boardWith);
            Player = new Robot(map, MoveDir.RIGHT);
            //Board.Init1();
        }

        public Game (Map map)
        {
            this.map = map;
            Player = new Robot(this.map, MoveDir.RIGHT);
        }

        public Game (Game other)
        {
            map = new Map(other.map);
            Player = new Robot(map, other.Player);
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
