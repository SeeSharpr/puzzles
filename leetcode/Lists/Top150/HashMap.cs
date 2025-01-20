using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Metrics;
using System.Reflection.Metadata;
using System.Text;

namespace leetcode.Lists.Top150
{
    public static class BitArrayExtensions
    {
        public static string AsBitString(this BitArray bits)
        {
            StringBuilder builder = new(bits.Length);

            for (int i = 0; i < bits.Length; i++)
            {
                builder.Append(bits[i] ? '1' : '0');
            }

            return builder.ToString();
        }
    }

    public class HashMap
    {
        // Given two strings ransomNote and magazine, return true if ransomNote can be constructed by using the letters from magazine and false otherwise.
        // Each letter in magazine can only be used once in ransomNote.
        [Theory]
        [InlineData("a", "b", false)]
        [InlineData("aa", "ab", false)]
        [InlineData("aa", "aab", true)]
        public void CanConstruct(string ransomNote, string magazine, bool expected)
        {
            Dictionary<char, int> letterBag = new();

            foreach (var letter in magazine)
            {
                if (letterBag.ContainsKey(letter))
                {
                    letterBag[letter]++;
                }
                else
                {
                    letterBag[letter] = 1;
                }
            }

            bool result = true;
            foreach (var letter in ransomNote)
            {
                if (letterBag.ContainsKey(letter))
                {
                    if (--letterBag[letter] == 0)
                    {
                        letterBag.Remove(letter);
                    }
                }
                else
                {
                    result = false;
                    break;
                }
            }

            Assert.Equal(expected, result);
        }

        // Given two strings s and t, determine if they are isomorphic.
        // Two strings s and t are isomorphic if the characters in s can be replaced to get t.
        // All occurrences of a character must be replaced with another character while preserving the order of characters.No two characters may map to the same character, but a character may map to itself.
        [Theory]
        [InlineData("egg", "add", true)]
        [InlineData("foo", "bar", false)]
        [InlineData("paper", "title", true)]
        [InlineData("bbbaaaba", "aaabbbba", false)]
        public void IsIsomorphic(string s, string t, bool expected)
        {
            static HashSet<string> ToIsoMap(string str)
            {
                Dictionary<char, BitArray> letters = new();

                for (int i = 0; i < str.Length; i++)
                {
                    if (!letters.TryGetValue(str[i], out BitArray? value))
                    {
                        letters[str[i]] = value = new BitArray(str.Length, false);
                    }

                    value.Set(i, true);
                }

                return letters.Values.Select(BitArrayExtensions.AsBitString).ToHashSet();
            }

            HashSet<string> isoMapS = ToIsoMap(s);
            HashSet<string> isoMapT = ToIsoMap(t);

            bool sMinusTEmpty = isoMapS.Except(isoMapT).Count() == 0;
            bool tMinusSEmpty = isoMapT.Except(isoMapS).Count() == 0;

            bool result = sMinusTEmpty && tMinusSEmpty;

            Assert.Equal(expected, result);
        }

        // Given a pattern and a string s, find if s follows the same pattern.
        // Here follow means a full match, such that there is a bijection between a letter in pattern and a non-empty word in s.Specifically:
        // Each letter in pattern maps to exactly one unique word in s.
        // Each unique word in s maps to exactly one letter in pattern.
        // No two letters map to the same word, and no two words map to the same letter.
        [Theory]
        [InlineData("abba", "dog cat cat dog", true)]
        [InlineData("abba", "dog cat cat fish", false)]
        [InlineData("aaaa", "dog cat cat dog", false)]
        [InlineData("abba", "dog dog dog dog", false)]
        public void WordPattern(string pattern, string s, bool expected)
        {
            Dictionary<char, string> map = new();
            Dictionary<string, char> reverse = new();

            string[] strings = s.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            bool result = pattern.Length == strings.Length;

            if (result)
            {
                for (int i = 0; i < pattern.Length; i++)
                {
                    char p = pattern[i];
                    string word = strings[i];

                    if (!map.TryGetValue(p, out string? match))
                    {
                        if (reverse.ContainsKey(word))
                        {
                            result = false;
                            break;
                        }

                        map[p] = word;
                        reverse[word] = p;

                    }
                    else if (!string.Equals(word, match))
                    {
                        result = false;
                        break;
                    }
                }
            }

            //bool result = true;
            //int pi = 0;
            //int si = 0;
            //while (si < s.Length && char.IsWhiteSpace(s[si])) si++;

            //while (true)
            //{
            //    char p = pattern[pi];

            //    int k = si;
            //    while (k < s.Length && char.IsLetter(s[k])) k++;
            //    string word = s.Substring(si, k - si);

            //    while (k < s.Length && char.IsWhiteSpace(s[k])) k++;
            //    si = k;

            //    if (!map.TryGetValue(p, out string? match))
            //    {
            //        if (reverse.ContainsKey(word))
            //        {
            //            result = false;
            //            break;
            //        }

            //        map[p] = word;
            //        reverse[word] = p;

            //    }
            //    else if (!string.Equals(word, match))
            //    {
            //        result = false;
            //        break;
            //    }

            //    pi++;

            //    if (pi == pattern.Length && si == s.Length)
            //    {
            //        break;
            //    }
            //    else if (pi == pattern.Length || si == s.Length)
            //    {
            //        result = false;
            //        break;
            //    }
            //}

            Assert.Equal(expected, result);
        }

        // Given two strings s and t, return true if t is an anagram of s, and false otherwise.
        [Theory]
        [InlineData("anagram", "nagaram", true)]
        [InlineData("rat", "car", false)]
        public void IsAnagram(string s, string t, bool expected)
        {
            int[] ss = new int[26];
            int[] ts = new int[26];
            bool result = s.Length == t.Length;

            if (result)
            {
                foreach (char c in s)
                {
                    ss[c - 'a']++;
                }

                foreach (char c in t)
                {
                    int i = c - 'a';

                    if (ss[i] == 0 || ss[i] <= ts[i])
                    {
                        result = false;
                        break;
                    }

                    ts[i]++;
                }

                result = result && ss.SequenceEqual(ts);
            }

            Assert.Equal(expected, result);
        }
    }
}
