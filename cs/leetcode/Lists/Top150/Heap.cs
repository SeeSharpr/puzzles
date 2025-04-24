using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Collections;
using System.Net.Quic;
using System.Runtime.Intrinsics.X86;

namespace leetcode.Lists.Top150
{
    public class Heap
    {
        [Trait("Difficulty", "Hard")]
        public class Hard
        {
            public class MedianFinder
            {
                private readonly PriorityQueue<int, int> lowerHeap = new(Comparer<int>.Create((a, b) => b - a));
                private readonly PriorityQueue<int, int> upperHeap = new();

                public MedianFinder()
                {
                }

                public void AddNum(int num)
                {
                    if (lowerHeap.Count < upperHeap.Count)
                    {
                        int minValue = upperHeap.EnqueueDequeue(num, num);
                        lowerHeap.Enqueue(minValue, minValue);
                    }
                    else if (upperHeap.Count < lowerHeap.Count)
                    {
                        int maxValue = lowerHeap.EnqueueDequeue(num, num);
                        upperHeap.Enqueue(maxValue, maxValue);
                    }
                    else if (upperHeap.TryPeek(out int _, out int minValue) && num > minValue)
                    {
                        upperHeap.Enqueue(num, num);
                    }
                    else
                    {
                        lowerHeap.Enqueue(num, num);
                    }
                }

                public double FindMedian()
                {
                    if (lowerHeap.Count < upperHeap.Count)
                    {
                        return upperHeap.Peek();
                    }
                    else if (upperHeap.Count < lowerHeap.Count)
                    {
                        return lowerHeap.Peek();
                    }
                    else
                    {
                        return (lowerHeap.Peek() + upperHeap.Peek()) / 2.0;
                    }
                }
            }

            /// <summary>
            /// 295. Find Median from Data Stream
            /// The median is the middle value in an ordered integer list.If the size of the list is even, there is no middle value, and the median is the mean of the two middle values.
            /// For example, for arr = [2, 3, 4], the median is 3.
            /// For example, for arr = [2, 3], the median is (2 + 3) / 2 = 2.5.
            /// Implement the MedianFinder class:
            /// MedianFinder() initializes the MedianFinder object.
            /// void addNum(int num) adds the integer num from the data stream to the data structure.
            /// double findMedian() returns the median of all elements so far.Answers within 10-5 of the actual answer will be accepted.
            /// </summary>
            /// <see cref="https://leetcode.com/problems/find-median-from-data-stream"/>
            [Theory]
            [InlineData("[MedianFinder,addNum,addNum,findMedian,addNum,findMedian]", "[[], [1], [2], [], [3], []]", "[null, null, null, 1.5, null, 2.0]")]
            public void MedianFinderTest(string actionInput, string argsInput, string output)
            {
                string[] actions = actionInput.Parse1DArray().ToArray();
                string[][] args = argsInput.Parse2DArray().Select(x => x.ToArray()).ToArray();
                string[] expected = output.Parse1DArray().ToArray();

                MedianFinder? finder = null;
                for (int i = 0; i < actions.Length; i++)
                {
                    switch (actions[i])
                    {
                        case "MedianFinder":
                            finder = new MedianFinder();
                            break;
                        case "addNum":
                            finder!.AddNum(int.Parse(args[i][0]));
                            break;
                        case "findMedian":
                            Assert.Equal(double.Parse(expected[i]), finder!.FindMedian());
                            break;
                        default:
                            throw new NotSupportedException(actions[i]);
                    }
                }
            }

            [Theory]
            [InlineData("[1,3,-1,-3,5,3,6,7]", 3, "[3,3,5,5,6,7]")]
            [InlineData("[1]", 1, "[1]")]
            public void MaxSlidingWindow(string input, int k, string output)
            {
                int[] nums = input.Parse1DArray(int.Parse).ToArray();
                int[] expected = output.Parse1DArray(int.Parse).ToArray();

                static void AddToBag(SortedDictionary<int, int> bag, int value)
                {
                    bag[value] = bag.TryGetValue(value, out var count) ? count + 1 : 1;
                }

                static void RemoveFromBag(SortedDictionary<int, int> bag, int value)
                {
                    if (--bag[value] < 1) bag.Remove(value);
                }

                static int[] SortedBag(int[] nums, int k)
                {
                    if (nums == null || nums.Length == 0) return [];

                    List<int> result = [];
                    SortedDictionary<int, int> bag = new(Comparer<int>.Create((a, b) => b - a));

                    // Add the first K elements
                    for (int i = 0; i < k; i++)
                    {
                        AddToBag(bag, nums[i]);
                    }

                    // Iffy...
                    result.Add(bag.Keys.ElementAt(0));

                    for (int i = 1; i <= nums.Length - k; i++)
                    {
                        RemoveFromBag(bag, nums[i - 1]);
                        AddToBag(bag, nums[i - 1 + k]);
                        result.Add(bag.Keys.ElementAt(0));
                    }

                    return result.ToArray();
                }

                static int[] MonotonicQueue(int[] nums, int k)
                {
                    if (nums == null || nums.Length == 0) return [];

                    List<int> result = [];
                    LinkedList<int> dq = new();

                    for (int i = 0; i < k; i++)
                    {
                        while (dq.Count > 0 && nums[i] >= nums[dq.Last!.Value]) dq.RemoveLast();
                        dq.AddLast(i);
                    }

                    result.Add(nums[dq.First!.Value]);

                    for (int i = k; i < nums.Length; i++)
                    {
                        if (dq.First!.Value == i - k) dq.RemoveFirst();
                        while (dq.Count > 0 && nums[i] >= nums[dq.Last!.Value]) dq.RemoveLast();
                        dq.AddLast(i);
                        result.Add(nums[dq.First!.Value]);
                    }


                    return result.ToArray();
                }

                foreach (Func<int[], int, int[]> solution in new[] { SortedBag, MonotonicQueue,  })
                {
                    int[] actual = solution.Invoke(nums, k);

                    Assert.Equal(expected, actual);
                }
            }
        }
    }
}
