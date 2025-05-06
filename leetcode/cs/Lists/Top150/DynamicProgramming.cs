using leetcode.Types.Trie;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Xunit.Sdk;

namespace leetcode.Lists.Top150
{
    public class DynamicProgramming
    {
        [Trait("Difficulty", "Easy")]
        public class Easy
        {
            /// <summary>
            /// 70. Climbing Stairs
            /// You are climbing a staircase. It takes n steps to reach the top.
            /// Each time you can either climb 1 or 2 steps.In how many distinct ways can you climb to the top?
            /// </summary>
            /// <see cref="https://leetcode.com/problems/climbing-stairs/description/?envType=study-plan-v2&envId=dynamic-programming"/>
            [Theory]
            [InlineData(2, 2)]
            [InlineData(3, 3)]
            public void ClimbStairs(int n, int expected)
            {
                int[] dp = new int[n];
                if (n > 0) dp[0] = 1;
                if (n > 1) dp[1] = 2;

                for (int i = 2; i < n; i++)
                {
                    dp[i] = dp[i - 1] + dp[i - 2];
                }

                int actual = dp[dp.Length - 1];

                Assert.Equal(expected, actual);
            }

            /// <summary>
            /// 509. Fibonacci Number
            /// The Fibonacci numbers, commonly denoted F(n) form a sequence, called the Fibonacci sequence, such that each number is the sum of the two preceding ones, starting from 0 and 1. That is,
            /// F(0) = 0, F(1) = 1
            /// F(n) = F(n - 1) + F(n - 2), for n > 1.
            /// Given n, calculate F(n).
            /// </summary>
            /// <see cref="https://leetcode.com/problems/fibonacci-number/description/?envType=study-plan-v2&envId=dynamic-programming"/>
            [Theory]
            [InlineData(2, 1)]
            [InlineData(3, 2)]
            [InlineData(4, 3)]
            [InlineData(5, 5)]
            [InlineData(6, 8)]
            public void Fib(int n, int expected)
            {
                int fib2Behind = 0;
                int fib1Behind = 1;
                int actual = 0;

                if (n == 0)
                {
                    actual = fib2Behind;
                }
                else if (n == 1)
                {
                    actual = fib1Behind;
                }
                else
                {
                    actual = fib2Behind + fib1Behind;

                    for (int i = 2; i <= n; i++)
                    {
                        actual = fib2Behind + fib1Behind;
                        fib2Behind = fib1Behind;
                        fib1Behind = actual;
                    }
                }

                Assert.Equal(expected, actual);
            }

            /// <summary>
            /// 746. Min Cost Climbing Stairs
            /// You are given an integer array cost where cost[i] is the cost of ith step on a staircase.Once you pay the cost, you can either climb one or two steps.
            /// You can either start from the step with index 0, or the step with index 1.
            /// Return the minimum cost to reach the top of the floor.
            /// </summary>
            /// <see cref="https://leetcode.com/problems/min-cost-climbing-stairs/description/?envType=study-plan-v2&envId=dynamic-programming"/>
            [Theory]
            [InlineData("[10,15,20]", 15)]
            [InlineData("[1,100,1,1,1,100,1,1,100,1]", 6)]
            public void MinCostClimbingStairs(string input, int expected)
            {
                int[] cost = input.Parse1DArray(int.Parse).ToArray();

                Dictionary<int, int> dp = new() { { 0, cost[0] }, { 1, cost[1] }, };

                for (int i = 2; i < cost.Length; i++)
                {
                    int current = cost[i] + Math.Min(dp[i - 2], dp[i - 1]);
                    dp[i] = current;
                }

                int actual = Math.Min(dp[cost.Length - 1], dp[cost.Length - 2]);

                Assert.Equal(expected, actual);
            }


            /// <summary>
            /// 1137. N-th Tribonacci Number
            /// The Tribonacci sequence Tn is defined as follows: 
            /// T0 = 0, T1 = 1, T2 = 1, and Tn+3 = Tn + Tn+1 + Tn+2 for n >= 0.
            /// Given n, return the value of Tn.
            /// </summary>
            /// <param name="n"></param>
            /// <param name="expected"></param>
            [Theory]
            [InlineData(0, 0)]
            [InlineData(1, 1)]
            [InlineData(2, 1)]
            [InlineData(3, 2)]
            [InlineData(4, 4)]
            [InlineData(25, 1389537)]
            public void Tribonacci(int n, int expected)
            {
                int actual = n < 1 ? 0 : n < 3 ? 1 : 2;

                int t1 = 0;
                int t2 = 1;
                int t3 = 1;

                for (int i = 3; i <= n; i++)
                {
                    actual = t1 + t2 + t3;
                    t1 = t2;
                    t2 = t3;
                    t3 = actual;
                }

                Assert.Equal(expected, actual);
            }
        }

