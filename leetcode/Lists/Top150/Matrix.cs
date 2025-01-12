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
    }
}
