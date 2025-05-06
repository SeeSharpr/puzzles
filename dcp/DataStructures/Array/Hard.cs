namespace dcp.DataStructures.Array
{
    public class Hard
    {
        // Problem #0095
        // This problem was asked by Palantir.
        // Given a number represented by a list of digits, find the next greater permutation of a number, in terms of lexicographic ordering. If there is not greater permutation possible, return the permutation with the lowest value/ordering.
        // For example, the list [1, 2, 3] should return [1, 3, 2]. The list [1, 3, 2] should return [2, 1, 3]. The list [3, 2, 1] should return [1, 2, 3].
        // Can you perform the operation without allocating extra memory (disregarding the input memory)?
        [Theory]
        [InlineData(new int[] { 1, 2, 3 }, new int[] { 1, 3, 2 })]
        [InlineData(new int[] { 1, 3, 2 }, new int[] { 2, 1, 3 })]
        [InlineData(new int[] { 3, 2, 1 }, new int[] { 1, 2, 3 })]
        public void NextPermutation(int[] input, int[] expected)
        {
            static int[] NextPermutation(int[] number)
            {
                if (number.Length < 2) return number;

                return number;
            }

            Assert.Equal(expected, NextPermutation(input));
        }
    }
}
