using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;



namespace WpfFiftens
{
    class Program
    {
        
    }

    public sealed class Session
    {
        private static Session instance = null;

        public static Session Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Session();
                }
                return instance;
            }
        }

        private Session() {
            int weight = 4, hight = 4, res = 50,i=4,hardmodN=0;
            char hardmodC='n';
            bool a = true;
            do
            {
                try
                {
                    Console.Write("\n vv count of stolbsow: ");
                    weight = Console.ReadKey().KeyChar - 48;
                    Console.Write("\n vv count of strok: ");
                    hight = Console.ReadKey().KeyChar - 48;
                    Console.Write("\n vv lvl from 1-10: ");
                    bool countof = Int32.TryParse(Console.ReadLine(), out res);
                    Console.Write("\n Do you whant to on hard mode(y/n):");
                    hardmodC = Console.ReadKey().KeyChar;
                }
                catch (InvalidCastException e)
                {
                    Console.WriteLine("Eror" + e);
                }
                if (((weight * hight) - 16 == 0) && (res > 0) && (res<=10) &&((hardmodC == 'n') || (hardmodC == 'y'))) a = false;
            } while (a);
            res = res * 10;
            Creator creator = new Creator(weight, hight);
            ITable table = creator.Create(res);
            if (hardmodC == 'y') hardmodN = 1;
            CommandSave commandS = new CommandSave(table);
            CommandRandom comandR = new CommandRandom(creator,res, table);
            //CommandUP commandU = new CommandUP(table);
            Invoker invoker = new Invoker();
            invoker.SetCommand(commandS);
            invoker.Run();

            while (equal(table.GetBoard(), weight, hight))
            {
                Console.WriteLine();
                if (i % 3 == 0)
                {
                    invoker.SetCommand(comandR);
                    invoker.Run();
                    Console.WriteLine("=) ahahaha =)");
                }
                i += hardmodN;
                invoker.SetCommand(commandS);
                cout(table.GetBoard());
                Console.WriteLine("1 - up, 2 - down, 3 - right, 4 - left, 5 - cansel, 9 - give up");
                int side = (int)Console.ReadKey().KeyChar - 48; 
                Console.Clear();
                switch (side)
                {
                    case 1:
                        {   invoker.Run(); 
                            //invoker.SetCommand(commandU); invoker.Run();
                            invoker.SetCommand(commandS); invoker.Run();
                            break; }
                    case 2:
                        { invoker.Run(); table.Down(); invoker.Run(); break; }
                    case 3:
                        { invoker.Run(); table.Right(); invoker.Run(); break; }
                    case 4:
                        { invoker.Run(); table.Left(); invoker.Run(); break; }
                    case 5:
                        { invoker.Cancel(); break; }
                    case 9:
                        {
                            Console.WriteLine("Loser!! ha");
                            Thread.Sleep(1000);
                            goto Exit;
                        }
                    default: { Console.WriteLine(" - incorect number"); break; }
                }
            }
            cout(table.GetBoard());
            Console.WriteLine("WIN !!!!!!!!!!!!!");
            Console.ReadKey();
            Exit: Environment.Exit(0);
        }

        public static void cout(int[,] a)
        {
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    Console.Write(a[i, j] + "\t");
                }
                Console.WriteLine();
            }
        }

        public static bool equal(int[,] a,int weight,int hight)
        {
            ITable table = new Table(weight, hight);
            Creator creatorempty = new Creator(weight, hight);
            creatorempty.Create(0);
            bool c = false;            
            for (int i = 0; i < a.GetLength(0); ++i)
            {
                for (int j = 0; j < a.GetLength(1); ++j)
                    if (a[i, j] != table.GetBoard()[i, j]) return true;
            }
            return c;
        }

    }
}
