using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tasks
{
    class Task9_BullsAndCows
    {
        public static void Run()
        {
            const string secretWord = "GAME";
            int attempts = 0;

            while (true)
            {
                Console.Write("Enter your 4-letter guess: ");
                string? guess = Console.ReadLine()?.ToUpper();
                attempts++;

                if (string.IsNullOrEmpty(guess) || guess.Length != 4)
                {
                    Console.WriteLine("Invalid input. Please enter a 4-letter word.");
                    continue;
                }

                int bulls = 0;
                int cows = 0;

                bool[] secretUsed = new bool[4];
                bool[] guessUsed = new bool[4];

                // Count bulls
                for (int i = 0; i < 4; i++)
                {
                    if (guess[i] == secretWord[i])
                    {
                        bulls++;
                        secretUsed[i] = true;
                        guessUsed[i] = true;
                    }
                }

                // Count cows
                for (int i = 0; i < 4; i++)
                {
                    if (guessUsed[i]) continue;

                    for (int j = 0; j < 4; j++)
                    {
                        if (!secretUsed[j] && guess[i] == secretWord[j])
                        {
                            cows++;
                            secretUsed[j] = true;
                            break;
                        }
                    }
                }

                Console.WriteLine($"{bulls} Bulls, {cows} Cows");

                if (bulls == 4)
                {
                    Console.WriteLine($"Congratulations! You guessed the word in {attempts} attempt(s).");
                    break;
                }
            }
        }
    }
}
