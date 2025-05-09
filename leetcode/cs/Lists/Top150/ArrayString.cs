﻿using System.Diagnostics.Metrics;
using System.Text;

namespace leetcode.Lists.Top150
{
    public class ArrayString
    {
        public abstract class Reader4(string input) : IDisposable
        {
            StringReader? reader = new(input);

            protected int Read4(char[] buf4)
            {
                if (buf4.Length != 4) throw new ArgumentException(buf4?.Length.ToString() ?? "null", nameof(buf4));

                return reader?.ReadBlock(buf4) ?? 0;
            }

            public abstract int Read(char[] buf, int n);

            public void Dispose()
            {
                reader?.Dispose();
                reader = null;
            }
        }

        [Trait("Difficulty", "Easy")]
        public class Easy
        {
            /// <summary>
            /// 67. Add Binary
            /// Given two binary strings a and b, return their sum as a binary string.
            /// </summary>
            /// <see cref="https://leetcode.com/problems/add-binary/description/"/>
            [Theory]
            [InlineData("11", "1", "100")]
            [InlineData("1010", "1011", "10101")]
            [InlineData("0", "0", "0")]
            [InlineData("10100000100100110110010000010101111011011001101110111111111101000000101111001110001111100001101", "110101001011101110001111100110001010100001101011101010000011011011001011101111001100000011011110011", "110111101100010011000101110110100000011101000101011001000011011000001100011110011010010011000000000")]
            public void AddBinary(string a, string b, string expected)
            {
                int n = Math.Max(a.Length, b.Length);
                char[] r = new char[n + 1];
                int cc = 0;

                for (int i = n, ia = a.Length - 1, ib = b.Length - 1; i > 0; i--, ia--, ib--)
                {
                    int ca = ia >= 0 ? a[ia] - '0' : 0;
                    int cb = ib >= 0 ? b[ib] - '0' : 0;

                    r[i] = (char)('0' + ((cc + ca + cb) % 2));
                    cc = (cc + ca + cb) / 2;
                }

                r[0] = (char)('0' + cc);

                char[] digits = r.Skip(1 - cc).ToArray();
                string actual = new(digits);

                Assert.Equal(expected, actual);
            }

            /// <summary>
            /// 157. Read N Characters Given Read4
            /// Given a file and assume that you can only read the file using a given method read4, implement a method to read n characters.
            /// </summary>
            /// <see cref="https://leetcode.com/problems/read-n-characters-given-read4/description/"/>
            public class LC157(string input) : Reader4(input)
            {
                public override int Read(char[] buf, int n)
                {
                    char[] read4 = new char[4];

                    int actual = 0;
                    for (int bufIndex = 0; actual < n; bufIndex += 4)
                    {
                        int read = Read4(read4);
                        if (read == 0) break;

                        int bufReq = Math.Min(read, n - actual);
                        Array.Copy(read4, 0, buf, bufIndex, bufReq);
                        actual += bufReq;
                    }

                    return actual;
                }
            }

            [Theory]
            [InlineData("abc", 4, 3)]
            [InlineData("abcde", 5, 5)]
            [InlineData("abcdABCD1234", 12, 12)]
            [InlineData("leetcode", 5, 5)]
            public void Read(string input, int n, int expected)
            {
                char[] buf = new char[expected];

                using LC157 reader = new(input);
                int actual = reader.Read(buf, n);

                Assert.Equal(expected, actual);
                Assert.Equal(input.ToCharArray().Take(n), buf);
            }

            /// <summary>
            /// 283. Move Zeroes
            /// Given an integer array nums, move all 0's to the end of it while maintaining the relative order of the non-zero elements.
            /// Note that you must do this in-place without making a copy of the array.
            /// </summary>
            /// <see cref="https://leetcode.com/problems/move-zeroes/description/"/>
            [Theory]
            [InlineData("[0,1,0,3,12]", "[1,3,12,0,0]")]
            [InlineData("[0]", "[0]")]
            [InlineData("[1]", "[1]")]
            public void MoveZeroes(string input, string output)
            {
                int[] nums = input.Parse1DArray(int.Parse).ToArray();
                int[] expected = output.Parse1DArray(int.Parse).ToArray();
                //--
                static void MoveNumbersToCorrectPosition(int[] nums)
                {
                    if (nums.Length < 2) return;

                    int count = 0;
                    for (int i = 0; i < nums.Length; i++)
                    {
                        if (count != i && nums[i] != 0)
                        {
                            nums[count++] = nums[i];
                        }
                    }

                    for (int i = count; i < nums.Length; i++)
                    {
                        nums[i] = 0;
                    }
                }

                static void BubbleZeroes(int[] nums)
                {
                    for (int left = 0, right = 0; right < nums.Length; right++)
                    {
                        if (nums[right] != 0)
                        {
                            if (left != right)
                            {
                                nums[left] ^= nums[right];
                                nums[right] ^= nums[left];
                                nums[left] ^= nums[right];
                            }

                            left++;
                        }
                    }
                }

                string solution = nameof(BubbleZeroes);
                switch (solution)
                {
                    case nameof(MoveNumbersToCorrectPosition):
                        MoveNumbersToCorrectPosition(nums);
                        break;

                    case nameof(BubbleZeroes):
                        BubbleZeroes(nums);
                        break;

                    default:
                        throw new NotSupportedException(solution);
                }
                //--
                Assert.Equal(expected, nums);
            }

