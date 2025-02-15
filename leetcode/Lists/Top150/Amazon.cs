namespace leetcode.Lists.Top150
{
    public class Amazon
    {
        // Fruit crush
        // Given a list of fruits (numbers), make pairs of different fruits until you can no longer do that
        // Return how many fruit left
        // Example 1: [1,2,3] => pair (1,2), 3 is left, result = 1
        // Example 2: [1,1,2,3] => pair (1,2), (1,3), none left, result = 0
        // Example 3: [1,1,2,2,3,3] => pair (1,2),(2,3),(1,3), none left, result = 0
        // Example 4: [1,1,1,1,1,2] => pair (1,2), 4 left, result = 4
        [Trait("Company", "Amazon")]
        [Theory]
        [InlineData("[3,3,1,1,2]", 1)]
        [InlineData("[1,1,2,2,3,3]", 0)]
        [InlineData("[1,2,6,7]", 0)]
        [InlineData("[1,1,1,1,1,2,3,4,5,6,7,8,9,10]", 0)]
        [InlineData("[5,5,5,5,5,4,4,4,4,3,3,3,2,2,1]", 1)]
        [InlineData("[5,5,5,5,5,4,4,4,4,1]", 0)]
        public void FruitCrush(string input, int expected)
        {
            int[] fruits = input.ParseArrayStringLC(int.Parse).ToArray();

            Dictionary<int, int> map = [];
            foreach (int fruit in fruits)
            {
                if (map.ContainsKey(fruit))
                {
                    map[fruit]++;
                }
                else
                {
                    map[fruit] = 1;
                }
            }

            int[] pairs = map.Values.ToArray();

            Comparison<int> comparer = new((a, b) => b - a);
            for (int tail = pairs.Length - 1; tail > 0;)
            {
                Array.Sort(pairs, comparer);

                if (tail > 1 && pairs[1] > pairs[2])
                {
                    int diff = pairs[1] - pairs[2];
                    if ((pairs[0] -= diff) == 0) tail--;
                    if ((pairs[1] -= diff) == 0) tail--;
                }
                else
                {
                    if (--pairs[0] == 0) tail--;
                    if (--pairs[1] == 0) tail--;
                }
            }

            //Comparison<int> comparer = new((a, b) => b - a);
            //for (int tail = pairs.Length - 1; tail > 0;)
            //{
            //    Array.Sort(pairs, comparer);

            //    if (--pairs[0] == 0) tail--;
            //    if (--pairs[1] == 0) tail--;
            //}

            int actual = pairs.Sum();

            Assert.Equal(expected, actual);
        }
    }
}
