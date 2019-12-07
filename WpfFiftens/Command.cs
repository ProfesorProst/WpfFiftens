using System;

namespace WpfFiftens
{
    abstract class Command
    {
        public abstract void Execute();
        public abstract void Undo();
    }

    class CommandSave : Command
    {
        ITable itable;
        GameHistory gameHistory = new GameHistory();

        public CommandSave(ITable r)
        {
             itable=r;
        }

        public override void Execute()
        {
            try
            {
                gameHistory.History.Push(new BoardMemento(itable.GetBoard()));
            }
            catch (Exception e) {  }
        }

        public override void Undo()
        {
            try
            {
                if (gameHistory.History.Pop() != null) itable.LoadBoard(gameHistory.History.Pop());
            }
            catch (InvalidOperationException e) { }      
        }
    }

    class CommandRandom : Command
    {
        GameHistory gameHistory = new GameHistory();
        ITable itable;
        Creator creator;
        int count=0;

        public CommandRandom(Creator creator, int lvl, ITable itable)
        {
            this.creator = creator;
            this.itable = itable;
            switch (lvl)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    {
                        count = 1;
                        break;
                    }
                case 5:
                case 6:
                case 7:
                    {
                        count = 2;
                        break;
                    }
                case 8:
                case 9:
                case 10:
                    {
                        count = 3;
                        break;
                    }
                default:
                    {
                        count = 1;
                        break;
                    }
            }
        }

        public override void Execute()
        {
            try
            {
                gameHistory.History.Push(new BoardMemento((new Creator((Table)itable)).Create(count).GetBoard()));
            }
            catch (Exception e) { }
        }

        public override void Undo()
        {
            try
            {
                if (gameHistory.History.Pop() != null) itable.LoadBoard(gameHistory.History.Pop());
            }
            catch (InvalidOperationException e)
            {

            }
        }
    }

    class Invoker
    {
        Command command;

        public void SetCommand(Command c)
        {
            command = c;
        }

        public void Run()
        {
            command.Execute();
        }

        public void Cancel()
        {
            command.Undo();
        }
    }
}
