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

        [Trait("Difficulty", "Hard")]
        public class Hard
        {
            private class TrieBag
            {
                class TrieBagNode
                {
                    public readonly Dictionary<char, TrieBagNode> children = new();
                    public int terminalCount = 0;
                }

                private readonly TrieBagNode root = new();
                public int wordCount = 0;

                public int AddSubstring(string text, int start, int length)
                {
                    var curr = root;
                    for (int i = 0; i < length; i++)
                    {
                        char c = text[start + i];
                        if (!curr.children.TryGetValue(c, out TrieBagNode next))
                        {
                            next = new TrieBagNode();
                            curr.children.Add(c, next);
                        }

                        curr = next;
                    }

                    curr.terminalCount++;
                    wordCount++;

                    return wordCount;
                }

                public int Add(string word)
                {
                    return AddSubstring(word, 0, word.Length);
                }

                public bool RemoveSubstring(string text, int start, int length)
                {
                    // We don't really need to remove the word, just uncheck the terminator (which indicates the word is there).
                    // This is actually better, because the next time the word is added, we just flip the flag back to true, no memory churn.
                    var curr = root;
                    for (int i = 0; i < length; i++)
                    {
                        var c = text[start + i];

                        if (!curr.children.TryGetValue(c, out TrieBagNode next))
                        {
                            return false;
                        }

                        curr = next;
                    }

                    if (curr.terminalCount == 0) return false;

                    curr.terminalCount--;
                    wordCount--;

                    return true;
                }

                public bool Remove(string word)
                {
                    return RemoveSubstring(word, 0, word.Length);
                }

                public bool HasSubstring(string text, int start, int length)
                {
                    var curr = root;
                    for (int i = 0; i < length; i++)
                    {
                        var c = text[start + i];

                        if (!curr.children.TryGetValue(c, out TrieBagNode next))
                        {
                            return false;
                        }

                        curr = next;
                    }

                    return curr.terminalCount > 0;
                }

                public bool Has(string word)
                {
                    return HasSubstring(word, 0, word.Length);
                }
            }

            /// <summary>
            /// 30. Substring with Concatenation of All Words
            /// You are given a string s and an array of strings words. All the strings of words are of the same length.
            /// A concatenated string is a string that exactly contains all the strings of any permutation of words concatenated.
            /// For example, if words = ["ab", "cd", "ef"], then "abcdef", "abefcd", "cdabef", "cdefab", "efabcd", and "efcdab" are all concatenated strings. "acdbef" is not a concatenated string because it is not the concatenation of any permutation of words.
            /// Return an array of the starting indices of all the concatenated substrings in s.You can return the answer in any order.
            /// </summary>
            /// <see cref="https://leetcode.com/problems/substring-with-concatenation-of-all-words/"/>
            [Trait("List", "TopInterview150")]
            [Theory]
            [InlineData("barfoothefoobarman", new string[] { "foo", "bar" }, new int[] { 0, 9 })]
            [InlineData("wordgoodgoodgoodbestword", new string[] { "word", "good", "best", "word" }, new int[] { })]
            [InlineData("barfoofoobarthefoobarman", new string[] { "bar", "foo", "the" }, new int[] { 6, 9, 12 })]
            [InlineData("aaa", new string[] { "a", "a" }, new int[] { 0, 1 })]
            public void SubstringWithConcatenationOfAllWords(string s, string[] words, int[] expected)
            {
                static int FindOneSubstring(TrieBag trie, string s, int start, int end, int length)
                {
                    int result = -1;
                    var removedWords = new List<Tuple<int, int>>();
                    for (int i = start; i < end; i += length)
                    {
                        if (!trie.RemoveSubstring(s, i, length)) break;

                        removedWords.Add(new Tuple<int, int>(i, length));

                        if (trie.wordCount == 0)
                        {
                            result = start;
                            break;
                        }
                    }

                    foreach (var removedWord in removedWords)
                    {
                        trie.AddSubstring(s, removedWord.Item1, removedWord.Item2);
                    }

                    return result;
                }

                static IList<int> FindSubstring(string s, string[] words)
                {
                    var trie = new TrieBag();
                    foreach (var word in words) trie.Add(word);

                    List<int> result = new();
                    int length = words[0].Length;
                    int permLength = words.Length * length;
                    int limit = s.Length - permLength + 1;

                    for (int i = 0; i < limit; i++)
                    {
                        var index = FindOneSubstring(trie, s, i, i + words.Length * length, length);

                        if (index > -1)
                        {
                            i = index;
                            result.Add(index);
                        }
                    }

                    return result;
                }

                var actual = FindSubstring(s, words);

                Assert.Equal(actual, expected);
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