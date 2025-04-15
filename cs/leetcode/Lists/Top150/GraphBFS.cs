using Microsoft.VisualBasic;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Drawing;
using Xunit;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System;

namespace leetcode.Lists.Top150
{
    public class GraphBFS
    {
        [Trait("Difficulty", "Medium")]
        public class Medium
        {
            /// <summary>
            /// 433. Minimum Genetic Mutation
            /// A gene string can be represented by an 8-character long string, with choices from 'A', 'C', 'G', and 'T'.
            /// Suppose we need to investigate a mutation from a gene string startGene to a gene string endGene where one mutation is defined as one single character changed in the gene string.
            /// For example, "AACCGGTT" --> "AACCGGTA" is one mutation.
            /// There is also a gene bank bank that records all the valid gene mutations. A gene must be in bank to make it a valid gene string.
            /// Given the two gene strings startGene and endGene and the gene bank bank, return the minimum number of mutations needed to mutate from startGene to endGene. If there is no such a mutation, return -1.
            /// Note that the starting point is assumed to be valid, so it might not be included in the bank.
            /// </summary>
            /// <see cref="https://leetcode.com/problems/minimum-genetic-mutation/description/?envType=study-plan-v2&envId=top-interview-150"/>
            [Theory]
            [InlineData("AACCGGTT", "AACCGGTA", "[AACCGGTA]", 1)]
            [InlineData("AACCGGTT", "AAACGGTA", "[AACCGGTA,AACCGCTA,AAACGGTA]", 2)]
            public void MinMutation(string startGene, string endGene, string bankInput, int expected)
            {
                string[] bank = bankInput.Parse1DArray(x => x).ToArray();

                static Dictionary<string, HashSet<string>> BuildGraph(List<string> bank)
                {
                    Dictionary<string, HashSet<string>> graph = [];

                    for (int i = 0; i < bank.Count - 1; i++)
                    {
                        string src = bank[i];
                        for (int j = i + 1; j < bank.Count; j++)
                        {
                            string dst = bank[j];

                            bool tooMany = false;
                            for (int k = 0, diff = 0; k < src.Length; k++)
                            {
                                if (src[k] != dst[k] && ++diff > 1)
                                {
                                    tooMany = true;
                                    break;
                                }
                            }

                            if (tooMany) continue;

                            if (!graph.TryGetValue(src, out var set))
                            {
                                graph[src] = set = [];
                            }

                            set.Add(dst);

                            if (!graph.TryGetValue(dst, out set))
                            {
                                graph[dst] = set = [];
                            }

                            set.Add(src);
                        }
                    }

                    return graph;
                }

                static int Dijkstra(string startGene, string endGene, List<string> genes)
                {
                    if (!genes.Contains(endGene)) return -1;
                    if (!genes.Contains(startGene)) genes.Add(startGene);

                    var graph = BuildGraph(genes);

                    Dictionary<string, int> minCosts = [];
                    minCosts.Add(startGene, 0);
                    minCosts.Add(endGene, int.MaxValue);

                    PriorityQueue<string, int> queue = new();
                    queue.Enqueue(startGene, 0);

                    while (queue.TryDequeue(out string? curKey, out int curCost))
                    {
                        // Skip nodes with no connectivity
                        if (!graph.TryGetValue(curKey, out var nextKeys)) continue;

                        foreach (var nextKey in nextKeys)
                        {
                            // Skip cases where we would increase the current minimum cost
                            if (minCosts.TryGetValue(nextKey, out int minCost) && minCost <= curCost) continue;

                            minCosts[nextKey] = curCost + 1;
                            queue.Enqueue(nextKey, curCost + 1);
                        }
                    }

                    return minCosts[endGene] == int.MaxValue ? -1 : minCosts[endGene];
                }

                static int SimpleBFS(string startGene, string endGene, string[] genes)
                {
                    Queue<Tuple<string, int>> queue = new([new(startGene, 0)]);
                    HashSet<string> visited = new([startGene]);
                    char[] chars = "ACTG".ToCharArray();

                    while (queue.TryDequeue(out var tuple))
                    {
                        string curGene = tuple.Item1;
                        int curCost = tuple.Item2;

                        if (curGene == endGene) return curCost;

                        char[] curChars = curGene.ToCharArray();
                        for (int i = 0; i < curGene.Length; i++)
                        {
                            char[] copy = curChars.ToArray();
                            foreach (char c in chars)
                            {
                                copy[i] = c;

                                string nextGene = new string(copy);

                                if (!genes.Contains(nextGene) || visited.Contains(nextGene)) continue;

                                visited.Add(nextGene);
                                queue.Enqueue(new(nextGene, curCost + 1));
                            }
                        }
                    }

                    return -1;
                }

                string solution = "simplebfs";
                int actual =
                    solution == "simplebfs" ? SimpleBFS(startGene, endGene, bank) :
                    solution == "dijkstra" ? Dijkstra(startGene, endGene, bank.ToList()) :
                    throw new NotImplementedException(solution);

                Assert.Equal(expected, actual);
            }

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
            [InlineData("[[-1,-1],[-1,1]]", 1)]
            [InlineData("[[-1,1,2,-1],[2,13,15,-1],[-1,10,-1,-1],[-1,6,2,8]]", 2)]
            [InlineData("[[-1,-1,-1,-1,48,5,-1],[12,29,13,9,-1,2,32],[-1,-1,21,7,-1,12,49],[42,37,21,40,-1,22,12],[42,-1,2,-1,-1,-1,6],[39,-1,35,-1,-1,39,-1],[-1,36,-1,-1,-1,-1,5]]", 3)]
            [InlineData("[]", 0)]
            [InlineData("[[-1]]", 0)]
            [InlineData("[[1]]", -1)]
            [InlineData("[[1,1][-1,1]]", -1)]
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
                    if (board.Length == 0) return 0;
                    if (board.Length == 1) return board[0][0] == 1 ? -1 : 0;

