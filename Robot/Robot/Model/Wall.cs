using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Model
{
    public class Wall : Field
    {
        public Wall(int col, int row) : base(col, row)
        {
        }

        public override bool AcceptsRobot()
        {
            return false;
        }
    }
}
