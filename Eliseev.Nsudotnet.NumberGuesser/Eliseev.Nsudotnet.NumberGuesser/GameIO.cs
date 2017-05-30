using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eliseev.Nsudotnet.NumberGuesser
{
    class GameIO
    {
        private static readonly string[] OATH =
        {
            "шнеле, %NAME!",
            "Не правильно ты считаешь, дядя %NAME?",
            "%NAME, поднажми!"
        };

        public static void WriteResult(int attempts, TimeSpan playedTime)
        {
            Console.WriteLine($"Отгадал за {attempts} попыток (за {playedTime}). Рубанешься?");
        }

        public static void WriteNumberIsMore()
        {
            Console.WriteLine("Моё число больше");
        }

        public static void WriteNumberIsLess()
        {
            Console.WriteLine("Моё число меньше");
        }

        public static void WriteRules(int minNumber, int maxNumber)
        {
            Console.WriteLine($"Какое моё число ({minNumber}-{maxNumber}).");
        }

        public static string ReadPlayerName()
        {
            WriteInputNameMessage();
            return Console.ReadLine();
        }

        private static void WriteInputNameMessage()
        {
            Console.Write("Введи имя: ");
        }

        public static void WriteSwearing(String userName, Random random)
        {
            Console.WriteLine( userName+ String.Format(OATH[random.Next(0, OATH.Length)], "%NAME"));
        }

        public static bool ReadYesNo()
        {
            for (;;)
            {
                WriteInputMessage("y/n");
                var input = Console.ReadLine().ToLower();
                if (input.Equals("y"))
                {
                    return true;
                }
                else if (input.Equals("n"))
                {
                    return false;
                }
                else
                {
                    WriteInputError();
                }
            }
        }

        private static void WriteInputError()
        {
            Console.WriteLine("Неверный ввод.");
        }

        private static void WriteInputMessage(String options = "")
        {
            Console.Write("ввод ");
            if (options != "")
            {
                Console.Write($"({options})");
            }
            Console.Write(": ");
        }

        public static int ReadNumber()
        {
            int result = 0;
            bool correctInput = false;
            while (!correctInput)
            {
                WriteInputMessage();
                
                    if (int.TryParse(Console.ReadLine(), out result))
                    {
                        correctInput = true;
                    }
                    else
                    WriteInputError();

            }
            return result;
        }
    }
}
