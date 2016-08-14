using System;
using System.Collections.Generic;
using System.Text;
using Com.Aurora.Shared.Helpers;

namespace Aurora.Studio._2048.Core.Game
{
    public static class Operator
    {
        public static Tile[][] New()
        {
            var p = new Tile[][] { new Tile[] { new Tile { Data = 0, Row = 0, Col = 0 }, new Tile { Data = 0, Row = 0, Col = 1 }, new Tile { Data = 0, Row = 0, Col = 2 }, new Tile { Data = 0, Row = 0, Col = 3 } }, new Tile[] { new Tile { Data = 0, Row = 1, Col = 0 }, new Tile { Data = 0, Row = 1, Col = 1 }, new Tile { Data = 0, Row = 1, Col = 2 }, new Tile { Data = 0, Row = 1, Col = 3 } }, new Tile[] { new Tile { Data = 0, Row = 2, Col = 0 }, new Tile { Data = 0, Row = 2, Col = 1 }, new Tile { Data = 0, Row = 2, Col = 2 }, new Tile { Data = 0, Row = 2, Col = 3 } }, new Tile[] { new Tile { Data = 0, Row = 3, Col = 0 }, new Tile { Data = 0, Row = 3, Col = 1 }, new Tile { Data = 0, Row = 3, Col = 2 }, new Tile { Data = 0, Row = 3, Col = 3 } } };
            Add(ref p);
            Add(ref p);
            return p;
        }

        public static void Add(ref Tile[][] matrix)
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
                            if (item.Data == 0)
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
            matrix[row[num]][col[num]] = Tools.RandomBool() ? new Tile { Data = 2, Row = row[num], Col = col[num] } : new Tile { Data = 4, Row = row[num], Col = col[num] };
        }

        public static string Print(Tile[][] matrix)
        {
            var sb = new StringBuilder();
            int i = 0;
            foreach (var row in matrix)
            {
                sb.Append("[" + i + "]: ");
                foreach (var item in row)
                {
                    sb.Append(item.ToString() + ", ");
                }
                sb.Remove(sb.Length - 2, 2);
                sb.Append("\r\n");
                i++;
            }
            return sb.ToString();
        }

