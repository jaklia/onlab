﻿using System.Collections.Generic;

namespace Robot.Model
{
    public class Robot
    {
        List<Item> Items;
        
        public MoveDir Dir { get; private set; }
        public Field Pos { get; private set; }
        public int Column {
            get { return Pos.Column; }
        }
        public int Row {
            get { return Pos.Row; }
        }

        private Map map;

        public Robot(Map map, MoveDir moveDir = MoveDir.RIGHT)
        {
            this.map = map;
            Dir = moveDir;
            Pos = this.map.Start;
            Items = new List<Item>();
        }

        public Robot (Map map, Robot other)
        {
            this.map = map;
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
            if (Column > 0 && map.GetField(Row, Column - 1).AcceptsRobot())
            {
                Pos = map.GetField(Row, Column - 1);
                return true;
            }
            return false;
        }
        private bool MoveRight()
        {
            if (Column < map.Width - 1 && map.GetField(Row, Column + 1).AcceptsRobot())
            {
                Pos = map.GetField(Row, Column + 1);
                return true;
            }
            return false;
        }
        private bool MoveUp()
        {
            if (Row > 0 && map.GetField(Row - 1, Column).AcceptsRobot())
            {
                Pos = map.GetField(Row - 1, Column);
                return true;
            }
            return false;
        }
        private bool MoveDown()
        {
            if (Row < map.Height - 1 && map.GetField(Row + 1, Column).AcceptsRobot())
            {
                Pos = map.GetField(Row + 1, Column);
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
