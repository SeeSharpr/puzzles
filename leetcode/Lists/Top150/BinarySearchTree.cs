using leetcode.Types.BinaryTree;
using leetcode.Types.LinkedList;
using System;

namespace leetcode.Lists.Top150
{
    public class BinarySearchTree
    {

        // 530. Minimum Absolute Difference in BST
        // Given the root of a Binary Search Tree (BST), return the minimum absolute difference between the values of any two different nodes in the tree.
        [Trait("Difficulty", "Easy")]
        [Theory]
        [InlineData("[4,2,6,1,3]", 1)]
        [InlineData("[1,0,48,null,null,12,49]", 1)]
        public void GetMinimumDifference(string input, int expected)
        {
            TreeNode? root = input.ParseLCTree(TreeNode.Create, TreeNode.Update);

            static void InternalTraverseTree(TreeNode? node, List<int> values)
            {
                if (node == null) return;

                InternalTraverseTree(node?.left, values);
                if (values.Count == 0 || values[values.Count - 1] != node.val) values.Add(node.val);
                InternalTraverseTree(node?.right, values);
            }

            List<int> list = [];
            InternalTraverseTree(root, list);

            int actual = list.Count < 1 ? 0 : int.MaxValue;

            if (list.Count > 1)
            {
                for (int i = 1; i < list.Count; i++)
                {
                    actual = Math.Min(actual, Math.Abs(list[i] - list[i - 1]));
                }
            }

            Assert.Equal(expected, actual);
        }

        // 230. Kth Smallest Element in a BST
        // Given the root of a binary search tree, and an integer k, return the kth smallest value(1-indexed) of all the values of the nodes in the tree.
        [Trait("Difficulty", "Medium")]
        [Theory]
        [InlineData("[3,1,4,null,2]", 1, 1)]
        [InlineData("[5,3,6,2,4,null,null,1]", 3, 3)]
        public void KthSmallest(string input, int k, int expected)
        {
            TreeNode? root = input.ParseLCTree(TreeNode.Create, TreeNode.Update);

            //static void InternalKthSmallest(TreeNode? node, int k, ref int count, ref int kth)
            //{
            //    if (node == null) return;

            //    if (count < k) InternalKthSmallest(node?.left, k, ref count, ref kth);

            //    if (count < k && ++count == k)
            //    {
            //        kth = node.val;
            //        return;
            //    }

            //    if (count < k) InternalKthSmallest(node?.right, k, ref count, ref kth);
            //}

            //int count = 0;
            //int actual = int.MinValue;
            //InternalKthSmallest(root, k, ref count, ref actual);

            TreeNode? node = root;
            for (Stack<TreeNode> stack = node == null ? new() : new([node]); ;)
            {
                for (; node != null; node = node.left) stack.Push(node);
                node = stack.Pop();
                if (--k == 0) break;
                node = node.right;
            }

            int actual = node.val;
            
            Assert.Equal(expected, actual);
        }
    }
}