        [Trait("Difficulty", "Medium")]
        public class Medium
        {
            /// <summary>
            /// 5. Longest Palindromic Substring
            /// Given a string s, return the longest palindromic substring in s.
            /// </summary>
            /// <see cref="https://leetcode.com/problems/longest-palindromic-substring/description/?envType=study-plan-v2&envId=dynamic-programming"/>
            [Theory]
            [InlineData("babad", "[bab,aba]")]
            [InlineData("cbbd", "[bb]")]
            [InlineData("bb", "[bb]")]
            [InlineData("a", "[a]")]
            [InlineData("aaaaa", "[aaaaa]")]
            public void LongestPalindrome(string s, string output)
            {
                HashSet<string> expected = output.Parse1DArray(x => x).ToHashSet();

                int limit = s.Length - 1;
                int maxStart = 0;
                int maxEnd = 0;

                Queue<Tuple<int, int>> queue = new();

                for (int i = 0; i <= limit;)
                {
                    // Find the longest repeating letter and treat it as the center of a palyndrome
                    int j = i;
                    while (j < limit && s[i] == s[j + 1]) j++;

                    queue.Enqueue(new Tuple<int, int>(i, j));
                    i = j + 1;
                }

                while (queue.TryDequeue(out var tuple))
                {
                    int start = tuple.Item1;
                    int end = tuple.Item2;

                    // Keep track of the longest so far
                    if (end - start > maxEnd - maxStart)
                    {
                        maxStart = start;
                        maxEnd = end;
                    }

                    // Try to expand the palyndrome as far as possible from the center
                    int i = 1;
                    for (; start - i >= 0 && end + i <= limit && s[start - i] == s[end + i]; i++) ;

                    // If we managed to expand (i is one ahead), enqueue the new palyndrome
                    if (--i > 0) queue.Enqueue(new Tuple<int, int>(start - i, end + i));
                }

                string actual = s.Substring(maxStart, maxEnd - maxStart + 1);

                Assert.Contains(actual, expected);
            }

