using System.Text;

namespace leetcode.Lists.Top150
{
    public class Intervals
    {
        // You are given a sorted unique integer array nums.
        // A range[a, b] is the set of all integers from a to b(inclusive).
        // Return the smallest sorted list of ranges that cover all the numbers in the array exactly.That is, each element of nums is covered by exactly one of the ranges, and there is no integer x such that x is in one of the ranges but not in nums.
        // Each range[a, b] in the list should be output as:
        // "a->b" if a != b
        // "a" if a == b
        [Theory]
        [InlineData("0, 1, 2, 4, 5, 7", "0->2, 4->5, 7")]
        [InlineData("0, 2, 3, 4, 6, 8, 9", "0, 2->4, 6, 8->9")]
        public void SummaryRanges(string input, string output)
        {
            int[] nums = input.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(int.Parse).ToArray();
            IList<string> expected = output.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();

            List<string> result = [];
            StringBuilder interval = new();

            if (nums.Length > 0)
            {
                int left = nums[0];
                interval.Append(left);

                for (int i = 1; i < nums.Length; i++)
                {
                    if (nums[i] != nums[i - 1] + 1)
                    {
                        if (nums[i - 1] != left) interval.Append($"->{nums[i - 1]}");

                        result.Add(interval.ToString());
                        left = nums[i];
                        interval.Clear();
                        interval.Append(left);
                    }
                }

                if (nums[nums.Length - 1] != left) interval.Append($"->{nums[nums.Length - 1]}");

                result.Add(interval.ToString());
            }

            Assert.True(expected.SequenceEqual(result));
        }
    }
}
