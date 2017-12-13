using System;
using System.Collections.Generic;
namespace TicTacToe
{
    public class Human : Player
    {
        public Human(int mark) : base(mark) { }

        public override int[] move(Board bd)
        {
            return getInput();
        }

        private int[] getInput()
        {
            Console.Write("Input row: ");
            var r = Console.ReadKey();
            Console.WriteLine();
            Console.Write("Input column: ");
            var c = Console.ReadKey();
            Console.WriteLine();
            var input = new int[2] { (int)r.KeyChar - 48, (int)c.KeyChar - 48 };
            if (!checkInput(input))
            {
                getInput();
            }
            return new int[2] {input[0]-1, input[1]-1};
        }

        private bool checkInput(int[] input)
        {
            if (input.Length != 2)
            {
                return false;
            }
			if (input[0] < 1 || input[0] > 3 || input[1] < 1 || input[1] > 3)
			{
				return false;
			}
            return true;
        }


    }
}
