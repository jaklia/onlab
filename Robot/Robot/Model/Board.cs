﻿using System;
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
                    Fields[i][j] = new Field(other.Fields[i][j]);
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
            Fields[1][1].item = new Item(0, "key");
            Fields[5][5] = new Wall(5, 5);
            Fields[5][6] = new Wall(5, 6);
            Fields[6][5] = new Wall(6, 5);
            Fields[6][6] = new Wall(6, 6);
        }
    }
}
