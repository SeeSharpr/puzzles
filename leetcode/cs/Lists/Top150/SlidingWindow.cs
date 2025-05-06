namespace leetcode.Lists.Top150
{
    public class SlidingWindow
    {
        [Trait("Difficulty", "Medium")]
        public class Medium
        {

            /// <summary>
            /// 3. Longest Substring Without Repeating Characters
            /// Given a string s, find the length of the longest substring without repeating characters.
            /// </summary>
            /// <see cref="https://leetcode.com/problems/longest-substring-without-repeating-characters/description/"/>
            [Trait("List", "TopInterview150")]
            [Trait("Company", "Meta")]
            [Theory]
            [InlineData("abcabcbb", 3)]
            [InlineData("bbbbb", 1)]
            [InlineData("pwwkew", 3)]
            [InlineData("aab", 2)]
            [InlineData(" ", 1)]
            [InlineData("", 0)]
            [InlineData("abba", 2)]
            public void LengthOfLongestSubstring(string s, int expected)
            {
                static int WithHashSet(string s)
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

                    return maxLength;
                }

                static int WithArray(string s)
                {
                    char rangeBottom = ' ';
                    char rangeTop = (char)127;
                    bool[] seen = new bool[rangeTop - rangeBottom + 1];

                    int max = 0;
                    for (int left = 0, right = 0; right < s.Length; right++)
                    {
                        int ri = s[right] - rangeBottom;

                        if (seen[ri])
                        {
                            while (left < right && s[left] != s[right])
                            {
                                int li = s[left] - rangeBottom;
                                seen[li] = false;
                                left++;
                            }

                            left++;
                        }

                        seen[ri] = true;
                        max = Math.Max(max, right - left + 1);
                    }

                    return max;
                }

                static int KeepIndex(string s)
                {
                    char rangeBottom = ' ';
                    char rangeTop = (char)127;
                    int[] seen = new int[rangeTop - rangeBottom + 1];
                    Array.Fill(seen, -1);

                    int max = 0;
                    for (int left = 0, right = 0; right < s.Length; right++)
                    {
                        int ri = s[right] - rangeBottom;

                        if (seen[ri] != -1)
                        {
                            int limit = seen[ri] + 1;

                            //for (int i = left; i < limit; i++)
                            //{
                            //    int li = s[i] - rangeBottom;
                            //    seen[li] = -1;
                            //}

                            //left = limit;
                            left = Math.Max(left, limit);
                        }

                        seen[ri] = right;
                        max = Math.Max(max, right - left + 1);
                    }

                    return max;
                }

                string solution = nameof(KeepIndex);
                int actual =
                    solution == nameof(WithHashSet) ? WithHashSet(s) :
                    solution == nameof(WithArray) ? WithArray(s) :
                    solution == nameof(KeepIndex) ? KeepIndex(s) :
                    throw new NotSupportedException(solution);

                Assert.Equal(expected, actual);
            }

            /// <summary>
            /// 340. Longest Substring with At Most K Distinct Characters
            /// Given a string s and an integer k, return the length of the longest substring of s that contains at most k distinct characters.
            /// </summary>
            /// <see cref="https://leetcode.com/problems/longest-substring-with-at-most-k-distinct-characters/description/"/>
            [Theory]
            [InlineData("eceba", 2, 3)]
            [InlineData("aa", 1, 2)]
            [InlineData("a", 0, 0)]
            [InlineData("", 42, 0)]
            public void LengthOfLongestSubstringKDistinct(string s, int k, int expected)
            {
                static int SlidingWindow(string s, int k)
                {
                    if (s.Length == 0 || k == 0) return 0;

                    Dictionary<char, int> bag = [];
                    int maxLen = 0;
                    for (int left = 0, right = 0; right < s.Length; right++)
                    {
                        bag[s[right]] = bag.GetValueOrDefault(s[right], 0) + 1;

                        while (left < right && bag.Count > k)
                        {
                            if (--bag[s[left]] == 0) bag.Remove(s[left]);
                            left++;
                        }

                        maxLen = Math.Max(maxLen, right - left + 1);
                    }
                    return maxLen;
                }

                static int SlidingWindow2(string s, int k)
                {
                    if (s.Length == 0 || k == 0) return 0;

                    Dictionary<char, int> bag = [];
                    int maxLen = 0;
                    for (int right = 0; right < s.Length; right++)
                    {
                        bag[s[right]] = bag.GetValueOrDefault(s[right], 0) + 1;

                        if (bag.Count > k)
                        {
                            if (--bag[s[right - maxLen]] == 0) bag.Remove(s[right - maxLen]);
                        }
                        else
                        {
                            maxLen++;
                        }
                    }

                    return maxLen;
                }

                string solution = nameof(SlidingWindow2);
                int actual =
                    solution == nameof(SlidingWindow) ? SlidingWindow(s, k) :
                    solution == nameof(SlidingWindow2) ? SlidingWindow2(s, k) :
                    throw new NotSupportedException(solution);

                Assert.Equal(expected, actual);
            }
        }

        // Given an array of positive integers nums and a positive integer target, return the minimal length of a subarray whose sum is greater than or equal to target.
        // If there is no such subarray, return 0 instead.
        [Trait("List", "TopInterview150")]
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
    }
}