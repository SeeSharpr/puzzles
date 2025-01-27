using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            Dictionary<char, int> letterBag = [];

            foreach (var letter in magazine)
            {
                if (!letterBag.TryGetValue(letter, out int value))
                {
                    value = 0;
                }

                letterBag[letter] = ++value;
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
                Dictionary<char, BitArray> letters = [];

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

            bool sMinusTEmpty = !isoMapS.Except(isoMapT).Any();
            bool tMinusSEmpty = !isoMapT.Except(isoMapS).Any();

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
            Dictionary<char, string> map = [];
            Dictionary<string, char> reverse = [];

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
            int[] chars = new int[26];
            bool result = s.Length == t.Length;

            if (result)
            {
                int limit = s.Length;
                for (int i = 0; i < limit; i++)
                {
                    chars[s[i] - 'a']++;
                }

                limit = t.Length;
                for (int i = 0; i < limit && result; i++)
                {
                    result = --chars[t[i] - 'a'] >= 0;
                }

                result = result && chars.Sum() == 0;
            }

            Assert.Equal(expected, result);
        }

        // Given an array of strings strs, group the anagrams together.You can return the answer in any order.
        [Theory]
        [InlineData("eat,tea,tan,ate,nat,bat", "bat,nat|tan,ate|eat|tea")]
        [InlineData("", "")]
        [InlineData("a", "a")]
        public void GroupAnagrams(string input, string output)
        {
            string[] strs = input.Split(',');
            IList<IList<string>> expected = output.Split(",").Select(i => i.Split("|").ToList() as IList<string>).ToList();

            Dictionary<string, IList<string>> map = [];

            foreach (string str in strs)
            {
                char[] keyChars = str.ToCharArray();
                Array.Sort(keyChars);
                string key = new(keyChars);

                if (!map.TryGetValue(key, out IList<string>? value))
                {
                    map.Add(key, value = []);
                }

                value.Add(str);
            }

            IList<IList<string>> result = [.. map.Values];

            List<string[]> sortedResult = result.Select(sl => { string[] sa = [.. sl]; Array.Sort(sa); return sa; }).ToList();
            List<string[]> sortedExpected = expected.Select(sl => { string[] sa = [.. sl]; Array.Sort(sa); return sa; }).ToList();

            while (sortedResult.Count > 0 && sortedExpected.Count > 0)
            {
                string[] oneResult = sortedResult[0];
                sortedResult.RemoveAt(0);

                for (int i = 0; i < sortedExpected.Count; i++)
                {
                    if (oneResult.SequenceEqual(sortedExpected[i]))
                    {
                        sortedExpected.RemoveAt(i);
                        break;
                    }
                }
            }

            Assert.Empty(sortedExpected);
            Assert.Empty(sortedExpected);
        }

        // Given an array of integers nums and an integer target, return indices of the two numbers such that they add up to target.
        // You may assume that each input would have exactly one solution, and you may not use the same element twice.
        // You can return the answer in any order.
        [Theory]
        [InlineData("2,7,11,15|9|0,1")]
        [InlineData("3,2,4|6|1,2")]
        [InlineData("3,3|6|0,1")]
        public void TwoSum(string input)
        {
            string[] inputs = input.Split('|');
            int[] nums = inputs[0].Split(',').Select(i => int.Parse(i)).ToArray();
            int target = int.Parse(inputs[1]);
            int[] expected = inputs[2].Split(',').Select(i => int.Parse(i)).ToArray();

            Dictionary<int, int> map = new();
            int[] result = [];

            for (int i = 0; i < nums.Length; i++)
            {
                if (map.TryGetValue(target - nums[i], out int j))
                {
                    result = [j, i];
                    break;
                }

                map[nums[i]] = i;
            }

            Array.Sort(result);
            Array.Sort(expected);
            Assert.True(expected.SequenceEqual(result));
        }

        // Write an algorithm to determine if a number n is happy.
        // A happy number is a number defined by the following process:
        // Starting with any positive integer, replace the number by the sum of the squares of its digits.
        // Repeat the process until the number equals 1 (where it will stay), or it loops endlessly in a cycle which does not include 1.
        // Those numbers for which this process ends in 1 are happy.
        // Return true if n is a happy number, and false if not.
        [Theory]
        [InlineData(19, true)]
        [InlineData(2, false)]
        public void IsHappy(int n, bool expected)
        {
            HashSet<int> visited = new();

            bool result = false;

            for (int candidate = n; !visited.Contains(candidate);)
            {
                visited.Add(candidate);

                int sum = 0;
                while (candidate > 0)
                {
                    sum += (candidate % 10) * (candidate % 10);
                    candidate /= 10;
                }

                if (sum == 1)
                {
                    result = true;
                    break;
                }
                else
                {
                    candidate = sum;
                }
            }

            Assert.Equal(expected, result);
        }
    }
}