            /// <summary>
            /// 2357. Make Array Zero by Subtracting Equal Amounts
            /// You are given a non-negative integer array nums.In one operation, you must:
            /// Choose a positive integer x such that x is less than or equal to the smallest non-zero element in nums.
            /// Subtract x from every positive element in nums.
            /// Return the minimum number of operations to make every element in nums equal to 0.
            /// </summary>
            /// <see cref="https://leetcode.com/problems/make-array-zero-by-subtracting-equal-amounts"/>
            [Theory]
            [InlineData("[1,5,0,3,5]", 3)]
            [InlineData("[0]", 0)]
            public void MinimumOperations(string input, int expected)
            {
                int[] nums = input.Parse1DArray(int.Parse).ToArray();
                //-
                static int InternalSet(int[] nums)
                {
                    if (nums == null || nums.Length < 1 || nums[^1] == 0) return 0;

                    HashSet<int> seen = [0];
                    int count = 0;
                    for (int i = 0; i < nums.Length; i++)
                    {
                        if (seen.Add(nums[i])) count++;
                    }

                    return count;
                }

                static int InternalSort(int[] nums)
                {
                    Array.Sort(nums);
                    int count = 1;
                    for (int i = 1; i < nums.Length; i++)
                    {
                        if (nums[i - 1] == 0 || nums[i - 1] == nums[i]) continue;
                        count++;
                    }

                    return count;
                }

                string solution = nameof(InternalSet);
                int actual =
                    solution == nameof(InternalSet) ? InternalSet(nums) :
                    solution == nameof(InternalSort) ? InternalSort(nums) :
                    throw new NotSupportedException(solution);

                Assert.Equal(expected, actual);
            }
        }

        [Trait("Difficulty", "Medium")]
        public class Medium
        {
            /// <summary>
            /// 161. One Edit Distance
            /// Given two strings s and t, return true if they are both one edit distance apart, otherwise return false.
            /// A string s is said to be one distance apart from a string t if you can:
            /// Insert exactly one character into s to get t.
            /// Delete exactly one character from s to get t.
            /// Replace exactly one character of s with a different character to get t.
            /// </summary>
            /// <see cref="https://leetcode.com/problems/one-edit-distance/description/"/>
            [Theory]
            [InlineData("ab", "acb", true)]
            [InlineData("", "", false)]
            [InlineData("a", "A", true)]
            [InlineData("cb", "ab", true)]
            [InlineData("a", "", true)]
            public void IsOneEditDistance(string s, string t, bool expected)
            {
                static bool InternalIsOneEditDistance(string s, string t)
                {
                    if (t.Length - s.Length > 1) return false;

                    int diff = t.Length - s.Length;
                    int si = 0;
                    for (int ti = 0; diff < 2 && si < s.Length && ti < t.Length; si++, ti++)
                    {
                        if (s[si] == t[ti]) continue;

                        if (s.Length == t.Length)
                        {
                            // Only allowed diff is replacement
                            diff++;
                        }
                        else
                        {
                            // Only allowed diff is skipping
                            si--;
                        }

                        if (diff > 1) return false;
                    }

                    return diff == 1 && si == s.Length;
                }

                bool actual = s.Length < t.Length ? InternalIsOneEditDistance(s, t) : InternalIsOneEditDistance(t, s);

                Assert.Equal(expected, actual);
            }

            /// <summary>
            /// 2214. Minimum Health to Beat Game
            /// You are playing a game that has n levels numbered from 0 to n - 1. You are given a 0-indexed integer array damage where damage[i] is the amount of health you will lose to complete the ith level.
            /// You are also given an integer armor. You may use your armor ability at most once during the game on any level which will protect you from at most armor damage.
            /// You must complete the levels in order and your health must be greater than 0 at all times to beat the game.
            /// Return the minimum health you need to start with to beat the game.
            /// </summary>
            /// <see cref="https://leetcode.com/problems/minimum-health-to-beat-game"/>
            [Theory]
            [InlineData("[2,7,4,3]", 4, 13)]
            [InlineData("[2,5,3,4]", 7, 10)]
            [InlineData("[3,3,3]", 0, 10)]
            public void MinimumHealth(string input, int armor, long expected)
            {
                int[] damage = input.Parse1DArray(int.Parse).ToArray();
                //-
                static long InternalMinimumHealth(int[] damage, int armor)
                {
                    int maxDamage = int.MinValue;
                    long health = 1;

                    foreach (int d in damage)
                    {
                        health += d;
                        maxDamage = Math.Max(maxDamage, d);
                    }

                    health -= maxDamage;
                    health += Math.Max(maxDamage - armor, 0);

                    return health;
                }
                //-
                long actual = InternalMinimumHealth(damage, armor);

                Assert.Equal(expected, actual);
            }
        }

