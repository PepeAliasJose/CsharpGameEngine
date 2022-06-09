using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngineS.motor
{
    /// <summary>
    /// Log para mostrar informacion por la terminal
    /// </summary>
    internal class log
    {
        public static void normal(string msg) 
        { 
        Console.WriteLine(msg);
        }

        public static void informacion(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(msg);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void advertencia(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(msg);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void error(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
