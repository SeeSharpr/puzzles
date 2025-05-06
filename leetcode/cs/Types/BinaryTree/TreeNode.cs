namespace leetcode.Types.BinaryTree
{
    public class TreeNode(int val = 0, TreeNode? left = null, TreeNode? right = null)
    {
        public readonly int val = val;
        public TreeNode? left = left;
        public TreeNode? right = right;

        public override string ToString()
        {
            return $"{left?.val}<-({val})->{right?.val}";
        }

        public static void AssertEqual(TreeNode? x, TreeNode? y)
        {
            if (x == null && y == null) return;

            if (x?.val != y?.val)
            {
                throw new InvalidDataException($"val: {x?.val} != {y?.val}");
            }

            if (x?.left?.val != y?.left?.val)
            {
                throw new InvalidDataException($"val: {x?.left} != {y?.left}");
            }

            if (x?.right?.val != y?.right?.val)
            {
                throw new InvalidDataException($"val: {x?.right} != {y?.right}");
            }
        }

        public static TreeNode? Create(string input)
        {
            if (!int.TryParse(input, out int value)) return null;

            return new TreeNode(value);
        }

        public static void Update(TreeNode? node, TreeNode? left, TreeNode? right)
        {
            if (node == null) return;

            node.left = left;
            node.right = right;
        }
    }
}