        [Trait("Difficulty", "Hard")]
        public class Hard
        {
            /// <summary>
            /// 42. Trapping Rain Water
            /// Given n non-negative integers representing an elevation map where the width of each bar is 1, compute how much water it can trap after raining.
            /// </summary>
            /// <see cref="https://leetcode.com/problems/trapping-rain-water/"/>
            [Trait("List", "TopInterview150")]
            [Theory]
            [InlineData(new[] { 0, 1, 0, 2, 1, 0, 1, 3, 2, 1, 2, 1 }, 6)]
            [InlineData(new[] { 4, 2, 0, 3, 2, 5 }, 9)]
            public void Trap(int[] height, int expected)
            {
                static int BruteForce(int[] height)
                {
                    int trappedWater = 0;

                    for (int i = 0; i < height.Length; i++)
                    {
                        int maxLeft = i > 0 ? height.Take(i).Max() : 0;
                        int maxRight = i + 1 < height.Length ? height.Skip(i + 1).Max() : 0;

                        trappedWater += Math.Max(0, Math.Min(maxLeft, maxRight) - height[i]);
                    }

                    return trappedWater;
                }

                static int DP(int[] height)
                {
                    int n = height.Length;

                    // Left to right, find what is the maximum height of the left border
                    int[] leftMax = new int[n];
                    leftMax[0] = height[0];
                    for (int i = 1; i < n; i++)
                    {
                        leftMax[i] = Math.Max(leftMax[i - 1], height[i]);
                    }

                    // Right to left, find what is the maximum height of the right border
                    int[] rightMax = new int[n];
                    rightMax[n - 1] = height[n - 1];
                    for (int i = n - 2; i >= 0; i--)
                    {
                        rightMax[i] = Math.Max(rightMax[i + 1], height[i]);
                    }

                    int trappedWater = 0;
                    for (int i = 0; i < n; i++)
                    {
                        trappedWater += Math.Min(leftMax[i], rightMax[i]) - height[i];
                    }

                    return trappedWater;
                }

                static int TwoPointers(int[] height)
                {
                    if (height.Length < 1) return 0;

                    int left = 0;
                    int right = height.Length - 1;
                    int water = 0, maxLeft = 0, maxRight = 0;
                    while (left < right)
                    {
                        if (height[left] < height[right])
                        {
                            maxLeft = Math.Max(maxLeft, height[left]);
                            water += Math.Min(maxLeft, height[right]) - height[left];
                            left++;
                        }
                        else
                        {
                            maxRight = Math.Max(maxRight, height[right]);
                            water += Math.Min(height[left], maxRight) - height[right];
                            right--;
                        }
                    }

                    return water;
                }

                foreach (Func<int[], int> solution in new[] { BruteForce, DP, TwoPointers, })
                {
                    int actual = solution.Invoke(height);

                    Assert.Equal(expected, actual);
                }
            }

            /// <summary>
            /// 158. Read N Characters Given read4 II - Call Multiple Times
            /// Given a file and assume that you can only read the file using a given method read4, implement a method read to read n characters.Your method read may be called multiple times.
            /// </summary>
            /// <see cref="https://leetcode.com/problems/read-n-characters-given-read4-ii-call-multiple-times/description/"/>
            public class LC158(string input) : Reader4(input)
            {
                private char[] read4 = new char[4];
                private int available = 0;
                private int read4Idx = 0;

                public override int Read(char[] buf, int n)
                {
                    int read = 0;
                    int bufIndex = 0;

                    while (n > 0)
                    {
                        if (available == 0)
                        {
                            available = Read4(read4);
                            read4Idx = 0;
                        }

                        if (available == 0) break;

                        while (n > 0 && available > 0)
                        {
                            buf[bufIndex++] = read4[read4Idx++];
                            read++;
                            available--;
                            n--;
                        }
                    }

                    return read;
                }
            }

            /// <summary>
            /// 767. Reorganize String
            /// Given a string s, rearrange the characters of s so that any two adjacent characters are not the same.
            /// Return any possible rearrangement of s or return "" if not possible.
            /// </summary>
            /// <see cref="https://leetcode.com/problems/reorganize-string"/>
            [Theory]
            [InlineData("aab", "aba")]
            [InlineData("aaab", "")]
            public void ReorganizeString(string s, string expected)
            {
                static string InternalReorganizeString(string s)
                {
                    if (s == null || s.Length == 0) return "";

                    if (s.Length == 1) return s;

                    StringBuilder sb = new();

                    // Count how many of each char
                    Dictionary<char, int> bag = [];
                    foreach (char c in s)
                    {
                        bag[c] = bag.TryGetValue(c, out int count) ? count + 1 : 1;
                    }

                    // Build a priority queue of character and count
                    PriorityQueue<char, int> queue = new(bag.Count, Comparer<int>.Create((a, b) => b - a));
                    foreach (var pair in bag)
                    {
                        queue.Enqueue(pair.Key, pair.Value);
                    }

                    // Combine characters from the first and second position of the queue
                    while (queue.TryDequeue(out char first, out int firstCount))
                    {
                        if (queue.Count == 0 && firstCount > 1) return "";

                        sb.Append(first);
                        if (queue.TryDequeue(out char second, out int secondCount))
                        {
                            sb.Append(second);
                        }

                        if (firstCount > 1) queue.Enqueue(first, firstCount - 1);
                        if (secondCount > 1) queue.Enqueue(second, secondCount - 1);
                    }

                    return sb.ToString();
                }

                string actual = InternalReorganizeString(s);

                Assert.Equal(expected, actual);
            }

