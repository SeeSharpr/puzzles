namespace leetcode.Lists
{
    public class Trie
    {
        [Trait("Difficulty", "Medium")]
        public class Medium
        {
            public class Trie
            {
                private bool isTerminal = false;
                private readonly Dictionary<char, Trie> children = [];

                public void Insert(string word)
                {
                    Trie? it = this;
                    foreach (var c in word)
                    {
                        if (!it.children.TryGetValue(c, out var child)) it.children.Add(c, child = new Trie());
                        it = child;
                    }

                    it.isTerminal = true;
                }

                public bool Search(string word)
                {
                    Trie? it = this;
                    foreach (var c in word)
                    {
                        if (!it.children.TryGetValue(c, out var child)) return false;
                        it = child;
                    }

                    return it.isTerminal;
                }

                public bool StartsWith(string prefix)
                {
                    Trie? it = this;
                    foreach (var c in prefix)
                    {
                        if (!it.children.TryGetValue(c, out var child)) return false;
                        it = child;
                    }

                    return true;
                }

                public override string ToString()
                {
                    return string.Join("", children.Select(pair => $"{pair.Key}{(pair.Value.isTerminal ? '.' : '_')}"));
                }
            }

            [Theory]
            [InlineData("[Trie,insert,search,search,startsWith,insert,search]", "[[],[apple],[apple],[app],[app],[app],[app]]", "[null,null,true,false,true,null,true]")]
            public void TrieTest(string inputAction, string inputArgs, string output)
            {
                string[] actions = inputAction.Parse1DArray().ToArray();
                string[][] args = inputArgs.Parse2DArray().Select(e => e.ToArray()).ToArray();
                string[] expected = output.Parse1DArray().ToArray();

                Assert.Equal(expected.Length, actions.Length);
                Assert.Equal(expected.Length, args.Length);

                Trie? trie = null;
                for (int i = 0; i < expected.Length; i++)
                {
                    switch (actions[i])
                    {
                        case "Trie":
                            trie = new Trie();
                            break;
                        case "insert":
                            trie!.Insert(args[i][0]);
                            break;
                        case "search":
                            bool expSearch = bool.Parse(expected[i]);
                            bool actSearch = trie!.Search(args[i][0]);
                            Assert.Equal(expSearch, actSearch);
                            break;
                        case "startsWith":
                            bool expStartsWith = bool.Parse(expected[i]);
                            bool actStartsWith = trie!.StartsWith(args[i][0]);
                            break;
                        default:
                            throw new NotImplementedException(actions[i]);
                    }
                }
            }
        }
    }
}