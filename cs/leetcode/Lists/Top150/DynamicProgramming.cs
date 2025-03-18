using System.ComponentModel.Design;
using static leetcode.Lists.BitManipulation;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace leetcode.Lists.Top150
{
    public class DynamicProgramming
    {
        /// <summary>
        /// 70. Climbing Stairs
        /// You are climbing a staircase. It takes n steps to reach the top.
        /// Each time you can either climb 1 or 2 steps.In how many distinct ways can you climb to the top?
        /// </summary>
        /// <see cref="https://leetcode.com/problems/climbing-stairs/description/?envType=study-plan-v2&envId=dynamic-programming"/>
        [Trait("Difficulty", "Easy")]
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
        /// 198. House Robber
        /// You are a professional robber planning to rob houses along a street.
        /// Each house has a certain amount of money stashed, the only constraint stopping you from robbing each of them is that adjacent houses have security systems connected and it will automatically contact the police if two adjacent houses were broken into on the same night.
        /// Given an integer array nums representing the amount of money of each house, return the maximum amount of money you can rob tonight without alerting the police.
        /// </summary>
        /// <see cref="https://leetcode.com/problems/house-robber/description/?envType=study-plan-v2&envId=dynamic-programming"/>
        [Trait("Difficulty", "Medium")]
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
        /// 509. Fibonacci Number
        /// The Fibonacci numbers, commonly denoted F(n) form a sequence, called the Fibonacci sequence, such that each number is the sum of the two preceding ones, starting from 0 and 1. That is,
        /// F(0) = 0, F(1) = 1
        /// F(n) = F(n - 1) + F(n - 2), for n > 1.
        /// Given n, calculate F(n).
        /// </summary>
        /// <see cref="https://leetcode.com/problems/fibonacci-number/description/?envType=study-plan-v2&envId=dynamic-programming"/>
        [Trait("Difficulty", "Easy")]
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
        /// 740. Delete and Earn
        /// You are given an integer array nums.You want to maximize the number of points you get by performing the following operation any number of times:
        /// Pick any nums[i] and delete it to earn nums[i] points.Afterwards, you must delete every element equal to nums[i] - 1 and every element equal to nums[i] + 1.
        /// Return the maximum number of points you can earn by applying the above operation some number of times.
        /// </summary>
        /// <see cref="https://leetcode.com/problems/delete-and-earn/description/?envType=study-plan-v2&envId=dynamic-programming"/>
        [Trait("Difficulty", "Medium")]
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

        /// <summary>
        /// 746. Min Cost Climbing Stairs
        /// You are given an integer array cost where cost[i] is the cost of ith step on a staircase.Once you pay the cost, you can either climb one or two steps.
        /// You can either start from the step with index 0, or the step with index 1.
        /// Return the minimum cost to reach the top of the floor.
        /// </summary>
        /// <see cref="https://leetcode.com/problems/min-cost-climbing-stairs/description/?envType=study-plan-v2&envId=dynamic-programming"/>
        [Trait("Difficulty", "Easy")]
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
        [Trait("Difficulty", "Easy")]
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
}
