using System.Collections.Immutable;
using Xunit.Sdk;

namespace leetcode.Lists.Top150
{
    public class TwoPointers
    {
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

        // Given an integer array nums, return all the triplets [nums[i], nums[j], nums[k]] such that i != j, i != k, and j != k, and nums[i] + nums[j] + nums[k] == 0.
        // Notice that the solution set must not contain duplicate triplets.
        // Notice that the order of the output and the order of the triplets does not matter.
        [Trait("List", "TopInterview150")]
        [Theory]
        [InlineData(new[] { -1, 0, 1, 2, -1, -4 }, new[] { -1, -1, 2 }, new[] { -1, 0, 1 })]
        [InlineData(new[] { 0, 1, 1 })]
        [InlineData(new[] { 0, 0, 0 }, new[] { 0, 0, 0 })]
        [InlineData(new[] { -2, 0, 1, 1, 2 }, new[] { -2, 0, 2 }, new[] { -2, 1, 1 })]
        public void ThreeSum(int[] nums, params int[][] results)
        {
            List<IList<int>> result = new();

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

            HashSet<Tuple<int, int, int>> actual = result.Select(x => { int[] a = x.ToArray(); Array.Sort(a); return new Tuple<int, int, int>(a[0], a[1], a[2]); }).ToHashSet();
            HashSet<Tuple<int, int, int>> expected = results.Select(x => { int[] a = x.ToArray(); Array.Sort(a); return new Tuple<int, int, int>(a[0], a[1], a[2]); }).ToHashSet();

            Assert.Equal(expected, actual);
        }
    }
}
