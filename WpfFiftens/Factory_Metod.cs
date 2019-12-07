using System;

namespace WpfFiftens
{
    interface ITable
    {
        bool Left();
        bool Up();
        bool Down();
        bool Right();
        int[,] GetBoard();
        void LoadBoard(BoardMemento boardmemento);
    }

    class Table : ITable
    {
        private int[,] board;
        private int height_zero, width_zero;

        public Table(int height, int width)
        {
            board = new int[height, width];

            int r = 1;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    board[i, j] = r;
                    r++;
                }
            }
            board[height - 1, width - 1] = 0;
            height_zero = height - 1;
            width_zero = width - 1;
        }

        public bool Left()
        {
            if ((width_zero - 1) > (board.GetLength(1) - 1) || (width_zero - 1) < 0) return false;
            Swap(ref board[height_zero, width_zero], ref board[height_zero, width_zero - 1]);
            width_zero += -1;
            return true;
        }

        public bool Up()
        {
            if ((height_zero - 1) > (board.GetLength(0) - 1) || (height_zero - 1) < 0) return false;
            Swap(ref board[height_zero, width_zero], ref board[height_zero - 1, width_zero]);
            height_zero += -1;
            return true;
        }

        public bool Down()
        {
            if ((height_zero + 1) > (board.GetLength(0) - 1) || (height_zero + 1) < 0) return false;
            Swap(ref board[height_zero, width_zero], ref board[height_zero + 1, width_zero]);
            height_zero += 1;
            return true;
        }

        public bool Right()
        {
            if ((width_zero + 1) > (board.GetLength(1) - 1) || (width_zero + 1) < 0) return false;
            Swap(ref board[height_zero, width_zero], ref board[height_zero, width_zero + 1]);
            width_zero += 1;
            return true;
        }

        private void Swap<T>(ref T a, ref T b)
        {
            T c = a;
            a = b;
            b = c;
        }

        public int[,] GetBoard()
        {
            return board;
        }

        public void LoadBoard(BoardMemento boardmemento)
        {
            board = boardmemento.boardsave;
            for (int i = 0; i < board.GetLength(0); i++)
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i, j] == 0)
                    {
                        height_zero = j;
                        width_zero = i;
                    }
                }
        }
    }

    abstract class ICreator
    {
        abstract public ITable Create(int countofswap);
    }

    class Creator : ICreator
    {
        Table table;
        enum side { up = 1, down = 2, right = 3, left = 4 };
        Array values = Enum.GetValues(typeof(side));
        Random random = new Random();
        side randomside_last = side.down;
        side randomside_last_last = side.down;

        public Creator(int width, int height)
        {
            table = new Table(height, width);
        }

        public Creator(Table table)
        {
            this.table = table;
        }

        override public ITable Create(int countofswap)
        {
            bool ifhave = false;
            for (int z = 0; z < countofswap;)
            {
                side randomside = (side)values.GetValue(random.Next(values.Length));
                if (randomside != randomside_last_last)
                {
                    switch (randomside)
                    {
                        case side.up:
                            {
                                ifhave = table.Up();
                                break;
                            }
                        case side.down:
                            {
                                ifhave = table.Down();
                                break;
                            }
                        case side.left:
                            {
                                ifhave = table.Left();
                                break;
                            }
                        case side.right:
                            {
                                ifhave = table.Right();
                                break;
                            }
                    }
                    if (ifhave == true) z++;
                    randomside_last_last = randomside_last;
                    randomside_last = randomside;
                    ifhave = false;
                }
            }
            return table;
        }

    }
}

