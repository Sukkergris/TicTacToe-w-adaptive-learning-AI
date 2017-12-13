using System;
using System.Collections.Generic;
namespace TicTacToe
{
    public class MiniMax : Player
    {
        Random rnd;

        public MiniMax(int mark)
			: base(mark)
		{
			rnd = new Random();
		}

		public override int[] move(Board bd)
		{
			List<int[]> possibles = bd.possibles();
            int[] moveToMake = new int[] { -1, -1 };

            int curVal = -1000;
            foreach (int[] mov in possibles)
            {
                bd.setAiMark(mov[0], mov[1], Mark);
                int val = minmax(bd, 0, Mark);
                bd.setAiMark(mov[0], mov[1], 0);
                if (val > curVal)
                {
                    curVal = val;
                    moveToMake = mov;
                }
            }

            return moveToMake;
		}

        private int minmax(Board bd, int depth, int m) {
			if (!bd.isDone() && bd.isFull())
			{
                return 0;
			}

            if (bd.isDone())
            {
                return bd.getWinnerMark() == Mark ? 10 - depth : depth - 10;
            }

            if (m != Mark)
            {
                int best = -1000;
                List<int[]> possibles = bd.possibles();
                foreach (var nextMov in possibles)
                {
                    bd.setAiMark(nextMov[0], nextMov[1], m);
                    best = Math.Max(best, minmax(bd, depth + 1, m == 1 ? -1 : 1));
                    bd.setAiMark(nextMov[0], nextMov[1], 0);
                }
                return best;
            }
            else 
            {
                int best = 1000;

				List<int[]> possibles = bd.possibles();
				foreach (var nextMov in possibles)
				{
                    bd.setAiMark(nextMov[0], nextMov[1], m);
					best = Math.Min(best, minmax(bd, depth + 1, m == 1 ? -1 : 1));
                    bd.setAiMark(nextMov[0], nextMov[1], 0);
				}

                return best;
            }
        }
    }
}
