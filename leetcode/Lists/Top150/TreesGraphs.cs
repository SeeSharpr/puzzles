using leetcode.Types.BinaryTree;

namespace leetcode.Lists.Top150
{
    public class TreesGraphs
    {
        // 297. Serialize and Deserialize Binary Tree
        // Serialization is the process of converting a data structure or object into a sequence of bits so that it can be stored in a file or memory buffer, or transmitted across a network connection link to be reconstructed later in the same or another computer environment.
        // Design an algorithm to serialize and deserialize a binary tree.There is no restriction on how your serialization/deserialization algorithm should work. You just need to ensure that a binary tree can be serialized to a string and this string can be deserialized to the original tree structure.
        // Clarification: The input/output format is the same as how LeetCode serializes a binary tree. You do not necessarily need to follow this format, so please be creative and come up with different approaches yourself.
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

        [Trait("Difficulty", "Hard")]
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
    }
}