                    int side = board.Length;
                    int limit = side * side;
                    int[] dp = new int[limit];
                    dp[^1] = -1;

                    for (int square = 1; square < limit; square++)
                    {
                        int currIndex = square - 1;

                        // We need to skip evaluating the squares that are the source of a black hole if we have not arrived there coming from somewhere else
                        ConvertIndex(side, currIndex, out int currRow, out int currCol);
                        if (dp[currIndex] == 0 && board[currRow][currCol] > 0) continue;

                        int rollLimit = square + Math.Min(6, limit - square);
                        int currentCost = dp[currIndex] + 1;
                        for (int nextSquare = square + 1; nextSquare <= rollLimit; nextSquare++)
                        {
                            ConvertIndex(side, nextSquare - 1, out int nextRow, out int nextCol);
                            int nextIndex = (board[nextRow][nextCol] == -1 ? nextSquare : board[nextRow][nextCol]) - 1;
                            dp[nextIndex] = dp[nextIndex] < 1 ? currentCost : Math.Min(dp[nextIndex], currentCost);
                        }
                    }

                    return dp[^1];
                }

                static int BFS(int[][] board)
                {
                    if (board.Length == 0) return 0;

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

                static int BFS2(int[][] board)
                {
                    if (board.Length == 0) return 0;
                    if (board.Length == 1) return board[0][0] == 1 ? -1 : 0;

                    int side = board.Length;
                    int limit = side * side;

                    int[] maxCost = new int[limit];
                    Array.Fill(maxCost, int.MaxValue);

                    PriorityQueue<int, int> queue = new();
                    queue.Enqueue(1, 0);
                    while (queue.TryDequeue(out int curr, out int cost))
                    {
                        if (cost >= maxCost[^1]) continue;

                        int nextCost = cost + 1;
                        int rollLimit = Math.Min(curr + 6, limit);

                        for (int roll = curr + 1; roll <= rollLimit; roll++)
                        {
                            ConvertIndex(board.Length, roll - 1, out int row, out int col);
                            int next = board[row][col] == -1 ? roll : board[row][col];

                            if (nextCost < maxCost[next - 1])
                            {
                                maxCost[next - 1] = nextCost;
                                queue.Enqueue(next, nextCost);
                            }
                        }
                    }

                    return maxCost[^1] == int.MaxValue ? -1 : maxCost[^1];
                }

                string solution = "bfs2";
                int actual =
                    solution == "dp" ? DP(board) :
                    solution == "bfs" ? BFS(board) :
                    solution == "bfs2" ? BFS2(board) :
                    throw new NotImplementedException(solution);

                Assert.Equal(expected, actual);
            }

