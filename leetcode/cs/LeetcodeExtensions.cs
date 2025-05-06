using System.Text;
using Xunit.Sdk;

namespace leetcode
{
    public static class LeetcodeExtensions
    {
        public interface INode<T>
        {
            T Data { get; set; }
            INode<T>? Left { get; set; }
            INode<T>? Right { get; set; }
        }

        public static IEnumerable<string> Parse1DArray(this string input)
        {
            return Parse1DArray(input, s => s);
        }

        public static IEnumerable<T> Parse1DArray<T>(this string input, Func<string, T> parse)
        {
            StringBuilder sb = new(Math.Min(256, input.Length));

            foreach (char c in input)
            {
                if (c == '[') continue;

                if (c == ',' || c == ']')
                {
                    if (sb.Length == 0) break;

                    yield return parse.Invoke(sb.ToString());
                    sb.Clear();

                    if (c == ']') break;
                }
                else
                {
                    sb.Append(c);
                }
            }
        }

        public static IEnumerable<IEnumerable<string>> Parse2DArray(this string input)
        {
            return Parse2DArray(input, s => s);
        }

        public static IEnumerable<IEnumerable<T>> Parse2DArray<T>(this string input, Func<string, T> parse)
        {
            for (int start = input.IndexOf('[', input.IndexOf('[') + 1); start > 0;)
            {
                int end = input.IndexOf(']', start + 1);
                int length = end - start + 1;

                yield return Parse1DArray(input.Substring(start, length), parse);

                start = input.IndexOf('[', end + 1);
            }
        }

        public static IEnumerable<IEnumerable<T>> ParseNestedArrayStringLC<T>(this string input, Func<string, T> parse, char elementSeparator = ',', char entrySeparator = '|')
        {
            return input
                .Replace("], [", $"{entrySeparator}")
                .Replace("],[", $"{entrySeparator}")
                .Replace("[", "")
                .Replace("]", "")
                .ParseNestedEnumerable(parse, elementSeparator, entrySeparator);
        }

        public static IEnumerable<T> ParseArrayStringLC<T>(this string input, Func<string, T> parse, char elementSeparator = ',')
        {
            return input
                .Replace("[", "")
                .Replace("]", "")
                .ParseEnumerable(parse, elementSeparator);
        }

        public static TNode? ParseLinkedListLC<TNode, TData>(this string input, Func<TData?, TNode> createNode, Action<TNode?, TNode?> setNext, Func<TNode?, TNode?> getNext, Func<string, TData> parse, char elementSeparator = ',')
        {
            TNode dummy = createNode(default);

            TNode? ptr = dummy;
            foreach (TData u in input.ParseArrayStringLC(parse, elementSeparator))
            {
                setNext(ptr, createNode(u));
                ptr = getNext(ptr);
            }

            TNode? result = getNext(dummy);
            setNext(dummy, default);

            return result;
        }

