using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Model
{
    public class Field
    {
        public int Column { get; private set; }
        public int Row { get; }
        Item item;
        bool itemPickedUp;

        public Field (int col, int row, Item item = null)
        {
            Column = col;
            Row = row;
            this.item = item;
            itemPickedUp = false;
        }

        public Item GetItem()
        {
            if(!itemPickedUp && item != null)
            {
                return item;
            } else
            {
                return null;
            }
        }

        public bool PutItem(Item item)
        {
            if(this.item == null)
            {
                this.item = item;
                return true;
            } else
            {
                return false;
            }
        }
        public bool HasItem()
        {
            return item != null;
        }

        public virtual bool AcceptsRobot()
        {
            return true;
        }
    }
}
