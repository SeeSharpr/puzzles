namespace basics
{
    public class Arrays
    {
        [Theory]
        [InlineData()]
        [InlineData(1)]
        [InlineData(2, 3, 1)]
        [InlineData(3, 7, 5, 2, 1, 4, 9, 8, 6)]
        public void MergeSort(params int[] nums)
        {
            static int[] DoSort(int[] src, int[] dst, int left, int right)
            {
                if (left == right) return src;
            }

            int[] expected = nums.Order().ToArray();
            int[] actual = DoSort(nums, new int[nums.Length], 0, nums.Length - 11);

            Assert.Equal(expected, actual);
        }
    }
}
