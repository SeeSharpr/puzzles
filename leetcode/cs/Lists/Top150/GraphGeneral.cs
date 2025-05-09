﻿using leetcode.Types.Graph;

namespace leetcode.Lists.Top150
{
    public class GraphGeneral
    {
        [Trait("Difficulty", "Medium")]
        public class Medium
        {
            // 200. Number of Islands
            // Given an m x n 2D binary grid grid which represents a map of '1's(land) and '0's(water)| return the number of islands.
            // An island is surrounded by water and is formed by connecting adjacent lands horizontally or vertically. You may assume all four edges of the grid are all surrounded by water.
            [Trait("Company", "Amazon")]
            [Theory]
            [InlineData("[[1,1,1,1,0],[1,1,0,1,0],[1,1,0,0,0],[0,0,0,0,0]]", 1)]
            [InlineData("[[1,1,0,0,0],[1,1,0,0,0],[0,0,1,0,0],[0,0,0,1,1]]", 3)]
            [InlineData("[[1,0,1,1,0,1,1]]", 3)]
            [InlineData("[[1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,0,1,0,1,1],[0,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,0],[1,0,1,1,1,0,0,1,1,0,1,1,1,1,1,1,1,1,1,1],[1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1],[1,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1],[1,0,1,1,1,1,1,1,0,1,1,1,0,1,1,1,0,1,1,1],[0,1,1,1,1,1,1,1,1,1,1,1,0,1,1,0,1,1,1,1],[1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,0,1,1],[1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1],[1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1],[0,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1],[1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1],[1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1],[1,1,1,1,1,0,1,1,1,1,1,1,1,0,1,1,1,1,1,1],[1,0,1,1,1,1,1,0,1,1,1,0,1,1,1,1,0,1,1,1],[1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,0],[1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,0,0],[1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1],[1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1],[1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1]]", 1)]
            public void NumIslands(string input, int expected)
            {
                static void NukeIsland(char[][] grid, int x, int y)
                {
                    grid[y][x] = '0';

                    if (x - 1 >= 0 && grid[y][x - 1] == '1') NukeIsland(grid, x - 1, y);
                    if (x + 1 < grid[y].Length && grid[y][x + 1] == '1') NukeIsland(grid, x + 1, y);
                    if (y - 1 >= 0 && grid[y - 1][x] == '1') NukeIsland(grid, x, y - 1);
                    if (y + 1 < grid.Length && grid[y + 1][x] == '1') NukeIsland(grid, x, y + 1);
                }

                static int Recursive(char[][] grid)
                {
                    int count = 0;
                    for (int y = 0; y < grid.Length; y++)
                    {
                        for (int x = 0; x < grid[y].Length; x++)
                        {
                            if (grid[y][x] == '0') continue;

                            count++;
                            NukeIsland(grid, x, y);
                        }
                    }

                    return count;
                }

                static void Visit(char[][] grid, int index, int cols, HashSet<int> visited)
                {
                    int y = index / cols;
                    int x = index % cols;
                    int indexUp = index - cols;
                    int indexDown = index + cols;
                    int indexLeft = index - 1;
                    int indexRight = index + 1;

                    if (y - 1 >= 0 && grid[y - 1][x] == '1' && visited.Add(indexUp)) Visit(grid, indexUp, cols, visited);
                    if (y + 1 < grid.Length && grid[y + 1][x] == '1' && visited.Add(indexDown)) Visit(grid, indexDown, cols, visited);
                    if (x - 1 >= 0 && grid[y][x - 1] == '1' && visited.Add(indexLeft)) Visit(grid, indexLeft, cols, visited);
                    if (x + 1 < cols && grid[y][x + 1] == '1' && visited.Add(indexRight)) Visit(grid, indexRight, cols, visited);
                }

                static int Recursive2(char[][] grid)
                {
                    if (grid == null || grid.Length == 0 || grid[0].Length == 0) return 0;

                    int cols = grid[0].Length;

                    HashSet<int> visited = [];
                    int count = 0;
                    for (int y = 0; y < grid.Length; y++)
                    {
                        for (int x = 0; x < grid[0].Length; x++)
                        {
                            if (grid[y][x] == '0') continue;

                            int index = y * cols + x;
                            if (visited.Add(index))
                            {
                                Visit(grid, index, cols, visited);
                                count++;
                            }
                        }
                    }

                    return count;
                }


                static int Interactive(char[][] grid)
                {
                    if (grid == null || grid.Length == 0) return 0;

                    int count = 0;

                    int rows = grid.Length;
                    int cols = grid[0].Length;

                    for (int j = 0; j < rows; j++)
                    {
                        for (int i = 0; i < cols; i++)
                        {
                            if (grid[j][i] == '0') continue;

                            grid[j][i] = '0';

                            count++;
                            Queue<int> queue = new([j * cols + i]);
                            while (queue.TryDequeue(out var offset))
                            {
                                int y = offset / cols;
                                int x = offset % cols;

                                if (y > 0 && grid[y - 1][x] == '1') { grid[y - 1][x] = '0'; queue.Enqueue((y - 1) * cols + x); }
                                if (y + 1 < rows && grid[y + 1][x] == '1') { grid[y + 1][x] = '0'; queue.Enqueue((y + 1) * cols + x); }
                                if (x > 0 && grid[y][x - 1] == '1') { grid[y][x - 1] = '0'; queue.Enqueue(y * cols + x - 1); }
                                if (x + 1 < cols && grid[y][x + 1] == '1') { grid[y][x + 1] = '0'; queue.Enqueue(y * cols + x + 1); }
                            }
                        }
                    }

                    return count;
                }

                foreach (Func<char[][], int> solution in new[] { Recursive, Interactive, Recursive2 })
                {
                    char[][] grid = input.Parse2DArray(char.Parse).Select(e => e.ToArray()).ToArray();

                    int actual = solution.Invoke(grid);

                    Assert.Equal(expected, actual);
                }
            }
        }

