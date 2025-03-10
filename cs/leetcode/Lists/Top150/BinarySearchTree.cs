using leetcode.Types.BinaryTree;
using leetcode.Types.LinkedList;
using System;
using System.ComponentModel;

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
                if (values.Count == 0 || values[^1] != node?.val) values.Add(node!.val);
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
            for (Stack<TreeNode> stack = new(); ;)
            {
                for (; node != null; node = node.left) stack.Push(node);
                node = stack.Pop();
                if (--k == 0) break;
                node = node.right;
            }

            int actual = node.val;

            Assert.Equal(expected, actual);
        }

        // 98. Validate Binary Search Tree
        // Given the root of a binary tree, determine if it is a valid binary search tree(BST).
        // A valid BST is defined as follows:
        // The left subtree of a node contains only nodes with keys less than the node's key.
        // The right subtree of a node contains only nodes with keys greater than the node's key.
        // Both the left and right subtrees must also be binary search trees.
        [Trait("Difficulty", "Medium")]
        [Theory]
        [InlineData("[2,1,3]", true)]
        [InlineData("[5,1,4,null,null,3,6]", false)]
        [InlineData("[5,4,6,null,null,3,7]", false)]
        [InlineData("[2,2,2]", false)]
        public void IsValidBST(string input, bool expected)
        {
            TreeNode? root = input.ParseLCTree(TreeNode.Create, TreeNode.Update);

            static bool InternalIsValidBST(TreeNode? node, ref TreeNode? limit)
            {
                if (node == null) return true;

                if (!InternalIsValidBST(node.left, ref limit)) return false;
                if (limit != null && node.val <= limit.val) return false;
                limit = node;

                return InternalIsValidBST(node.right, ref limit);
            }

            TreeNode? limit = null;
            bool actual = InternalIsValidBST(root, ref limit);

            Assert.Equal(expected, actual);
        }
    }
}
