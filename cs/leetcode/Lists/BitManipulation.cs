using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

using System.Xml;
using System.Xml.Linq;
using System;

/// <summary>
/// <see cref="https://leetcode.com/problem-list/bit-manipulation/"/>
/// </summary>
namespace leetcode.Lists
{
    [Trait("Category", "BitManipulation")]
    public class BitManipulation
    {
        [Trait("Difficulty", "Easy")]
        public class Easy
        {
            /// <summary>
            /// 190. Reverse Bits
            /// Reverse bits of a given 32 bits unsigned integer.
            /// Note:
            /// Note that in some languages, such as Java, there is no unsigned integer type.In this case, both input and output will be given as a signed integer type. They should not affect your implementation, as the integer's internal binary representation is the same, whether it is signed or unsigned.
            /// In Java, the compiler represents the signed integers using 2's complement notation. Therefore, in Example 2 above, the input represents the signed integer -3 and the output represents the signed integer -1073741825.
            /// </summary>
            [Theory]
            [InlineData(0b00000010100101000001111010011100, 0b00111001011110000010100101000000)]
            [InlineData(0b11111111111111111111111111111101, 0b10111111111111111111111111111111)]
            public void reverseBits(uint n, uint expected)
            {
                uint actual = 0;

                string solution = "bitmask";

                switch (solution)
                {
                    case "shift":
                        for (int i = 0; i < 32; i++)
                        {
                            actual <<= 1;
                            actual |= (n & 0b1);
                            n >>= 1;
                        }
                        break;
                    case "bitmask":
                        actual = n;
                        actual = (actual << 16) | (actual >> 16);
                        actual = ((actual & 0x00ff00ff) << 8) | ((actual & 0xff00ff00) >> 8);
                        actual = ((actual & 0x0f0f0f0f) << 4) | ((actual & 0xf0f0f0f0) >> 4);
                        actual = ((actual & 0x33333333) << 2) | ((actual & 0xcccccccc) >> 2);
                        actual = ((actual & 0x55555555) << 1) | ((actual & 0xaaaaaaaa) >> 1);
                        break;
                    default:
                        throw new NotImplementedException(solution);
                }

                Assert.Equal(expected, actual);
            }