            /// <summary>
            /// 139. Word Break
            /// Given a string s and a dictionary of strings wordDict, return true if s can be segmented into a space-separated sequence of one or more dictionary words.
            /// Note that the same word in the dictionary may be reused multiple times in the segmentation.
            /// </summary>
            /// <see cref="https://leetcode.com/problems/word-break/description/?envType=study-plan-v2&envId=dynamic-programming"/>
            [Theory]
            [InlineData("leetcode", "[leet,code]", true)]
            [InlineData("applepenapple", "[apple,pen]", true)]
            [InlineData("catsandog", "[cats,dog,sand,and,cat]", false)]
            [InlineData("a", "[b]", false)]
            [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaab", "[a,aa,aaa,aaaa,aaaaa,aaaaaa,aaaaaaa,aaaaaaaa,aaaaaaaaa,aaaaaaaaaa]", false)]
            [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "[a,aa,aaa,aaaa,aaaaa,aaaaaa,aaaaaaa,aaaaaaaa,aaaaaaaaa,aaaaaaaaaa]", true)]
            public void WordBreak(string s, string inputWordDict, bool expected)
            {
                IList<string> wordDict = inputWordDict.Parse1DArray(x => x).ToList();

                bool actual = s.Length == 0;

                string solution = "trie";
                switch (solution)
                {
                    case "buffer":
                        SortedDictionary<int, HashSet<string>> l2w = new();
                        foreach (string word in wordDict)
                        {
                            if (!l2w.TryGetValue(word.Length, out HashSet<string>? set))
                            {
                                l2w[word.Length] = set = [];
                            }

                            set.Add(word);
                        }

                        int minLength = l2w.Keys.ElementAt(0);
                        int maxLength = l2w.Keys.ElementAt(l2w.Count - 1);

                        SortedSet<string> queue = new([s]);

                        while (!actual && queue.Count > 0)
                        {
                            string phrase = queue.Min!;
                            queue.Remove(phrase);

                            // Can't even make the minimum length, move on
                            if (phrase.Length < minLength) continue;

                            StringBuilder sb = new(phrase.Substring(0, minLength));

                            for (int index = minLength; index <= maxLength; index++)
                            {
                                // Each time we can find a match, add that to the end of the queue to continue the search
                                if (l2w.TryGetValue(sb.Length, out HashSet<string>? set) && set.Contains(sb.ToString()))
                                {
                                    // We managed to consume the whole string, bail out
                                    if (index == phrase.Length)
                                    {
                                        actual = true;
                                        break;
                                    }
                                    else if (phrase.Length - index >= minLength)
                                    {
                                        // Otherwise continue searching
                                        queue.Add(phrase.Substring(index));
                                    }
                                }

                                // Accept one more letter from the word until we can no longer grow
                                if (index < phrase.Length) sb.Append(phrase[index]);
                            }
                        }
                        break;

                    case "trie":
                        MyNode trie = MyNode.Make(wordDict);

                        bool[] dptrie = new bool[s.Length + 1];
                        dptrie[0] = true;

                        for (int j = 1; !actual && j <= s.Length; j++)
                        {
                            // Ignore places where we can't start from
                            if (dptrie[j - 1] != true) continue;

                            foreach (int next in trie.Matches(s, j - 1))
                            {
                                // If we will end up outside of the string or we already know we can get there, ignore
                                if (next >= s.Length || dptrie[next + 1] == true) continue;

                                // If we got to the end, bail out
                                if (next == s.Length - 1)
                                {
                                    actual = true;
                                    break;
                                }

                                dptrie[next + 1] = true;
                            }
                        }

                        break;

                    case "dp":
                        bool[] dppure = new bool[s.Length + 1];
                        dppure[0] = true;

                        for (int j = 1; !actual && j <= s.Length; j++)
                        {
                            // Ignore places where we can't start from
                            if (dppure[j - 1] != true) continue;

                            foreach (string word in wordDict)
                            {
                                int next = j + word.Length - 1;

                                // If we will end up outside of the string or we already know we can get there, ignore
                                if (next > s.Length || dppure[next] == true) continue;

                                bool match = true;
                                for (int i = 0; i < word.Length; i++)
                                {
                                    if (s[j - 1 + i] != word[i])
                                    {
                                        match = false;
                                        break;
                                    }
                                }

                                if (!match) continue;

                                if (next == s.Length)
                                {
                                    actual = true;
                                    break;
                                }

                                dppure[next] = true;
                            }
                        }

                        break;

                    default:
                        throw new NotImplementedException(solution);
                }

                Assert.Equal(expected, actual);
            }

            /// <summary>
            /// 198. House Robber
            /// You are a professional robber planning to rob houses along a street.
            /// Each house has a certain amount of money stashed, the only constraint stopping you from robbing each of them is that adjacent houses have security systems connected and it will automatically contact the police if two adjacent houses were broken into on the same night.
            /// Given an integer array nums representing the amount of money of each house, return the maximum amount of money you can rob tonight without alerting the police.
            /// </summary>
            /// <see cref="https://leetcode.com/problems/house-robber/description/?envType=study-plan-v2&envId=dynamic-programming"/>
            [Theory]
            [InlineData("[1,2,3,1]", 4)]
            [InlineData("[2,7,9,3,1]", 12)]
            [InlineData("[2,1,1,2]", 4)]
            [InlineData("[1]", 1)]
            [InlineData("[1,2]", 2)]
            public void Rob(string input, int expected)
            {
                foreach (string solution in new string[] { "left2right", "right2left", })
                {
                    int[] nums = input.Parse1DArray(int.Parse).ToArray();

                    int actual;
                    switch (solution)
                    {
                        case "left2right":
                            for (int i = 1; i < nums.Length; i++)
                            {
                                nums[i] = Math.Max(nums[i - 1], nums[i] + (i > 1 ? nums[i - 2] : 0));
                            }

                            actual = nums.Length < 2 ? nums[0] : Math.Max(nums[^1], nums[^2]);

                            break;
                        case "right2left":
                            int[] maxValue = new int[nums.Length + 1];
                            maxValue[^1] = 0;
                            maxValue[^2] = nums[^1];

                            for (int i = nums.Length - 2; i >= 0; i--)
                            {
                                maxValue[i] = Math.Max(maxValue[i + 1], maxValue[i + 2] + nums[i]);
                            }

                            actual = maxValue[0];
                            break;

                        default:
                            throw new NotImplementedException(solution);
                    }

                    Assert.Equal(expected, actual);
                }
            }

