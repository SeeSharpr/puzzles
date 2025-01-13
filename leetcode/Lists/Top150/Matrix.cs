namespace leetcode.Lists.Top150
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

        // You are given an n x n 2D matrix representing an image, rotate the image by 90 degrees (clockwise).
        [Fact]
        public void Rotate()
        {
            foreach (var tuple in new Tuple<int[][], int[][]>[] {
                new ([[1,2,3],[4,5,6],[7,8,9]], [[7,4,1],[8,5,2],[9,6,3]]),
                new ([[5,1,9,11],[2,4,8,10],[13,3,6,7],[15,14,12,16]],[[15,13,2,5],[14,3,4,1],[12,6,8,9],[16,7,10,11]]),
            })
            {
                int[][] matrix = tuple.Item1;
                int[][] expected = tuple.Item2;

                static void RotateRight(ref int a1, ref int a2, ref int a3, ref int a4)
                {
                    int temp = a4;
                    a4 = a3;
                    a3 = a2;
                    a2 = a1;
                    a1 = temp;
                }

                if (matrix.Length == 0) return;

                int left = 0;
                int right = matrix[0].Length - 1;
                int top = 0;
                int bottom = matrix.Length - 1;

                while (left < right && top < bottom)
                {
                    int limit = right - left;

                    for (int i = 0; i < limit; i++)
                    {
                        RotateRight(ref matrix[top][left + i], ref matrix[top + i][right], ref matrix[bottom][right - i], ref matrix[bottom - i][left]);
                    }

                    left++;
                    right--;
                    top++;
                    bottom--;
                }

                for (int i = 0; i < matrix.Length; i++)
                {
                    Assert.True(expected[i].SequenceEqual(matrix[i]));
                }
            }
        }

        // Given an m x n integer matrix matrix, if an element is 0, set its entire row and column to 0's.
        [Fact]
        public void SetZeroes()
        {
            foreach (var tuple in new Tuple<int[][], int[][]>[] {
            new ([[1,1,1],[1,0,1],[1,1,1]], [[1,0,1],[0,0,0],[1,0,1]]),
            new ([[0, 1, 2, 0], [3, 4, 5, 2], [1, 3, 1, 5]], [[0, 0, 0, 0], [0, 4, 5, 0], [0, 3, 1, 0]]),
            new ([[1,2,3,4],[5,0,7,8],[0,10,11,12],[13,14,15,0]],[[0,0,3,0],[0,0,0,0],[0,0,0,0],[0,0,0,0]])
            })
            {
                int[][] matrix = tuple.Item1;
                int[][] expected = tuple.Item2;

                if (matrix.Length == 0) return;

                int height = matrix.Length;
                int width = matrix[0].Length;

                //HashSet<int> cols = new();
                //HashSet<int> rows = new();

                //for (int j = 0; j < height; j++)
                //{
                //    for (int i = 0; i < width; i++)
                //    {
                //        if (matrix[j][i] != 0) continue;

                //        cols.Add(i);
                //        rows.Add(j);
                //    }
                //}

                //foreach (var col in cols)
                //{
                //    for (int k = 0; k < height; k++) matrix[k][col] = 0;
                //}

                //foreach (var row in rows)
                //{
                //    for (int k = 0; k < width; k++) matrix[row][k] = 0;
                //}

                bool shouldZeroFirstRow = false;
                bool shouldZeroFirstCol = false;

                for (int j = 0; j < height; j++)
                {
                    for (int i = 0; i < width; i++)
                    {
                        if (matrix[j][i] != 0) continue;

                        if (j == 0) shouldZeroFirstRow = true;
                        if (i == 0) shouldZeroFirstCol = true;

                        matrix[0][i] = 0;
                        matrix[j][0] = 0;
                    }
                }

                for (int col = 1; col < width; col++)
                {
                    if (matrix[0][col] != 0) continue;

                    for (int k = 0; k < height; k++) matrix[k][col] = 0;
                }

                for (int row = 1; row < height; row++)
                {
                    if (matrix[row][0] != 0) continue;

                    for (int k = 0; k < width; k++) matrix[row][k] = 0;
                }

                if (shouldZeroFirstCol)
                {
                    for (int k = 0; k < height; k++) matrix[k][0] = 0;
                }

                if (shouldZeroFirstRow)
                {
                    for (int k = 0; k < width; k++) matrix[0][k] = 0;
                }

                for (int i = 0; i < matrix.Length; i++)
                {
                    Assert.True(expected[i].SequenceEqual(matrix[i]));
                }
            }
        }
    }
}
