using leetcode.Lists.Top150;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Generic;
using Xunit;

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
    }
}
