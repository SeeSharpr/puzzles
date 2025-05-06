using System.Collections;

namespace basics;

public class LinkedLists
{
    private class SingleNode<T>(T value, SingleNode<T>? next = null) : IEnumerable<SingleNode<T>>
    {
        private static int idGen = 0;

        private readonly int id = Interlocked.Increment(ref idGen);

        public T Value { get; set; } = value;

        public SingleNode<T>? Next { get; set; } = next;

        public SingleNode<T>? Clone()
        {
            SingleNode<T>? result = null;
            for (SingleNode<T>? pred = null, curr = this; curr != null; curr = curr.Next)
            {
                if (result == null)
                {
                    pred = result = new SingleNode<T>(curr.Value);
                }
                else
                {
                    pred.Next = new SingleNode<T>(curr.Value);
                    pred = pred.Next;
                }
            }

            return result;
        }

        #region object overrides
        public override string ToString()
        {
            return $"(id:{id}, Value:{Value}, Next:{Next?.id ?? 0})";
        }

        public override bool Equals(object? obj)
        {
            return obj is SingleNode<T> other && other != null && id == other.id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(id);
        }
        #endregion

        #region IEnumerable<SingleNode<T>>
        public IEnumerator<SingleNode<T>> GetEnumerator()
        {
            return new NodeEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new NodeEnumerator<T>(this);
        }

        private class NodeEnumerator<T>(SingleNode<T>? head) : IEnumerator<SingleNode<T>>
        {
            private readonly SingleNode<T>? head = head;

            private SingleNode<T>? current = null;

            SingleNode<T> IEnumerator<SingleNode<T>>.Current => current;

            object IEnumerator.Current => current;

            public void Dispose()
            {
                // Nothing to dispose
            }

            bool IEnumerator.MoveNext()
            {
                current = (current == null) ? head : current.Next;

                return current != null;
            }

            void IEnumerator.Reset()
            {
                current = null;
            }
        }
        #endregion
    }

    [Fact]
    public void SingleNodeEnumerator()
    {
        Assert.Single(new SingleNode<int>(1));
        Assert.Equal(2, new SingleNode<int>(1, new SingleNode<int>(2)).Count());
    }

    [Fact]
    public void SingleNodeEqualsAndGetHashCode()
    {
        SingleNode<int>? src = new(1, new(2));
        SingleNode<int>? dst = new(1, new(2));

        Assert.True(src.Equals(dst));
        Assert.Equal(src.GetHashCode(), dst.GetHashCode());
    }

    [Fact]
    public void SingleNodeClone()
    {
        SingleNode<int>? src = new(1, new(2));
        SingleNode<int>? dst = src.Clone();

        Assert.Equal(src.Select(s => s.Value).ToArray(), dst.Select(d => d.Value).ToArray());
    }
}
