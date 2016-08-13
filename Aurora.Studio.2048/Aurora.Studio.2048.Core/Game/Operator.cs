using System;
using System.Collections.Generic;
using Com.Aurora.Shared.Helpers;

namespace Aurora.Studio._2048.Core.Game
{
    public static class Operator
    {
        public static int[][] New()
        {
            var p = new int[][] { new int[] { 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0 } };
            Add(ref p);
            Add(ref p);
            return p;
        }

        public static void Add(ref int[][] matrix)
        {
            int i = 0, j = 0;
            List<int> row = new List<int>();
            List<int> col = new List<int>();
            if (matrix != null && matrix.Length == 4)
            {
                foreach (var m_row in matrix)
                {
                    if (m_row != null && m_row.Length == 4)
                    {
                        j = 0;
                        foreach (var item in m_row)
                        {
                            if (item == 0)
                            {
                                row.Add(i);
                                col.Add(j);
                            }
                            j++;
                        }
                        i++;
                    }
                    else
                    {
                        throw new ArgumentException("Not a valid matrix");
                    }
                }
            }
            else
            {
                throw new ArgumentException("Not a valid matrix");
            }
            var num = Tools.Random.Next(row.Count);
            matrix[row[num]][col[num]] = Tools.RandomBool() ? 2 : 4;
        }

        public static bool MoveLeft(ref int[][] matrix)
        {
            int i, j;
            var b = false;
            foreach (var row in matrix)
            {
                for (i = 0, j = 1; i < row.Length; i++, j = i + 1)
                {
                    if (j == row.Length)
                        break;
                    while (j < row.Length && row[j++] == 0) ;
                    if (row[i] == 0)
                    {
                        row[i] = row[j - 1];
                        row[j - 1] = 0;
                    }
                    if (row[i] == row[--j])
                    {
                        row[i] *= 2;
                        row[j] = 0;
                        b = true;
                        continue;
                    }
                    if (i + 1 != j)
                    {
                        row[i + 1] = row[j];
                        b = true;
                    }
                }
            }
            return b;
        }
    }
}
