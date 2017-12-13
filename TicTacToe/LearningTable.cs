using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
namespace TicTacToe
{
    [Serializable]
    public struct Data 
    {
        public Board bd;
        public double px;
        public double po;
    }

    [Serializable]
    public class LearningTable
    {
        private SortedDictionary<int, Data> dc;
        private double learningRate;
        private int amountOfGamesPlayed;

        private LearningTable(SortedDictionary<int,Data> dict)
        {
            dc = dict;
            learningRate = 0.5;
            amountOfGamesPlayed = 0;
        }

        public double getLearningRate()
        {
            return learningRate;
        }

        public int DataCount()
        {
            return dc.Count;
        }

        public void addBoard(Board bd)
        {
            int key = bd.GetHashCode();
			if (!dc.ContainsKey(key))
			{
                Data tmp = new Data();
                tmp.bd = new Board(bd.copyBoard());
                tmp.px = 0.5;
                tmp.po = 0.5;
				dc.Add(key, tmp);
			}
        }

        public int updatePX(Board bd, double val)
        {
            int key = bd.GetHashCode();

            if (!dc.ContainsKey(key))
            {
                addBoard(bd);
            }

			Data tmp = dc[key];
			tmp.px = val;
			dc[key] = tmp;

            return 0;
        }

		public int updatePO(Board bd, double val)
		{
			int key = bd.GetHashCode();

			if (!dc.ContainsKey(key))
			{
				addBoard(bd);
			}

			Data tmp = dc[key];
			tmp.po = val;
			dc[key] = tmp;

			return 0;
		}

        public double getPX(Board bd)
        {
            int key = bd.GetHashCode();
			if (!dc.ContainsKey(key))
			{
                addBoard(bd);
                return dc[key].px;
			}
            return dc[key].px;
        }
		
        public double getPO(Board bd)
		{
			int key = bd.GetHashCode();
			if (!dc.ContainsKey(key))
			{
				addBoard(bd);
				return dc[key].po;
			}
			return dc[key].po;
		}

        public void updateProbabilities(List<Board> aftermath, int winnerMark)
        {
            if (aftermath.Count < 3)
            {
                Console.WriteLine("Aftermath list error");
                return;
            }

            aftermath.Reverse();
            Board[] aftermathArr = aftermath.ToArray();
            addBoard(aftermathArr[0]);
            addBoard(aftermathArr[1]);

            if (winnerMark == 1)
            {
                updatePX(aftermathArr[0], 1.0);
                updatePO(aftermathArr[0], 0.0);
                updatePO(aftermathArr[1], 0.0);
                updatePX(aftermathArr[1], 1.0);
            }
            if (winnerMark == -1) {
                updatePO(aftermathArr[0], 1.0);
                updatePX(aftermathArr[0], 0.0);
                updatePX(aftermathArr[1], 0.0);
                updatePO(aftermathArr[1], 1.0);
            }

            for (int i = 2; i < aftermathArr.Length; i++)
            {
                Board curState = aftermathArr[i];
                Board afterState = aftermathArr[i - 2];
                if (winnerMark == 1)
                {
                    if (i % 2 == 0)
                    {
                        double pc = getPX(curState);
                        double pa = getPX(afterState);
                        double val = pc + learningRate * (pa - pc);
                        updatePX(curState, val);
                    } else
                    {
                        double pc = getPO(curState);
                        double pa = getPO(afterState);
						double val = pc + learningRate * (pa - pc);
						updatePO(curState, val);
                    }
                }
                if (winnerMark == -1)
                {
					if (i % 2 == 0)
					{
                        double pc = getPO(curState);
                        double pa = getPO(afterState);
						double val = pc + learningRate * (pa - pc);
						updatePO(curState, val);
					}
					else
					{
                        double pc = getPX(curState);
                        double pa = getPX(afterState);
						double val = pc + learningRate * (pa - pc);
						updatePX(curState, val);
					}
                }
            }
            amountOfGamesPlayed++;
            if (amountOfGamesPlayed % 10000 == 0)
            {
                learningRate = learningRate * 0.99;
            }
        }

		public void saveData()
		{
			string fileName = "data";
			IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
			formatter.Serialize(stream, this);
			stream.Close();
		}

        public static LearningTable loadData(string fileName)
		{
			if (!File.Exists(fileName))
			{
                return new LearningTable(new SortedDictionary<int, Data>());
			}
			IFormatter formatter = new BinaryFormatter();
			Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            LearningTable obj = (LearningTable)formatter.Deserialize(stream);
            stream.Close();
            return obj;
		}

        public void printProbabilites()
        {
            foreach (var item in dc)
            {
                Console.WriteLine(String.Format("(PX,PO) = ({0},{1})", item.Value.px, item.Value.po));
            }
        }
    }
}
