﻿using static System.Runtime.InteropServices.JavaScript.JSType;

namespace leetcode.Lists.Top150
{
    public class Others
    {
        [Trait("Difficulty", "Medium")]
        public class Medium
        {
            /// <summary>
            /// 253. Meeting Rooms II
            /// Given an array of meeting time intervals intervals where intervals[i] = [starti, endi], return the minimum number of conference rooms required.
            /// </summary>
            /// <see cref="https://leetcode.com/problems/meeting-rooms-ii"/>
            [Theory]
            [InlineData("[[0,30],[5,10],[15,20]]", 2)]
            [InlineData("[[7,10],[2,4]]", 1)]
            [InlineData("[[0,1],[1,2],[2,3]]", 1)]
            [InlineData("[[0,2],[1,3],[2,4]]", 2)]
            [InlineData("[[0,2],[1,4],[2,3],[3,5],[5,6]]", 2)]
            public void MinMeetingRooms(string input, int expected)
            {
                // -
                int MinHeap(int[][] intervals)
                {
                    if (intervals == null || intervals.Length < 1) return 0;

                    Array.Sort(intervals, Comparer<int[]>.Create((a, b) => a[0] - b[0]));

                    PriorityQueue<int, int> minHeap = new();
                    for (int i = 0; i < intervals.Length; i++)
                    {
                        int curStart = intervals[i][0];
                        int curEnd = intervals[i][1];

                        if (!minHeap.TryPeek(out int previousStart, out int previousEnd) || curEnd < previousEnd || curStart < previousEnd)
                        {
                            minHeap.Enqueue(curStart, curEnd);
                        }
                        else
                        {
                            minHeap.DequeueEnqueue(curStart, curEnd);
                        }
                    }

                    return minHeap.Count;
                }
                // -
                foreach (Func<int[][], int> solution in new[] { MinHeap, })
                {
                    int[][] intervals = input.Parse2DArray(int.Parse).Select(a => a.ToArray()).ToArray();

                    int actual = solution.Invoke(intervals);

                    Assert.Equal(expected, actual);
                }
            }
        }

        // Given a signed 32-bit integer x, return x with its digits reversed. If reversing x causes the value to go outside the signed 32-bit integer range [-231, 231 - 1], then return 0.
        // Assume the environment does not allow you to store 64-bit integers(signed or unsigned).
        [Trait("Company", "Amazon")]
        [Theory]
        [InlineData(123, 321)]
        [InlineData(-123, -321)]
        [InlineData(120, 21)]
        [InlineData(int.MinValue, 0)]
        [InlineData(int.MaxValue, 0)]
        [InlineData(1056389759, 0)]
        public void Reverse(int x, int expected)
        {
            int actual = 0;

            while (x != 0)
            {
                int previous = actual;

                actual = 10 * actual + x % 10;
                x = x / 10;

                if (Math.Abs(previous) > Math.Abs(actual / 10))
                {
                    actual = 0;
                    break;
                }
            }
            Assert.Equal(expected, actual);
        }

        // 763. Partition Labels
        // You are given a string s.We want to partition the string into as many parts as possible so that each letter appears in at most one part.
        // Note that the partition is done so that after concatenating all the parts in order, the resultant string should be s.
        // Return a list of integers representing the size of these parts.
        [Trait("Company", "Amazon")]
        [Theory]
        [InlineData("ababcbacadefegdehijhklij", "[9,7,8]")]
        [InlineData("eccbbbbdec", "[10]")]
        public void PartitionLabels(string s, string output)
        {
            IList<int> expected = output.ParseArrayStringLC(int.Parse).ToList();

            Dictionary<char, int[]> map = [];

            // Figure out all intervals containing the same letter
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                if (!map.TryGetValue(c, out int[]? value))
                {
                    value = [i, i];
                    map[c] = value;
                }
                else
                {
                    map[c] = [value[0], i];
                }
            }

            // Sort intervals
            int[][] intervals = map.Values.ToArray();
            var comparer = Comparer<int[]>.Create(new Comparison<int[]>((left, right) => Comparer<int>.Default.Compare(left[0], right[0])));
            Array.Sort(intervals, comparer);

            // Merge intervals
            List<int> actual = [];
            int[] max = intervals[0];
            for (int i = 1; i < intervals.Length; i++)
            {
                int[] curr = intervals[i];

                if (curr[0] > max[1])
                {
                    // If the current interval begins after the previous max, we found a non-overlapping interval
                    actual.Add(max[1] - max[0] + 1);
                    max = curr;
                }
                else
                {
                    // We need to increase the max interval size due to overlap
                    max[0] = Math.Min(max[0], curr[0]);
                    max[1] = Math.Max(max[1], curr[1]);
                }
            }

            // Add the final interval
            actual.Add(max[1] - max[0] + 1);

            Assert.Equal(expected, actual);
        }
    }
}
