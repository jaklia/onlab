using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Model
{
    public class Board
    {
        public int Height { get; private set; }
        public int Width { get; private set; }
        Field[][] Fields;
        Field start, dest;

        public Board(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            Fields = new Field[Height][];
            for (int i = 0; i < Height; i++)
            {
                Fields[i] = new Field[Width];
                for (int j = 0; j < Width; j++)
                {
                    Fields[i][j] = new Field(i, j);
                }
            }
        }

        public Field GetField(int col, int row)
        {
            return Fields[row][col];
        }

        public void Init1()
        {
            start = Fields[0][0];
            dest = Fields[Height - 1][Width - 1];
            Fields[1][1].PutItem(new Item(0, "key"));
        }
    }
}