            [Theory]
            [InlineData("abc", "[1,2,1]", "[1,2,0]")]
            [InlineData("abc", "[4,1]", "[3,0]")]
            public void Read(string inputFile, string inputQuery, string output)
            {
                int[] query = inputQuery.Parse1DArray(int.Parse).ToArray();
                int[] expected = output.Parse1DArray(int.Parse).ToArray();

                char[] buf = new char[query.Max()];
                int bufPtr = 0;

                using LC158 reader = new(inputFile);
                for (int i = 0; i < query.Length; i++)
                {
                    int actual = reader.Read(buf, query[i]);
                    Assert.Equal(expected[i], actual);
                    Assert.Equal(inputFile.Skip(bufPtr).Take(actual), buf.Take(actual));

                    bufPtr += actual;
                }
            }
        }

        // You are given two integer arrays nums1 and nums2, sorted in non-decreasing order, and two integers m and n, representing the number of elements in nums1 and nums2 respectively.
        // Merge nums1 and nums2 into a single array sorted in non-decreasing order.
        // The final sorted array should not be returned by the function, but instead be stored inside the array nums1. To accommodate this, nums1 has a length of m + n, where the first m elements denote the elements that should be merged, and the last n elements are set to 0 and should be ignored. nums2 has a length of n.
        [Trait("List", "TopInterview150")]
        [Trait("List", "TopInterview150")]
        [Theory]
        [InlineData(new int[] { 1, 2, 3, 0, 0, 0 }, 3, new int[] { 2, 5, 6 }, 3, new int[] { 1, 2, 2, 3, 5, 6 })]
        [InlineData(new int[] { 1 }, 1, new int[] { }, 0, new int[] { 1 })]
        [InlineData(new int[] { 0 }, 0, new int[] { 1 }, 1, new int[] { 1 })]
        [InlineData(new int[] { 2, 0 }, 1, new int[] { 1 }, 1, new int[] { 1, 2 })]
        public void Merge(int[] nums1, int m, int[] nums2, int n, int[] expected)
        {
            static void InternalMerge(int[] nums1, int m, int[] nums2, int n)
            {
                if (n == 0)
                {
                    return;
                }

                int dst = m + n;

                while (dst > m)
                {
                    if (m == 0 || nums1[m - 1] < nums2[n - 1])
                    {
                        nums1[--dst] = nums2[--n];
                    }
                    else if (n == 0 || nums1[m - 1] >= nums2[n - 1])
                    {
                        nums1[--dst] = nums1[--m];
                    }
                }
            }

            InternalMerge(nums1, m, nums2, n);
            Assert.Equal(expected, nums1);
        }

        // Given an integer array nums and an integer val, remove all occurrences of val in nums in-place. The order of the elements may be changed. Then return the number of elements in nums which are not equal to val.
        // Consider the number of elements in nums which are not equal to val be k, to get accepted, you need to do the following things:
        // Change the array nums such that the first k elements of nums contain the elements which are not equal to val.The remaining elements of nums are not important as well as the size of nums.
        // Return k.
        [Trait("List", "TopInterview150")]
        [Trait("List", "TopInterview150")]
        [Theory]
        [InlineData(new int[] { 3, 2, 2, 3 }, 3, 2, new int[] { 2, 2 })]
        [InlineData(new int[] { 0, 1, 2, 2, 3, 0, 4, 2 }, 2, 5, new int[] { 0, 1, 3, 0, 4 })]
        public void RemoveElement(int[] nums, int val, int n, int[] expected)
        {
            int left = 0;
            int right = 0;
            int actual = 0;
            int limit = nums.Length;
            while (right < limit)
            {
                if (nums[right] != val)
                {
                    nums[left++] = nums[right++];
                    actual++;
                }
                else
                {
                    right++;
                }
            }

            Assert.Equal(n, actual);
            Assert.True(expected.SequenceEqual(nums.Take(actual).ToArray()));
        }

        // Given an integer array nums sorted in non-decreasing order, remove the duplicates in-place such that each unique element appears only once.The relative order of the elements should be kept the same.Then return the number of unique elements in nums.
        // Consider the number of unique elements of nums to be k, to get accepted, you need to do the following things:
        // Change the array nums such that the first k elements of nums contain the unique elements in the order they were present in nums initially.The remaining elements of nums are not important as well as the size of nums.
        // Return k.
        [Trait("List", "TopInterview150")]
        [Trait("List", "TopInterview150")]
        [Theory]
        [InlineData(new int[] { 1, 1, 2 }, 2, new int[] { 1, 2 })]
        [InlineData(new int[] { 0, 0, 1, 1, 1, 2, 2, 3, 3, 4 }, 5, new int[] { 0, 1, 2, 3, 4 })]
        [InlineData(new int[] { }, 0, new int[] { })]
        [InlineData(new int[] { 1 }, 1, new int[] { 1 })]
        public void RemoveDuplicates(int[] nums, int expCount, int[] expNums)
        {
            int t = nums.Length;
            int i = 0;

            for (int j = i + 1; j < t; j++)
            {
                if (nums[i] != nums[j])
                {
                    nums[++i] = nums[j];
                }
            }

            if (t > 0)
            {
                i++;
            }

            Assert.Equal(expCount, i);
            Assert.True(expNums.SequenceEqual(nums.Take(i).ToArray()));
        }

