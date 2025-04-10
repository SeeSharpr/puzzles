using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Drawing;

namespace leetcode.Lists.Top150
{
    public class SortSearch
    {
        [Trait("Difficulty", "Medium")]
        public class Medium
        {
            // 215. Kth Largest Element in an Array
            // Given an integer array nums and an integer k, return the kth largest element in the array.
            // Note that it is the kth largest element in the sorted order, not the kth distinct element.
            // Can you solve it without sorting?
            [Trait("Company", "Amazon")]
            [Trait("Company", "Meta")]
            [Theory]
            [InlineData("[3,2,1,5,6,4]", 2, 5)]
            [InlineData("[3,2,3,1,2,4,5,5,6]", 4, 4)]
            [InlineData("[1]", 1, 1)]
            public void FindKthLargest(string input, int k, int expected)
            {
                int[] nums = input.ParseArrayStringLC(int.Parse).ToArray();
                // --
                static int SortedBag(int[] nums, int k)
                {
                    SortedDictionary<int, int> topK = new();
                    int totalCount = 0;

                    foreach (int num in nums)
                    {
                        if (totalCount < k)
                        {
                            if (!topK.ContainsKey(num))
                            {
                                topK[num] = 1;
                            }
                            else
                            {
                                topK[num]++;
                            }

                            totalCount++;
                        }
                        else
                        {
                            int min = topK.Keys.First();

                            if (num > min)
                            {
                                topK[min]--;
                                if (topK[min] == 0) topK.Remove(min);

                                if (!topK.ContainsKey(num))
                                {
                                    topK[num] = 1;
                                }
                                else
                                {
                                    topK[num]++;
                                }
                            }
                        }
                    }

                    return topK.Keys.First();
                }

                static void Heapify2(ref int a, ref int b)
                {
                    if (b > a)
                    {
                        a ^= b;
                        b ^= a;
                        a ^= b;
                    }
                }

                static void Heapify3(ref int a, ref int b, ref int c)
                {
                    if (b > c) Heapify2(ref a, ref b);
                    else if (c > b) Heapify2(ref a, ref c);
                }

                static void HeapifyUp(int[] heap, int from, int to)
                {
                    int limit = (to - from + 1) / 2;

                    for (int left = limit; left > 0; left--)
                    {
                        int right = left + 1;
                        int head = left / 2;

                        if (from + right <= to) Heapify3(ref heap[from + head], ref heap[from + left], ref heap[from + right]);
                        else Heapify2(ref heap[from + head], ref heap[from + left]);
                    }
                }

                static int Heap(int[] nums, int k)
                {
                    int[] heap = new int[k + 1];
                    int limit = -1;

                    foreach (var num in nums)
                    {
                        if (limit + 1 < heap.Length)
                        {
                            limit++;
                        }

                        heap[limit] = num;

                        for (int i = 0; i <= limit; i++) HeapifyUp(heap, i, limit);

                    }

                    return heap[k - 1];
                }

                static int PQueue(int[] nums, int k)
                {
                    PriorityQueue<int, int> pq = new(k + 1);

                    foreach (var num in nums)
                    {
                        pq.Enqueue(num, num);

                        if (pq.Count > k) pq.Dequeue();
                    }

                    return pq.Dequeue();
                }

                string solution = nameof(PQueue);
                int actual =
                    solution == nameof(SortedBag) ? SortedBag(nums, k) :
                    solution == nameof(Heap) ? Heap(nums, k) :
                    solution == nameof(PQueue) ? PQueue(nums, k) :
                    throw new NotSupportedException(solution);
                // --
                Assert.Equal(expected, actual);
            }
        }

        // 973. K Closest Points to Origin
        // Given an array of points where points[i] = [xi, yi] represents a point on the X-Y plane and an integer k, return the k closest points to the origin(0, 0).
        // The distance between two points on the X-Y plane is the Euclidean distance(i.e., √(x1 - x2)2 + (y1 - y2)2).
        // You may return the answer in any order.The answer is guaranteed to be unique (except for the order that it is in).
        [Trait("Company", "Amazon")]
        [Theory]
        [InlineData("[1,3|-2,2]", 1, "[-2,2]")]
        [InlineData("[3,3|5,-1|-2,4]]", 2, "[3,3|-2,4]")]
        [InlineData("[0,1|1,0]", 2, "[0,1|1,0]")]
        public void KClosest(string input, int k, string output)
        {
            int[][] points = input.ParseNestedArrayStringLC(int.Parse).Select(e => e.ToArray()).ToArray();
            int[][] expected = output.ParseNestedArrayStringLC(int.Parse).Select(e => e.ToArray()).ToArray();

            SortedDictionary<int, List<int[]>> map = new();

            foreach (int[] pair in points)
            {
                int key = pair[0] * pair[0] + pair[1] * pair[1];

                if (!map.TryGetValue(key, out List<int[]>? list))
                {
                    list = new();
                    map.Add(key, list);
                }

                list.Add(pair);
            }

            List<int[]> result = new();

            foreach (var values in map.Values)
            {
                foreach (int[] point in values)
                {
                    result.Add(point);
                    if (result.Count == k) break;
                }

                if (result.Count == k) break;
            }

            int[][] actual = result.ToArray();

            Assert.Equal(expected.Length, actual.Length);

            string[] expectedStr = expected.Select(e => $"{e[0]}|{e[1]}").ToArray();
            string[] actualStr = actual.Select(e => $"{e[0]}|{e[1]}").ToArray();

            Assert.Equal(expectedStr, actualStr);
        }
    }
}
