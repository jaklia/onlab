﻿namespace Robot.Model
{
    public class Map
    {
        public int Height { get; private set; }
        public int Width { get; private set; }
        Field[][] Fields;
        Field start, dest;

        public Field Finish { get { return dest; } }
        public Field Start {  get { return start; } }
        

        public Map(int height, int width)
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

        public Map (Map other)
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
                        Fields[i][j] = other.Fields[i][j].Clone();
                    }
                }
            }
            start = Fields[other.start.Row][other.start.Column];
            dest = Fields[other.dest.Row][other.dest.Column];
        }

        public Field GetField(int row, int col)
        {
            return Fields[row][col];
        }

        public void SetStartField(int row, int col)
        {
            start = Fields[row][col];
        }

        public void SetWall(int row, int col)
        {
            Fields[row][col] = new Wall(row, col);
        }

        public void SetFinishField(int row, int col)
        {
            dest = Fields[row][col];
        }

        public void Key(int row, int col)
        {
            Fields[row][col].PutItem(new Item(0, "key"));
        }

        public void Init1()
        {
            start = Fields[0][0];
            dest = Fields[Height - 1][Width - 1];
            Fields[3][4].item = new Item(0, "key");
            Fields[0][2] = new Wall(0, 2);
            Fields[1][3] = new Wall(1, 3);
            Fields[2][4] = new Wall(2, 4);
            Fields[3][5] = new Wall(3, 5);
            Fields[1][0] = new Wall(1, 0);
            Fields[2][1] = new Wall(2, 1);
            Fields[3][2] = new Wall(3, 2);
            Fields[4][6] = new Wall(4, 6);
            Fields[5][6] = new Wall(5, 6);
            Fields[6][1] = new Wall(6, 1);
            Fields[6][6] = new Wall(6, 6);

            Fields[7][1] = new Wall(7, 1);
            Fields[7][2] = new Wall(7, 2);
            Fields[7][5] = new Wall(7, 5);
            Fields[7][7] = new Wall(7, 7);
            Fields[8][3] = new Wall(8, 3);
            Fields[9][4] = new Wall(9, 4);
            Fields[9][8] = new Wall(9, 8);
        }
        public void Init2()
        {
            start = Fields[0][0];
            dest = Fields[Height - 1][Width - 1];
            Fields[3][4].item = new Item(0, "key");
            Fields[0][2] = new Wall(0, 2);
            Fields[1][3] = new Wall(1, 3);
            Fields[2][4] = new Wall(2, 4);
            Fields[3][5] = new Wall(3, 5);
            Fields[1][0] = new Wall(1, 0);
            Fields[2][1] = new Wall(2, 1);
            Fields[3][2] = new Wall(3, 2);
            Fields[4][6] = new Wall(4, 6);
            Fields[5][6] = new Wall(5, 6);
            Fields[6][1] = new Wall(6, 1);
            Fields[6][6] = new Wall(6, 6);

            Fields[7][1] = new Wall(7, 1);
            Fields[7][2] = new Wall(7, 2);
            Fields[7][9] = new Wall(7, 9);
            Fields[8][2] = new Wall(8, 2);
            Fields[8][7] = new Wall(8, 7);
            Fields[8][8] = new Wall(8, 8);
            Fields[9][3] = new Wall(9, 3);
        }
    }
}