        public static IEnumerable<T> ParseEnumerable<T>(this string input, Func<string, T> parse, char elementSeparator = ',')
        {
            return input
                .Split(elementSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(parse);
        }

        public static IEnumerable<IEnumerable<T>> ParseNestedEnumerable<T>(this string input, Func<string, T> parse, char elementSeparator = ',', char entrySeparator = '|')
        {
            return input
                .Split(entrySeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(pair => pair.ParseEnumerable<T>(parse, elementSeparator));
        }

        public static string ToLCString<T>(this IEnumerable<T> input, char elementSeparator = ',')
        {
            return string.Join(elementSeparator, input);
        }

        public static string ToLCDoubleString<T>(this IEnumerable<IEnumerable<T>> input, char elementSeparator = ',', char entrySeparator = '|')
        {
            return string.Join(entrySeparator, input.Select(entry => entry.ToLCString(elementSeparator)));
        }

        public static TNode? ParseLCTree<TNode>(this string input, Func<string, TNode> nodeBuilder, Action<TNode, TNode, TNode> nodeUpdater, char elementSeparator = ',')
        {
            string[] tokens = input.Replace("[", "").Replace("]", "").Split(elementSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (tokens.Length == 0) return default;

            TNode root = nodeBuilder.Invoke(tokens[0]);
            Queue<TNode> queue = new([root]);

            for (int i = 1; i < tokens.Length;)
            {
                if (!queue.TryDequeue(out TNode? head)) throw new InvalidDataException($"Invalid input: {input}, failed at token {tokens[i]}");

                TNode? left = nodeBuilder.Invoke(i < tokens.Length ? tokens[i++] : "");
                TNode? right = nodeBuilder.Invoke(i < tokens.Length ? tokens[i++] : "");

                nodeUpdater.Invoke(head, left, right);

                if (left != null) queue.Enqueue(left);
                if (right != null) queue.Enqueue(right);
            }

            return root;
        }
    }

    [Trait("Category","Helpers")]
    public class LeetcodeExtensionsTests
    {
        private class TestNode(int val, TestNode? left = null, TestNode? right = null)
        {
            public enum TraversalOrder
            {
                PreOrder,
                InOrder,
                PostOrder,
            }

            public readonly int val = val;
            public TestNode? left = left;
            public TestNode? right = right;

            public override string ToString()
            {
                return $"{left?.val}<-({val})->{right?.val}";
            }

            public static TestNode? Create(string input)
            {
                if (!int.TryParse(input, out int value)) return null;

                return new TestNode(value);
            }

            public static void Update(TestNode? node, TestNode? left, TestNode? right)
            {
                if (node == null) return;

                node.left = left;
                node.right = right;
            }

            public static string Traverse(TestNode? node, TraversalOrder order)
            {
                if (node == null) return "";

                return
                    order == TraversalOrder.PreOrder ? node.val + Traverse(node.left, order) + Traverse(node.right, order) :
                    order == TraversalOrder.InOrder ? Traverse(node.left, order) + node.val + Traverse(node.right, order) :
                    order == TraversalOrder.PostOrder ? Traverse(node.left, order) + Traverse(node.right, order) + node.val :
                    throw new ArgumentOutOfRangeException(nameof(order), order.ToString());
            }
        }

        [Fact]
        public void ParseLCTree_EmptyString()
        {
            Assert.Null("".ParseLCTree(TestNode.Create, TestNode.Update));
        }

        [Theory]
        [InlineData("1")]
        [InlineData("1,null")]
        [InlineData("1,null, null")]
        public void ParseLCTree_SingleNode(string input)
        {
            TestNode? actual = input.ParseLCTree(TestNode.Create, TestNode.Update);

            Assert.NotNull(actual);
            Assert.Null(actual.left);
            Assert.Null(actual.right);
            Assert.Equal(1, actual.val);
        }

        [Theory]
        [InlineData("1,2")]
        [InlineData("1,2,null")]
        public void ParseLCTree_TwoNodes(string input)
        {
            TestNode? actual = input.ParseLCTree(TestNode.Create, TestNode.Update);

            Assert.NotNull(actual);
            Assert.Null(actual.right);
            Assert.Equal(1, actual.val);

            Assert.NotNull(actual.left);
            Assert.Null(actual.left.left);
            Assert.Null(actual.left.right);
            Assert.Equal(2, actual.left.val);
        }

        [Theory]
        [InlineData("1,2,3")]
        [InlineData("1,2,3,null")]
        [InlineData("1,2,3,null, null")]
        [InlineData("1,2,3,null, null, null")]
        [InlineData("1,2,3,null, null, null, null")]
        public void ParseLCTree_ThreeNodes(string input)
        {
            TestNode? actual = input.ParseLCTree(TestNode.Create, TestNode.Update);

            Assert.NotNull(actual);
            Assert.Equal(1, actual.val);

            Assert.NotNull(actual.left);
            Assert.Null(actual.left.left);
            Assert.Null(actual.left.right);
            Assert.Equal(2, actual.left.val);

            Assert.NotNull(actual.right);
            Assert.Null(actual.right.left);
            Assert.Null(actual.right.right);
            Assert.Equal(3, actual.right.val);
        }

        [Theory]
        [InlineData("[3,5,1,6,2,0,8,null,null,7,4]", "356274108", "657243018", "674250813")]
        public void ParseLCTree_RandomTrees(string input, string preOrder, string inOrder, string postOrder)
        {
            TestNode? actual = input.ParseLCTree(TestNode.Create, TestNode.Update);

            Assert.Equal(preOrder, TestNode.Traverse(actual, TestNode.TraversalOrder.PreOrder));
            Assert.Equal(inOrder, TestNode.Traverse(actual, TestNode.TraversalOrder.InOrder));
            Assert.Equal(postOrder, TestNode.Traverse(actual, TestNode.TraversalOrder.PostOrder));
        }

        [Fact]
        public void Parse1DArray_int()
        {
            foreach (var pair in new Dictionary<string, IEnumerable<int>?>()
            {
                { "[]", []},
                { "[1]", [1]},
                { "[1,22]", [1,22]},
            })
            {
                var input = pair.Key;
                var expected = pair.Value;
                var actual = input.Parse1DArray(int.Parse);

                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        public void Parse2DArray_int()
        {
            foreach (var pair in new Dictionary<string, IEnumerable<IEnumerable<int>>>()
            {
                { "[]", [] },
                { "[[]]", [[]] },
                { "[[1]]", [[1]] },
                { "[[1,22],[1,22,333]]", [[1,22],[1,22,333]] },
            })
            {
                string input = pair.Key;
                var expected = pair.Value.ToArray();
                var actual = input.Parse2DArray(int.Parse).ToArray();

                Assert.Equal(expected?.Length, actual.Length);

                for (int i = 0; i < expected?.Length; i++)
                {
                    Assert.Equal(expected[i], actual[i]);
                }
            }
        }
    }
}
