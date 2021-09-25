using Controller;
using System;
using System.Threading;

namespace Racesimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Data.Initialize();
            Data.NextRace();
            Console.WriteLine(Data.CurrentRace);

            while (true)
            {
                Thread.Sleep(100);
            }
        }
    }
}
