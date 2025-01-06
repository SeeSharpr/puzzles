using System.Data.SqlTypes;

namespace leetcode.Lists.Top150
{
    public class SlidingWindow
    {
        // Given an array of positive integers nums and a positive integer target, return the minimal length of a subarray whose sum is greater than or equal to target.
        // If there is no such subarray, return 0 instead.
        [Theory]
        [InlineData(7, new[] { 2, 3, 1, 2, 4, 3 }, 2)]
        [InlineData(4, new[] { 1, 4, 4 }, 1)]
        [InlineData(11, new[] { 1, 1, 1, 1, 1, 1, 1, 1 }, 0)]
        public void MinSubArrayLen(int target, int[] nums, int expected)
        {
            int n = nums.Length;
            int left = 0;
            int sum = 0;

            int minLength = int.MaxValue;
            for (int right = 0; right < n; right++)
            {
                sum += nums[right];

                while (sum >= target && left <= right)
                {
                    minLength = Math.Min(minLength, right - left + 1);
                    sum -= nums[left];
                    left++;
                }
            }

            if (minLength == int.MaxValue)
            {
                minLength = 0;
            }

            Assert.Equal(expected, minLength);
        }

        // Given a string s, find the length of the longest substring without repeating characters.
        [Theory]
        [InlineData("abcabcbb", 3)]
        [InlineData("bbbbb", 1)]
        [InlineData("pwwkew", 3)]
        [InlineData("aab", 2)]
        public void LengthOfLongestSubstring(string s, int expected)
        {
            HashSet<char> seen = new();

            int maxLength = 0;
            int currentLength = 0;
            int left = 0;
            for (int right = 0; right < s.Length; right++)
            {
                if (seen.Add(s[right]))
                {
                    currentLength++;
                }
                else
                {
                    while (left < right && s[left] != s[right])
                    {
                        seen.Remove(s[left]);
                        left++;
                        currentLength--;
                    }

                    left++;
                }

                maxLength = Math.Max(maxLength, currentLength);
            }

            Assert.Equal(expected, maxLength);
        }
    }
}