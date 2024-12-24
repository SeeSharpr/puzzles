namespace leetcode.Lists
{
    public class Top150
    {
        [Theory]
        [InlineData(new int[] { 1, 2, 3, 0, 0, 0 }, 3, new int[] { 2, 5, 6 }, 3, new int[] { 1, 2, 2, 3, 5, 6 })]
        [InlineData(new int[] { 1 }, 1, new int[] { }, 0, new int[] { 1 })]
        [InlineData(new int[] { 0 }, 0, new int[] { 1 }, 1, new int[] { 1 })]
        public void Merge(int[] nums1, int m, int[] nums2, int n, int[] expected)
        {
            int i = 0;
            int j = 0;
            int p = 0;
            int t = m + n;
            int[] result = new int[t];

            while (p < t)
            {
                if ((i < m) && (j < n))
                {
                    if (nums1[i] < nums2[j])
                    {
                        result[p++] = nums1[i++];
                    }
                    else
                    {
                        result[p++] = nums2[j++];
                    }
                }
                else if (i < m)
                {
                    result[p++] = nums1[i++];
                }
                else // j < n
                {
                    result[p++] = nums2[j++];
                }
            }

            Array.Copy(result, nums1, t);

            Assert.True(expected.SequenceEqual(nums1));
        }

        [Theory]
        [InlineData(new int[] { 3, 2, 2, 3 }, 3, 2, new int[] { 2, 2 })]
        [InlineData(new int[] { 0, 1, 2, 2, 3, 0, 4, 2 }, 2, 5, new int[] { 0, 1, 3, 0, 4 })]
        public void RemoveElement(int[] nums, int val, int n, int[] expected)
        {
            int i = 0;
            int j = 0;
            int c = 0;
            int t = nums.Length;

            while (j < t)
            {
                if (nums[j] != val)
                {
                    nums[i++] = nums[j++];
                    c++;
                }
                else
                {
                    j++;
                }
            }

            Assert.Equal(n, c);
            Assert.True(expected.SequenceEqual(nums.Take(c).ToArray()));
        }

        [Theory]
        [InlineData(new int[] { 1, 1, 2 }, 2, new int[] { 1, 2 })]
        [InlineData(new int[] { 0, 0, 1, 1, 1, 2, 2, 3, 3, 4 }, 5, new int[] { 0, 1, 2, 3, 4 })]
        [InlineData(new int[] { }, 0, new int[] { })]
        [InlineData(new int[] { 1 }, 1, new int[] { 1 })]
        public void RemoveDuplicates(int[] nums, int expCount, int[] expNums)
        {
            int t = nums.Length;
            int i = 0;

            for (int j = i + 1; j < t; j++)
            {
                if (nums[i] != nums[j])
                {
                    nums[++i] = nums[j];
                }
            }

            if (t > 0)
            {
                i++;
            }

            Assert.Equal(expCount, i);
            Assert.True(expNums.SequenceEqual(nums.Take(i).ToArray()));
        }
    }
}
