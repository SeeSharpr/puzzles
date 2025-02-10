using System.Collections;
using System.Diagnostics.Contracts;

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
                List<TreeNode?> list = [];
                TraversePreOrder(this, list);

                return list.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                List<TreeNode?> list = [];
                TraversePreOrder(this, list);

                return list.GetEnumerator();
            }

            private static void TraversePreOrder(TreeNode? node, List<TreeNode?> list)
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
            static int _MaxDepth(TreeNode? node)
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
            static bool _IsSameTree(TreeNode? p, TreeNode? q)
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
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            [null, null],
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            ];
        [Theory, MemberData(nameof(InvertTreeData))]
        public void InvertTree(TreeNode root, TreeNode expected)
        {
            static TreeNode? _InvertTree(TreeNode? node)
            {
                if (node == null) return null;

                TreeNode? newRight = _InvertTree(node.left);
                TreeNode? newLeft = _InvertTree(node.right);

                node.left = newLeft;
                node.right = newRight;

                return node;
            }

            TreeNode? actual = _InvertTree(root);

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
            static bool _IsMirrorRecursive(TreeNode? left, TreeNode? right)
            {
                return (left == null && right == null) ||
                    (left?.val == right?.val && _IsMirrorRecursive(left?.left, right?.right) && _IsMirrorRecursive(left?.right, right?.left));
            }

            static bool _IsMirrorInteractive(TreeNode? left, TreeNode? right)
            {
                Queue<TreeNode?> leftQueue = new([left]);
                Queue<TreeNode?> rightQueue = new([right]);

                while (leftQueue.TryDequeue(out TreeNode? leftHead) && rightQueue.TryDequeue(out TreeNode? rightHead))
                {
                    if (leftHead == null && rightHead == null) continue;

                    if (leftHead?.val != rightHead?.val) return false;

                    leftQueue.Enqueue(leftHead?.left);
                    rightQueue.Enqueue(rightHead?.right);
                    rightQueue.Enqueue(leftHead?.right);
                    leftQueue.Enqueue(rightHead?.left);
                }

                if (leftQueue.Count > 0 || rightQueue.Count > 0) return false;

                return true;
            }

            bool recursiveResult = root == null || _IsMirrorRecursive(root?.left, root?.right);
            bool interactiveResult = root == null || _IsMirrorInteractive(root?.left, root?.right);

            Assert.Equal(expected, recursiveResult);
            Assert.Equal(expected, interactiveResult);
        }

        // 105. Construct Binary Tree from Preorder and Inorder Traversal
        // Given two integer arrays preorder and inorder where preorder is the preorder traversal of a binary tree and inorder is the inorder traversal of the same tree, construct and return the binary tree.
        public static readonly IEnumerable<object[]> BuildTreeData =
            [
            ["[3,9,20,15,7]", "[9,3,15,20,7]", new TreeNode(3,new TreeNode(9), new TreeNode(20, new TreeNode(15), new TreeNode(7)))],
            ["[-1]","[-1]", new TreeNode(-1)],
            ];

        [Theory, MemberData(nameof(BuildTreeData))]

        public void BuildTree(string preorderInput, string inorderInput, TreeNode expected)
        {
            int[] preorder = preorderInput.ParseArrayStringLC(int.Parse).ToArray();
            int[] inorder = inorderInput.ParseArrayStringLC(int.Parse).ToArray();

            static TreeNode? _BuildTree(int[] po, int[] io, int ipo, int lio, int rio)
            {
                if (lio == rio) return null;

                // Find the pivot in the in-order array
                int indexNode = -1;
                for (int i = lio; i < rio; i++)
                {
                    if (io[i] == po[ipo])
                    {
                        indexNode = i;
                        break;
                    }
                }

                int nodesToLeft = indexNode - lio;

                TreeNode? leftNode = _BuildTree(po, io, ipo + 1, lio, indexNode);
                TreeNode? rightNode = _BuildTree(po, io, ipo + 1 + nodesToLeft, indexNode + 1, rio);

                return new TreeNode(po[ipo], leftNode, rightNode);
            }

            TreeNode? actual = _BuildTree(preorder, inorder, 0, 0, preorder.Length);

            Assert.Equal(expected?.Select(n => n?.val), actual?.Select(n => n?.val));
        }

        // 106. Construct Binary Tree from Inorder and Postorder Traversal
        // Given two integer arrays inorder and postorder where inorder is the inorder traversal of a binary tree and postorder is the postorder traversal of the same tree, construct and return the binary tree.
        public static readonly IEnumerable<object[]> BuildTree2Data =
            [
            ["[9,3,15,20,7]", "[9,15,7,20,3]", new TreeNode(3,new TreeNode(9), new TreeNode(20, new TreeNode(15), new TreeNode(7)))],
            ["[-1]","[-1]", new TreeNode(-1)],
            ];

        [Theory, MemberData(nameof(BuildTree2Data))]
        public void BuildTree2(string inorderInput, string postorderInput, TreeNode expected)
        {
            int[] inorder = inorderInput.ParseArrayStringLC(int.Parse).ToArray();
            int[] postorder = postorderInput.ParseArrayStringLC(int.Parse).ToArray();

            static TreeNode? _BuildTree(int[] po, int[] io, int ipo, int lio, int rio)
            {
                if (lio == rio) return null;

                // Find the pivot in the in-order array
                int indexNode = -1;
                for (int i = lio; i < rio; i++)
                {
                    if (io[i] == po[ipo])
                    {
                        indexNode = i;
                        break;
                    }
                }

                int nodesToRight = rio - indexNode - 1;

                TreeNode? leftNode = _BuildTree(po, io, ipo - nodesToRight - 1, lio, indexNode);
                TreeNode? rightNode = _BuildTree(po, io, ipo - 1, indexNode + 1, rio);

                return new TreeNode(po[ipo], leftNode, rightNode);
            }

            TreeNode? actual = _BuildTree(postorder, inorder, postorder.Length - 1, 0, postorder.Length);

            Assert.Equal(expected?.Select(n => n?.val), actual?.Select(n => n?.val));
        }
    }
}