        // 130. Surrounded Regions
        // You are given an m x n matrix board containing letters 'X' and 'O', capture regions that are surrounded:
        // Connect: A cell is connected to adjacent cells horizontally or vertically.
        // Region: To form a region connect every 'O' cell.
        // Surround: The region is surrounded with 'X' cells if you can connect the region with 'X' cells and none of the region cells are on the edge of the board.
        // To capture a surrounded region, replace all 'O's with 'X's in-place within the original board.You do not need to return anything.
        [Trait("Difficulty", "Medium")]
        [Theory]
        [InlineData("[[\"X\",\"X\",\"X\",\"X\"],[\"X\",\"O\",\"O\",\"X\"],[\"X\",\"X\",\"O\",\"X\"],[\"X\",\"O\",\"X\",\"X\"]]", "[[\"X\",\"X\",\"X\",\"X\"],[\"X\",\"X\",\"X\",\"X\"],[\"X\",\"X\",\"X\",\"X\"],[\"X\",\"O\",\"X\",\"X\"]]")]
        [InlineData("[[\"X\"]]", "[[\"X\"]]")]
        [InlineData("[[\"O\",\"O\",\"O\"],[\"O\",\"O\",\"O\"],[\"O\",\"O\",\"O\"]]", "[[\"O\",\"O\",\"O\"],[\"O\",\"O\",\"O\"],[\"O\",\"O\",\"O\"]]")]
        public void Solve(string input, string output)
        {
            char[][] board = input.ParseNestedArrayStringLC(x => x.Contains('X') ? 'X' : 'O').Select(x => x.ToArray()).ToArray();
            char[][] expected = output.ParseNestedArrayStringLC(x => x.Contains('X') ? 'X' : 'O').Select(x => x.ToArray()).ToArray();

            static void MarkSafeInteractive(char[][] a, int x, int y)
            {
                if (a[y][x] != 'O') return;

                int jl = a.Length - 1;
                int il = a[0].Length - 1;

                Queue<int> iq = new([x]);
                Queue<int> jq = new([y]);

                while (iq.TryDequeue(out int i) && jq.TryDequeue(out int j))
                {
                    if (a[j][i] == 'S') continue;

                    a[j][i] = 'S';

                    if (i > 0 && a[j][i - 1] == 'O') { iq.Enqueue(i - 1); jq.Enqueue(j); }
                    if (i < il && a[j][i + 1] == 'O') { iq.Enqueue(i + 1); jq.Enqueue(j); }
                    if (j > 0 && a[j - 1][i] == 'O') { iq.Enqueue(i); jq.Enqueue(j - 1); }
                    if (j < jl && a[j + 1][i] == 'O') { iq.Enqueue(i); jq.Enqueue(j + 1); }
                }
            }

            static void MarkSafeRecursive(char[][] a, int x, int y)
            {
                if (a[y][x] != 'O') return;

                if (a[y][x] == 'O') a[y][x] = 'S';

                if (y > 0) MarkSafeRecursive(a, x, y - 1);
                if (x > 0) MarkSafeRecursive(a, x - 1, y);
                if (x < a[0].Length - 1) MarkSafeRecursive(a, x + 1, y);
                if (y < a.Length - 1) MarkSafeRecursive(a, x, y + 1);

            }

            if (board.Length > 0)
            {
                for (int y = 0; y < board.Length; y++)
                {
                    MarkSafeInteractive(board, 0, y);
                    MarkSafeInteractive(board, board[0].Length - 1, y);
                    //if (board[y][0] == 'O') MarkSafeRecursive(board, 0, y);
                    //if (board[y][board[0].Length - 1] == 'O') MarkSafeRecursive(board, board[0].Length - 1, y);
                }

                for (int x = 0; x < board[0].Length - 1; x++)
                {
                    MarkSafeInteractive(board, x, 0);
                    MarkSafeInteractive(board, x, board.Length - 1);
                    //if (board[0][x] == 'O') MarkSafeRecursive(board, x, 0);
                    //if (board[board.Length - 1][x] == 'O') MarkSafeRecursive(board, x, board.Length - 1);
                }

                for (int y = 0; y < board.Length; y++)
                {
                    for (int x = 0; x < board[0].Length; x++)
                    {
                        switch (board[y][x])
                        {
                            case 'S':
                                board[y][x] = 'O';
                                break;
                            case 'O':
                                board[y][x] = 'X';
                                break;
                        }
                    }
                }
            }

            for (int y = 0; y < board.Length; y++)
            {
                Assert.Equal(expected[y], board[y]);
            }
        }

