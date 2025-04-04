using System.Collections;

namespace leetcode.Types.LinkedList
{
    public class Node(int val, Node? next = null) : IEnumerable<Node>
    {
        private static int idGen = -1;

        public readonly int id = Interlocked.Increment(ref idGen);
        public int val = val;
        public Node? next = next;
        public Node? random = null;

        public IEnumerator<Node> GetEnumerator()
        {
            return new NodeEnumerator(this);
        }

        public override string ToString()
        {
            return ToString(false);
        }

        private string ToString(bool useId)
        {
            return $"({val}{(useId ? id.ToString() : "")})->{next?.val.ToString() ?? "null"}/{random?.val.ToString() ?? "null"}";
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new NodeEnumerator(this);
        }

        private class NodeEnumerator(Node head) : IEnumerator<Node>
        {
            private readonly Node head = head;
            Node? prev = null;

            public Node Current => prev!;

            object IEnumerator.Current => prev!;

            public void Dispose()
            {
                // Do nothing
            }

            public bool MoveNext()
            {
                prev = (prev == null) ? head : prev?.next;

                return prev?.next != null;
            }

            public void Reset()
            {
                prev = null;
            }
        }
    }
}
