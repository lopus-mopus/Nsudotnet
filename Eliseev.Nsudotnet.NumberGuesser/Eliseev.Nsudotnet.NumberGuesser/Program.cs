using System;
using System.Collections.Generic;
using System.Text;

namespace Eliseev.Nsudotnet.NumberGuesser
{
    class Program
    {
        private const int MinNumber = 0;
        private const int MaxNumber = 100;

        static void Main(string[] args)
        {
            var random = new Random();
            string userName = GameIO.ReadPlayerName();

            bool playAgain = true;
            while (playAgain)
            {
                int hiddenNumber = random.Next(MinNumber, MaxNumber);
                int attempts = 0;
                GameIO.WriteRules(MinNumber, MaxNumber);

                bool won = false;
                DateTime startTime = DateTime.Now;
                while (!won)
                {
                    int number = GameIO.ReadNumber();
                    attempts++;

                    if (number == hiddenNumber)
                    {
                        EndGame(attempts, DateTime.Now - startTime, out won, out playAgain);
                    }
                    else
                    {
                        if (number < hiddenNumber)
                        {
                            GameIO.WriteNumberIsMore();
                        }
                        else
                        {
                            GameIO.WriteNumberIsLess();
                        }
                        if (attempts % 4 == 0)
                        {
                            GameIO.WriteSwearing(userName, random);
                        }
                    }
                }
            }

        }

        private static void EndGame(int attempts, TimeSpan playedTime, out bool won, out bool playAgain)
        {
            won = true;
            GameIO.WriteResult(attempts, playedTime);
            playAgain = GameIO.ReadYesNo();
        }
    }
}
