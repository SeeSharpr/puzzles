﻿using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Runtime.InteropServices;
using System.Text;
using static System.Formats.Asn1.AsnWriter;
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
        [Trait("Difficulty", "Medium")]
        public class Medium
        {
            /// <summary>
            /// 49. Group Anagrams
            /// Given an array of strings strs, group the anagrams together.You can return the answer in any order.
            /// </summary>
            /// <see cref="https://leetcode.com/problems/group-anagrams/"/>
            [Trait("List", "TopInterview150")]
            [Theory]
            [InlineData("eat,tea,tan,ate,nat,bat", "bat|nat,tan|ate,eat,tea")]
            [InlineData("", "")]
            [InlineData("a", "a")]
            public void GroupAnagrams(string input, string output)
            {
                string[] strs = input.ParseEnumerable(s => s).ToArray();
                IList<IList<string>> expected = output.ParseNestedEnumerable(s => s).Select(e => e.ToList() as IList<string>).ToList();

                static IList<IList<string>> DictionaryOfSorted(string[] strs)
                {
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

                    return map.Values.ToList();
                }

                static IList<IList<string>> DictionaryOfCounts(string[] strs)
                {
                    Dictionary<string, IList<string>> map = [];

                    foreach (string str in strs)
                    {
                        string key = string.Join(':', str.Aggregate(new int[26], (a, c) => { a[char.ToLower(c) - 'a']++; return a; }));

                        if (!map.TryGetValue(key, out IList<string>? value))
                        {
                            map.Add(key, value = []);
                        }

                        value.Add(str);
                    }

                    return map.Values.ToList();
                }

                foreach (Func<string[], IList<IList<string>>> solution in new[] { DictionaryOfSorted, DictionaryOfCounts, })
                {
                    var actual = solution.Invoke(strs);

                    List<string[]> sortedResult = actual.Select(sl => { string[] sa = [.. sl]; Array.Sort(sa); return sa; }).ToList();
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
            }

            /// <summary>
            /// 1152. Analyze User Website Visit Pattern
            /// You are given two string arrays username and website and an integer array timestamp.All the given arrays are of the same length and the tuple[username[i], website[i], timestamp[i]] indicates that the user username[i] visited the website website[i] at time timestamp[i].
            /// A pattern is a list of three websites (not necessarily distinct).
            /// For example, ["home", "away", "love"], ["leetcode", "love", "leetcode"], and["luffy", "luffy", "luffy"] are all patterns.
            /// The score of a pattern is the number of users that visited all the websites in the pattern in the same order they appeared in the pattern.
            /// For example, if the pattern is ["home", "away", "love"], the score is the number of users x such that x visited "home" then visited "away" and visited "love" after that.
            /// Similarly, if the pattern is ["leetcode", "love", "leetcode"], the score is the number of users x such that x visited "leetcode" then visited "love" and visited "leetcode" one more time after that.
            /// Also, if the pattern is ["luffy", "luffy", "luffy"], the score is the number of users x such that x visited "luffy" three different times at different timestamps.
            /// Return the pattern with the largest score.If there is more than one pattern with the same largest score, return the lexicographically smallest such pattern.
            /// Note that the websites in a pattern do not need to be visited contiguously, they only need to be visited in the order they appeared in the pattern.
            /// </summary>
            /// <see cref="https://leetcode.com/problems/analyze-user-website-visit-pattern"/>
            [Theory]
            [InlineData("[joe,joe,joe,james,james,james,james,mary,mary,mary]", "[1,2,3,4,5,6,7,8,9,10]", "[home,about,career,home,cart,maps,home,home,about,career]", "[home,about,career]")]
            [InlineData("[ua,ua,ua,ub,ub,ub]", "[1,2,3,4,5,6]", "[a,b,a,a,b,c]", "[a,b,a]")]
            [InlineData("[zkiikgv,zkiikgv,zkiikgv,zkiikgv]", "[436363475,710406388,386655081,797150921]", "[wnaaxbfhxp,mryxsjc,oz,wlarkzzqht]", "[oz,mryxsjc,wlarkzzqht]")]
            public void MostVisitedPattern(string inputUsername, string inputTimestamp, string inputWebsite, string output)
            {
                string[] username = inputUsername.Parse1DArray().ToArray();
                int[] timestamp = inputTimestamp.Parse1DArray(int.Parse).ToArray();
                string[] website = inputWebsite.Parse1DArray().ToArray();
                IList<string> expected = output.Parse1DArray().ToList();
                // -

                static IList<string> WithDictionary(string[] username, int[] timestamp, string[] website)
                {
                    // First, build the tuples
                    Dictionary<string, SortedList<int, string>> tuples = [];
                    for (int i = 0; i < username.Length; i++)
                    {
                        if (!tuples.TryGetValue(username[i], out var list))
                        {
                            tuples[username[i]] = list = [];
                        }

                        list.Add(timestamp[i], website[i]);
                    }

                    // Next, build the patterns for each user
                    string? maxPattern = null;
                    int maxCount = int.MinValue;
                    Dictionary<string, HashSet<string>> patterns = new();
                    foreach (var tuple in tuples)
                    {
                        string user = tuple.Key;
                        IList<string> sites = tuple.Value.Values;

                        for (int i = 0; i < sites.Count - 2; i++)
                        {
                            for (int j = i + 1; j < sites.Count - 1; j++)
                            {
                                for (int k = j + 1; k < sites.Count; k++)
                                {
                                    string pattern = $"{sites[i]};{sites[j]};{sites[k]}";

                                    if (!patterns.TryGetValue(pattern, out var users))
                                    {
                                        patterns[pattern] = users = [];
                                    }

                                    users.Add(user);

                                    if (users.Count > maxCount || (users.Count == maxCount && (maxPattern?.CompareTo(pattern) >= 0)))
                                    {
                                        maxPattern = pattern;
                                        maxCount = users.Count;
                                    }
                                }
                            }
                        }
                    }

                    return maxPattern?.Split(';') ?? [];
                }

                IList<string> actual = WithDictionary(username, timestamp, website);

                Assert.Equal(expected, actual);
            }
        }

        [Trait("Difficulty", "Hard")]
        public class Hard
        {
            /// <summary>
            /// 273. Integer to English Words
            /// Convert a non-negative integer num to its English words representation.
            /// </summary>
            /// <see cref="https://leetcode.com/problems/integer-to-english-words"/>
            private static readonly Dictionary<int, string> _names = new()
            {
                {1, "One" },{2,"Two"},{3,"Three"},{4,"Four"}, {5, "Five"}, {6,"Six"},{7,"Seven"},{8,"Eight"},{9,"Nine"},
                {10,"Ten"},{11,"Eleven"},{12,"Twelve"},{13,"Thirteen"},{14,"Fourteen"},{15,"Fifteen"},{16,"Sixteen"},{17,"Seventeen"},{18,"Eighteen"},{19,"Nineteen"},
                {20,"Twenty"},{30,"Thirty"},{40,"Forty"},{50,"Fifty"},{60,"Sixty"},{70,"Seventy"},{80,"Eighty"},{90,"Ninety"}, {100, "Hundred"},
            };

            private static readonly Dictionary<int, string> _magnitude = new()
            {
                {1, "Thousand"},
                {2, "Million"},
                {3, "Billion"},
                {4, "Trillion"},
                {5, "Quadrillion"},
            };

            [Theory]
            [InlineData(123, "One Hundred Twenty Three")]
            [InlineData(12345, "Twelve Thousand Three Hundred Forty Five")]
            [InlineData(1234567, "One Million Two Hundred Thirty Four Thousand Five Hundred Sixty Seven")]
            [InlineData(1000001, "One Million One")]
            public void NumberToWords(int num, string expected)
            {
                static List<string> Recursive(int num, int magnitude)
                {
                    if (magnitude == 0 && num == 0) return ["Zero"];

                    if (num == 0) return [];

                    List<string> result = Recursive(num / 1000, magnitude + 1);

                    num %= 1000;
                    magnitude = num > 0 ? magnitude : 0;

                    if (num >= 100)
                    {
                        result.Add(_names[num / 100]);
                        result.Add(_names[100]);
                    }

                    num %= 100;

                    if (num >= 20 || num == 10)
                    {
                        result.Add(_names[(num / 10) * 10]);
                    }
                    else if (num > 10)
                    {
                        result.Add(_names[num]);
                        num = 0;
                    }

                    num %= 10;

                    if (num > 0)
                    {
                        result.Add(_names[num]);
                    }

                    if (_magnitude.TryGetValue(magnitude, out string magStr)) result.Add(magStr);

                    return result;
                }

                string actual = string.Join(" ", Recursive(num, 0));

                Assert.Equal(expected, actual);
            }
        }

        // Given two strings ransomNote and magazine, return true if ransomNote can be constructed by using the letters from magazine and false otherwise.
        // Each letter in magazine can only be used once in ransomNote.
        [Trait("List", "TopInterview150")]
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
        [Trait("List", "TopInterview150")]
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
        [Trait("List", "TopInterview150")]
        [Theory]
        [InlineData("abba", "dog cat cat dog", true)]
        [InlineData("abba", "dog cat cat fish", false)]
        [InlineData("aaaa", "dog cat cat dog", false)]
        [InlineData("abba", "dog dog dog dog", false)]
        public void WordPattern(string pattern, string s, bool expected)
        {
            Dictionary<char, string> map = [];
            Dictionary<string, char> reverse = [];

            string[] strings = s.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

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
        [Trait("List", "TopInterview150")]
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

        // Given an array of integers nums and an integer target, return indices of the two numbers such that they add up to target.
        // You may assume that each input would have exactly one solution, and you may not use the same element twice.
        // You can return the answer in any order.
        [Trait("List", "TopInterview150")]
        [Theory]
        [InlineData("2,7,11,15", 9, "0,1")]
        [InlineData("3,2,4", 6, "1,2")]
        [InlineData("3,3", 6, "0,1")]
        public void TwoSum(string inputArray, int target, string output)
        {
            int[] nums = inputArray.ParseEnumerable(int.Parse).ToArray();
            int[] expected = output.ParseEnumerable(int.Parse).ToArray();

            Dictionary<int, int> map = [];
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
        [Trait("List", "TopInterview150")]
        [Theory]
        [InlineData(19, true)]
        [InlineData(2, false)]
        public void IsHappy(int n, bool expected)
        {
            HashSet<int> visited = [];

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

        // Given an integer array nums and an integer k, return true if there are two distinct indices i and j in the array such that nums[i] == nums[j] and abs(i - j) <= k.
        [Trait("List", "TopInterview150")]
        [Theory]
        [InlineData("1, 2, 3, 1", 3, true)]
        [InlineData("1, 0, 1, 1", 1, true)]
        [InlineData("1, 2, 3, 1, 2, 3", 2, false)]
        public void ContainsNearbyDuplicate(string input, int k, bool expected)
        {
            int[] nums = input.ParseEnumerable(int.Parse).ToArray();

            Dictionary<int, int> firstIndexOf = [];

            bool result = false;

            for (int i = 0; i < nums.Length; i++)
            {
                if (firstIndexOf.TryGetValue(nums[i], out int j) && nums[j] == nums[i])
                {
                    if (Math.Abs(i - j) <= k)
                    {
                        result = true;
                        break;
                    }
                    else
                    {
                        firstIndexOf[nums[i]] = i;
                    }
                }
                else
                {
                    firstIndexOf.Add(nums[i], i);
                }
            }

            Assert.Equal(expected, result);
        }

        // Given an unsorted array of integers nums, return the length of the longest consecutive elements sequence.
        // You must write an algorithm that runs in O(n) time.
        [Trait("List", "TopInterview150")]
        [Theory]
        [InlineData("100, 4, 200, 1, 3, 2", 4)]
        [InlineData("0,3,7,2,5,8,4,6,0,1", 9)]
        public void LongestConsecutive(string input, int expected)
        {
            int[] nums = input.ParseEnumerable(int.Parse).ToArray();

            HashSet<int> visited = [];

            for (int i = 0; i < nums.Length; i++)
            {
                visited.Add(nums[i]);
            }

            int result = 0;

            foreach (int i in visited)
            {
                // Ignore the successor since we can count backwards from the successor
                if (visited.Contains(i + 1)) continue;

                // Whatever number we end in it's the maximum of a given sequence
                int count = 1;
                int cand = i;
                while (visited.Contains(cand - 1))
                {
                    count++;
                    cand--;
                }

                if (count > result)
                {
                    result = count;
                }
            }

            Assert.Equal(expected, result);
        }
    }
}
