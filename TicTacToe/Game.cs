using System;
using System.Collections.Generic;
namespace TicTacToe
{
    public class Game
    {
        private Random rnd;
        private int firstMark;
        private int secondMark;
        private Player p1;
        private Player p2;
        private LearningAI lai;
        Board bd;

        public Game()
        {
            rnd = new Random();
            lai = null;
        }

        public Game(int amount)
        {
            p1 = new MiniMax(1);
            p2 = new LearningAI(-1);
            lai = (LearningAI)p2;
            Teach(amount);
        }

        public void SetupGame()
        {
			firstMark = rnd.NextDouble() < 0.5 ? -1 : 1;
			secondMark = firstMark == 1 ? -1 : 1;
			p1 = initPlayer(firstMark, 1);
			p2 = initPlayer(firstMark == 1 ? -1 : 1, 2);
			bd = new Board();
        }

        public void Teach(int amount)
        {
            List<Board> aftermath = new List<Board>();
            Player curPlayer = p1;
            int[] pMove = new int[2] { -1, -1 };

            Console.WriteLine("\n >> Teaching...");

            double curPercent = 0.0;
            int pointOnePercent = amount / 10000;

            for (int i = 0; i < amount; i++)
            {
                
                if (i % pointOnePercent == 0)
                {
                    curPercent = curPercent + 0.01;
                    Console.Write(String.Format(" >> {0:F1}%", curPercent));
                    Console.CursorLeft = 0;
                }

                bd = new Board();
                aftermath = new List<Board>();

				while (!bd.isDone() && !bd.isFull())
				{
					pMove = curPlayer.move(bd);

					bd.setMark(pMove[0], pMove[1], curPlayer.Mark);

					aftermath.Add(new Board(bd.copyBoard()));

					curPlayer = curPlayer == p1 ? p2 : p1;
				}

				aftermath.Add(new Board(bd.copyBoard()));
				lai.updateProbabilities(aftermath, bd.getWinnerMark());
            }

            Console.CursorLeft = 0;
			Console.Write(" >> 100.0%");

            Console.WriteLine("\n >> Saving data...\n");
            lai.saveData();
        }

        public void PlayGame()
        {
            lai = p1 is LearningAI ? (LearningAI)p1 : (p2 is LearningAI ? (LearningAI)p2 : null);

			List<Board> aftermath = new List<Board>();

			Player curPlayer = p1;

			int[] pMove = new int[2] { -1, -1 };

			bd.printBoard();

			while (!bd.isDone() && !bd.isFull())
			{
				pMove = curPlayer.move(bd);

				if (!bd.setMark(pMove[0], pMove[1], curPlayer.Mark))
				{
					Console.Clear();
					Console.WriteLine("Invalid input");
					bd.printBoard();
					continue;
				}

				if (lai != null)
				{
					aftermath.Add(new Board(bd.copyBoard()));
				}

                curPlayer = curPlayer == p1 ? p2 : p1;
				
				bd.printBoard();
			}

			if (lai != null)
			{
				aftermath.Add(new Board(bd.copyBoard()));
				lai.updateProbabilities(aftermath, bd.getWinnerMark());
			}

            Rematch();
		}

        public void Rematch()
        {
			Console.Write("Rematch? [y/n]: ");
			var p = Console.ReadKey().KeyChar;

			if (p == 'y')
			{
                Console.WriteLine();
                SetupGame();
                PlayGame();
			}

			if (p == 'n')
			{
                if (lai != null)
                {
                    lai.saveData();
                }
                Console.WriteLine();
                Environment.Exit(0);
			}
            else
            {
                Console.Clear();
                Rematch();
            }
        }

		public Player initPlayer(int mark, int player)
		{
			Console.Write(String.Format("Player {0} is a Human / Random AI / Learning AI / Impossible AI [h/r/l/i]: ", player));
			var p = Console.ReadKey().KeyChar;

			if (p == 'h')
			{
				Console.WriteLine();
				return new Human(mark);
			}
			if (p == 'r')
			{
				Console.WriteLine();
				return new RndAI(mark);
			}
			if (p == 'l')
			{
				Console.WriteLine();
				return new LearningAI(mark);
			}
			if (p == 'i')
			{
				Console.WriteLine();
                return new MiniMax(mark);
			}
			Console.Clear();
			Console.WriteLine("Input was retarded.");
			return initPlayer(mark, player);
		}
    }
}