        // Given an integer array nums sorted in non-decreasing order, remove some duplicates in-place such that each unique element appears at most twice.The relative order of the elements should be kept the same.
        // Since it is impossible to change the length of the array in some languages, you must instead have the result be placed in the first part of the array nums.More formally, if there are k elements after removing the duplicates, then the first k elements of nums should hold the final result. It does not matter what you leave beyond the first k elements.
        // Return k after placing the final result in the first k slots of nums.
        // Do not allocate extra space for another array. You must do this by modifying the input array in-place with O(1) extra memory.
        [Trait("List", "TopInterview150")]
        [Theory]
        [InlineData(new int[] { 1, 1, 1, 2, 2, 3 }, 5, new int[] { 1, 1, 2, 2, 3 })]
        [InlineData(new int[] { 0, 0, 1, 1, 1, 1, 1, 2, 3, 3 }, 7, new int[] { 0, 0, 1, 1, 2, 3, 3 })]
        [InlineData(new int[] { 1, 1, 2 }, 3, new int[] { 1, 1, 2 })]
        [InlineData(new int[] { 0, 0, 1, 1, 1, 2, 2, 3, 3, 4 }, 9, new int[] { 0, 0, 1, 1, 2, 2, 3, 3, 4 })]
        [InlineData(new int[] { }, 0, new int[] { })]
        [InlineData(new int[] { 1 }, 1, new int[] { 1 })]
        public void RemoveDuplicates2(int[] nums, int expCount, int[] expNums)
        {
            int t = nums.Length;
            int i = 0;

            bool canRepeat = true;
            for (int j = i + 1; j < t; j++)
            {
                if (nums[i] == nums[j])
                {
                    if (canRepeat)
                    {
                        nums[++i] = nums[j];
                        canRepeat = false;
                    }
                }
                else
                {
                    nums[++i] = nums[j];
                    canRepeat = true;
                }
            }

            if (t > 0)
            {
                i++;
            }

            Assert.Equal(expCount, i);
            Assert.True(expNums.SequenceEqual(nums.Take(i).ToArray()));
        }

        // Given an array nums of size n, return the majority element.
        // The majority element is the element that appears more than ⌊n / 2⌋ times.You may assume that the majority element always exists in the array.
        [Trait("List", "TopInterview150")]
        [Theory]
        [InlineData(new int[] { 3, 2, 3 }, 3)]
        [InlineData(new int[] { 2, 2, 1, 1, 1, 2, 2 }, 2)]
        public void MajorityElement(int[] nums, int expected)
        {
            // The question assumes the majority number *always* exists, so basically we just need to find the number with highest occurrences

            int maxNumber = 0;
            int count = 0;
            int limit = nums.Length;

            for (int i = 0; i < limit; i++)
            {
                if (count == 0)
                {
                    maxNumber = nums[i];
                }

                if (nums[i] == maxNumber)
                {
                    count++;
                }
                else
                {
                    count--;
                }
            }


            //Dictionary<int, int> counts = new();
            //int maxCount = int.MinValue;
            //int maxNumber = int.MinValue;

            //foreach (int i in nums)
            //{
            //    if (!counts.ContainsKey(i))
            //    {
            //        counts[i] = 1;

            //        if (1 > maxCount)
            //        {
            //            maxCount = 1;
            //            maxNumber = i;
            //        }
            //    }
            //    else
            //    {
            //        int newCount = ++counts[i];

            //        if (newCount > maxCount)
            //        {
            //            maxCount = newCount;
            //            maxNumber = i;
            //        }
            //    }
            //}

            Assert.Equal(expected, maxNumber);
        }

        // Given an integer array nums, rotate the array to the right by k steps, where k is non-negative.
        [Trait("List", "TopInterview150")]
        [Theory]
        [InlineData(new int[] { 1, 2, 3, 4, 5, 6, 7 }, 3, new int[] { 5, 6, 7, 1, 2, 3, 4 })]
        [InlineData(new int[] { -1, -100, 3, 99 }, 2, new int[] { 3, 99, -1, -100 })]
        public void Rotate(int[] nums, int k, int[] expected)
        {
            static void reverse(int[] array, int start, int end)
            {
                int l = (end - start + 1) / 2;

                for (int i = 0; i < l; i++)
                {
                    int from = start + i;
                    int to = end - i;

                    array[from] ^= array[to];
                    array[to] ^= array[from];
                    array[from] ^= array[to];
                }
            }

            k %= nums.Length;
            int l = nums.Length - 1;

            reverse(nums, 0, l - k);
            reverse(nums, l - k + 1, l);
            reverse(nums, 0, l);

            Assert.True(expected.SequenceEqual(nums));
        }

