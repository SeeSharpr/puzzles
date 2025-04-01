using leetcode.Lists.Top150;
using Microsoft.VisualBasic;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using static leetcode.Lists.Trie.Medium;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                        if (!it.children.TryGetValue(c, out var child)) it.children.Add(c, child = new());
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

            /// <summary>
            /// 208. Implement Trie (Prefix Tree)
            /// A trie(pronounced as "try") or prefix tree is a tree data structure used to efficiently store and retrieve keys in a dataset of strings.There are various applications of this data structure, such as autocomplete and spellchecker.
            /// Implement the Trie class:
            /// Trie() Initializes the trie object.
            /// void insert(String word) Inserts the string word into the trie.
            /// boolean search(String word) Returns true if the string word is in the trie(i.e., was inserted before), and false otherwise.
            /// boolean startsWith(String prefix) Returns true if there is a previously inserted string word that has the prefix prefix, and false otherwise.
            /// </summary>
            /// <see cref="https://leetcode.com/problems/implement-trie-prefix-tree/description/?envType=study-plan-v2&envId=top-interview-150">
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

            public class WordDictionary
            {
                private bool isTerminal = false;
                private readonly Dictionary<char, WordDictionary> children = [];

                public WordDictionary()
                {
                }

                public void AddWord(string word)
                {
                    WordDictionary? it = this;
                    foreach (var c in word)
                    {
                        if (!it.children.TryGetValue(c, out var child)) it.children.Add(c, child = new());
                        it = child;
                    }

                    it.isTerminal = true;
                }

                public bool Search(string word)
                {
                    WordDictionary? it = this;
                    for (int i = 0; i < word.Length; i++)
                    {
                        char c = word[i];

                        if (c == '.')
                        {
                            string rest = word.Substring(i + 1);

                            return it.children.Any(child => child.Value.Search(rest));
                        }
                        else
                        {
                            if (!it.children.TryGetValue(c, out var child)) return false;
                            it = child;
                        }
                    }

                    return it.isTerminal;
                }
            }

            /// <summary>
            /// 211. Design Add and Search Words Data Structure
            /// Design a data structure that supports adding new words and finding if a string matches any previously added string.
            /// Implement the WordDictionary class:
            /// WordDictionary() Initializes the object.
            /// void addWord(word) Adds word to the data structure, it can be matched later.
            /// bool search(word) Returns true if there is any string in the data structure that matches word or false otherwise.word may contain dots '.' where dots can be matched with any letter.
            /// </summary>
            /// <see cref="https://leetcode.com/problems/design-add-and-search-words-data-structure/?envType=study-plan-v2&envId=top-interview-150"/>
            [Theory]
            [InlineData("[WordDictionary,addWord,addWord,addWord,search,search,search,search]", "[[],[bad],[dad],[mad],[pad],[bad],[.ad],[b..]]", "[null,null,null,null,false,true,true,true]")]
            public void WordDictionaryTest(string inputAction, string inputArgs, string output)
            {
                string[] actions = inputAction.Parse1DArray().ToArray();
                string[][] args = inputArgs.Parse2DArray().Select(x => x.ToArray()).ToArray();
                string[] expects = output.Parse1DArray().ToArray();

                Assert.Equal(expects.Length, actions.Length);
                Assert.Equal(expects.Length, args.Length);

                WordDictionary? wd = null;
                for (int i = 0; i < expects.Length; i++)
                {
                    switch (actions[i])
                    {
                        case "WordDictionary":
                            wd = new WordDictionary();
                            break;
                        case "addWord":
                            wd!.AddWord(args[i][0]);
                            break;
                        case "search":
                            Assert.Equal(bool.Parse(expects[i]), wd!.Search(args[i][0]));
                            break;
                        default:
                            throw new NotSupportedException(actions[i]);
                    }
                }
            }
        }
    }
}