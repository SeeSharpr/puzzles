namespace leetcode.Types.BinaryTree
{
    public class Node(int val, Node? left = null, Node? right = null, Node? next = null)
    {
        public readonly int val = val;
        public Node? left = left;
        public Node? right = right;
        public Node? next = next;

        public override string ToString()
        {
            return $"{left?.val}<-({val})->{right?.val}=>{next?.val}";
        }

        public static void AssertEqual(Node? x, Node? y)
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

            if (x?.next?.val != y?.next?.val)
            {
                throw new InvalidDataException($"val: {x?.next} != {y?.next}");
            }
        }

        public static Node? Create(string input)
        {
            if (!int.TryParse(input, out int value)) return null;

            return new Node(value);
        }

        public static void Update(Node? node, Node? left, Node? right)
        {
            if (node == null) return;

            node.left = left;
            node.right = right;
        }

        public static Node? CloneExceptNext(Node? node)
        {
            return node == null ? null : new Node(node.val, CloneExceptNext(node?.left), CloneExceptNext(node?.right));
        }

        public static void MapNext(Node? root, int[][]? nextMap)
        {
            if (nextMap == null || nextMap.Length == 0) return;

            Dictionary<int, Node> map = [];
            BuildNodeMap(root, map);

            foreach (int[] pair in nextMap)
            {
                int src = pair[0];
                int dst = pair[1];

                map[src].next = map[dst];
            }
        }

        public static void VisitPreOrder(Node? node, Action<Node> visitor)
        {
            if (node == null) return;

            visitor(node);
            VisitPreOrder(node.left, visitor);
            VisitPreOrder(node.right, visitor);
        }

        public static void VisitInOrder(Node? node, Action<Node> visitor)
        {
            if (node == null) return;

            VisitInOrder(node.left, visitor);
            visitor(node);
            VisitInOrder(node.right, visitor);
        }

        public static void VisitPostOrder(Node? node, Action<Node> visitor)
        {
            if (node == null) return;

            VisitPostOrder(node.left, visitor);
            visitor(node);
            VisitPostOrder(node.right, visitor);
        }

        private static void BuildNodeMap(Node? node, Dictionary<int, Node> map)
        {
            if (node == null) return;

            map.Add(node.val, node);
            BuildNodeMap(node.left, map);
            BuildNodeMap(node.right, map);
        }
    }
}