        // You are given an array prices where prices[i] is the price of a given stock on the ith day.
        // You want to maximize your profit by choosing a single day to buy one stock and choosing a different day in the future to sell that stock.
        // Return the maximum profit you can achieve from this transaction.If you cannot achieve any profit, return 0.
        [Trait("List", "TopInterview150")]
        [Theory]
        [InlineData(new int[] { 7, 1, 5, 3, 6, 4 }, 5)]
        [InlineData(new int[] { 7, 6, 4, 3, 1 }, 0)]
        public void MaxProfit(int[] prices, int expected)
        {
            int maxProfit = 0;

            if (prices.Length > 1)
            {
                int minPrice = prices[0];
                for (int i = 1; i < prices.Length; i++)
                {
                    if (prices[i] < minPrice)
                    {
                        minPrice = prices[i];
                    }
                    else if (prices[i] - minPrice > maxProfit)
                    {
                        maxProfit = prices[i] - minPrice;
                    }
                }
            }

            Assert.Equal(expected, maxProfit);
        }

        // You are given an integer array prices where prices[i] is the price of a given stock on the ith day.
        // On each day, you may decide to buy and/or sell the stock.You can only hold at most one share of the stock at any time.However, you can buy it then immediately sell it on the same day.
        // Find and return the maximum profit you can achieve.
        [Trait("List", "TopInterview150")]
        [Theory]
        [InlineData(new int[] { 7, 1, 5, 3, 6, 4 }, 7)]
        [InlineData(new int[] { 1, 2, 3, 4, 5 }, 4)]
        [InlineData(new int[] { 7, 6, 4, 3, 1 }, 0)]
        public void MaxProfit2(int[] prices, int expected)
        {
            int totalProfit = 0;

            if (prices.Length > 1)
            {
                for (int i = 1; i < prices.Length; i++)
                {
                    if (prices[i] > prices[i - 1])
                    {
                        totalProfit += prices[i] - prices[i - 1];
                    }
                }
            }

            Assert.Equal(expected, totalProfit);
        }

        // You are given an integer array nums. You are initially positioned at the array's first index, and each element in the array represents your maximum jump length at that position.
        // Return true if you can reach the last index, or false otherwise.
        [Trait("List", "TopInterview150")]
        [Theory]
        [InlineData(new[] { 2, 3, 1, 1, 4 }, true)]
        [InlineData(new[] { 3, 2, 1, 0, 4 }, false)]
        [InlineData(new[] { 2, 0 }, true)]
        [InlineData(new[] { 5, 0, 0, 0, 0, 0 }, true)]
        [InlineData(new[] { 0 }, true)]
        [InlineData(new[] { 0, 1 }, false)]
        public void CanJump(int[] nums, bool expected)
        {
            // Greedy approach - We are not trying to optimize the number of steps, so all we care is whether we can avoid landing on a 0 with no steps left
            // To do so, all we need to do is to take the maximum number of steps from the current position or previous position
            // We also don't care about the last place, since it's the destination, all we care is whether we don't stop at a 0 right before the last
            int target = nums.Length - 1;
            bool canJump = true;

            int next = 0;
            for (int i = 0; i < target; i++)
            {
                next = Math.Max(next - 1, nums[i]);

                if (next == 0)
                {
                    canJump = false;
                    break;
                }
            }

            Assert.Equal(expected, canJump);

            //HashSet<int> visited = new() { 0 };
            //int target = nums.Length - 1;

            //bool canJump = target == 0;
            //while (!canJump)
            //{
            //    HashSet<int> visitedNext = new();

            //    foreach(int index in visited)
            //    {
            //        for (int i = 1; i <= nums[index]; i++)
            //        {
            //            int next = index + i;

            //            if (next == target)
            //            {
            //                canJump = true;
            //                break;
            //            }
            //            if (next > target || visited.Contains(next))
            //            {
            //                continue;
            //            }
            //            else
            //            {
            //                visitedNext.Add(next);
            //            }
            //        }

            //        if (canJump) break;
            //    }

            //    if (visitedNext.Count == 0)
            //    {
            //        break;
            //    }

            //    visited = visitedNext;
            //}

            //HashSet<int> visited = new() { 0 };

            //int target = nums.Length - 1;

            //bool canJump = nums.Length == 1;
            //for (int i = 0; !canJump && i < nums.Length; i++)
            //{
            //    if (!visited.Contains(i))
            //    {
            //        continue;
            //    }

            //    for (int j = 1; j <= nums[i]; j++)
            //    {
            //        int next = i + j;

            //        if (visited.Contains(next))
            //        {
            //            continue;
            //        }
            //        else if (next == target)
            //        {
            //            canJump = true;
            //            break;
            //        }
            //        else if (next < target)
            //        {
            //            visited.Add(next);
            //        }
            //    }
            //}
            //bool[] visited = new bool[nums.Length];
            //visited[0] = true;
            //int target = nums.Length - 1;
            //bool canJump = nums.Length == 1;
            //for (int i = 0; !canJump && i < nums.Length; i++)
            //{
            //    if (!visited[i])
            //    {
            //        continue;
            //    }

            //    for (int j = 1; j <= nums[i]; j++)
            //    {
            //        int next = i + j;

            //        if (next < visited.Length && visited[next])
            //        {
            //            continue;
            //        }
            //        else if (next == target)
            //        {
            //            canJump = true;
            //            break;
            //        }
            //        else if (next < target)
            //        {
            //            visited[next] = true;
            //        }
            //    }
            //}

            Assert.Equal(expected, canJump);
        }

