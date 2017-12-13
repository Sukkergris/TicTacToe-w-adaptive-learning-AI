using System;
using System.Collections.Generic;
namespace TicTacToe
{
    class MainClass
    {
        public static void Main(string[] args)
        {

            if (args.Length > 0)
            {
				if (args[0] == "print")
				{
					LearningAI lai = new LearningAI(1);
					lai.printProbabilities();
                    return;
				}

                if (args.Length == 2)
                {
                    int amount;
                    if (args[0] == "teach" && Int32.TryParse(args[1], out amount))
                    {
                        Game teachingGame = new Game(amount);
                        return;
                    }
                }
                Console.WriteLine("Usage: mono TicTacToe.exe print");
                Console.WriteLine("or");
                Console.WriteLine("Usage: mono TicTacToe.exe teach <AmountOfGamesToPlay>");
            } else
            {
                Game game = new Game();
                game.SetupGame();
                game.PlayGame();
            }
        }
    }
}
