
namespace leetcode.Types.Graph
{
    public class Node
    {
        public int val;
        public IList<Node> neighbors;

        public Node()
        {
            val = 0;
            neighbors = new List<Node>();
        }

        public Node(int _val)
        {
            val = _val;
            neighbors = new List<Node>();
        }

        public Node(int _val, List<Node> _neighbors)
        {
            val = _val;
            neighbors = _neighbors;
        }

        public override string ToString()
        {
            return $"{val} ({string.Join(",", neighbors.Select(n => n.val))})";
        }

        public static Node? CreateGraph(int[][] edges)
        {
            SortedDictionary<int, Node> nodes = new();

            for (int i = 0; i < edges.Length; i++)
            {
                nodes.Add(i + 1, new Node(i + 1));
            }

            for (int i = 0; i < edges.Length; i++)
            {
                foreach (int neighbor in edges[i])
                {
                    nodes[i + 1].neighbors.Add(nodes[neighbor]);
                }
            }

            return nodes.Values.FirstOrDefault();
        }

        public static int[][] ToEdgeArray(Node? node)
        {
            if (node == null) return null;

            SortedDictionary<int, Node> nodeMap = [];
            Queue<Node> queue = new([node]);
            List<int[]> result = [];

            while (queue.TryDequeue(out Node? queueNode))
            {
                _ = nodeMap.TryAdd(queueNode.val, queueNode);

                foreach (Node neighbor in queueNode.neighbors)
                {
                    if (nodeMap.TryAdd(neighbor.val, neighbor))
                    {
                        queue.Enqueue(neighbor);
                    }
                }
            }

            foreach (Node mapNode in nodeMap.Values)
            {
                result.Add(mapNode.neighbors.Select(n => n.val).ToArray());
            }

            return result.ToArray();
        }
    }
}
