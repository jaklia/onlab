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
        public enum TurnDir { LEFT = -1, RIGHT = 1 };
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

        public void Move(int amount)
        {
            /*
             * x, y legyen property, és a getter/setter ellenőrizze a dolgokat 
             *  itt csak a  +=amount legyen      ??????
             *  
             *  x, y helyett  row/column
             */
            switch (Dir)
            {
                case MoveDir.UP:
                    //if (Row - amount >= 0)
                    //{
                        while (amount > 0)
                        {
                            MoveUp();
                            amount--;
                        }
                   // }
                    break;
                case MoveDir.RIGHT:
                    //if (Column + amount < Board.Width)
                    //{
                    while (amount > 0)
                    {
                        MoveRight();
                        amount--;
                    }
                    //}
                    break;
                case MoveDir.DOWN:
                   // if (Row + amount < Board.Height)
                   // {
                        while (amount > 0)
                        {
                            MoveDown();
                            amount--;
                        }
                   // }
                    break;
                case MoveDir.LEFT:
                   // if (Column - amount >= 0)
                  //  {
                        while (amount > 0)
                        {
                            MoveLeft();
                            amount--;
                        }
                  //  }
                    break;
                default:
                    break;
            }
        }
        private void MoveLeft()
        {
            if (Column > 0 && Board.GetField(Row, Column - 1).AcceptsRobot())
            {
                //System.Threading.Thread.Sleep(500);
                Pos = Board.GetField(Row, Column - 1);
            }
        }
        private void MoveRight()
        {
            if (Column < Board.Width - 1 && Board.GetField(Row, Column + 1).AcceptsRobot())
            {
               // System.Threading.Thread.Sleep(500);
                Pos = Board.GetField(Row, Column + 1);
            }
           // MessageBox.Show("");
        }
        private void MoveUp()
        {
            if (Row > 0 && Board.GetField(Row - 1, Column).AcceptsRobot())
            {
                //System.Threading.Thread.Sleep(500);
                Pos = Board.GetField(Row - 1, Column);
            }
            
        }
        private void MoveDown()
        {
            if (Row < Board.Height - 1 && Board.GetField(Row + 1, Column).AcceptsRobot())
            {
               // System.Threading.Thread.Sleep(500);
                Pos = Board.GetField(Row + 1, Column);
            }
            
        }

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
            }
            //System.Threading.Thread.Sleep(500);
        }

        public void PickUp()
        {
            Items.Add(Pos.PickUpItem());
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