            /// <summary>
            /// 191. Number of 1 Bits
            /// 
            /// Given a positive integer n, write a function that returns the number of set bits in its binary representation (also known as the Hamming weight).
            /// </summary>
            /// <see cref="https://leetcode.com/problems/number-of-1-bits/description/?envType=problem-list-v2&envId=bit-manipulation"/>
            [Theory]
            [InlineData(11, 3)]
            [InlineData(128, 1)]
            [InlineData(2147483645, 30)]
            public void HammingWeight(int n, int expected)
            {
                int actual = 0;
                for (; n != 0; n &= (n - 1))
                {
                    actual++;
                }

                Assert.Equal(expected, actual);
            }

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
            [InlineData(5, 0, 8)]
            [InlineData(4, 3, 8)]
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
            /// 2595. Number of Even and Odd Bits
            /// You are given a positive integer n.
            /// Let even denote the number of even indices in the binary representation of n with value 1.
            /// Let odd denote the number of odd indices in the binary representation of n with value 1.
            /// Note that bits are indexed from right to left in the binary representation of a number.
            /// Return the array [even, odd].
            /// </summary>
            [Theory]
            [InlineData(50, "[1,2]")]
            [InlineData(2, "[0,1]")]
            public void EvenOddBit(int n, string output)
            {
                int[] expected = output.Parse1DArray(int.Parse).ToArray();

                int[] actual = new int[2];
                for (int j = 0; n != 0; n >>= 1, j = 1 - j)
                {
                    actual[j] += n & 1;
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

            /// <summary>
            /// 3370. Smallest Number With All Set Bits
            /// You are given a positive number n.
            /// Return the smallest number x greater than or equal to n, such that the binary representation of x contains only set bits
            /// </summary>
            [Theory]
            [InlineData(5, 7)]
            [InlineData(10, 15)]
            [InlineData(3, 3)]
            public void SmallestNumber(int n, int expected)
            {
                int msb = 0;
                for (; n > 0; n &= (n - 1))
                {
                    msb = (n & (-n));
                }

                int actual = (msb << 1) - 1;

                Assert.Equal(expected, actual);
            }
        }

        [Trait("Difficulty", "Medium")]
        public class Medium
        {
            /// <summary>
            /// 2429. Minimize XOR
            /// Given two positive integers num1 and num2, find the positive integer x such that:
            /// * x has the same number of set bits as num2, and
            /// * The value x XOR num1 is minimal.
            /// Note that XOR is the bitwise XOR operation.
            /// Return the integer x.The test cases are generated such that x is uniquely determined.
            /// The number of set bits of an integer is the number of 1's in its binary representation.
            /// </summary>
            /// <see cref="https://leetcode.com/problems/minimize-xor/description/?envType=problem-list-v2&envId=bit-manipulation"/>
            [Theory]
            [InlineData(3, 5, 3)]
            [InlineData(1, 12, 3)]
            public void MinimizeXor(int num1, int num2, int expected)
            {
                static int popcount(int n)
                {
                    int result = 0;

                    for (; n != 0; n &= n - 1)
                    {
                        result++;
                    }

                    return result;
                }

                int pop1 = popcount(num1);
                int pop2 = popcount(num2);
                int popx = pop2;

                // The lowest XOR (zero) will be with num1 itself
                int actual = num1;

                // While we have less bits to use, lets drop the lsb's first so that the msb's cancel out those from num1
                while (popx < pop1)
                {
                    actual &= (actual - 1);
                    popx++;
                }

                // While we have more bits to use, lets inject them on the lsb so that the XOR doesn't go beyond the msb's
                // To do so, we will need to add 1's where actual has 0's. Easiest way to get those 1's is to invert actual and borrow the 1's that corresponds to the 0's
                int actual_inv = ~actual;
                while (popx > pop1)
                {
                    actual |= (actual_inv & (-actual_inv)); // add inv's lsb to actual
                    actual_inv &= actual_inv - 1; // clear inv's lsb
                    popx--;
                }

                Assert.Equal(expected, actual);
            }

            /// <summary>
            /// 3011. Find if Array Can Be Sorted
            /// You are given a 0-indexed array of positive integers nums.
            /// In one operation, you can swap any two adjacent elements if they have the same number of set bits. You are allowed to do this operation any number of times (including zero).
            /// Return true if you can sort the array in ascending order, else return false.
            /// </summary>
            /// <see cref="https://leetcode.com/problems/find-if-array-can-be-sorted/?envType=problem-list-v2&envId=bit-manipulation"/>
            [Theory]
            [InlineData("[8,4,2,30,15]", true)]
            [InlineData("[1,2,3,4,5]", true)]
            [InlineData("[3,16,8,4,2]", false)]
            public void CanSortArray(string input, bool expected)
            {
                int[] nums = input.Parse1DArray(int.Parse).ToArray();

                static int popc(int n)
                {
                    int result = 0;

                    for (; n != 0; n &= n - 1) result++;

                    return result;
                }

                Dictionary<int, int> map = new();

                bool actual = true;
                for (int i = 1; actual && i < nums.Length; i++)
                {
                    // If numbers are in order, no need to do anything
                    if (nums[i - 1] <= nums[i]) continue;

                    // Okay, numbers are out of order, let's try to swap them until they are in order
                    if (!map.TryGetValue(nums[i], out int curr))
                    {
                        map[nums[i]] = curr = popc(nums[i]);
                    }

                    for (int j = i; j > 0 && nums[j - 1] > nums[j]; j--)
                    {
                        // Numbers are still out of order and they cannot be swapped, bail out with failure
                        if (!map.TryGetValue(nums[j - 1], out int prev))
                        {
                            map[nums[j - 1]] = prev = popc(nums[j - 1]);
                        }

                        if (prev != curr)
                        {
                            actual = false;
                            break;
                        }

                        // Swap
                        nums[j - 1] ^= nums[j];
                        nums[j] ^= nums[j - 1];
                        nums[j - 1] ^= nums[j];
                    }
                }

                Assert.Equal(expected, actual);
            }
        }
    }
}