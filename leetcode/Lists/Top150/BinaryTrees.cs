using leetcode.Types.BinaryTree;

namespace leetcode.Lists.Top150
{
    public class BinaryTrees
    {
        // 104. Maximum Depth of Binary Tree
        // Given the root of a binary tree, return its maximum depth.
        // A binary tree's maximum depth is the number of nodes along the longest path from the root node down to the farthest leaf node.
        [Trait("List", "TopInterview150")]
        [Theory]
        [InlineData("[3,9,20,null,null,15,7]", 3)]
        [InlineData("[1,null,2]", 2)]
        public void MaxDepth(string input, int expected)
        {
            TreeNode? root = input.ParseLCTree(TreeNode.Create, TreeNode.Update);

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
        [Trait("List", "TopInterview150")]
        [Theory]
        [InlineData("[1,2,3]", "[1,2,3]", true)]
        [InlineData("[1,2]", "[1,null,2]", false)]
        [InlineData("[1,2,1]", "[1,1,2]", false)]
        public void IsSameTree(string pStr, string qStr, bool expected)
        {
            TreeNode? p = pStr.ParseLCTree(TreeNode.Create, TreeNode.Update);
            TreeNode? q = qStr.ParseLCTree(TreeNode.Create, TreeNode.Update);

            static bool _IsSameTree(TreeNode? p, TreeNode? q)
            {
                return (p == null && q == null) || ((p?.val == q?.val) && _IsSameTree(p?.left, q?.left) && _IsSameTree(p?.right, q?.right));
            }

            bool result = _IsSameTree(p, q);

            Assert.Equal(expected, result);
        }

        // 226. Invert Binary Tree
        // Given the root of a binary tree, invert the tree, and return its root.
        [Trait("List", "TopInterview150")]
        [Theory]
        [InlineData("[4,2,7,1,3,6,9]", "[4,7,2,9,6,3,1]")]
        [InlineData("[2,1,3]", "[2,3,1]")]
        [InlineData("[]", "[]")]
        public void InvertTree(string input, string output)
        {
            TreeNode? root = input.ParseLCTree(TreeNode.Create, TreeNode.Update);
            TreeNode? expected = output.ParseLCTree(TreeNode.Create, TreeNode.Update);

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

            TreeNode.AssertEqual(expected, actual);
        }


        // 101. Symmetric Tree
        // Given the root of a binary tree, check whether it is a mirror of itself(i.e., symmetric around its center).
        [Trait("List", "TopInterview150")]
        [Theory]
        [InlineData("[1,2,2,3,4,4,3]", true)]
        [InlineData("[1,2,2,null,3,null,3]", false)]
        public void IsSymmetric(string input, bool expected)
        {
            TreeNode? root = input.ParseLCTree(TreeNode.Create, TreeNode.Update);

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
        [Trait("List", "TopInterview150")]
        [Theory]
        [InlineData("[3,9,20,15,7]", "[9,3,15,20,7]", "[3,9,20,null,null,15,7]")]
        [InlineData("[-1]", "[-1]", "[-1]")]
        public void BuildTree(string preorderInput, string inorderInput, string output)
        {
            TreeNode? expected = output.ParseLCTree(TreeNode.Create, TreeNode.Update);
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

            TreeNode.AssertEqual(expected, actual);
        }

        // 106. Construct Binary Tree from Inorder and Postorder Traversal
        // Given two integer arrays inorder and postorder where inorder is the inorder traversal of a binary tree and postorder is the postorder traversal of the same tree, construct and return the binary tree.
        [Trait("List", "TopInterview150")]
        [Theory]
        [InlineData("[9,3,15,20,7]", "[9,15,7,20,3]", "[3,9,20,null,null,15,7]")]
        [InlineData("[-1]", "[-1]", "[-1]")]
        public void BuildTree2(string inorderInput, string postorderInput, string output)
        {
            TreeNode? expected = output.ParseLCTree(TreeNode.Create, TreeNode.Update);
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

            TreeNode.AssertEqual(expected, actual);
        }

        // 117. Populating Next Right Pointers in Each Node II
        // Populate each next pointer to point to its next right node.If there is no next right node, the next pointer should be set to NULL.
        public static readonly IEnumerable<object[]> ConnectData =
            [
            [new Node(1, new Node(2, new Node(4), new Node(5)), new Node(3, right: new Node(7))), (int[][])[[2,3],[4,5],[5,7]]],
            [null, (int[][])[]],
            ];

        [Trait("List", "TopInterview150")]
        [Theory, MemberData(nameof(ConnectData))]
        public void Connect(Node? root, int[][]? expectedMap)
        {
            Node? expected = Node.CloneExceptNext(root);
            Node.MapNext(expected, expectedMap);

            Node? rootBFS = Node.CloneExceptNext(root);
            Node? rootRec = Node.CloneExceptNext(root);

            static void _ConnectBFS(Node? node)
            {
                if (node == null) return;

                Queue<Tuple<int, Node>> queue = new();
                queue.Enqueue(new Tuple<int, Node>(0, node!));

                while (queue.TryDequeue(out var currentTuple))
                {
                    int currentLevel = currentTuple.Item1;
                    Node currentNode = currentTuple.Item2;

                    // We only link nodes in the same level
                    if (queue.TryPeek(out var nextTuple) && currentLevel == nextTuple.Item1)
                    {
                        currentNode.next = nextTuple.Item2;
                    }

                    if (currentNode.left != null) queue.Enqueue(new Tuple<int, Node>(currentLevel + 1, currentNode?.left));
                    if (currentNode.right != null) queue.Enqueue(new Tuple<int, Node>(currentLevel + 1, currentNode?.right));
                }
            }

            static void _ConnectRec(Node? node)
            {
                if (node == null) return;

                // Connect the children
                if (node.left != null) node.left.next = node.right;

                _ConnectRec(node?.right);
                _ConnectRec(node?.left);

                // Connect the cousins
                Node? rightMost = node?.right ?? node?.left;
                if (rightMost == null) return;

                for (Node? sibling = node?.next; sibling != null; sibling = sibling?.next)
                {
                    if (sibling?.left != null)
                    {
                        rightMost.next = sibling.left;
                        rightMost = sibling.left;
                    }

                    if (sibling?.right != null)
                    {
                        rightMost.next = sibling.right;
                        rightMost = sibling.right;
                    }
                }
            }

            _ConnectBFS(rootBFS);
            _ConnectRec(rootRec);

            Node.AssertEqual(expected, rootBFS);
            Node.AssertEqual(expected, rootRec);
        }

        // 114. Flatten Binary Tree to Linked List
        // Given the root of a binary tree, flatten the tree into a "linked list":
        // The "linked list" should use the same TreeNode class where the right child pointer points to the next node in the list and the left child pointer is always null.
        // The "linked list" should be in the same order as a pre-order traversal of the binary tree.
        [Trait("List", "TopInterview150")]
        [Theory]
        [InlineData("[1,2,5,3,4,null,6]", "[1,null,2,null,3,null,4,null,5,null,6]")]
        [InlineData("[]", "[]")]
        [InlineData("[0]", "[0]")]
        public void Flatten(string input, string output)
        {
            TreeNode? root = input.ParseLCTree(TreeNode.Create, TreeNode.Update);
            TreeNode? expected = output.ParseLCTree(TreeNode.Create, TreeNode.Update);

            static void _Flatten(TreeNode? node)
            {
                if (node == null) return;

                if (node?.left != null) _Flatten(node?.left);
                if (node?.right != null) _Flatten(node?.right);

                // Find the last element in the list
                TreeNode? rightMost = node?.left;
                while (rightMost?.right != null) rightMost = rightMost.right;

                // Splice the list before the right subtree
                if (rightMost != null)
                {
                    rightMost.right = node?.right;
                    node.right = node?.left;
                }

                // Kill the left subtree
                node.left = null;
            }

            _Flatten(root);

            TreeNode.AssertEqual(expected, root);
        }

        // 112. Path Sum
        // Given the root of a binary tree and an integer targetSum, return true if the tree has a root-to-leaf path such that adding up all the values along the path equals targetSum.
        // A leaf is a node with no children.
        public static readonly IEnumerable<object[]> HasPathSumData =
            [
            [new TreeNode(5, new TreeNode(4, left: new TreeNode(11, new TreeNode(7), new TreeNode(2))), new TreeNode(8, new TreeNode(13), new TreeNode(4, right: new TreeNode(1)))), 22, true],
            [new TreeNode(1, new TreeNode(2), new TreeNode(3)), 5, false],
            [null, 0, false],
            [new TreeNode(1, new TreeNode(2), null), 1, false],
            ];

        [Trait("Difficulty", "Easy")]
        [Theory]
        [InlineData("[5,4,8,11,null,13,4,7,2,null,null,null,1]", 22, true)]
        [InlineData("[1,2,3]", 5, false)]
        [InlineData("[]", 0, false)]
        [InlineData("[1,2]", 1, false)]
        public void HasPathSum(string input, int targetSum, bool expected)
        {
            TreeNode? root = input.ParseLCTree(TreeNode.Create, TreeNode.Update);

            static bool InternalHasPathSum(TreeNode? node, int target, int current)
            {
                if (node == null)
                {
                    return false;
                }
                else
                {
                    current += node.val;

                    if (node?.left == null && node?.right == null)
                    {
                        return target == current;
                    }
                    else
                    {
                        return InternalHasPathSum(node.left, target, current) || InternalHasPathSum(node.right, target, current);
                    }
                }
            }

            bool actual = InternalHasPathSum(root, targetSum, 0);

            Assert.Equal(expected, actual);
        }

        // 129. Sum Root to Leaf Numbers
        // You are given the root of a binary tree containing digits from 0 to 9 only.
        // Each root-to-leaf path in the tree represents a number.
        // For example, the root-to-leaf path 1 -> 2 -> 3 represents the number 123.
        // Return the total sum of all root-to-leaf numbers.Test cases are generated so that the answer will fit in a 32-bit integer.
        // A leaf node is a node with no children.
        [Trait("Difficulty", "Medium")]
        [Theory]
        [InlineData("[1,2,3]", 25)]
        [InlineData("[4,9,0,5,1]", 1026)]
        public void SumNumbers(string input, int expected)
        {
            TreeNode? root = input.ParseLCTree(TreeNode.Create, TreeNode.Update);

            static int InternalSumNumbers(TreeNode? node, int current)
            {
                int next = current * 10 + node.val;

                if (node?.left == null && node?.right == null)
                {
                    return next;
                }
                else
                {
                    return (node?.left != null ? InternalSumNumbers(node.left, next) : 0) +
                        (node?.right != null ? InternalSumNumbers(node.right, next) : 0);
                }
            }

            int actual = root == null ? 0 : InternalSumNumbers(root, 0);

            Assert.Equal(expected, actual);
        }

        // 124. Binary Tree Maximum Path Sum
        // A path in a binary tree is a sequence of nodes where each pair of adjacent nodes in the sequence has an edge connecting them.A node can only appear in the sequence at most once.Note that the path does not need to pass through the root.
        // The path sum of a path is the sum of the node's values in the path.
        // Given the root of a binary tree, return the maximum path sum of any non-empty path.
        [Trait("Difficulty", "Hard")]
        [Theory]
        [InlineData("[1,2,3]", 6)]
        [InlineData("[-10,9,20,null,null,15,7]", 42)]
        [InlineData("[]", 0)]
        [InlineData("[2,-1]", 2)]
        [InlineData("[2,-1,-2]", 2)]
        [InlineData("[-3,-1]", -1)]
        public void MaxPathSum(string input, int expected)
        {
            TreeNode? root = input.ParseLCTree(TreeNode.Create, TreeNode.Update);

            static int InternalMaxPathSum(TreeNode? node, ref int maxSum)
            {
                if (node == null) return 0;

                int leftGain = Math.Max(InternalMaxPathSum(node.left, ref maxSum), 0);
                int rightGain = Math.Max(InternalMaxPathSum(node.right, ref maxSum), 0);
                int thisSum = node.val + leftGain + rightGain;

                maxSum = Math.Max(maxSum, thisSum);

                return node.val + Math.Max(leftGain, rightGain);
            }

            int maxSum = int.MinValue;
            int result = InternalMaxPathSum(root, ref maxSum);
            int actual = Math.Max(result, maxSum);

            Assert.Equal(expected, actual);
        }

        // 173. Binary Search Tree Iterator
        // Implement the BSTIterator class that represents an iterator over the in-order traversal of a binary search tree(BST) :
        // BSTIterator(TreeNode root) Initializes an object of the BSTIterator class. The root of the BST is given as part of the constructor.The pointer should be initialized to a non-existent number smaller than any element in the BST.
        // boolean hasNext() Returns true if there exists a number in the traversal to the right of the pointer, otherwise returns false.
        // int next() Moves the pointer to the right, then returns the number at the pointer.
        // Notice that by initializing the pointer to a non-existent smallest number, the first call to next() will return the smallest element in the BST.
        // You may assume that next() calls will always be valid. That is, there will be at least a next number in the in-order traversal when next() is called.
        public class BSTIterator
        {
            private readonly Stack<TreeNode> stack = new();
            private TreeNode? current;

            public BSTIterator(TreeNode root)
            {
                this.current = root;
            }

            public int Next()
            {
                while (current != null)
                {
                    stack.Push(current);
                    current = current.left;
                }

                int result = stack.Peek().val;

                current = stack.Pop().right;

                return result;
            }

            public bool HasNext()
            {
                return stack.Count > 0 || current != null;
            }
        }

        [Trait("Difficulty", "Medium")]
        [Theory]
        [InlineData("[7,3,15,null,null,9,20]", "next,next,hasNext,next,hasNext,next,hasNext,next,hasNext", "3,7,true,9,true,15,true,20,false")]
        public void BSTIteratorTest(string inputRoot, string inputOperations, string inputExpectations)
        {
            TreeNode? root = inputRoot.ParseLCTree(TreeNode.Create, TreeNode.Update);
            string[] operations = inputOperations.ParseEnumerable(x => x).ToArray();
            string[] expectations = inputExpectations.ParseEnumerable(x => x).ToArray();

            BSTIterator it = new(root);
            for (int i = 0; i < operations.Length; i++)
            {
                switch (operations[i])
                {
                    case "next":
                        int expNext = int.Parse(expectations[i]);
                        Assert.Equal(expNext, it.Next());
                        break;
                    case "hasNext":
                        bool expHasNext = bool.Parse(expectations[i]);
                        Assert.Equal(expHasNext, it.HasNext());
                        break;
                }
            }
        }

        // 222. Count Complete Tree Nodes
        // Given the root of a complete binary tree, return the number of the nodes in the tree.
        // According to Wikipedia, every level, except possibly the last, is completely filled in a complete binary tree, and all nodes in the last level are as far left as possible.It can have between 1 and 2h nodes inclusive at the last level h.
        // Design an algorithm that runs in less than O(n) time complexity.
        public static readonly IEnumerable<object[]> CountNodesData =
            [
            [new TreeNode(1, new TreeNode(2, new TreeNode(4), new TreeNode(5)), new TreeNode(3, left: new TreeNode(6))), 6],
            [new TreeNode(1), 1],
            [null, 0]
            ];

        [Trait("Difficulty", "Easy")]
        [Theory]
        [InlineData("[1,2,3,4,5,6]", 6)]
        [InlineData("[1]", 1)]
        [InlineData("[]", 0)]
        public void CountNodes(string input, int expected)
        {
            TreeNode? root = input.ParseLCTree(TreeNode.Create, TreeNode.Update);

            static int InternalCountNodes(TreeNode? node, int heightLeft, int heightRight)
            {
                if (node == null) return 0;

                if (heightLeft == 0) for (TreeNode? curr = node?.left; curr != null; curr = curr.left) heightLeft++;
                if (heightRight == 0) for (TreeNode? curr = node?.right; curr != null; curr = curr.right) heightRight++;

                if (heightLeft == heightRight) return (1 << (heightLeft + 1)) - 1;

                return 1 + InternalCountNodes(node?.left, heightLeft - 1, 0) + InternalCountNodes(node?.right, 0, heightRight - 1);
            }

            int actual = InternalCountNodes(root, 0, 0);

            Assert.Equal(expected, actual);
        }


        // 236. Lowest Common Ancestor of a Binary Tree
        // Given a binary tree, find the lowest common ancestor(LCA) of two given nodes in the tree.
        // According to the definition of LCA on Wikipedia: “The lowest common ancestor is defined between two nodes p and q as the lowest node in T that has both p and q as descendants (where we allow a node to be a descendant of itself).”
        [Trait("Difficulty", "Medium")]
        [Theory]
        [InlineData("[3,5,1,6,2,0,8,null,null,7,4]", 5, 1, 3)]
        [InlineData("[3,5,1,6,2,0,8,null,null,7,4]", 5, 4, 5)]
        [InlineData("[1,2]", 1, 2, 1)]
        public void LowestCommonAncestor(string input, int p, int q, int expected)
        {
            TreeNode? root = input.ParseLCTree(TreeNode.Create, TreeNode.Update);

            int actual = int.MinValue;

            Assert.Equal(expected, actual);
        }
    }
}