        // You are given a 0-indexed array of integers nums of length n. You are initially positioned at nums[0].
        // Each element nums[i] represents the maximum length of a forward jump from index i.In other words, if you are at nums[i], you can jump to any nums[i + j] where:
        // 0 <= j <= nums[i] and
        // i + j<n
        // Return the minimum number of jumps to reach nums[n - 1]. The test cases are generated such that you can reach nums[n - 1].
        [Trait("List", "TopInterview150")]
        [Theory]
        [InlineData(new[] { 2, 3, 1, 1, 4 }, 2)]
        [InlineData(new[] { 2, 3, 0, 1, 4 }, 2)]
        [InlineData(new[] { 2, 1 }, 1)]
        public void CanJump2(int[] nums, int expected)
        {
            int limit = nums.Length - 1;
            int[] totalJumps = new int[nums.Length];

            for (int i = 0; i < limit; i++)
            {
                for (int j = Math.Min(nums[i], limit - i); j >= 1; j--)
                {
                    int jumpsTillHere = totalJumps[i];
                    int jumpsTillThere = totalJumps[i + j];

                    if (jumpsTillHere + 1 < jumpsTillThere || jumpsTillThere == 0)
                    {
                        totalJumps[i + j] = jumpsTillHere + 1;
                    }
                }
            }

            Assert.Equal(expected, totalJumps[limit]);
        }

        // Given an array of integers citations where citations[i] is the number of citations a researcher received for their ith paper, return the researcher's h-index.
        // According to the definition of h-index on Wikipedia: The h-index is defined as the maximum value of h such that the given researcher has published at least h papers that have each been cited at least h times.
        [Trait("List", "TopInterview150")]
        [Theory]
        [InlineData(new[] { 3, 0, 6, 1, 5 }, 3)]
        [InlineData(new[] { 1, 3, 1 }, 1)]
        [InlineData(new[] { 0, 0, 0 }, 0)]
        [InlineData(new[] { 1, 1, 1 }, 1)]
        [InlineData(new[] { 5, 5, 5, 5, 5 }, 5)]
        [InlineData(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, 5)]
        [InlineData(new[] { 100 }, 1)]
        [InlineData(new[] { 0 }, 0)]
        [InlineData(new[] { 0, 1 }, 1)]
        [InlineData(new int[] { }, 0)]
        public void HIndex(int[] citations, int expected)
        {
            Array.Sort(citations);

            int limit = citations.Length;
            int actual = 0;

            for (int i = 0; i < limit; i++)
            {
                if (citations[i] >= limit - i)
                {
                    actual = limit - i;
                    break;
                }
            }

            Assert.Equal(expected, actual);
        }

        public class RandomizedSet
        {
            private readonly Random random = new();
            private readonly Dictionary<int, int> val2idx = new();
            private readonly Dictionary<int, int> idx2val = new();

            public RandomizedSet()
            {
            }

            public bool Insert(int val)
            {
                int idx = idx2val.Count + 1;

                bool result = val2idx.TryAdd(val, idx);

                if (result)
                {
                    idx2val.Add(idx, val);
                }

                return result;
            }

            public bool Remove(int val)
            {
                bool result = val2idx.TryGetValue(val, out int idx);

                if (result)
                {
                    int lastIdx = idx2val.Count;

                    // Remove from both
                    val2idx.Remove(val);
                    idx2val.Remove(idx);

                    // Fill the gap, if any
                    if (lastIdx != idx)
                    {
                        int lastVal = idx2val[lastIdx];
                        idx2val.Remove(lastIdx);
                        val2idx[lastVal] = idx;
                        idx2val[idx] = lastVal;
                    }
                }

                return result;
            }

            public int GetRandom()
            {
                return idx2val[random.Next(idx2val.Count) + 1];
            }
        }

        [Trait("List", "TopInterview150")]
        [Theory]
        [InlineData(new int[] { })]
        [InlineData(10, 10)]
        [InlineData(-10)]
        [InlineData(10, 20, 30, -30)]
        [InlineData(10, 20, 30, -20)]
        [InlineData(10, 20, 30, -10)]
        [InlineData(10, -20, 20, 0, -10, 20, 0)]
        public void RandomizedSetTest(params int[] vals)
        {
            RandomizedSet set = new();

            foreach (int val in vals)
            {
                if (val > 0)
                {
                    set.Insert(val);
                }
                else if (val < 0)
                {
                    set.Remove(-val);
                }
                else
                {
                    int _ = set.GetRandom();
                }
            }
        }

