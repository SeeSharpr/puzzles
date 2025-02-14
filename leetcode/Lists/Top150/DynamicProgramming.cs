using static System.Runtime.InteropServices.JavaScript.JSType;

namespace leetcode.Lists.Top150
{
    public class DynamicProgramming
    {
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
    }
}
