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

            Visualization.Initialize();

            while (true)
            {
                Thread.Sleep(100);
            }
        }
    }
}