        // 133. Clone Graph
        // Given a reference of a node in a connected undirected graph.
        // Return a deep copy (clone) of the graph.
        // Each node in the graph contains a value (int) and a list (List[Node]) of its neighbors.
        // Test case format:
        // For simplicity, each node's value is the same as the node's index(1-indexed). For example, the first node with val == 1, the second node with val == 2, and so on.The graph is represented in the test case using an adjacency list.
        // An adjacency list is a collection of unordered lists used to represent a finite graph.Each list describes the set of neighbors of a node in the graph.
        // The given node will always be the first node with val = 1.You must return the copy of the given node as a reference to the cloned graph.
        [Trait("Difficulty", "Medium")]
        [Theory]
        [InlineData("[[2,4],[1,3],[2,4],[1,3]]", "[[2,4],[1,3],[2,4],[1,3]]")]
        [InlineData("[[]]", "[[]]")]
        [InlineData("[]", "[]")]
        public void CloneGraph(string input, string output)
        {
            Node? node = Node.CreateGraph(input.Parse2DArray(int.Parse).Select(e => e.ToArray()).ToArray());
            Node? expected = Node.CreateGraph(output.Parse2DArray(int.Parse).Select(e => e.ToArray()).ToArray());

            static SortedDictionary<int, Node> InternalDiscoverNodes(Node node)
            {
                SortedDictionary<int, Node> result = new();
                Queue<Node> queue = new([node]);

                while (queue.TryDequeue(out Node? queueNode))
                {
                    _ = result.TryAdd(queueNode.val, queueNode);

                    foreach (Node queueNeighbor in queueNode.neighbors)
                    {
                        if (result.TryAdd(queueNeighbor.val, queueNeighbor))
                        {
                            queue.Enqueue(queueNeighbor);
                        }
                    }
                }

                return result;
            }

            static Node? InternalCloneGraph(Node? node)
            {
                if (node == null) return null;

                SortedDictionary<int, Node> oldNodes = InternalDiscoverNodes(node);
                SortedDictionary<int, Node> newNodes = new();

                foreach (Node oldNode in oldNodes.Values)
                {
                    newNodes.Add(oldNode.val, new Node(oldNode.val));
                }

                foreach (Node oldNode in oldNodes.Values)
                {
                    foreach (var oldNeighbor in oldNode.neighbors)
                    {
                        newNodes[oldNode.val].neighbors.Add(newNodes[oldNeighbor.val]);
                    }
                }

                return newNodes.Values.FirstOrDefault();
            }

            Node? actual = InternalCloneGraph(node);

            Assert.Equal(Node.ToEdgeArray(expected), Node.ToEdgeArray(actual));
        }