        // Given an integer array nums, return an array answer such that answer[i] is equal to the product of all the elements of nums except nums[i].
        // The product of any prefix or suffix of nums is guaranteed to fit in a 32-bit integer.
        // You must write an algorithm that runs in O(n) time and without using the division operation.
        [Trait("List", "TopInterview150")]
        [Theory]
        [InlineData(new[] { 1, 2, 3, 4 }, new[] { 24, 12, 8, 6 })]
        [InlineData(new[] { -1, 1, 0, -3, 3 }, new[] { 0, 0, 9, 0, 0 })]
        [InlineData(new[] { -1 }, new[] { 0 })]
        [InlineData(new[] { -1, -1 }, new[] { -1, -1 })]
        [InlineData(new[] { 1, -1 }, new[] { -1, 1 })]
        public void ProductExceptSelf(int[] nums, int[] expected)
        {
            int[] result = new int[nums.Length];
            for (int i = 0; i < result.Length; i++) result[i] = nums.Length == 1 ? 0 : 1;

            int left = 1;
            for (int i = 0; i < nums.Length; i++)
            {
                result[i] *= left;
                left *= nums[i];
            }

            int right = 1;
            for (int i = result.Length - 1; i >= 0; i--)
            {
                result[i] *= right;
                right *= nums[i];
            }

            Assert.True(expected.SequenceEqual(result));
        }

        // There are n gas stations along a circular route, where the amount of gas at the ith station is gas[i].
        // You have a car with an unlimited gas tank and it costs cost[i] of gas to travel from the ith station to its next(i + 1)th station.You begin the journey with an empty tank at one of the gas stations.
        // Given two integer arrays gas and cost, return the starting gas station's index if you can travel around the circuit once in the clockwise direction, otherwise return -1. If there exists a solution, it is guaranteed to be unique.
        [Trait("List", "TopInterview150")]
        [Theory]
        [InlineData(new[] { 1, 2, 3, 4, 5 }, new[] { 3, 4, 5, 1, 2 }, 3)]
        [InlineData(new[] { 2, 3, 4 }, new[] { 3, 4, 3 }, -1)]
        public void CanCompleteCircuit(int[] gas, int[] cost, int expected)
        {
            int totalGas = 0;
            int totalCost = 0;
            int currentGas = 0;
            int startStation = 0;

            for (int i = 0; i < gas.Length; i++)
            {
                totalGas += gas[i];
                totalCost += cost[i];
                currentGas += gas[i] - cost[i];

                if (currentGas < 0)
                {
                    startStation = i + 1;
                    currentGas = 0;
                }
            }

            int result = totalGas >= totalCost ? startStation : -1;

            Assert.Equal(expected, result);
        }

        // There are n children standing in a line.Each child is assigned a rating value given in the integer array ratings.
        // You are giving candies to these children subjected to the following requirements:
        // Each child must have at least one candy.
        // Children with a higher rating get more candies than their neighbors.
        // Return the minimum number of candies you need to have to distribute the candies to the children.
        [Trait("List", "TopInterview150")]
        [Theory]
        [InlineData(new[] { 1, 0, 2 }, 5)]
        [InlineData(new[] { 1, 2, 2 }, 4)]
        [InlineData(new[] { 1, 3, 2, 2, 1 }, 7)]
        public void Candy(int[] ratings, int expected)
        {
            int t = ratings.Length;
            int[] candies = new int[t];

            // Give each kid a candy
            for (int i = 0; i < t; i++) candies[i] = 1;

            // From left to right, give extra candy if the kid on the right has better rating
            for (int i = 1; i < t; i++)
            {
                if (ratings[i] > ratings[i - 1]) candies[i] = candies[i - 1] + 1;
            }

            // From right to left, give extra candy if the kid on the left has better rating
            for (int i = t - 2; i >= 0; i--)
            {
                if (ratings[i] > ratings[i + 1]) candies[i] = Math.Max(candies[i + 1] + 1, candies[i]);
            }

            int n = 0;
            for (int i = 0; i < t; i++) n += candies[i];

            Assert.Equal(expected, n);
        }

        // The string "PAYPALISHIRING" is written in a zigzag pattern on a given number of rows like this: (you may want to display this pattern in a fixed font for better legibility)
        // 
        // P   A   H   N
        // A P L S I I G
        // Y   I   R
        // And then read line by line: "PAHNAPLSIIGYIR"
        // 
        // Write the code that will take a string and make this conversion given a number of rows:
        [Trait("List", "TopInterview150")]
        [Theory]
        [InlineData("PAYPALISHIRING", 3, "PAHNAPLSIIGYIR")]
        [InlineData("PAYPALISHIRING", 4, "PINALSIGYAHRPI")]
        [InlineData("A", 1, "A")]
        [InlineData("AB", 1, "AB")]
        public void ZigZag(string s, int numRows, string expected)
        {
            string result;

            if (numRows > 1)
            {
                List<StringBuilder> stringBuilders = new();
                for (int i = 0; i < numRows; i++)
                {
                    stringBuilders.Add(new StringBuilder());
                }

                int row = 0;
                bool down = true;
                foreach (char c in s)
                {
                    if (down)
                    {
                        stringBuilders[row++].Append(c);
                        if (row == numRows)
                        {
                            down = false;
                            row--;
                        }
                    }
                    else
                    {
                        stringBuilders[--row].Append(c);
                        if (row == 0)
                        {
                            down = true;
                            row++;
                        }
                    }
                }

                result = stringBuilders
                    .Aggregate(new StringBuilder(), (acc, builder) => acc.Append(builder.ToString()))
                    .ToString();
            }
            else
            {
                result = s;
            }

            Assert.Equal(expected, result);
        }
    }
}