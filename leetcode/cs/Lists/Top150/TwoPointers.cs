using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;
using System.Collections.Immutable;
using System.Diagnostics;
using Xunit.Sdk;

namespace leetcode.Lists.Top150
{
    public class TwoPointers
    {
        [Trait("Difficulty", "Medium")]
        public class Medium
        {

            /// <summary>
            /// 15. 3Sum
            /// Given an integer array nums, return all the triplets [nums[i], nums[j], nums[k]] such that i != j, i != k, and j != k, and nums[i] + nums[j] + nums[k] == 0.
            /// Notice that the solution set must not contain duplicate triplets.
            /// Given an integer array nums, return all the triplets [nums[i], nums[j], nums[k]] such that i != j, i != k, and j != k, and nums[i] + nums[j] + nums[k] == 0.
            /// </summary>
            /// <see cref="https://leetcode.com/problems/3sum/"/>
            [Theory]
            [InlineData("[-1,0,1,2,-1,-4]", "[[-1,-1,2],[-1,0,1]]")]
            [InlineData("[0,1,1]", "[]")]
            [InlineData("[0,0,0]", "[[0,0,0]]")]
            public void ThreeSum(string input, string output)
            {
                int[] nums = input.Parse1DArray(int.Parse).ToArray();
                int[][] results = output.Parse2DArray(int.Parse).Select(x => x.ToArray()).ToArray();

                // --
                static List<IList<int>> SortAndSearch(int[] nums)
                {
                    List<IList<int>> result = [];

                    Array.Sort(nums);

                    int n = nums.Length - 2;

                    for (int i = 0; i < n; i++)
                    {
                        if (i > 0 && nums[i] == nums[i - 1]) continue;

                        int left = i + 1;
                        int right = n + 1;

                        while (left < right)
                        {
                            int sum = nums[i] + nums[left] + nums[right];

                            if (sum < 0)
                            {
                                left++;
                            }
                            else if (sum > 0)
                            {
                                right--;
                            }
                            else // sum == 0
                            {
                                result.Add(new List<int>() { nums[i], nums[left], nums[right] });

                                while (left < right && nums[left] == nums[left + 1]) left++;
                                while (left < right && nums[right - 1] == nums[right]) right--;

                                left++;
                                right--;
                            }
                        }
                    }

                    return result;
                }

                static bool ComplementSet2(int[] nums, int from, int to, int target, out List<List<int>>? result)
                {
                    result = [];
                    HashSet<int> others = [];
                    for (int i = from; i < to; i++)
                    {
                        int other = target - nums[i];

                        if (others.Contains(other))
                        {
                            result.Add([nums[i], other]);
                        }

                        others.Add(nums[i]);
                    }

                    return result.Count > 0;
                }

                static List<IList<int>> ComplementSet3(int[] nums)
                {
                    List<IList<int>> result = [];

                    Array.Sort(nums);

                    for (int i = 0, limit = nums.Length - 2; i < limit; i++)
                    {
                        if (nums[i] == nums[i] + 1) continue;

                        if (ComplementSet2(nums, i + 1, nums.Length, -nums[i], out var threeSums))
                        {
                            foreach (var threeSum in threeSums!) threeSum.Insert(0, nums[i]);
                            result.AddRange(threeSums);
                        }
                    }

                    return result;
                }

                static List<IList<int>> NoSort(int[] nums)
                {
                    HashSet<Tuple<int, int, int>> seen = [];

                    List<IList<int>> result = [];

                    for (int i = 0, limit = nums.Length - 2; i < limit; i++)
                    {
                        if (ComplementSet2(nums, i + 1, nums.Length, -nums[i], out var threeSums))
                        {
                            foreach (var threeSum in threeSums!)
                            {
                                threeSum.Add(nums[i]);
                                threeSum.Sort();

                                if (!seen.Add(new(threeSum[0], threeSum[1], threeSum[2]))) continue;

                                result.Add(threeSum);
                            }
                        }
                    }

                    return result;
                }

                string solution = nameof(NoSort);
                List<IList<int>> result =
                    solution == nameof(SortAndSearch) ? SortAndSearch(nums) :
                    solution == nameof(ComplementSet3) ? ComplementSet3(nums) :
                    solution == nameof(NoSort) ? NoSort(nums) :
                    throw new NotSupportedException(solution);

                // --
                HashSet<Tuple<int, int, int>> actual = result.Select(x => { int[] a = x.ToArray(); Array.Sort(a); return new Tuple<int, int, int>(a[0], a[1], a[2]); }).ToHashSet();
                HashSet<Tuple<int, int, int>> expected = results.Select(x => { int[] a = x.ToArray(); Array.Sort(a); return new Tuple<int, int, int>(a[0], a[1], a[2]); }).ToHashSet();

                Assert.Equal(expected, actual);
            }
        }

