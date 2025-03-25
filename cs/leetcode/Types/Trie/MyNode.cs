using System.Text;

namespace leetcode.Types.Trie
{
    public class MyNode
    {
        private bool isTerminal = false;

        private readonly Dictionary<char, MyNode> children = [];

        public bool TryAdd(string s)
        {
            MyNode node = this;

            bool wasAdded = false;
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];

                if (!node.children.TryGetValue(c, out MyNode? child))
                {
                    // Add a new letter and track that we added at least one letter
                    node.children[c] = child = new();
                    wasAdded = true;
                }
                else if (i == s.Length - 1 && child.isTerminal == false)
                {
                    // Even if the letter already existed, if it is the last and it wasn't marked as terminal, mark as such
                    wasAdded = true;
                }

                // Navigate down the trie
                node = child;
            }

            if (wasAdded)
            {
                node.isTerminal = true;
            }

            return wasAdded;
        }

        public bool Contains(string s)
        {
            return Contains(s, 0, s.Length);
        }
        public bool Contains(string s, int from, int to)
        {
            MyNode? node = this;
            for (int i = from; i < s.Length && i < to; i++)
            {
                char c = s[i];

                if (!node.children.TryGetValue(c, out node))
                {
                    return false;
                }
            }

            return node.isTerminal;
        }

        public void Traverse(Action<string> callback)
        {
            static void InternalTraverse(MyNode node, StringBuilder sb, Action<string> callback)
            {
                if (node.isTerminal)
                {
                    callback(sb.ToString());
                }

                foreach (var pair in node.children)
                {
                    sb.Append(pair.Key);
                    InternalTraverse(pair.Value, sb, callback);
                    sb.Remove(sb.Length - 1, 1);
                }
            }

            InternalTraverse(this, new StringBuilder(), callback);
        }

        public IEnumerable<int> Matches(string s, int from = 0)
        {
            MyNode? node = this;
            for (int to = from; to < s.Length && node.children.TryGetValue(s[to], out node); to++)
            {
                if (node.isTerminal)
                {
                    yield return to;
                }
            }
        }

        public override string ToString()
        {
            return $"[{string.Join(",", children.Select(pair => $"{pair.Key}:{(pair.Value.isTerminal ? "" : "_")}"))}]";
        }

        public static MyNode Make(IEnumerable<string> words)
        {
            MyNode node = new();

            foreach (var word in words)
            {
                if (!node.TryAdd(word))
                {
                    throw new ArgumentException($"Duplicate word '{word}'", nameof(words));
                }
            }

            return node;
        }
    }

    public class MyNodeTests
    {
        [Theory]
        [InlineData("")]
        [InlineData("a")]
        public void Contains_AlwaysFalseIfEmpty(string word)
        {
            Assert.False(new MyNode().Contains(word));
        }

        [Theory]
        [InlineData([new string[] { "a", "a" }])]
        [InlineData([new string[] { "a", "aa", "a" }])]
        public void TryAdd_FailsOnRepeated(string[] words)
        {
            MyNode node = new();

            bool result = true;
            foreach (string word in words)
            {
                result &= node.TryAdd(word);
            }

            Assert.False(result);
        }

        [Theory]
        [InlineData([new string[] { "aa", "a" }])]
        public void TryAdd_AddsSubstring(string[] words)
        {
            MyNode node = new();

            bool result = true;
            foreach (string word in words)
            {
                result &= node.TryAdd(word);
            }

            Assert.True(result);
        }

        [Theory]
        [InlineData([new string[] { "a", "aa", "b", "bb", "ab", "abc" }])]
        public void Traverse_ReturnsAllTerminals(string[] words)
        {
            MyNode node = MyNode.Make(words);

            SortedSet<string> traversed = new();
            node.Traverse(t => traversed.Add(t));

            Assert.Equal(new SortedSet<string>(words), traversed);
        }
        [Theory]
        [InlineData([new string[] { "a", "aa", "b", "bb", "ab", "abc", "abcd" }, "", 0])]
        [InlineData([new string[] { "a", "aa", "b", "bb", "ab", "abc", "abcd" }, "z", 0])]
        [InlineData([new string[] { "a", "aa", "b", "bb", "ab", "abc", "abcd" }, "zz", 0])]
        [InlineData([new string[] { "a", "aa", "b", "bb", "ab", "abc", "abcd" }, "zzz", 0])]
        [InlineData([new string[] { "a", "aa", "b", "bb", "ab", "abc", "abcd" }, "zzzz", 0])]
        [InlineData([new string[] { "a", "aa", "b", "bb", "ab", "abc", "abcd" }, "abczzzz", 3])]
        public void Matches_ReturnsEmptyIfNoMatches(string[] words, string input, int from)
        {
            MyNode node = MyNode.Make(words);

            Assert.Empty(node.Matches(input, from));
        }

        [Theory]
        [InlineData([new string[] { "a", "aa", "b", "bb", "ab", "abc", "abcd" }, "zzzabczzz", 3, new int[] { 3, 4, 5 }])]
        [InlineData([new string[] { "a", "aa", "b", "bb", "ab", "abc", "abcd" }, "zzzabcda", 3, new int[] { 3, 4, 5, 6 }])]
        [InlineData([new string[] { "a", "aa", "b", "bb", "ab", "abc", "abcd" }, "zzzabcd", 3, new int[] { 3, 4, 5, 6 }])]
        public void Matches_ReturnsMatches(string[] words, string input, int from, int[] expected)
        {
            MyNode node = MyNode.Make(words);

            Assert.Equal(expected, node.Matches(input, from));
        }
    }
}