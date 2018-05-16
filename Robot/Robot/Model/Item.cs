using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Model
{
    public class Item
    {
        int id;
        string name;

        public Item(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public Item(Item other)
        {

            id = other.id;
            name = new string(other.name.ToCharArray());
        }
    }
}
