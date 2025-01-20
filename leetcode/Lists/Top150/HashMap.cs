using System.Collections;
using System.Diagnostics.CodeAnalysis;
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
    }
}
