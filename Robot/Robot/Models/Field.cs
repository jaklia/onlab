namespace Robot.Model
{
    public class Field
    {
        public int Column { get; private set; }
        public int Row { get; }

        public Item item { get; set; }

        public Field (int row, int col, Item item = null)
        {
            Column = col;
            Row = row;
            this.item = item;
        }

        public Field (Field other)
        {
            Column = other.Column;
            Row = other.Row;
            if (other.item != null)
            {
                item = new Item(other.item);
            }
        }

        public Item PickUpItem ()
        {
            Item tmp = item;
            item = null;
            return tmp;
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

        public virtual Field Clone()
        {
            Item newItem = item == null ? null : new Item(item);
            Field f = new Field(Row, Column, newItem);
            return f;
        }
    }
}
