using System;
using System.Collections.Generic;
namespace TicTacToe
{
    [Serializable]
    public class Board
    {
        private int[,] bd;
        private int winnerMark;

        public Board()
        {
            initBoard();
            winnerMark = 0;
        }

		public Board(int[,] state)
		{
            bd = state;
		}

        private void initBoard()
        {
            bd = new int[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    bd[i, j] = 0;
                }
            }
        }

        public int getWinnerMark()
        {
            return winnerMark;
        }

        public int[,] getBoard() 
        {
            return (int[,])bd.Clone();
        }

        public void setBoard(int[,] bd) 
        {
            this.bd = bd;
        }

        public int getXlen() 
        {
            return bd.GetLength(0);
        }

		public int getYlen()
		{
			return bd.GetLength(1);
		}

        public bool setMark(int x, int y, int val)
        {
            int xMax = bd.GetLength(0);
            int yMax = bd.GetLength(1);
            if (x < 0 || x > xMax || y < 0 || y > yMax)
            {
                return false;
            }
            if (bd[x, y] != 0)
            {
                return false;
            }
            bd[x, y] = val;
            return true;
        }

        public bool setAiMark(int x, int y, int val) {
			if (x < 0 || x > bd.GetLength(0) || y < 0 || y > bd.GetLength(1))
			{
				return false;
			}
            bd[x, y] = val;
            return true;
        }

        public bool isFull()
        {
            bool retVal = true;
            for (int i = 0; i < bd.GetLength(0); i++)
            {
                for (int j = 0; j < bd.GetLength(1); j++)
                {
                    if (bd[i,j] == 0)
                    {
                        retVal = false;
                    }
                }
            }

            return retVal;
        }

        public bool isDone()
        {
            bool retVal = false;
            for (int c = 0; c < 3; c++)
            {
                if (bd[0,c] == bd[1,c] && bd[1, c] == bd[2,c] && bd[1, c] != 0)
                {
                    winnerMark = bd[0, c];
                    retVal = true;
                }
            }

			for (int r = 0; r < 3; r++)
			{
				if (bd[r, 0] == bd[r, 1] && bd[r, 1] == bd[r, 2] && bd[r, 1] != 0)
				{
                    winnerMark = bd[r, 0];
					retVal = true;
				}
			}

            if (bd[0,0] == bd[1,1] && bd[1, 1] == bd[2, 2] && bd[1,1] != 0)
            {
                winnerMark = bd[0, 0];
                retVal = true;
            }

			if (bd[2,0] == bd[1, 1] && bd[1, 1] == bd[0,2] && bd[1,1] != 0)
			{
                winnerMark = bd[1, 1];
				retVal = true;
			}

            return retVal;
        }

        public List<int[]> possibles()
        {
            List<int[]> retVal = new List<int[]>();

            for (int r = 0; r < bd.GetLength(0); r++)
            {
                for (int c = 0; c < bd.GetLength(1); c++)
                {
                    if (bd[r,c] == 0)
                    {
                        retVal.Add(new int[2] {r,c});
                    }
                }
            }

            return retVal;
        }

		public int[,] copyBoard()
		{
			int[,] retVal = new int[getXlen(), getYlen()];
			for (int x = 0; x < getXlen(); x++)
			{
				for (int y = 0; y < getYlen(); y++)
				{
                    retVal[x, y] = bd[x, y];
				}
			}
			return retVal;
		}

        private char playerIntToChar(int i) 
        {
            switch (i)
            {
                case -1:
                    return 'o';
                case 1:
                    return 'x';
                default:
                    return ' ';
            }
        }

        private void printSeperatorLine()
        {
            for (int i = 0; i < 13; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine();
        }

        private void printLine(int l)
        {
            for (int i = 0; i < 3; i++)
            {
                Console.Write(String.Format("| {0} ", (playerIntToChar(bd[l,i]))));
            }
            Console.WriteLine("|");
        }

        public void printBoard()
        {
            for (int i = 0; i < 3; i++)
            {
                printSeperatorLine();
                printLine(i);
            }
            printSeperatorLine();
        }

        public override int GetHashCode()
        {
            int total = 0;
            for (int x = 0; x < this.getXlen(); x++)
            {
                for (int y = 0; y < this.getYlen(); y++)
                {
                    total = 10 * total + this.bd[x,y];
                }
            }
            return total;
        }
    }
}
