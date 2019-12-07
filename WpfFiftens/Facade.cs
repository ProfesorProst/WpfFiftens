using System;

namespace WpfFiftens
{
    class Facade
    {
        int rows, colums;
        public int[,] board { private set; get; }
        Creator creator;
        ITable table;
        CommandSave commandS;
        CommandRandom commandR;
        Invoker invoker;

        //private static Facade instance = null;

        //public static Facade Instance(int row,int col, int lvl, int clearlvl)
        //{
        //    if (instance == null)
        //    {
        //        instance = new Facade(row,col,lvl, clearlvl);
        //    }
        //    return instance;
        //}


        public Facade(int row, int col, int lvl,int clearlvl)
        {
            this.rows = row;
            this.colums = col;
            bool gamestatus = false;
            do
            {
                creator = new Creator(colums , rows);
                table = creator.Create(lvl);
                board = table.GetBoard();
                gamestatus = equal(table.GetBoard());  
            }
            while (gamestatus == false);
            commandS = new CommandSave(table);
            commandR = new CommandRandom(creator, clearlvl, table);
            invoker = new Invoker();
            invoker.SetCommand(commandS);
        }

        public bool Rouler(int firstnumber, int secondnumber, bool a)
        {
            if (a == true) return equal(table.GetBoard());
            int firstnumberrow = 0, firstnumbercol = 0, secondnumberrow = 0, secondnumbercol = 0;
            for (int i = 0; i < board.GetLength(0); i++)
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i, j] == firstnumber)
                    {
                        firstnumberrow = j;
                        firstnumbercol = i;
                    }
                }
            for (int i = 0; i < board.GetLength(0); i++)
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i, j] == secondnumber)
                    {
                        secondnumberrow = j;
                        secondnumbercol = i;
                    }
                }
            try
            {
                Saver();
                if (((firstnumberrow - secondnumberrow) == 0) && ((firstnumbercol - secondnumbercol) == 1)) table.Down();
                if (((firstnumberrow - secondnumberrow) == 0) && ((firstnumbercol - secondnumbercol) == -1)) table.Up();
                if (((firstnumberrow - secondnumberrow) == 1) && ((firstnumbercol - secondnumbercol) == 0)) table.Right();
                if (((firstnumberrow - secondnumberrow) == -1) && ((firstnumbercol - secondnumbercol) == 0)) table.Left();
                Saver();
            }
            catch (Exception e) { }
            if (equal(table.GetBoard())) return true;
            return false;
        }

        public void Random()
        {
            invoker.SetCommand(commandR);
            invoker.Run();
            board = table.GetBoard();
        }

        public void Saver()
        {
            invoker.SetCommand(commandS);
            invoker.Run();
        }

        public void Cancel()
        {
            invoker.SetCommand(commandS);
            invoker.Cancel();
            board = table.GetBoard();
        }


        private static bool equal(int[,] a)
        {
            ITable table = new Table(a.GetLength(0), a.GetLength(1));
            Creator creatorempty = new Creator(a.GetLength(0), a.GetLength(1));
            creatorempty.Create(0);
            for (int i = 0; i < a.GetLength(0); ++i)
            {
                for (int j = 0; j < a.GetLength(1); ++j)
                    if (a[i, j] != table.GetBoard()[i, j]) return true;
            }
            return false;
        }
    }
}
