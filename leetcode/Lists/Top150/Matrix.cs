﻿namespace leetcode.Lists.Top150
{
    public class Matrix
    {

        [Theory]
        [InlineData(new string[] {
            "53..7....",
            "6..195...",
            ".98....6.",
            "8...6...3",
            "4..8.3..1",
            "7...2...6",
            ".6....28.",
            "...419..5",
            "....8..79"},
            true)]
        [InlineData(new string[] {
            "83..7....",
            "6..195...",
            ".98....6.",
            "8...6...3",
            "4..8.3..1",
            "7...2...6",
            ".6....28.",
            "...419..5",
            "....8..79" },
            false)]
        public void IsValidSudoku(string[] boardArray, bool expected)
        {
            char[][] board = boardArray.Select(x => x.ToCharArray()).ToArray();

            static bool IsValidLine(char[][] array, int n, bool isRow, bool[] f)
            {
                Array.Clear(f);

                bool isValid = true;
                for (int i = 0; i < 9; i++)
                {
                    int x = isRow ? i : n;
                    int y = isRow ? n : i;

                    if (!char.IsDigit(array[y][x])) continue;

                    int value = array[y][x] - '0';

                    if (f[value - 1])
                    {
                        isValid = false;
                        break;
                    }
                    else
                    {
                        f[value - 1] = true;
                    }
                }

                return isValid;
            }

            static bool IsValidBlock(char[][] array, int x, int y, bool[] f)
            {
                Array.Clear(f);

                bool isValid = true;
                int lj = y + 3;
                int li = x = 3;
                for (int j = y; j < lj; j++)
                {
                    for (int i = x; i < li; i++)
                    {
                        if (!char.IsDigit(array[j][i])) continue;

                        int value = array[j][i] - '0';

                        if (f[value - 1])
                        {
                            isValid = false;
                            break;
                        }
                        else
                        {
                            f[value - 1] = true;
                        }
                    }
                }

                return isValid;
            }

            bool[] seen = new bool[9];

            bool valid = true;
            for (int row = 0; valid && row < 9; row++)
            {
                valid = valid && IsValidLine(board, row, isRow: true, seen);
            }

            for (int col = 0; valid && col < 9; col++)
            {
                valid = valid && IsValidLine(board, col, isRow: false, seen);
            }

            for (int j = 0; valid && j < 9; j += 3)
            {
                for (int i = 0; valid && i < 9; i += 3)
                {
                    valid = valid && IsValidBlock(board, i, j, seen);
                }
            }

            Assert.Equal(expected, valid);
        }

        [Fact]
        public void SpiralOrder()
        {
            Tuple<int[][], IList<int>>[] inlineData = [
                new ([[1, 2, 3], [4, 5, 6], [7, 8, 9]], [1, 2, 3, 6, 9, 8, 7, 4, 5]),
                new ([[1,2,3,4],[5,6,7,8],[9,10,11,12]], [1,2,3,4,8,12,11,10,9,5,6,7]),
                new ([[1,2,3]], [1,2,3]),
                new ([[1],[4],[7]], [1,4,7]),
                new ([[3],[2]], [3,2]),
                ];

            foreach (var tuple in inlineData)
            {
                int[][] matrix = tuple.Item1;
                IList<int> expected = tuple.Item2;

                List<int> result = [];

                if (matrix.Length > 0 && matrix[0].Length > 0)
                {
                    int xi = 0;
                    int xf = matrix[0].Length - 1;
                    int yi = 0;
                    int yf = matrix.Length - 1;

                    while (xi <= xf && yi <= yf)
                    {
                        for (int i = xi; i <= xf; i++) result.Add(matrix[yi][i]);
                        yi++;

                        for (int j = yi; j <= yf; j++) result.Add(matrix[j][xf]);
                        xf--;

                        if (yi <= yf)
                        {
                            for (int i = xf; i >= xi; i--) result.Add(matrix[yf][i]);
                            yf--;
                        }

                        if (xi <= xf)
                        {
                            for (int j = yf; j >= yi; j--) result.Add(matrix[j][xi]);
                            xi++;
                        }
                    }
                }

                Assert.True(expected.SequenceEqual(result));
            }
        }
    }
}
