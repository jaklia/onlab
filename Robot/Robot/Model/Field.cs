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
        public Item Item { get; private set; }

        public Field (int col, int row)
        {
            Column = col;
            Row = row;
        }

        public virtual bool AcceptsRobot()
        {
            return true;
        }
    }
}
