using System.Collections;

namespace leetcode.Types.LinkedList
{
    public class Node(int val, Node? next = null)
    {
        private static int idGen = -1;

        public readonly int id = Interlocked.Increment(ref idGen);
        public int val = val;
        public Node? next = next;
        public Node? random = null;

        public override string ToString()
        {
            return ToString(false);
        }

        private string ToString(bool useId)
        {
            return $"({val}{(useId ? id.ToString() : "")})->{next?.val.ToString() ?? "null"}/{random?.val.ToString() ?? "null"}";
        }
    }
}
