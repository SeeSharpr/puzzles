using System.Runtime.InteropServices;
using System;

namespace leetcode.Lists
{
    public class Top150
    {
        [Theory]
        [InlineData(new int[] { 1, 2, 3, 0, 0, 0 }, 3, new int[] { 2, 5, 6 }, 3, new int[] { 1, 2, 2, 3, 5, 6 })]
        [InlineData(new int[] { 1 }, 1, new int[] { }, 0, new int[] { 1 })]
        [InlineData(new int[] { 0 }, 0, new int[] { 1 }, 1, new int[] { 1 })]
        // You are given two integer arrays nums1 and nums2, sorted in non-decreasing order, and two integers m and n, representing the number of elements in nums1 and nums2 respectively.
        // Merge nums1 and nums2 into a single array sorted in non-decreasing order.
        // The final sorted array should not be returned by the function, but instead be stored inside the array nums1. To accommodate this, nums1 has a length of m + n, where the first m elements denote the elements that should be merged, and the last n elements are set to 0 and should be ignored. nums2 has a length of n.
        public void Merge(int[] nums1, int m, int[] nums2, int n, int[] expected)
        {
            int i = 0;
            int j = 0;
            int p = 0;
            int t = m + n;
            int[] result = new int[t];
            while (p < t)
            {
                if ((i < m) && (j < n))
                {
                    if (nums1[i] < nums2[j])
                    {
                        result[p++] = nums1[i++];
                    }
                    else
                    {
                        result[p++] = nums2[j++];
                    }
                }
                else if (i < m)
                {
                    result[p++] = nums1[i++];
                }
                else // j < n
                {
                    result[p++] = nums2[j++];
                }
            }
            Array.Copy(result, nums1, t);
            Assert.True(expected.SequenceEqual(nums1));
        }

        [Theory]
        [InlineData(new int[] { 3, 2, 2, 3 }, 3, 2, new int[] { 2, 2 })]
        [InlineData(new int[] { 0, 1, 2, 2, 3, 0, 4, 2 }, 2, 5, new int[] { 0, 1, 3, 0, 4 })]
        // Given an integer array nums and an integer val, remove all occurrences of val in nums in-place. The order of the elements may be changed. Then return the number of elements in nums which are not equal to val.
        // Consider the number of elements in nums which are not equal to val be k, to get accepted, you need to do the following things:
        // Change the array nums such that the first k elements of nums contain the elements which are not equal to val.The remaining elements of nums are not important as well as the size of nums.
        // Return k.
        public void RemoveElement(int[] nums, int val, int n, int[] expected)
        {
            int i = 0;
            int j = 0;
            int c = 0;
            int t = nums.Length;
            while (j < t)
            {
                if (nums[j] != val)
                {
                    nums[i++] = nums[j++];
                    c++;
                }
                else
                {
                    j++;
                }
            }

            Assert.Equal(n, c);
            Assert.True(expected.SequenceEqual(nums.Take(c).ToArray()));
        }

        [Theory]
        [InlineData(new int[] { 1, 1, 2 }, 2, new int[] { 1, 2 })]
        [InlineData(new int[] { 0, 0, 1, 1, 1, 2, 2, 3, 3, 4 }, 5, new int[] { 0, 1, 2, 3, 4 })]
        [InlineData(new int[] { }, 0, new int[] { })]
        [InlineData(new int[] { 1 }, 1, new int[] { 1 })]
        // Given an integer array nums sorted in non-decreasing order, remove the duplicates in-place such that each unique element appears only once.The relative order of the elements should be kept the same.Then return the number of unique elements in nums.
        // Consider the number of unique elements of nums to be k, to get accepted, you need to do the following things:
        // Change the array nums such that the first k elements of nums contain the unique elements in the order they were present in nums initially.The remaining elements of nums are not important as well as the size of nums.
        // Return k.
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

        [Theory]
        [InlineData(new int[] { 1, 1, 1, 2, 2, 3 }, 5, new int[] { 1, 1, 2, 2, 3 })]
        [InlineData(new int[] { 0, 0, 1, 1, 1, 1, 1, 2, 3, 3 }, 7, new int[] { 0, 0, 1, 1, 2, 3, 3 })]
        [InlineData(new int[] { 1, 1, 2 }, 3, new int[] { 1, 1, 2 })]
        [InlineData(new int[] { 0, 0, 1, 1, 1, 2, 2, 3, 3, 4 }, 9, new int[] { 0, 0, 1, 1, 2, 2, 3, 3, 4 })]
        [InlineData(new int[] { }, 0, new int[] { })]
        [InlineData(new int[] { 1 }, 1, new int[] { 1 })]
        // Given an integer array nums sorted in non-decreasing order, remove some duplicates in-place such that each unique element appears at most twice.The relative order of the elements should be kept the same.
        // Since it is impossible to change the length of the array in some languages, you must instead have the result be placed in the first part of the array nums.More formally, if there are k elements after removing the duplicates, then the first k elements of nums should hold the final result. It does not matter what you leave beyond the first k elements.
        // Return k after placing the final result in the first k slots of nums.
        // Do not allocate extra space for another array. You must do this by modifying the input array in-place with O(1) extra memory.
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

