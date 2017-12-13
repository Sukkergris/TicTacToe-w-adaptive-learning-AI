using System;
using System.Collections.Generic;
namespace TicTacToe
{
    public abstract class Player
    {
        public readonly int Mark;

        public Player(int mark)
        {
            Mark = mark;
        }

        public abstract int[] move(Board bd);

		public override bool Equals(object obj)
		{
			if (obj is RndAI)
			{
				RndAI tmp = (RndAI)obj;
				return tmp.Mark == this.Mark;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format(String.Format("Player {0}", Mark));
		}
    }
}
