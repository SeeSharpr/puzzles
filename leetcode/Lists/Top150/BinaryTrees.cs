namespace leetcode.Lists.Top150
{
    public class BinaryTrees
    {
        public class TreeNode
        {
            public int val;
            public TreeNode left;
            public TreeNode right;
            public TreeNode(int val = 0, TreeNode left = null, TreeNode right = null)
            {
                this.val = val;
                this.left = left;
                this.right = right;
            }
        }

        // 104. Maximum Depth of Binary Tree
        // Given the root of a binary tree, return its maximum depth.
        // A binary tree's maximum depth is the number of nodes along the longest path from the root node down to the farthest leaf node.
        public static IEnumerable<object[]> MaxDepthData =>
            [
            [new TreeNode(3, new TreeNode(9), new TreeNode(20, new TreeNode(15), new TreeNode(7))), 3],
            [new TreeNode(1, null, new TreeNode(2)), 2]
            ];

        [Theory]
        [MemberData(nameof(MaxDepthData))]
        public void MaxDepth(TreeNode root, int expected)
        {
            static int _MaxDepth(TreeNode node)
            {
                return node == null ? 0 : 1 + Math.Max(_MaxDepth(node?.left), _MaxDepth(node?.right));
            }

            int result = _MaxDepth(root);

            Assert.Equal(expected, result);
        }
    }
}