        // 399. Evaluate Division
        // You are given an array of variable pairs equations and an array of real numbers values, where equations[i] = [Ai, Bi] and values[i] represent the equation Ai / Bi = values[i]. Each Ai or Bi is a string that represents a single variable.
        // You are also given some queries, where queries[j] = [Cj, Dj] represents the jth query where you must find the answer for Cj / Dj = ?.
        // Return the answers to all queries.If a single answer cannot be determined, return -1.0.
        // Note: The input is always valid. You may assume that evaluating the queries will not result in division by zero and that there is no contradiction.
        // Note: The variables that do not occur in the list of equations are undefined, so the answer cannot be determined for them.
        [Trait("Difficulty", "Medium")]
        [Theory]
        [InlineData("[[\"a\",\"b\"],[\"b\",\"c\"]]", "[2.0,3.0]", "[[\"a\",\"c\"],[\"b\",\"a\"],[\"a\",\"e\"],[\"a\",\"a\"],[\"x\",\"x\"]]", "[6.00000,0.50000,-1.00000,1.00000,-1.00000]")]
        [InlineData("[[\"a\",\"b\"],[\"b\",\"c\"],[\"bc\",\"cd\"]]", "[1.5,2.5,5.0]", "[[\"a\",\"c\"],[\"c\",\"b\"],[\"bc\",\"cd\"],[\"cd\",\"bc\"]]", "[3.75000,0.40000,5.00000,0.20000]")]
        [InlineData("[[\"a\",\"b\"]]", "[0.5]", "[[\"a\",\"b\"],[\"b\",\"a\"],[\"a\",\"c\"],[\"x\",\"y\"]]", "[0.50000,2.00000,-1.00000,-1.00000]")]
        public void CalcEquation(string inputEquations, string inputValues, string inputQueries, string output)
        {
            IList<IList<string>> equations = inputEquations.Parse2DArray(s => s.Replace("\"", "")).Select(e => (IList<string>)e.ToList()).ToList();
            double[] values = inputValues.Parse1DArray(double.Parse).ToArray();
            IList<IList<string>> queries = inputQueries.Parse2DArray(s => s.Replace("\"", "")).Select(e => (IList<string>)e.ToList()).ToList();
            double[] expected = output.Parse1DArray(double.Parse).ToArray();

            static Dictionary<string, Dictionary<string, double>> InternalBuild(IList<IList<string>> equations, double[] values)
            {
                Dictionary<string, Dictionary<string, double>> nodes = new();

                for (int i = 0; i < values.Length; i++)
                {
                    string num = equations[i][0];
                    string den = equations[i][1];
                    Dictionary<string, double>? edges;

                    if (!nodes.TryGetValue(num, out edges))
                    {
                        edges = new Dictionary<string, double>();
                        nodes[num] = edges;
                    }

                    edges[den] = values[i];

                    if (!nodes.TryGetValue(den, out edges))
                    {
                        edges = new Dictionary<string, double>();
                        nodes[den] = edges;
                    }

                    edges[num] = 1 / values[i];
                }

                return nodes;
            }

            static double[] InternalEval(Dictionary<string, Dictionary<string, double>> nodes, IList<IList<string>> queries)
            {
                List<double> result = [];

                foreach (var q in queries)
                {
                    string src = q[0];
                    string tgt = q[1];

                    // If the variable is undefined, bail out
                    if (!nodes.ContainsKey(src) || !nodes.ContainsKey(tgt))
                    {
                        result.Add(-1);
                        continue;
                    }

                    // Traverse the nodes
                    Queue<KeyValuePair<string, double>> queue = new([new(src, 1)]);
                    HashSet<string> attempted = new([src]);
                    bool found = false;
                    while (queue.TryDequeue(out KeyValuePair<string, double> queueNode))
                    {
                        // If we found the target node, bail out
                        if (queueNode.Key == tgt)
                        {
                            found = true;
                            result.Add(queueNode.Value);
                            break;
                        }

                        foreach (var neighbor in nodes[queueNode.Key])
                        {
                            // If we have already attempted this node, skip it
                            if (attempted.Contains(neighbor.Key)) continue;

                            // Keep track of attempted nodes
                            attempted.Add(neighbor.Key);

                            // Accumulates the result of the search
                            queue.Enqueue(new(neighbor.Key, queueNode.Value * neighbor.Value));
                        }
                    }

                    if (!found)
                    {
                        result.Add(-1);
                    }
                }

                return result.ToArray();
            }

            double[] actual = InternalEval(InternalBuild(equations, values), queries);

            Assert.Equal(expected, actual);
        }

