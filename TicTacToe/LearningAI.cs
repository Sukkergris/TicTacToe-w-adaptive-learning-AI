using System;
using System.Collections.Generic;

namespace TicTacToe
{
    public class LearningAI : Player
    {
        private LearningTable tb;
        private Random rnd;

        public LearningAI(int mark) : base(mark)
        {
            tb = LearningTable.loadData("data");
            rnd = new Random();
        }

        public override int[] move(Board bd)
        {
            List<int[]> possibles = bd.possibles();

            if (rnd.NextDouble() < tb.getLearningRate())
			{
				int tmp = rnd.Next(possibles.Count);
                return new int[2] { possibles[tmp][0], possibles[tmp][1] };
			}

			int[] finalMove = new int[2] { -1, -1 };
            double probability = -1.0;

            foreach (var item in possibles)
            {
                Board afterState = new Board(bd.copyBoard());
                afterState.setMark(item[0], item[1], Mark);
                if (finalMove[0] == -1 && finalMove[1] == -1)
                {
                    finalMove = item;
                    probability = Mark == 1 ? tb.getPX(afterState) : tb.getPO(afterState);
                }
                else
                {
                    if (Mark == 1)
                    {
                        double tmpProb = tb.getPX(afterState);
                        finalMove = probability < tmpProb ? item : finalMove;
                        probability = probability < tmpProb ? tmpProb : probability;
                    }
                    else
                    {
                        double tmpProb = tb.getPO(afterState);
                        finalMove = probability < tmpProb ? item : finalMove;
                        probability = probability < tmpProb ? tmpProb : probability;
                    }
                }
            }
            return finalMove;
        }

        public void updateProbabilities(List<Board> aftermath, int winnerMark)
        {
            tb.updateProbabilities(aftermath, winnerMark);
        }

        public void addBoard(Board bd)
        {
            tb.addBoard(bd);
        }

        public void saveData()
        {
            tb.saveData();
        }

        public void printProbabilities()
        {
            tb.printProbabilites();
        }

        public int getDataCount()
        {
            return tb.DataCount();
        }
    }
}
