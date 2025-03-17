using static leetcode.Lists.BitManipulation;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Drawing;

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
