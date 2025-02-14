using Microsoft.VisualStudio.TestPlatform.Utilities;
using System.Net;
using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using static leetcode.Lists.Top150.BinaryTrees;

namespace leetcode.Lists.Top150
{
    public class TreesGraphs
    {
        // 200. Number of Islands
        // Given an m x n 2D binary grid grid which represents a map of '1's(land) and '0's(water)| return the number of islands.
        // An island is surrounded by water and is formed by connecting adjacent lands horizontally or vertically. You may assume all four edges of the grid are all surrounded by water.
        [Trait("Company", "Amazon")]
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

        // 297. Serialize and Deserialize Binary Tree
        // Serialization is the process of converting a data structure or object into a sequence of bits so that it can be stored in a file or memory buffer, or transmitted across a network connection link to be reconstructed later in the same or another computer environment.
        // Design an algorithm to serialize and deserialize a binary tree.There is no restriction on how your serialization/deserialization algorithm should work. You just need to ensure that a binary tree can be serialized to a string and this string can be deserialized to the original tree structure.
        // Clarification: The input/output format is the same as how LeetCode serializes a binary tree. You do not necessarily need to follow this format, so please be creative and come up with different approaches yourself.
        [Trait("Difficulty", "Hard")]
        [Trait("Company", "Amazon")]
        [Theory]
        [MemberData(nameof(CodecData))]
        public void CodecTest(TreeNode? root)
        {
            Codec codec = new();

            TreeNode.AssertEqual(root, codec.deserialize(codec.serialize(root)));
        }

        public static IEnumerable<object[]> CodecData =
            [
            [new TreeNode(-11, new TreeNode(2), new TreeNode(3, new TreeNode(4), new TreeNode(5)))],
            [new TreeNode(1, new TreeNode(2), new TreeNode(3, new TreeNode(4), new TreeNode(5)))],
            [null],
            ];

        public class TreeNode
        {
            public int val;
            public TreeNode left;
            public TreeNode right;
            public TreeNode(int x, TreeNode left = null, TreeNode right = null) { val = x; this.left = left; this.right = right; }

            public override string ToString()
            {
                return $"{val}[{left?.val}][{right?.val}]";
            }

            public static void AssertEqual(TreeNode? a, TreeNode? b)
            {
                if (a == null && b == null) return;

                Assert.Equal(a?.val, b?.val);

                AssertEqual(a?.left, b?.left);
                AssertEqual(a?.right, b?.right);
            }
        }

        public class Codec
        {

            // Encodes a tree to a single string.
            public string serialize(TreeNode root)
            {
                return root == null ? "#" : serialize(root.right) + "|" + serialize(root.left) + "|" + root.val;
            }

            // Decodes your encoded data to tree.
            public TreeNode deserialize(string data)
            {
                Stack<TreeNode> stack = new();

                int sign = 1;
                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i] == '-')
                    {
                        sign = -1;
                    }
                    else if (char.IsDigit(data[i]))
                    {
                        int val = 0;
                        while (i < data.Length && char.IsDigit(data[i])) { val = 10 * val + (data[i] - '0'); i++; }
                        val *= sign;
                        sign = 1;

                        TreeNode n = new TreeNode(val);
                        stack.TryPop(out n.left);
                        stack.TryPop(out n.right);
                        stack.Push(n);
                    }
                    else if (data[i] == '#')
                    {
                        stack.Push(null);
                    }
                }

                stack.TryPop(out TreeNode? result);

                return result;
            }
        }

    }
}
