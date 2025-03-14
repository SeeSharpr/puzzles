using leetcode.Lists.Top150;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Generic;
using Xunit;
using System.Collections.Immutable;
using Xunit.Sdk;
using System;
using System.Collections;
using System.Xml.Linq;

/// <summary>
/// <see cref="https://leetcode.com/problem-list/bit-manipulation/"/>
/// </summary>
namespace leetcode.Lists
{
    [Trait("Difficulty", "Medium")]
    public class BitManipulation_Medium
    {
        /// <summary>
        /// 868. Binary Gap
        /// 
        /// Given a positive integer n, find and return the longest distance between any two adjacent 1's in the binary representation of n. If there are no two adjacent 1's, return 0.
        /// Two 1's are adjacent if there are only 0's separating them(possibly no 0's). The distance between two 1's is the absolute difference between their bit positions.For example, the two 1's in "1001" have a distance of 3.
        /// </summary>
        /// <see cref="https://leetcode.com/problems/binary-gap/description/?envType=problem-list-v2&envId=bit-manipulation"/>
        [Theory]
        [InlineData(22, 2)]
        [InlineData(8, 0)]
        [InlineData(5, 2)]
        [InlineData(6, 1)]
        public void BinaryGap(int n, int expected)
        {
            static int InternalPopCount(int n)
            {
                for (int count = 0; ; count++)
                {
                    if (n == 0) return count;

                    n &= (n - 1);
                }
            }

            static int InternalBinaryGap(int n)
            {
                string solution = "bitwise";

                switch (solution)
                {
                    case "shift":
                        int maxZeroes = -1;

                        while ((n & 1) != 1) n >>= 1;
                        for (int zeroes = -1; n > 0; n >>= 1)
                        {
                            if ((n & 0x1) == 1)
                            {
                                maxZeroes = Math.Max(maxZeroes, zeroes);
                                zeroes = 0;
                            }
                            else
                            {
                                zeroes++;
                            }
                        }

                        return maxZeroes == -1 ? 0 : maxZeroes + 1;

                    case "bitwise":
                        int maxGap = 0;
                        while (n > 0)
                        {
                            // Take the rightmost bit 1
                            int right = n & (-n);

                            // Zero that bit
                            n ^= right;

                            // If there are no bits left, bail out
                            if (n == 0) break;

                            // Take the rightmost bit 1
                            int left = n & (-n);

                            // Set all bits between those 2 to 1 and the rest to 0
                            int center = (left - 1) & (-right);

                            // Count how many 1s
                            maxGap = Math.Max(maxGap, InternalPopCount(center));
                        }

                        return maxGap;
                }

                throw new NotImplementedException();
            }

            int actual = InternalBinaryGap(n);

            Assert.Equal(expected, actual);
        }


        /// <summary>
        /// 1009. Complement of Base 10 Integer
        /// 
        /// The complement of an integer is the integer you get when you flip all the 0's to 1's and all the 1's to 0's in its binary representation.
        /// For example, The integer 5 is "101" in binary and its complement is "010" which is the integer 2.
        /// Given an integer n, return its complement.
        /// </summary>
        [Theory]
        [InlineData(5, 2)]
        [InlineData(7, 0)]
        [InlineData(10, 5)]
        [InlineData(0, 1)]
        public void BitwiseComplement(int n, int expected)
        {
            int mask = n;

            for (int lsb = n; lsb != 0; lsb &= (lsb - 1))
            {
                mask = lsb;
            }

            mask = (mask << 1) - 1;

            int actual = n == 0 ? 1 : n ^ mask;

            Assert.Equal(expected, actual);
        }

