using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfFiftens
{
    class BoardMemento               
    {
        public int[,] boardsave { get; private set; }
       
        public BoardMemento(int[,] boardsave)
        {
            this.boardsave = (int[,])boardsave.Clone();
        }
    }

    class GameHistory
    {
        public Stack<BoardMemento> History { get; private set; }

        public GameHistory()
        {
            History = new Stack<BoardMemento>();
        }
    }   
}