        // 207. Course Schedule
        // There are a total of numCourses courses you have to take, labeled from 0 to numCourses - 1. You are given an array prerequisites where prerequisites[i] = [ai, bi] indicates that you must take course bi first if you want to take course ai.
        // For example, the pair [0, 1], indicates that to take course 0 you have to first take course 1.
        // Return true if you can finish all courses.Otherwise, return false.
        [Trait("Difficulty", "Medium")]
        [Theory]
        [InlineData(1, "[]", true)]
        [InlineData(2, "[[1,0]]", true)]
        [InlineData(2, "[[1,0],[0,1]]", false)]
        [InlineData(5, "[[1,4],[2,4],[3,1],[3,2]]", true)]
        public void CanFinish(int numCourses, string inputPrereq, bool expected)
        {
            int[][] prerequisites = inputPrereq.Parse2DArray(int.Parse).Select(e => e.ToArray()).ToArray();

            static Dictionary<int, HashSet<int>> InternalBuildGraph(int[][] prereqs)
            {
                Dictionary<int, HashSet<int>> inbound = new();

                for (int i = 0; i < prereqs.Length; i++)
                {
                    int from = prereqs[i][1];
                    int to = prereqs[i][0];

                    if (!inbound.TryGetValue(to, out HashSet<int>? predecessors))
                    {
                        predecessors = new();
                        inbound.Add(to, predecessors);
                    }

                    predecessors.Add(from);
                }

                return inbound;
            }

            static bool InternalHasCycles(int numCourses, Dictionary<int, HashSet<int>> inbound)
            {
                if (inbound.Count == 0) return false;

                // Find all the possible starting nodes
                Queue<int> queue = new();
                for (int i = 0; i < numCourses; i++)
                {
                    if (inbound.ContainsKey(i)) continue;

                    queue.Enqueue(i);
                }

                // Eliminate each possible starting node
                while (queue.TryDequeue(out int node))
                {
                    List<int> removes = new();

                    // Remove node as a predecessor of all nodes
                    foreach (var predPair in inbound)
                    {
                        if (predPair.Value.Remove(node) && predPair.Value.Count == 0)
                        {
                            queue.Enqueue(predPair.Key);
                            removes.Add(predPair.Key);
                        }
                    }

                    // Remove nodes that have been disconnected
                    foreach (var remove in removes)
                    {
                        inbound.Remove(remove);
                    }
                }

                return !inbound.Values.All(pred => pred.Count == 0);
            }

            bool actual = !InternalHasCycles(numCourses, InternalBuildGraph(prerequisites));

            Assert.Equal(expected, actual);
        }

        // 210. Course Schedule II
        // There are a total of numCourses courses you have to take, labeled from 0 to numCourses - 1. You are given an array prerequisites where prerequisites[i] = [ai, bi] indicates that you must take course bi first if you want to take course ai.
        // For example, the pair [0, 1], indicates that to take course 0 you have to first take course 1.
        // Return the ordering of courses you should take to finish all courses. If there are many valid answers, return any of them.If it is impossible to finish all courses, return an empty array.
        [Trait("Difficulty", "Medium")]
        [Theory]
        [InlineData(2, "[[1,0]]", "[0,1]")]
        [InlineData(4, "[[1,0],[2,0],[3,1],[3,2]]", "[0,1,2,3]")]
        [InlineData(1, "[]", "[0]")]
        public void FindOrder(int numCourses, string inputPrereqs, string output)
        {
            int[][] prerequisites = inputPrereqs.Parse2DArray(int.Parse).Select(e => e.ToArray()).ToArray();
            int[] expected = output.Parse1DArray(int.Parse).ToArray();

            static Dictionary<int, HashSet<int>> InternalBuildInboundGraph(int[][] prereqs)
            {
                Dictionary<int, HashSet<int>> inbound = new();

                foreach (int[] prereq in prereqs)
                {
                    int from = prereq[1];
                    int to = prereq[0];

                    if (!inbound.TryGetValue(to, out HashSet<int>? pred))
                    {
                        pred = new();
                        inbound[to] = pred;
                    }

                    pred.Add(from);
                }

                return inbound;
            }

            static int[] InternalFindOrder(int numCourses, Dictionary<int, HashSet<int>> inbound)
            {
                List<int> result = [];
                Queue<int> queue = new();
                for (int i = 0; i < numCourses; i++)
                {
                    if (!inbound.ContainsKey(i)) queue.Enqueue(i);
                }

                while (queue.TryDequeue(out int node))
                {
                    result.Add(node);

                    foreach (var inboundPair in inbound)
                    {
                        int to = inboundPair.Key;
                        HashSet<int> from = inboundPair.Value;

                        if (from.Remove(node) && from.Count == 0) queue.Enqueue(to);
                    }
                }

                int remainingNodes = inbound.Values.Select(set => set.Count).Sum();

                return remainingNodes == 0 ? result.ToArray() : [];
            }

            int[] actual = InternalFindOrder(numCourses, InternalBuildInboundGraph(prerequisites));

            Assert.Equal(expected, actual);
        }
    }
}