        /// <summary>
        /// 1018. Binary Prefix Divisible By 5
        /// You are given a binary array nums (0-indexed).
        /// We define xi as the number whose binary representation is the subarray nums[0..i] (from most-significant-bit to least-significant-bit).
        /// For example, if nums = [1, 0, 1], then x0 = 1, x1 = 2, and x2 = 5.
        /// eturn an array of booleans answer where answer[i] is true if xi is divisible by 5.
        /// </summary>
        /// <see cref="https://leetcode.com/problems/binary-prefix-divisible-by-5/description/?envType=problem-list-v2&envId=bit-manipulation"/>
        [Theory]
        [InlineData("[0,1,1]", "[true,false,false]")]
        [InlineData("[1,1,1]", "[false,false,false]")]
        [InlineData("[1,0,0,1,0,1,0,0,1,0,1,1,1,1,1,1,1,1,1,1,0,0,0,0,1,0,1,0,0,0,0,1,1,0,1,0,0,0,1]", "[false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,true,true,true,true,false]")]
        public void PrefixesDivBy5(string input, string output)
        {
            int[] nums = input.Parse1DArray(int.Parse).ToArray();
            IList<bool> expected = output.Parse1DArray(bool.Parse).ToList();

            static IList<bool> InternalPrefixesDivBy5(int[] nums)
            {
                List<bool> result = new();

                int cand = 0;
                foreach (int n in nums)
                {
                    cand <<= 1;
                    cand |= n;
                    cand %= 10;

                    result.Add(cand % 5 == 0);
                }

                return result;
            }

            IList<bool> actual = InternalPrefixesDivBy5(nums);

            Assert.Equal(expected, actual);
        }

        /// <summary>
        /// 1486. XOR Operation in an Array
        /// 
        /// You are given an integer n and an integer start.
        /// Define an array nums where nums[i] = start + 2 * i(0 - indexed) and n == nums.length.
        /// Return the bitwise XOR of all elements of nums.
        /// </summary>
        /// <see cref="https://leetcode.com/problems/xor-operation-in-an-array/description/?envType=problem-list-v2&envId=bit-manipulation"/>
        [Theory]
        [InlineData(5,0,8)]
        [InlineData(4,3,8)]
        public void XorOperation(int n, int start, int expected)
        {
            int actual = 0;

            for (int value = start, i = 0; i < n; i++, value += 2)
            {
                actual ^= value;
            }

            Assert.Equal(expected, actual);
        }

        /// <summary>
        /// 3314. Construct the Minimum Bitwise Array I
        /// 
        /// You are given an array nums consisting of n prime integers.
        /// You need to construct an array ans of length n, such that, for each index i, the bitwise OR of ans[i] and ans[i] + 1 is equal to nums[i], i.e. ans[i] OR (ans[i] + 1) == nums[i].
        /// Additionally, you must minimize each value of ans[i] in the resulting array.
        /// If it is not possible to find such a value for ans[i] that satisfies the condition, then set ans[i] = -1.
        /// </summary>
        /// <see cref="https://leetcode.com/problems/construct-the-minimum-bitwise-array-i/description/?envType=problem-list-v2&envId=bit-manipulation"/>
        [Theory]
        [InlineData("[2,3,5,7]", "[-1,1,4,3]")]
        [InlineData("[11,13,31]", "[9,12,15]")]
        [InlineData("[41,5,7,47,47,43]", "[40,4,3,39,39,41]")]
        public void MinBitwiseArray(string input, string output)
        {
            IList<int> nums = input.Parse1DArray(int.Parse).ToList();
            int[] expected = output.Parse1DArray(int.Parse).ToArray();

            static int[] InternalMinBitwiseArray(IList<int> nums)
            {
                string solution = "brute";
                List<int> ans = new();

                switch (solution)
                {
                    case "brute":
                        foreach (int num in nums)
                        {
                            bool found = false;
                            for (int i = 0; i < num; i++)
                            {
                                int value = i | (i + 1);
                                if (value == num)
                                {
                                    found = true;
                                    ans.Add(i);
                                    break;
                                }
                            }

                            if (!found)
                            {
                                ans.Add(-1);
                            }
                        }

                        break;

                    case "memo":
                        Dictionary<int, int> memo = new();
                        int[] sorted = nums.ToHashSet().ToArray();
                        Array.Sort(sorted);

                        for (int n = 0; n < sorted.Length; n++)
                        {
                            if (!memo.TryGetValue(sorted[n], out int i))
                            {
                                for (i = (n == 0 ? 0 : sorted[n - 1]); i < sorted[n]; i++)
                                {
                                    memo.TryAdd(i | (i + 1), i);
                                }
                            }
                        }

                        foreach (int num in nums)
                        {
                            ans.Add(memo.TryGetValue(num, out int i) ? i : -1);
                        }

                        break;
                }

                return ans.ToArray();
            }

            int[] actual = InternalMinBitwiseArray(nums);

            Assert.Equal(expected, actual);
        }
    }
}
