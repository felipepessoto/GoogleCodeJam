using System;
using Algoritmos.AlienNumbers;
using Algoritmos.AlwaysTurnLeft;

namespace Algoritmos
{
    class Program
    {
        static void Main(string[] args)
        {
            new AlgoritmoAlienNumbers().Executar();
            new AlgoritmoAlwaysTurnLeft().Executar();
            
            Console.ReadKey(true);
        }
    }
}
