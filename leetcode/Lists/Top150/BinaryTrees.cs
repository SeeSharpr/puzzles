using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System.Collections;

namespace leetcode.Lists.Top150
{
    public class BinaryTrees
    {
        public class TreeNode : IEnumerable<TreeNode>
        {
            public int val;
            public TreeNode? left;
            public TreeNode? right;
            public TreeNode(int val = 0, TreeNode? left = null, TreeNode? right = null)
            {
                this.val = val;
                this.left = left;
                this.right = right;
            }

            public override string ToString()
            {
                return $"[{val}, [{left?.val}], [{right?.val}]";
            }

            public IEnumerator<TreeNode> GetEnumerator()
            {
                List<TreeNode> list = [];
                TraversePreOrder(this, list);

                return list.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                List<TreeNode> list = [];
                TraversePreOrder(this, list);

                return list.GetEnumerator();
            }

            private static void TraversePreOrder(TreeNode? node, List<TreeNode> list)
            {
                if (node == null)
                {
                    list.Add(null);
                }
                else
                {
                    list.Add(node);
                    TraversePreOrder(node.left, list);
                    TraversePreOrder(node.right, list);
                }
            }
        }

        // 104. Maximum Depth of Binary Tree
        // Given the root of a binary tree, return its maximum depth.
        // A binary tree's maximum depth is the number of nodes along the longest path from the root node down to the farthest leaf node.
        public static readonly IEnumerable<object[]> MaxDepthData =
            [
            [new TreeNode(3, new TreeNode(9), new TreeNode(20, new TreeNode(15), new TreeNode(7))), 3],
            [new TreeNode(1, null, new TreeNode(2)), 2]
            ];

        [Theory, MemberData(nameof(MaxDepthData))]
        public void MaxDepth(TreeNode root, int expected)
        {
            static int _MaxDepth(TreeNode node)
            {
                return node == null ? 0 : 1 + Math.Max(_MaxDepth(node?.left), _MaxDepth(node?.right));
            }

            int result = _MaxDepth(root);

            Assert.Equal(expected, result);
        }

        // 100. Same Tree
        // Given the roots of two binary trees p and q, write a function to check if they are the same or not.
        // Two binary trees are considered the same if they are structurally identical, and the nodes have the same value.
        public static readonly IEnumerable<object[]> IsSameTreeData =
            [
            [new TreeNode(1, new TreeNode(2), new TreeNode(3)), new TreeNode(1, new TreeNode(2), new TreeNode(3)), true],
            [new TreeNode(1, new TreeNode(2)), new TreeNode(1,null, new TreeNode(2)), false],
            [new TreeNode(1, new TreeNode(2), new TreeNode(1)), new TreeNode(1, new TreeNode(1), new TreeNode(2)), false],
            ];

        [Theory, MemberData(nameof(IsSameTreeData))]
        public void IsSameTree(TreeNode p, TreeNode q, bool expected)
        {
            static bool _IsSameTree(TreeNode p, TreeNode q)
            {
                return (p == null && q == null) || ((p?.val == q?.val) && _IsSameTree(p?.left, q?.left) && _IsSameTree(p?.right, q?.right));
            }

            bool result = _IsSameTree(p, q);

            Assert.Equal(expected, result);
        }

        // 226. Invert Binary Tree
        // Given the root of a binary tree, invert the tree, and return its root.
        public static readonly IEnumerable<object[]> InvertTreeData =
            [
            [new TreeNode(4, new TreeNode(2, new TreeNode(1), new TreeNode(3)),new TreeNode(7, new TreeNode(6), new TreeNode(9))), new TreeNode(4, new TreeNode(7, new TreeNode(9), new TreeNode(6)), new TreeNode(2, new TreeNode(3), new TreeNode(1)))],
            [new TreeNode(2, new TreeNode(1), new TreeNode(3)), new TreeNode(2, new TreeNode(3), new TreeNode(1))],
            [null, null],
            ];
        [Theory, MemberData(nameof(InvertTreeData))]
        public void InvertTree(TreeNode root, TreeNode expected)
        {
            static TreeNode _InvertTree(TreeNode node)
            {
                if (node == null) return null;

                TreeNode newRight = _InvertTree(node.left);
                TreeNode newLeft = _InvertTree(node.right);

                node.left = newLeft;
                node.right = newRight;

                return node;
            }

            TreeNode actual = _InvertTree(root);

            Assert.Equal(expected?.Select(n => n?.val), actual?.Select(n => n?.val));
        }


        // 101. Symmetric Tree
        // Given the root of a binary tree, check whether it is a mirror of itself(i.e., symmetric around its center).
        public static readonly IEnumerable<object[]> IsSymmetricData =
            [
            [new TreeNode(1, new TreeNode(2, new TreeNode(3), new TreeNode(4)), new TreeNode(2, new TreeNode(4), new TreeNode(3))), true],
            [new TreeNode(1, new TreeNode(2, null, new TreeNode(3)), new TreeNode(2, null, new TreeNode(3))), false],
            ];
        [Theory, MemberData(nameof(IsSymmetricData))]
        public void IsSymmetric(TreeNode root, bool expected)
        {
            static bool _IsMirror(TreeNode? left, TreeNode? right)
            {
                return (left == null && right == null)|| 
                    (left?.val == right?.val && _IsMirror(left?.left, right?.right) && _IsMirror(left?.right, right?.left));
            }

            bool result = root == null || _IsMirror(root?.left, root?.right);

            Assert.Equal(expected, result);
        }
    }
}