            /// <summary>
            /// </summary>
            /// <see cref=""/>
            [Theory]
            [InlineData("[[2,1,1],[1,1,0],[0,1,1]]", 4)]
            [InlineData("[[2,1,1],[0,1,1],[1,0,1]]", -1)]
            [InlineData("[[0,2]]", 0)]
            public void OrangesRotting(string input, int expected)
            {
                int[][] grid = input.Parse2DArray(int.Parse).Select(a => a.ToArray()).ToArray();
                //-
                static int InternalBFS(int[][] grid)
                {
                    int fresh = 0;
                    int rows = grid.Length;
                    int cols = grid[0].Length;

                    // Initialize queue of rotten and count fresh
                    List<int> firstBatch = [];
                    for (int j = 0; j < rows; j++)
                    {
                        for (int i = 0; i < cols; i++)
                        {
                            switch (grid[j][i])
                            {
                                case 0:
                                    continue;
                                case 1:
                                    fresh++;
                                    break;
                                case 2:
                                    firstBatch.Add(j * cols + i);
                                    break;
                            }
                        }
                    }

                    Queue<int[]> queue = [];
                    queue.Enqueue(firstBatch.ToArray());

                    // Count how long it will take for all to rot
                    int duration = 0;
                    while (fresh > 0 && queue.TryDequeue(out int[]? rotIndexes))
                    {
                        duration++;

                        List<int> nextBatch = [];
                        foreach (int rotIndex in rotIndexes)
                        {
                            int row = rotIndex / cols;
                            int col = rotIndex % cols;

                            if (row > 0 && grid[row - 1][col] == 1)
                            {
                                grid[row - 1][col] = 2;
                                fresh--;
                                nextBatch.Add(rotIndex - cols);
                            }

                            if (row + 1 < rows && grid[row + 1][col] == 1)
                            {
                                grid[row + 1][col] = 2;
                                fresh--;
                                nextBatch.Add(rotIndex + cols);
                            }

                            if (col > 0 && grid[row][col - 1] == 1)
                            {
                                grid[row][col - 1] = 2;
                                fresh--;
                                nextBatch.Add(rotIndex - 1);
                            }

                            if (col + 1 < cols && grid[row][col + 1] == 1)
                            {
                                grid[row][col + 1] = 2;
                                fresh--;
                                nextBatch.Add(rotIndex + 1);
                            }
                        }

                        if (nextBatch.Count > 0) queue.Enqueue(nextBatch.ToArray());
                    }

                    return fresh == 0 ? duration : -1;
                }

                int actual = InternalBFS(grid);

                Assert.Equal(expected, actual);
            }
        }

