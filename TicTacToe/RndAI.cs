using System;
using System.Collections.Generic;
namespace TicTacToe
{
    public class RndAI : Player
    {
        private Random rnd;

        public RndAI(int mark)
            : base(mark)
        {
            rnd = new Random();
        }

        public override int[] move(Board bd)
        {
            List<int[]> possibles = bd.possibles();
            int tmp = rnd.Next(possibles.Count);
            return new int[2] {possibles[tmp][0], possibles[tmp][1] };
        }
    }
}