            /// <summary>
            /// 300. Longest Increasing Subsequence
            /// Given an integer array nums, return the length of the longest strictly increasing subsequence.
            /// </summary>
            /// <see cref="https://leetcode.com/problems/longest-increasing-subsequence/?envType=study-plan-v2&envId=top-interview-150"/>
            [Theory]
            [InlineData("[10,9,2,5,3,7,101,18]", 4)]
            [InlineData("[0,1,0,3,2,3]", 4)]
            [InlineData("[7,7,7,7,7,7,7]", 1)]
            public void LengthOfLIS(string input, int expected)
            {
                int[] nums = input.Parse1DArray(int.Parse).ToArray();

                static int PureDP(int[] nums)
                {
                    int[] dp = new int[nums.Length];

                    for (int i = 0; i < nums.Length; i++)
                    {
                        dp[i] = 1;

                        for (int j = 0; j < i; j++)
                        {
                            if (nums[j] < nums[i])
                            {
                                dp[i] = Math.Max(dp[i], dp[j] + 1);
                            }
                        }
                    }

                    return dp.Max();
                }

                static int Build(int[] nums)
                {
                    if (nums.Length == 0) return 0;

                    List<List<int>> seqs = [];
                    List<List<int>> newSeqs = [];
                    seqs.Add([nums[0]]);

                    for (int i = 1; i < nums.Length; i++)
                    {
                        for (int j = 0; j < seqs.Count; j++)
                        {
                            if (nums[i] > seqs[j][^1])
                            {
                                seqs[j].Add(nums[i]);
                            }
                            else
                            {
                                List<int> newSeq = seqs[j].Where(x => x < nums[i]).ToList();
                                newSeq.Add(nums[i]);
                                newSeqs.Add(newSeq);
                            }
                        }

                        seqs.AddRange(newSeqs);
                        newSeqs.Clear();
                    }

                    return seqs.Select(seq => seq.Count).Max();
                }

                static int Build2(int[] nums)
                {
                    if (nums.Length == 0) return 0;

                    List<int> seq = [nums[0]];

                    for (int i = 1; i < nums.Length; i++)
                    {
                        if (nums[i] > seq[^1])
                        {
                            seq.Add(nums[i]);
                        }
                        else
                        {
                            int j = 0;
                            while (j < seq.Count && seq[j] < nums[i]) j++;
                            seq[j] = nums[i];
                        }
                    }

                    return seq.Count;
                }

                string solution = "build2";

                int actual =
                    solution == "dp" ? PureDP(nums) :
                    solution == "build" ? Build(nums) :
                    solution == "build2" ? Build2(nums) :
                    throw new NotImplementedException(solution);

                Assert.Equal(expected, actual);
            }

            // 322. Coin Change
            // You are given an integer array coins representing coins of different denominations and an integer amount representing a total amount of money.
            // Return the fewest number of coins that you need to make up that amount.If that amount of money cannot be made up by any combination of the coins, return -1.
            // You may assume that you have an infinite number of each kind of coin.
            [Trait("Company", "Amazon")]
            [Theory]
            [InlineData("[1,2,5]", 11, 3)]
            [InlineData("[2]", 3, -1)]
            [InlineData("[1]", 0, 0)]
            [InlineData("[2,5,10,1]", 27, 4)]
            [InlineData("[186,419,83,408]", 6249, 20)]
            public void CoinChange(string input, int amount, int expected)
            {
                int[] coins = input.ParseArrayStringLC(int.Parse).ToArray();

                // Start from the highest value (greedy)
                Array.Sort(coins, new Comparison<int>((a, b) => Comparer<int>.Default.Compare(b, a)));

                // Keep track of all reachable amounts
                int[] amounts = new int[amount + 1];
                Array.Fill(amounts, int.MaxValue);
                amounts[0] = 0;

                foreach (int coin in coins)
                {
                    for (int i = coin; i < amounts.Length; i++)
                    {
                        // Ignore cases where we haven't reached that value before
                        if (amounts[i - coin] == int.MaxValue) continue;

                        // Find out whether it's cheaper to reach that amount with or without the current coin
                        amounts[i] = Math.Min(amounts[i - coin] + 1, amounts[i]);
                    }
                }

                // If we reached the target amount, that will have the minimum number of coins. Otherwise return -1
                int actual = amounts[amount] != int.MaxValue ? amounts[amount] : -1;

                Assert.Equal(expected, actual);
            }

