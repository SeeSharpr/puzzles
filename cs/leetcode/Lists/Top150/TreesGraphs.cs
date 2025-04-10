using leetcode.Types.BinaryTree;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;

namespace leetcode.Lists.Top150
{
    public class TreesGraphs
    {
        [Trait("Difficulty", "Easy")]
        public class Easy
        {
            /// <summary>
            /// 543. Diameter of Binary Tree
            /// Given the root of a binary tree, return the length of the diameter of the tree.
            /// The diameter of a binary tree is the length of the longest path between any two nodes in a tree. This path may or may not pass through the root.
            /// The length of a path between two nodes is represented by the number of edges between them.
            /// </summary>
            /// <see cref="https://leetcode.com/problems/diameter-of-binary-tree/description/"/>
            [Theory]
            [InlineData("[1,2,3,4,5]", 3)]
            [InlineData("[1,2]", 1)]
            public void DiameterOfBinaryTree(string input, int expected)
            {
                TreeNode? root = input.ParseLCTree(TreeNode.Create, TreeNode.Update);
                //-
                static int Diameter(TreeNode? node, ref int max)
                {
                    if (node == null) return 0;

                    int left = Diameter(node?.left, ref max);
                    int right = Diameter(node?.right, ref max);
                    int diameter = left + right;

                    max = Math.Max(max, diameter);

                    return 1 + Math.Max(left, right);
                }

                int actual = 0;
                _ = Diameter(root, ref actual);
                //-
                Assert.Equal(expected, actual);
            }
        }

        [Trait("Difficulty", "Hard")]
        public class Hard
        {
            /// <summary>
            /// 269. Alien Dictionary
            /// There is a new alien language that uses the English alphabet.However, the order of the letters is unknown to you.
            /// You are given a list of strings words from the alien language's dictionary. Now it is claimed that the strings in words are sorted lexicographically by the rules of this new language.
            /// If this claim is incorrect, and the given arrangement of string in words cannot correspond to any order of letters, return "".
            /// Otherwise, return a string of the unique letters in the new alien language sorted in lexicographically increasing order by the new language's rules. If there are multiple solutions, return any of them.
            /// </summary>
            /// <see cref="https://leetcode.com/problems/alien-dictionary/description/"/>
            public class AlienNode(char letter)
            {
                public readonly char letter = letter;
                public readonly HashSet<AlienNode> pred = [];
                public readonly HashSet<AlienNode> succ = [];

                public override bool Equals(object? obj)
                {
                    return obj is AlienNode node && node.letter == letter;
                }

                public override int GetHashCode()
                {
                    return letter.GetHashCode();
                }

                public override string ToString()
                {
                    return $"{{{string.Join("", pred.Select(p => p.letter))}}}<-({letter})->{{{string.Join("", succ.Select(p => p.letter))}}}";
                }
            }

