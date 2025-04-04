using System.Collections;

namespace leetcode.Types.LinkedList
{
    public class ListNode : IEnumerable<ListNode>
    {
        private static int idGen = -1;
        public readonly int id = Interlocked.Increment(ref idGen);

        public int x;

        public int val;

        public ListNode? next;

        public ListNode() { }

        public ListNode(int x, ListNode? next = null)
        {
            this.x = x;
            val = x;
            this.next = next;
        }

        public void CreateLoop(int index)
        {
            if (index == -1) return;

            ListNode? ptr = this;
            while (index-- > 0)
            {
                ptr = ptr!.next;
            }

            ListNode? last = ptr!.next;
            while (last!.next != null)
            {
                last = last!.next;
            }

            last.next = ptr;
        }

        public override string ToString()
        {
            return ToString(false);
        }

        private string ToString(bool showId)
        {
            return $"({val}{(showId ? $": {id}" : "")})->{(next?.val.ToString() ?? "null")}";
        }

        public IEnumerator<ListNode> GetEnumerator()
        {
            return new NodeEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new NodeEnumerator(this);
        }

        public static ListNode? ParseFromLC(string input)
        {
            return input.ParseLinkedListLC(data => new ListNode(data), (node, next) => node!.next = next, node => node?.next, int.Parse);
        }

        private class NodeEnumerator(ListNode? head) : IEnumerator<ListNode>
        {
            private readonly int limit = 20;
            private int count = 0;
            private readonly ListNode? head = head;

            private ListNode? prev = null;

            public ListNode Current => prev ?? throw new InvalidOperationException();

            object IEnumerator.Current => prev ?? throw new InvalidOperationException();

            public void Dispose()
            {
                // Nothing to dispose
            }

            public bool MoveNext()
            {
                prev = (prev == null) ? head : prev?.next;
                count++;

                return prev != null && count < limit;
            }

            public void Reset()
            {
                prev = null;
                count = 0;
            }
        }
    }
}