            /// <summary>
            /// 516. Longest Palindromic Subsequence
            /// Given a string s, find the longest palindromic subsequence's length in s.
            /// A subsequence is a sequence that can be derived from another sequence by deleting some or no elements without changing the order of the remaining elements.
            /// </summary>
            /// <see cref="https://leetcode.com/problems/longest-palindromic-subsequence/?envType=study-plan-v2&envId=dynamic-programming"/>

            [Theory]
            [InlineData("bbbab", 4)]
            [InlineData("cbbd", 2)]
            [InlineData("xaxbxcyzbzaz", 5)]
            [InlineData("aabaa", 5)]
            public void LongestPalindromeSubseq(string s, int expected)
            {
                static int Recursive(string s, int from, int to, int[][] memo)
                {
                    if (from > to)
                    {
                        return 0;
                    }

                    if (memo[from][to] > 0) return memo[from][to];

                    if (from == to)
                    {
                        return memo[from][to] = 1;
                    }
                    else if (s[from] == s[to])
                    {
                        return memo[from][to] = 2 + Recursive(s, from + 1, to - 1, memo);
                    }
                    else
                    {
                        return memo[from][to] = Math.Max(
                            Recursive(s, from + 1, to, memo),
                            Recursive(s, from, to - 1, memo));
                    }
                }

                static int InteractiveDP(string s)
                {
                    int n = s.Length;
                    int[][] dp = Enumerable.Range(0, n).Select(_ => new int[n]).ToArray();

                    for (int from = n - 1; from >= 0; from--)
                    {
                        dp[from][from] = 1;
                        for (int to = from + 1; to < n; to++)
                        {
                            if (s[from] == s[to])
                            {
                                dp[from][to] = dp[from + 1][to - 1] + 2;
                            }
                            else
                            {
                                dp[from][to] = Math.Max(dp[from + 1][to], dp[from][to - 1]);
                            }
                        }
                    }

                    return dp[0][n - 1];
                }

                static int OptimizedDP(string s)
                {
                    int n = s.Length;
                    int[] dp = new int[n];
                    int[] dpPrev = new int[n];

                    for (int from = n - 1; from >= 0; from--)
                    {
                        dp[from] = 1;
                        for (int to = from + 1; to < n; to++)
                        {
                            if (s[from] == s[to])
                            {
                                dp[to] = dpPrev[to - 1] + 2;
                            }
                            else
                            {
                                dp[to] = Math.Max(dpPrev[to], dp[to - 1]);
                            }
                        }

                        Array.Copy(dp, dpPrev, dp.Length);
                    }

                    return dp[n - 1];
                }

                foreach (string solution in new string[] { "recursive", "interactive", "optimized" })
                {

                    int actual = solution == "recursive" ? Recursive(s, 0, s.Length - 1, Enumerable.Range(0, s.Length).Select(_ => new int[s.Length]).ToArray()) :
                        solution == "interactive" ? InteractiveDP(s) :
                        solution == "optimized" ? OptimizedDP(s) :
                        throw new NotImplementedException(solution);

                    Assert.Equal(expected, actual);
                }
            }

            /// <summary>
            /// 740. Delete and Earn
            /// You are given an integer array nums.You want to maximize the number of points you get by performing the following operation any number of times:
            /// Pick any nums[i] and delete it to earn nums[i] points.Afterwards, you must delete every element equal to nums[i] - 1 and every element equal to nums[i] + 1.
            /// Return the maximum number of points you can earn by applying the above operation some number of times.
            /// </summary>
            /// <see cref="https://leetcode.com/problems/delete-and-earn/description/?envType=study-plan-v2&envId=dynamic-programming"/>
            [Theory]
            [InlineData("[3,4,2]", 6)]
            [InlineData("[2,2,3,3,3,4]", 9)]
            [InlineData("[1,1,1,2,4,5,5,5,6]", 18)]
            public void DeleteAndEarn(string input, int expected)
            {
                int[] nums = input.Parse1DArray(int.Parse).ToArray();

                Dictionary<int, int> points = new();

                int limit = int.MinValue;
                foreach (int num in nums)
                {
                    points[num] = num + points.GetValueOrDefault(num);
                    limit = Math.Max(limit, num);
                }

                int twoBehind = 0;
                int oneBehind = points.GetValueOrDefault(1);
                for (int i = 2; i <= limit; i++)
                {
                    int temp = oneBehind;
                    oneBehind = Math.Max(oneBehind, twoBehind + points.GetValueOrDefault(i));
                    twoBehind = temp;
                }

                int actual = oneBehind;

                Assert.Equal(expected, actual);
            }

        }
    }
}