        [Trait("Difficulty", "Hard")]
        public class Hard
        {
            /// <summary>
            /// 127. Word Ladder
            /// A transformation sequence from word beginWord to word endWord using a dictionary wordList is a sequence of words beginWord -> s1 -> s2 -> ... -> sk such that:
            /// Every adjacent pair of words differs by a single letter.
            /// Every si for 1 <= i <= k is in wordList.Note that beginWord does not need to be in wordList.
            /// sk == endWord
            /// Given two words, beginWord and endWord, and a dictionary wordList, return the number of words in the shortest transformation sequence from beginWord to endWord, or 0 if no such sequence exists.
            /// </summary>
            /// <see cref="https://leetcode.com/problems/word-ladder/?envType=study-plan-v2&envId=top-interview-150"/>
            [Theory]
            [InlineData("hit", "cog", "[hot,dot,dog,lot,log,cog]", 5)]
            [InlineData("hit", "cog", "[hot,dot,dog,lot,log]", 0)]
            public void LadderLength(string beginWord, string endWord, string listInput, int expected)
            {
                IList<string> wordList = listInput.Parse1DArray(x => x).ToList();

                static Dictionary<string, HashSet<int>> BuildGraph(IList<string> words)
                {
                    Dictionary<string, HashSet<int>> graph = new();

                    for (int left = 0; left < words.Count - 1; left++)
                    {
                        string leftWord = words[left];

                        for (int right = left + 1; right < words.Count; right++)
                        {
                            string rightWord = words[right];

                            bool valid = true;
                            for (int i = 0, diff = 0; valid && i < leftWord.Length; i++)
                            {
                                if (leftWord[i] != rightWord[i] && ++diff > 1) valid = false;
                            }

                            // Skip invalid words (differ by more than 1 character)
                            if (!valid) continue;

                            if (!graph.TryGetValue(leftWord, out var leftSet)) graph[leftWord] = leftSet = [];
                            leftSet.Add(right);

                            if (!graph.TryGetValue(rightWord, out var rightSet)) graph[rightWord] = rightSet = [];
                            rightSet.Add(left);
                        }
                    }

                    return graph;
                }

                static int BFS(string src, string dst, IList<string> words)
                {
                    if (!words.Contains(dst)) return 0;
                    if (!words.Contains(src)) words.Add(src);
                    words.Add(dst);

                    var graph = BuildGraph(words);

                    Queue<Tuple<string, int>> queue = new([new(src, 0)]);
                    HashSet<string> visited = new([src]);

                    while (queue.TryDequeue(out var tuple))
                    {
                        string currWord = tuple.Item1;
                        int currCost = tuple.Item2;

                        if (currWord == dst) return 1 + currCost;

                        // Skip empty edges
                        if (!graph.TryGetValue(currWord, out var nextIdxs)) continue;

                        foreach (var nextIdx in nextIdxs)
                        {
                            string nextWord = words[nextIdx];

                            // Skip visited words
                            if (visited.Contains(nextWord)) continue;

                            // Enqueue next exploration
                            queue.Enqueue(new(nextWord, currCost + 1));
                        }
                    }

                    return 0;
                }

                static int BFSDual(string src, string dst, IList<string> words)
                {
                    if (!words.Contains(dst)) return 0;
                    if (!words.Contains(src)) words.Add(src);
                    words.Add(dst);

                    var graph = BuildGraph(words);

                    Queue<Tuple<string, int, char>> queue = new([new(src, 0, 's'), new(dst, 0, 'd')]);
                    Dictionary<string, int> visitedSrc = new() { { src, 0 }, };
                    Dictionary<string, int> visitedDst = new() { { dst, 0 }, };

                    while (queue.TryDequeue(out var tuple))
                    {
                        string currWord = tuple.Item1;
                        int currCost = tuple.Item2;
                        char direction = tuple.Item3;

                        if (visitedSrc.TryGetValue(currWord, out int srcCost) && visitedDst.TryGetValue(currWord, out int dstCost))
                        {
                            // Found on both sides, this is the stopping criteria
                            return 1 + srcCost + dstCost;
                        }

                        // Skip empty edges
                        if (!graph.TryGetValue(currWord, out var nextIdxs)) continue;

                        foreach (var nextIdx in nextIdxs)
                        {
                            string nextWord = words[nextIdx];

                            // Skip visited words
                            if ((direction == 's' && visitedSrc.ContainsKey(nextWord)) || (direction == 'd' && visitedDst.ContainsKey(nextWord))) continue;

                            // Enqueue next exploration
                            switch (direction)
                            {
                                case 's':
                                    visitedSrc.Add(nextWord, currCost + 1);
                                    break;
                                case 'd':
                                    visitedDst.Add(nextWord, currCost + 1);
                                    break;
                            }

                            queue.Enqueue(new(nextWord, currCost + 1, direction));
                        }
                    }

                    return 0;
                }

                static int Dijkstra(string src, string dst, IList<string> words)
                {
                    if (!words.Contains(dst)) return 0;
                    if (!words.Contains(src)) words.Add(src);
                    words.Add(dst);

                    var graph = BuildGraph(words);

                    Dictionary<string, int> minCosts = new() { { src, 0 }, { dst, int.MaxValue }, };
                    PriorityQueue<string, int> queue = new([(src, 0)]);

                    while (queue.TryDequeue(out string? currWord, out int currCost))
                    {
                        // Skip empty edges
                        if (!graph.TryGetValue(currWord, out var nextIdxs)) continue;

                        foreach (var nextIdx in nextIdxs)
                        {
                            string nextWord = words[nextIdx];

                            // Skip increased costs
                            if (minCosts.TryGetValue(nextWord, out int minCost) && minCost <= currCost) continue;

                            // Enqueue next exploration
                            queue.Enqueue(nextWord, minCosts[nextWord] = currCost + 1);
                        }
                    }

                    return minCosts.TryGetValue(dst, out int result) && result != int.MaxValue ? 1 + result : 0;
                }

                string solution = "bfsdual";
                int actual =
                    solution == "bfs" ? BFS(beginWord, endWord, wordList) :
                    solution == "bfsdual" ? BFSDual(beginWord, endWord, wordList) :
                    solution == "dijkstra" ? Dijkstra(beginWord, endWord, wordList) :
                    throw new NotImplementedException(solution);

                Assert.Equal(expected, actual);
            }
        }
    }
}