        private static void moveLeftBlank(ref Tile[][] matrix)
        {
            foreach (var row in matrix)
            {
                for (int i = 0, j = 1; j < row.Length; i++, j = i + 1)
                {
                    while (j < row.Length && row[j++].Data == 0) ;
                    j--;
                    if (row[j].Data != 0)
                    {
                        if (row[i].Data == 0)
                        {
                            row[i] = row[j];
                            row[j].Data = 0;
                        }
                        else if (j != i + 1)
                        {
                            row[i + 1] = row[j];
                            row[j].Data = 0;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        public static bool MoveLeft(ref Tile[][] matrix)
        {
            Tile[][] p;
            int i, j;
            Copy(matrix, out p);
            moveLeftBlank(ref matrix);
            foreach (var row in matrix)
            {
                for (i = 0, j = 1; j < row.Length; i++, j = i + 1)
                {
                    if (row[j].Data == 0)
                    {
                        break;
                    }
                    if (row[i].Data == row[j].Data)
                    {
                        row[i].Data *= 2;
                        row[j].Data = 0;
                    }
                }
            }
            moveLeftBlank(ref matrix);
            return !AreEqual(matrix, p);
        }

        private static void moveRightBlank(ref Tile[][] matrix)
        {
            foreach (var row in matrix)
            {
                for (int i = row.Length - 1, j = row.Length - 2; j > -1; i--, j = i - 1)
                {
                    while (j > -1 && row[j--].Data == 0) ;
                    j++;
                    if (row[j].Data != 0)
                    {
                        if (row[i].Data == 0)
                        {
                            row[i] = row[j];
                            row[j].Data = 0;
                        }
                        else if (j != i - 1)
                        {
                            row[i - 1] = row[j];
                            row[j].Data = 0;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        public static bool MoveRight(ref Tile[][] matrix)
        {
            Tile[][] p;
            int i, j;
            Copy(matrix, out p);
            moveRightBlank(ref matrix);
            foreach (var row in matrix)
            {
                for (i = row.Length - 1, j = row.Length - 2; j > -1; i--, j = i - 1)
                {
                    if (row[j].Data == 0)
                    {
                        break;
                    }
                    if (row[i].Data == row[j].Data)
                    {
                        row[i].Data *= 2;
                        row[j].Data = 0;
                    }
                }
            }
            moveRightBlank(ref matrix);
            return !AreEqual(matrix, p);
        }

        private static void moveUpBlank(ref Tile[][] matrix)
        {
            for (int k = 0; k < matrix[0].Length; k++)
            {
                for (int i = matrix.Length - 1, j = matrix.Length - 2; j > -1; i--, j = i - 1)
                {
                    while (j > -1 && matrix[j--][k].Data == 0) ;
                    j++;
                    if (matrix[j][k].Data != 0)
                    {
                        if (matrix[i][k].Data == 0)
                        {
                            matrix[i][k] = matrix[j][k];
                            matrix[j][k].Data = 0;
                        }
                        else if (j != i - 1)
                        {
                            matrix[i - 1][k] = matrix[j][k];
                            matrix[j][k].Data = 0;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        public static bool MoveUp(ref Tile[][] matrix)
        {
            Tile[][] p;
            int i, j;
            Copy(matrix, out p);
            moveUpBlank(ref matrix);
            for (int k = 0; k < matrix[0].Length; k++)
            {
                for (i = matrix.Length - 1, j = matrix.Length - 2; j > -1; i--, j = i - 1)
                {
                    if (matrix[j][k].Data == 0)
                    {
                        break;
                    }
                    if (matrix[i][k].Data == matrix[j][k].Data)
                    {
                        matrix[i][k].Data *= 2;
                        matrix[j][k].Data = 0;
                    }
                }
            }
            moveUpBlank(ref matrix);
            return !AreEqual(matrix, p);
        }

        private static void moveDownBlank(ref Tile[][] matrix)
        {
            for (int k = 0; k < matrix[0].Length; k++)
            {
                for (int i = 0, j = 1; j < matrix.Length; i++, j = i + 1)
                {
                    while (j < matrix.Length && matrix[j++][k].Data == 0) ;
                    j--;
                    if (matrix[j][k].Data != 0)
                    {
                        if (matrix[i][k].Data == 0)
                        {
                            matrix[i][k] = matrix[j][k];
                            matrix[j][k].Data = 0;
                        }
                        else if (j != i + 1)
                        {
                            matrix[i + 1][k] = matrix[j][k];
                            matrix[j][k].Data = 0;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        public static bool MoveDown(ref Tile[][] matrix)
        {
            Tile[][] p;
            int i, j;
            Copy(matrix, out p);
            moveDownBlank(ref matrix);
            for (int k = 0; k < matrix[0].Length; k++)
            {
                for (i = 0, j = 1; j < matrix.Length; i++, j = i + 1)
                {
                    if (matrix[j][k].Data == 0)
                    {
                        break;
                    }
                    if (matrix[i][k].Data == matrix[j][k].Data)
                    {
                        matrix[i][k].Data *= 2;
                        matrix[j][k].Data = 0;
                    }
                }
            }
            moveDownBlank(ref matrix);
            return !AreEqual(matrix, p);
        }

        public static void Copy(Tile[][] matrix, out Tile[][] p)
        {
            p = new Tile[][] { new Tile[] { new Tile { Data = 0, Row = 0, Col = 0 }, new Tile { Data = 0, Row = 0, Col = 1 }, new Tile { Data = 0, Row = 0, Col = 2 }, new Tile { Data = 0, Row = 0, Col = 3 } }, new Tile[] { new Tile { Data = 0, Row = 1, Col = 0 }, new Tile { Data = 0, Row = 1, Col = 1 }, new Tile { Data = 0, Row = 1, Col = 2 }, new Tile { Data = 0, Row = 1, Col = 3 } }, new Tile[] { new Tile { Data = 0, Row = 2, Col = 0 }, new Tile { Data = 0, Row = 2, Col = 1 }, new Tile { Data = 0, Row = 2, Col = 2 }, new Tile { Data = 0, Row = 2, Col = 3 } }, new Tile[] { new Tile { Data = 0, Row = 3, Col = 0 }, new Tile { Data = 0, Row = 3, Col = 1 }, new Tile { Data = 0, Row = 3, Col = 2 }, new Tile { Data = 0, Row = 3, Col = 3 } } };
            int i = 0;
            int j = 0;
            foreach (var row in matrix)
            {
                j = 0;
                foreach (var item in row)
                {
                    p[i][j] = item;
                    j++;
                }
                i++;
            }
        }

        private static bool AreEqual(Tile[][] matrix, Tile[][] p)
        {
            int i = 0, j = 0;
            foreach (var row in matrix)
            {
                j = 0;
                foreach (var item in row)
                {
                    if (p[i][j].Data != item.Data)
                    {
                        return false;
                    }
                    j++;
                }
                i++;
            }
            return true;
        }
    }
}
