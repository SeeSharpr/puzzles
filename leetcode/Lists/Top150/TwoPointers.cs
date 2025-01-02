namespace leetcode.Lists.Top150
{
    public class TwoPointers
    {
        // A phrase is a palindrome if, after converting all uppercase letters into lowercase letters and removing all non-alphanumeric characters, it reads the same forward and backward. Alphanumeric characters include letters and numbers.
        // Given a string s, return true if it is a palindrome, or false otherwise.
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
    }
}
