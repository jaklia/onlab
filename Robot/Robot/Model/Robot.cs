using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Robot.Model
{
    public class Robot
    {
        public enum MoveDir { UP = 0, RIGHT = 1, DOWN = 2, LEFT = 3 };
        public enum TurnDir { LEFT = -1, RIGHT = 1, BACK = 2 };
        List<Item> Items;
        
        public MoveDir Dir { get; private set; }
        public Field Pos { get; private set; }
        public int Column {
            get { return Pos.Column; }
        }
        public int Row {
            get { return Pos.Row; }
        }

        private Board Board;

        public Robot(Board board, MoveDir moveDir = MoveDir.RIGHT)
        {
            Board = board;
            Dir = moveDir;
            Pos = Board.GetField(0, 0);
            Items = new List<Item>();
        }

        public Robot (Board board, Robot other)
        {
            Board = board;
            Dir = other.Dir;
            Pos = new Field(other.Pos);
            Items = new List<Item>(other.Items);
        }
      

        /** return int instead of void (number of steps the robot could make)
        *     nem biztos h a robot meg tudja tenni az adott számú lépéseket
        *     tudni kell h mennyi lepest tudott megtenni, a parancsok visszavonasakor igy tudni
        *     fogjuk h hova kell visszaleptetni 
        */
        public int Move(int amount)
        {
            int total = amount;
            switch (Dir)
            {
                case MoveDir.UP:
                    while (amount > 0 && MoveUp())
                    {
                        // MoveUp();
                        amount--;
                    }
                    break;
                case MoveDir.RIGHT:
                    while (amount > 0 && MoveRight())
                    {
                        // MoveRight();
                        amount--;
                    }
                    break;
                case MoveDir.DOWN:
                    while (amount > 0 && MoveDown())
                    {
                        // MoveDown();
                        amount--;
                    }
                    break;
                case MoveDir.LEFT:
                    while (amount > 0 && MoveLeft())
                    {
                        // MoveLeft();
                        amount--;
                    }
                    break;
                default:
                    break;
            }
            return total - amount;
        }
        // return bool from the MoveXXX functions
        // to detect if the robot could step in that direction
        private bool MoveLeft()
        {
            if (Column > 0 && Board.GetField(Row, Column - 1).AcceptsRobot())
            {
                Pos = Board.GetField(Row, Column - 1);
                return true;
            }
            return false;
        }
        private bool MoveRight()
        {
            if (Column < Board.Width - 1 && Board.GetField(Row, Column + 1).AcceptsRobot())
            {
                Pos = Board.GetField(Row, Column + 1);
                return true;
            }
            return false;
        }
        private bool MoveUp()
        {
            if (Row > 0 && Board.GetField(Row - 1, Column).AcceptsRobot())
            {
                Pos = Board.GetField(Row - 1, Column);
                return true;
            }
            return false;
        }
        private bool MoveDown()
        {
            if (Row < Board.Height - 1 && Board.GetField(Row + 1, Column).AcceptsRobot())
            {
                Pos = Board.GetField(Row + 1, Column);
                return true;
            }
            return false;
        }

        //  turn.back  -->  180 fokos fordulat
        public void Turn(TurnDir turnDir)
        {
            switch (turnDir)
            {
                case TurnDir.LEFT:
                    Dir = (Dir == MoveDir.UP) ? Dir + 3 : Dir - 1;
                    break;
                case TurnDir.RIGHT:
                    Dir = (Dir == MoveDir.LEFT) ? Dir - 3 : Dir + 1;
                    break;
                case TurnDir.BACK:
                    Dir = (Dir == MoveDir.DOWN || Dir == MoveDir.LEFT) ? Dir - 2 : Dir + 2;
                    break;
            }
        }

        // return the id of the item, so we can save it in the pick up cmd
        //  when we want to undo the pick up cmd we
        // (undo)
        public int PickUp()
        {
            Items.Add(Pos.PickUpItem());
            return Items.Count - 1;
        }

        public void Drop(int itemId)
        {
           if( Pos.PutItem(Items[itemId]))
            {
                Items.RemoveAt(itemId);
            }
        }
       
    }
}