            [Theory]
            [InlineData("[wrt,wrf,er,ett,rftt]", "wertf")]
            [InlineData("[z,x]", "zx")]
            [InlineData("[z,x,z]", "")]
            [InlineData("[zy,zx]", "zyx")]
            [InlineData("[ac,ab,b]", "acb")]
            [InlineData("[ac,ab,zc,zb]", "aczb")]
            [InlineData("[abc,ab]", "")]
            [InlineData("[wrt,wrtkj]", "wrtkj")]
            [InlineData("[aba]", "ab")]
            public void AlienOrder(string input, string expected)
            {
                string[] words = input.Parse1DArray().ToArray();

                static string InternalAlienOrder(string[] words)
                {
                    Dictionary<char, AlienNode> graph = [];

                    // Initialize the graph
                    foreach (var word in words)
                    {
                        foreach (var letter in word)
                        {
                            if (!graph.ContainsKey(letter))
                            {
                                graph[letter] = new(letter);
                            }
                        }
                    }

                    // Infer order
                    for (int i = 1; i < words.Length; i++)
                    {
                        string prev = words[i - 1];
                        string curr = words[i];

                        // Claim is incorrect, prefix word cannot come after
                        if (curr.Length < prev.Length && prev.StartsWith(curr)) return "";

                        for (int pi = 0, ci = 0; pi < prev.Length && ci < curr.Length; pi++, ci++)
                        {
                            char p = prev[pi];
                            char c = curr[pi];

                            // If letters are the same we can't infer any information
                            if (p == c) continue;

                            // They are different, we can infer precedence between these two letters
                            var pSucc = graph[p].succ;
                            var cPred = graph[c].pred;

                            pSucc.Add(graph[c]);
                            cPred.Add(graph[p]);

                            // Once we found the first difference, there is nothing else we can infer
                            break;
                        }
                    }

                    // Traverse in order
                    StringBuilder sb = new();
                    Queue<AlienNode> queue = new(graph.Values.Where(n => n.pred.Count == 0));

                    while (queue.TryDequeue(out AlienNode? node))
                    {
                        sb.Append(node.letter);

                        foreach (var succ in node.succ)
                        {
                            // Removes the edge between the current node and its successor
                            // If the successor becomes a starter, add it to the queue for further processing
                            if (succ.pred.Remove(node) && succ.pred.Count == 0) queue.Enqueue(succ);
                        }

                        // Removes the successors from this node
                        node.succ.Clear();

                        // Removes the node from the graph
                        graph.Remove(node.letter);
                    }

                    // Return the partial order or empty if a cycle was detected
                    return graph.Count == 0 ? sb.ToString() : "";
                }

                string actual = InternalAlienOrder(words);

                Assert.Equal(expected, actual);
            }


            // 297. Serialize and Deserialize Binary Tree
            // Serialization is the process of converting a data structure or object into a sequence of bits so that it can be stored in a file or memory buffer, or transmitted across a network connection link to be reconstructed later in the same or another computer environment.
            // Design an algorithm to serialize and deserialize a binary tree.There is no restriction on how your serialization/deserialization algorithm should work. You just need to ensure that a binary tree can be serialized to a string and this string can be deserialized to the original tree structure.
            // Clarification: The input/output format is the same as how LeetCode serializes a binary tree. You do not necessarily need to follow this format, so please be creative and come up with different approaches yourself.
            public class Codec
            {
                // Encodes a tree to a single string.
                public string serialize(TreeNode? root)
                {
                    return root == null ? "#" : serialize(root.right) + "|" + serialize(root.left) + "|" + root.val;
                }

                // Decodes your encoded data to tree.
                public TreeNode? deserialize(string data)
                {
                    Stack<TreeNode?> stack = new();

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

                            TreeNode n = new(val);
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

            [Trait("Company", "Amazon")]
            [Theory]
            [InlineData("[1,2,3,null,null,4,5]")]
            [InlineData("[]")]
            [InlineData("[-11,2,3,null,null,4,5]")]
            public void CodecTest(string input)
            {
                TreeNode? root = input.ParseLCTree(TreeNode.Create, TreeNode.Update);

                Codec codec = new();

                TreeNode.AssertEqual(root, codec.deserialize(codec.serialize(root)));
            }

            /// <summary>
            /// 317. Shortest Distance from All Buildings
            /// You are given an m x n grid grid of values 0, 1, or 2, where:
            /// each 0 marks an empty land that you can pass by freely,
            /// each 1 marks a building that you cannot pass through, and
            /// each 2 marks an obstacle that you cannot pass through.
            /// You want to build a house on an empty land that reaches all buildings in the shortest total travel distance.You can only move up, down, left, and right.
            /// Return the shortest travel distance for such a house.If it is not possible to build such a house according to the above rules, return -1.
            /// The total travel distance is the sum of the distances between the houses of the friends and the meeting point.
            /// </summary>
            /// <see cref="https://leetcode.com/problems/shortest-distance-from-all-buildings/description/"/>
            [Theory]
            [InlineData("[[1,0,2,0,1],[0,0,0,0,0],[0,0,1,0,0]]", 7)]
            [InlineData("[[1,0]]", 1)]
            [InlineData("[[1]]", -1)]
            public void ShortestDistance(string input, int expected)
            {
                int[][] grid = input.Parse2DArray(int.Parse).Select(x => x.ToArray()).ToArray();
                //-

                int actual = -1;
                Assert.Equal(expected, actual);
            }
        }
    }
}