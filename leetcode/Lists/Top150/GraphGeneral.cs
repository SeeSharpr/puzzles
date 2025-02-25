using leetcode.Types.Graph;

namespace leetcode.Lists.Top150
{
    public class GraphGeneral
    {
        // 200. Number of Islands
        // Given an m x n 2D binary grid grid which represents a map of '1's(land) and '0's(water)| return the number of islands.
        // An island is surrounded by water and is formed by connecting adjacent lands horizontally or vertically. You may assume all four edges of the grid are all surrounded by water.
        [Trait("Company", "Amazon")]
        [Trait("Difficulty", "Medium")]
        [Theory]
        [InlineData("1,1,1,1,0|1,1,0,1,0|1,1,0,0,0|0,0,0,0,0", 1)]
        [InlineData("1,1,0,0,0|1,1,0,0,0|0,0,1,0,0|0,0,0,1,1", 3)]
        [InlineData("1,0,1,1,0,1,1", 3)]
        public void NumIslands(string input, int expected)
        {
            char[][] grid = input.ParseNestedArrayStringLC(char.Parse).Select(e => e.ToArray()).ToArray();

            static void NukeIsland(char[][] grid, int x, int y)
            {
                grid[y][x] = '0';

                if (x - 1 >= 0 && grid[y][x - 1] == '1') NukeIsland(grid, x - 1, y);
                if (x + 1 < grid[y].Length && grid[y][x + 1] == '1') NukeIsland(grid, x + 1, y);
                if (y - 1 >= 0 && grid[y - 1][x] == '1') NukeIsland(grid, x, y - 1);
                if (y + 1 < grid.Length && grid[y + 1][x] == '1') NukeIsland(grid, x, y + 1);
            }

            int actual = 0;
            for (int y = 0; y < grid.Length; y++)
            {
                for (int x = 0; x < grid[y].Length; x++)
                {
                    if (grid[y][x] == '1')
                    {
                        actual++;
                        NukeIsland(grid, x, y);
                    }
                }
            }

            Assert.Equal(expected, actual);
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
                    _= result.TryAdd(queueNode.val, queueNode);

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
    }
}
