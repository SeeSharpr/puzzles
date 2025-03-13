using leetcode.Lists.Top150;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Generic;
using Xunit;
using System.Collections.Immutable;

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
        /// 3314. Construct the Minimum Bitwise Array I
        /// 
        /// You are given an array nums consisting of n prime integers.
        /// You need to construct an array ans of length n, such that, for each index i, the bitwise OR of ans[i] and ans[i] + 1 is equal to nums[i], i.e. ans[i] OR (ans[i] + 1) == nums[i].
        /// Additionally, you must minimize each value of ans[i] in the resulting array.
        /// If it is not possible to find such a value for ans[i] that satisfies the condition, then set ans[i] = -1.
        /// </summary>
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