        [Theory]
        [InlineData(new int[] { 3, 2, 3 }, 3)]
        [InlineData(new int[] { 2, 2, 1, 1, 1, 2, 2 }, 2)]
        // Given an array nums of size n, return the majority element.
        // The majority element is the element that appears more than ⌊n / 2⌋ times.You may assume that the majority element always exists in the array.
        public void MajorityElement(int[] nums, int expected)
        {
            // The question assumes the majority number *always* exists, so basically we just need to find the number with highest occurrences
            Dictionary<int, int> counts = new();
            int maxCount = int.MinValue;
            int maxNumber = int.MinValue;

            foreach (int i in nums)
            {
                if (!counts.ContainsKey(i))
                {
                    counts[i] = 1;

                    if (1 > maxCount)
                    {
                        maxCount = 1;
                        maxNumber = i;
                    }
                }
                else
                {
                    int newCount = ++counts[i];

                    if (newCount > maxCount)
                    {
                        maxCount = newCount;
                        maxNumber = i;
                    }
                }
            }

            Assert.Equal(expected, maxNumber);
        }

        [Theory]
        [InlineData(new int[] { 1, 2, 3, 4, 5, 6, 7 }, 3, new int[] { 5, 6, 7, 1, 2, 3, 4 })]
        [InlineData(new int[] { -1, -100, 3, 99 }, 2, new int[] { 3, 99, -1, -100 })]
        // Given an integer array nums, rotate the array to the right by k steps, where k is non-negative.
        public void Rotate(int[] nums, int k, int[] expected)
        {
            var reverse = delegate (int[] array, int start, int end)
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
            };

            k %= nums.Length;
            int l = nums.Length - 1;

            reverse(nums, 0, l - k);
            reverse(nums, l - k + 1, l);
            reverse(nums, 0, l);

            Assert.True(expected.SequenceEqual(nums));
        }

        [Theory]
        [InlineData(new int[] { 7, 1, 5, 3, 6, 4 }, 5)]
        [InlineData(new int[] { 7, 6, 4, 3, 1 }, 0)]
        // You are given an array prices where prices[i] is the price of a given stock on the ith day.
        // You want to maximize your profit by choosing a single day to buy one stock and choosing a different day in the future to sell that stock.
        // Return the maximum profit you can achieve from this transaction.If you cannot achieve any profit, return 0.
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

        [Theory]
        [InlineData(new int[] { 7, 1, 5, 3, 6, 4 }, 7)]
        [InlineData(new int[] { 1, 2, 3, 4, 5 }, 4)]
        [InlineData(new int[] { 7, 6, 4, 3, 1 }, 0)]
        // You are given an integer array prices where prices[i] is the price of a given stock on the ith day.
        // On each day, you may decide to buy and/or sell the stock.You can only hold at most one share of the stock at any time.However, you can buy it then immediately sell it on the same day.
        // Find and return the maximum profit you can achieve.
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

        [Theory]
        [InlineData(new[] { 2, 3, 1, 1, 4 }, true)]
        [InlineData(new[] { 3, 2, 1, 0, 4 }, false)]
        [InlineData(new[] { 2, 0 }, true)]
        [InlineData(new[] { 0 }, true)]
        [InlineData(new[] { 0, 1 }, false)]
        // You are given an integer array nums. You are initially positioned at the array's first index, and each element in the array represents your maximum jump length at that position.
        // Return true if you can reach the last index, or false otherwise.
        public void CanJump(int[] nums, bool expected)
        {
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

        [Theory]
        [InlineData(new[] { 2, 3, 1, 1, 4 }, 2)]
        [InlineData(new[] { 2, 3, 0, 1, 4 }, 2)]
        [InlineData(new[] { 2, 1 }, 1)]
        // You are given a 0-indexed array of integers nums of length n. You are initially positioned at nums[0].
        // Each element nums[i] represents the maximum length of a forward jump from index i.In other words, if you are at nums[i], you can jump to any nums[i + j] where:
        // 0 <= j <= nums[i] and
        // i + j<n
        // Return the minimum number of jumps to reach nums[n - 1]. The test cases are generated such that you can reach nums[n - 1].
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
        // Given an array of integers citations where citations[i] is the number of citations a researcher received for their ith paper, return the researcher's h-index.
        // According to the definition of h-index on Wikipedia: The h-index is defined as the maximum value of h such that the given researcher has published at least h papers that have each been cited at least h times.
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
    }
}
