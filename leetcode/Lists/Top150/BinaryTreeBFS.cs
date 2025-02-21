using leetcode.Types.BinaryTree;
using Newtonsoft.Json.Linq;

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

            List<int> actual = [];
            if (root != null)
            {
                actual.Add(root.val);
                int previous = 0;
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

        // 637. Average of Levels in Binary Tree
        // Given the root of a binary tree, return the average value of the nodes on each level in the form of an array.Answers within 10-5 of the actual answer will be accepted.
        [Trait("Difficulty", "Easy")]
        [Theory]
        [InlineData("[3,9,20,null,null,15,7]", "[3.00000,14.50000,11.00000]")]
        [InlineData("[3,9,20,15,7]", "[3.00000,14.50000,11.00000]")]
        public void AverageOfLevels(string input, string output)
        {
            TreeNode? root = input.ParseLCTree(TreeNode.Create, TreeNode.Update);
            IList<double> expected = output.ParseArrayStringLC(double.Parse).ToList();

            List<double> actual = [];

            if (root != null)
            {
                Queue<KeyValuePair<int, TreeNode>> queue = new([new(0, root)]);
                int current = 0;
                int count = 0;
                double sum = 0;
                while (queue.TryDequeue(out KeyValuePair<int, TreeNode> pair))
                {
                    if (pair.Key == current)
                    {
                        count++;
                        sum += pair.Value.val;
                    }
                    else
                    {
                        actual.Add(sum / count);
                        current = pair.Key;
                        sum = pair.Value.val;
                        count = 1;
                    }

                    if (pair.Value.left != null) queue.Enqueue(new(pair.Key + 1, pair.Value.left));
                    if (pair.Value.right != null) queue.Enqueue(new(pair.Key + 1, pair.Value.right));
                }

                if (count > 0) actual.Add(sum / count);
            }

            Assert.Equal(expected, actual);
        }
    }
}
