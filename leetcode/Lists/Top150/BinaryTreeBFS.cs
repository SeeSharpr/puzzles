using leetcode.Types.BinaryTree;

namespace leetcode.Lists.Top150
{
    public class BinaryTreeBFS
    {
        // 199. Binary Tree Right Side View
        // Given the root of a binary tree, imagine yourself standing on the right side of it, return the values of the nodes you can see ordered from top to bottom.
        [Trait("Difficulty", "Medium")]
        [Theory]
        [InlineData("[1,2,3,null,5,null,4]", "[1,3,4]")]
        [InlineData("[1,2,3,4,null,null,null,5]", "[1,3,4,5]")]
        [InlineData("[1,null,3]", "[1,3]")]
        [InlineData("[]", "[]")]
        public void RightSideView(string input, string output)
        {
            TreeNode? root = input.ParseLCTree(TreeNode.Create, TreeNode.Update);
            IList<int> expected = output.ParseArrayStringLC(int.Parse).ToList();

            int previous = 0;
            List<int> actual = [];
            if (root != null)
            {
                actual.Add(root.val);
                Queue<KeyValuePair<int, TreeNode>> queue = new([new(0, root)]);

                while (queue.TryDequeue(out KeyValuePair<int, TreeNode> pair))
                {
                    if (pair.Key > previous)
                    {
                        actual.Add(pair.Value!.val);
                        previous = pair.Key;
                    }

                    if (pair.Value.right != null) queue.Enqueue(new KeyValuePair<int, TreeNode>(pair.Key + 1, pair.Value.right));
                    if (pair.Value.left != null) queue.Enqueue(new KeyValuePair<int, TreeNode>(pair.Key + 1, pair.Value.left));
                }
            }

            Assert.Equal(expected, actual);
        }
    }
}
