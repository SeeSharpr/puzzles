using System;

namespace leetcode.Lists.Top150
{
    public class GraphBFS
    {
        [Trait("Difficulty", "Medium")]
        public class Medium
        {
            /// <summary>
            /// 909. Snakes and Ladders
            /// 
            /// You are given an n x n integer matrix board where the cells are labeled from 1 to n2 in a Boustrophedon style starting from the bottom left of the board (i.e. board[n - 1][0]) and alternating direction each row.
            /// You start on square 1 of the board. In each move, starting from square curr, do the following:
            /// * Choose a destination square next with a label in the range [curr + 1, min(curr + 6, n2)].
            ///   - This choice simulates the result of a standard 6-sided die roll: i.e., there are always at most 6 destinations, regardless of the size of the board.
            /// * If next has a snake or ladder, you must move to the destination of that snake or ladder. Otherwise, you move to next.
            /// * The game ends when you reach the square n2.
            /// A board square on row r and column c has a snake or ladder if board[r][c] != -1. The destination of that snake or ladder is board[r][c]. Squares 1 and n2 are not the starting points of any snake or ladder.
            /// Note that you only take a snake or ladder at most once per dice roll. If the destination to a snake or ladder is the start of another snake or ladder, you do not follow the subsequent snake or ladder.
            /// * For example, suppose the board is [[-1,4],[-1,3]], and on the first move, your destination square is 2. You follow the ladder to square 3, but do not follow the subsequent ladder to 4.
            /// Return the least number of dice rolls required to reach the square n2. If it is not possible to reach the square, return -1.
            /// </summary>
            /// <see cref="https://leetcode.com/problems/snakes-and-ladders/description/?envType=study-plan-v2&envId=top-interview-150"/>
            [Theory]
            [InlineData("[[-1,-1,-1,-1,-1,-1],[-1,-1,-1,-1,-1,-1],[-1,-1,-1,-1,-1,-1],[-1,35,-1,-1,13,-1],[-1,-1,-1,-1,-1,-1],[-1,15,-1,-1,-1,-1]]", 4)]
            [InlineData("[[-1,-1],[-1,3]]", 1)]
            [InlineData("[]", 0)]
            [InlineData("[[-1]]", 0)]
            [InlineData("[[1]]", -1)]
            [InlineData("[[1,1][-1,1]]", -1)]
            [InlineData("[[-1,-1],[-1,1]]", 1)]
            [InlineData("[[-1,1,2,-1],[2,13,15,-1],[-1,10,-1,-1],[-1,6,2,8]]", 2)]
            public void SnakesAndLadders(string input, int expected)
            {
                int[][] board = [.. input.Parse2DArray(int.Parse).Select(e => e.ToArray())];

                static void ConvertIndex(int side, int index, out int row, out int col)
                {
                    row = index / side;
                    col = index % side;
                    col = row % 2 == 0 ? col : side - col - 1;
                    row = side - row - 1;
                }

                static int DP(int[][] board)
                {
                    int limit = board.Length * board.Length;
                    int[] dp = new int[limit];
                    if (dp.Length > 1) dp[^1] = -1;
                    if (dp.Length == 1) dp[^1] = board[0][0] == -1 ? 0 : -1;

                    for (int curr = 0; curr < limit; curr++)
                    {
                        ConvertIndex(board.Length, curr, out int row, out int col);

                        // Skip the jumps since we analyze them in advance down the road
                        if (board[row][col] > curr) continue;

                        int rollLimit = Math.Min(6, limit - 1 - curr);
                        int defaultCost = dp[curr] + 1;

                        for (int roll = 1; roll <= rollLimit; roll++)
                        {
                            ConvertIndex(board.Length, curr + roll, out int nextRow, out int nextCol);

                            int dpNext = board[nextRow][nextCol] < 0 ? curr + roll : board[nextRow][nextCol] - 1;

                            dp[dpNext] = dp[dpNext] < 1 ? defaultCost : Math.Min(dp[dpNext], defaultCost);
                        }
                    }

                    return board.Length < 1 ? 0 : dp[^1];
                }

                static int BFS(int[][] board)
                {
                    int result = -1;
                    int limit = board.Length * board.Length;

                    HashSet<int> visited = [];
                    PriorityQueue<int, int> queue = new();
                    queue.Enqueue(0, board[0][0] == 1 ? -1 : 0);

                    while (queue.TryDequeue(out int curr, out int cost))
                    {
                        // Skip visited nodes
                        if (!visited.Add(curr)) continue;

                        // If we got to the end, bail out
                        if (curr == limit - 1 || limit == 0)
                        {
                            result = cost;
                            break;
                        }

                        int rollLimit = Math.Min(6, limit - 1 - curr);

                        for (int roll = 1; roll <= rollLimit; roll++)
                        {
                            int index = roll + curr;

                            ConvertIndex(board.Length, index, out int row, out int col);
                            int next = board[row][col] == -1 ? index : board[row][col] - 1;

                            queue.Enqueue(next, cost + 1);
                        }
                    }

                    return result;
                }

                string solution = "bfs";
                int actual =
                    solution == "dp" ? DP(board) :
                    solution == "bfs" ? BFS(board) :
                    throw new NotImplementedException(solution);

                Assert.Equal(expected, actual);
            }
        }
    }
}