        // A phrase is a palindrome if, after converting all uppercase letters into lowercase letters and removing all non-alphanumeric characters, it reads the same forward and backward. Alphanumeric characters include letters and numbers.
        // Given a string s, return true if it is a palindrome, or false otherwise.
        [Trait("List", "TopInterview150")]
        [Theory]
        [InlineData("A man, a plan, a canal: Panama", true)]
        [InlineData("race a car", false)]
        [InlineData(" ", true)]
        public void IsPalindrome(string s, bool expected)
        {
            int left = 0;
            int right = s.Length - 1;

            bool result = true;
            while (true)
            {
                while (left < right && !char.IsLetter(s[left]) && !char.IsDigit(s[left])) left++;
                while (right > left && !char.IsLetter(s[right]) && !char.IsDigit(s[right])) right--;

                if (left >= right) break;

                if (char.ToLowerInvariant(s[left]) == char.ToLowerInvariant(s[right]))
                {
                    left++;
                    right--;
                }
                else
                {
                    result = false;
                    break;
                }
            }

            Assert.Equal(expected, result);
        }

        // Given two strings s and t, return true if s is a subsequence of t, or false otherwise.
        // A subsequence of a string is a new string that is formed from the original string by deleting some (can be none) of the characters without disturbing the relative positions of the remaining characters. (i.e., "ace" is a subsequence of "abcde" while "aec" is not).
        [Trait("List", "TopInterview150")]
        [Theory]
        [InlineData("abc", "ahbgdc", true)]
        [InlineData("axc", "ahbgdc", false)]
        [InlineData("", "ahbgdc", true)]
        [InlineData("aaaaaa", "bbaaaa", false)]
        [InlineData("bb", "ahbgdc", false)]
        [InlineData("abc", "acabcbac", true)]
        public void IsSubsequence(string s, string t, bool expected)
        {
            int si = 0;
            int ti = 0;

            while (si < s.Length && ti < t.Length)
            {
                if (s[si] == t[ti]) si++;
                ti++;
            }

            bool result = si == s.Length;

            Assert.Equal(expected, result);
        }

        // Given a 1-indexed array of integers numbers that is already sorted in non-decreasing order, find two numbers such that they add up to a specific target number. Let these two numbers be numbers[index1] and numbers[index2] where 1 <= index1 < index2 <= numbers.length.
        // Return the indices of the two numbers, index1 and index2, added by one as an integer array [index1, index2] of length 2.
        // The tests are generated such that there is exactly one solution. You may not use the same element twice.
        // Your solution must use only constant extra space.
        [Trait("List", "TopInterview150")]
        [Theory]
        [InlineData(new[] { 2, 7, 11, 15 }, 9, new[] { 1, 2 })]
        [InlineData(new[] { 2, 3, 4 }, 6, new[] { 1, 3 })]
        [InlineData(new[] { -1, 0 }, -1, new[] { 1, 2 })]
        public void TwoSum2(int[] numbers, int target, int[] expected)
        {
            int n = numbers.Length - 1;
            int i = 0;
            int j = n;

            while (i < j)
            {
                int sum = numbers[i] + numbers[j];

                if (sum < target)
                {
                    if (j < n && numbers[i] + numbers[j + 1] < target) j++;
                    else i++;
                }
                else if (sum > target)
                {
                    if (i > 0 && numbers[i - 1] + numbers[j] > target) i--;
                    else j--;
                }
                else
                {
                    break;
                }
            }

            int[] result = new[] { i + 1, j + 1 };

            Assert.True(expected.SequenceEqual(result));
        }

        // You are given an integer array height of length n. There are n vertical lines drawn such that the two endpoints of the ith line are (i, 0) and (i, height[i]).
        // Find two lines that together with the x-axis form a container, such that the container contains the most water.
        // Return the maximum amount of water a container can store.
        [Trait("List", "TopInterview150")]
        [Theory]
        [InlineData(new[] { 1, 8, 6, 2, 5, 4, 8, 3, 7 }, 49)]
        [InlineData(new[] { 1, 1 }, 1)]
        public void MaxArea(int[] height, int expected)
        {
            int maxArea = 0;
            int left = 0;
            int right = height.Length - 1;

            while (left < right)
            {
                int width = right - left;
                int minHeight = Math.Min(height[left], height[right]);
                int currentArea = width * minHeight;
                maxArea = Math.Max(maxArea, currentArea);

                if (height[left] < height[right])
                {
                    left++;
                }
                else
                {
                    right--;
                }
            }

            Assert.Equal(expected, maxArea);
        }
    }
}
