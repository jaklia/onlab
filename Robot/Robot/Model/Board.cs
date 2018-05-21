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

        public Board (Board other)
        {
            Width = other.Width;
            Height = other.Height;
            Fields = new Field[Height][];
            for (int i = 0; i < Height; i++)
            {
                Fields[i] = new Field[Width];
                for (int j = 0; j < Width; j++)
                {
                    if (other.Fields[i][j] != null)
                    {
                        //Fields[i][j] = new Field(other.Fields[i][j]);
                        Fields[i][j] = other.Fields[i][j].Clone();
                    }
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
            Fields[4][3].item = new Item(0, "key");
            Fields[2][0] = new Wall(2, 0);
            Fields[3][1] = new Wall(3, 1);
            Fields[4][2] = new Wall(4, 2);
            Fields[5][3] = new Wall(5, 3);
            Fields[0][1] = new Wall(0, 1);
            Fields[1][2] = new Wall(1, 2);
            Fields[2][3] = new Wall(2, 3);
            Fields[6][4] = new Wall(6, 4);
            Fields[6][5] = new Wall(6, 5);
            Fields[1][6] = new Wall(1, 6);
            Fields[6][6] = new Wall(6, 6);

            Fields[1][7] = new Wall(1, 7);
            Fields[2][7] = new Wall(2, 7);
            Fields[5][7] = new Wall(5, 7);
            Fields[7][7] = new Wall(7, 7);
            Fields[3][8] = new Wall(3, 8);
            Fields[4][9] = new Wall(4, 9);
            Fields[8][9] = new Wall(8, 9);
        }
        public void Init2()
        {
            start = Fields[0][0];
            dest = Fields[Height - 1][Width - 1];
            Fields[4][3].item = new Item(0, "key");
            Fields[2][0] = new Wall(2, 0);
            Fields[3][1] = new Wall(3, 1);
            Fields[4][2] = new Wall(4, 2);
            Fields[5][3] = new Wall(5, 3);
            Fields[0][1] = new Wall(0, 1);
            Fields[1][2] = new Wall(1, 2);
            Fields[2][3] = new Wall(2, 3);
            Fields[6][4] = new Wall(6, 4);
            Fields[6][5] = new Wall(6, 5);
            Fields[1][6] = new Wall(1, 6);
            Fields[6][6] = new Wall(6, 6);

            Fields[1][7] = new Wall(1, 7);
            Fields[2][7] = new Wall(2, 7);
            Fields[9][7] = new Wall(9, 7);
            Fields[2][8] = new Wall(2, 8);
            Fields[7][8] = new Wall(7, 8);
            Fields[8][8] = new Wall(8, 8);
            Fields[3][9] = new Wall(3, 9);
        }
    }
}
