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

        [Theory]
        [InlineData(new int[] { 1, 1, 1, 2, 2, 3 }, 5, new int[] { 1, 1, 2, 2, 3 })]
        [InlineData(new int[] { 0, 0, 1, 1, 1, 1, 1, 2, 3, 3 }, 7, new int[] { 0, 0, 1, 1, 2, 3, 3 })]
        [InlineData(new int[] { 1, 1, 2 }, 3, new int[] { 1, 1, 2 })]
        [InlineData(new int[] { 0, 0, 1, 1, 1, 2, 2, 3, 3, 4 }, 9, new int[] { 0, 0, 1, 1, 2, 2, 3, 3, 4 })]
        [InlineData(new int[] { }, 0, new int[] { })]
        [InlineData(new int[] { 1 }, 1, new int[] { 1 })]
        public void RemoveDuplicates2(int[] nums, int expCount, int[] expNums)
        {
            int t = nums.Length;
            int i = 0;

            bool canRepeat = true;
            for (int j = i + 1; j < t; j++)
            {
                if (nums[i] == nums[j])
                {
                    if (canRepeat)
                    {
                        nums[++i] = nums[j];
                        canRepeat = false;
                    }
                }
                else
                {
                    nums[++i] = nums[j];
                    canRepeat = true;
                }
            }

            if (t > 0)
            {
                i++;
            }

            Assert.Equal(expCount, i);
            Assert.True(expNums.SequenceEqual(nums.Take(i).ToArray()));
        }

        [Theory]
        [InlineData(new int[] { 3, 2, 3 }, 3)]
        [InlineData(new int[] { 2, 2, 1, 1, 1, 2, 2 }, 2)]
        public void MajorityElement(int[] nums, int expected)
        {
            // The question assumes the majority number *always* exists, so basically we just need to find the number with highest occurrences
            Dictionary<int, int> counts = new();
            int maxCount = int.MinValue;
            int maxNumber = int.MinValue;

            foreach (int i in nums)
            {
                if (!counts.ContainsKey(i))
                {
                    counts[i] = 1;

                    if (1 > maxCount)
                    {
                        maxCount = 1;
                        maxNumber = i;
                    }
                }
                else
                {
                    int newCount = ++counts[i];

                    if (newCount > maxCount)
                    {
                        maxCount = newCount;
                        maxNumber = i;
                    }
                }
            }

            Assert.Equal(expected, maxNumber);
        }

        [Theory]
        [InlineData(new int[] { 1, 2, 3, 4, 5, 6, 7 }, 3, new int[] { 5, 6, 7, 1, 2, 3, 4 })]
        [InlineData(new int[] { -1, -100, 3, 99 }, 2, new int[] { 3, 99, -1, -100 })]
        public void Rotate(int[] nums, int k, int[] expected)
        {
            var reverse = delegate (int[] array, int start, int end)
            {
                int l = (end - start + 1) / 2;

                for (int i = 0; i < l; i++)
                {
                    int from = start + i;
                    int to = end - i;

                    array[from] ^= array[to];
                    array[to] ^= array[from];
                    array[from] ^= array[to];
                }
            };

            k %= nums.Length;
            int l = nums.Length - 1;

            reverse(nums, 0, l - k);
            reverse(nums, l - k + 1, l);
            reverse(nums, 0, l);

            Assert.True(expected.SequenceEqual(nums));
        }

        [Theory]
        [InlineData(new int[] { 7, 1, 5, 3, 6, 4 }, 5)]
        [InlineData(new int[] { 7, 6, 4, 3, 1 }, 0)]
        public void MaxProfit(int[] prices, int expected)
        {
            int maxProfit = 0;

            if (prices.Length > 1)
            {
                int minPrice = prices[0];
                for (int i = 1; i < prices.Length; i++)
                {
                    if (prices[i] < minPrice)
                    {
                        minPrice = prices[i];
                    }
                    else if (prices[i] - minPrice > maxProfit)
                    {
                        maxProfit = prices[i] - minPrice;
                    }
                }
            }

            Assert.Equal(expected, maxProfit);
        }

        [Theory]
        [InlineData(new int[] { 7, 1, 5, 3, 6, 4 }, 7)]
        [InlineData(new int[] { 1, 2, 3, 4, 5 }, 4)]
        [InlineData(new int[] { 7, 6, 4, 3, 1 }, 0)]
        public void MaxProfit2(int[] prices, int expected)
        {
            int totalProfit = 0;

            if (prices.Length > 1)
            {
                for (int i = 1; i < prices.Length; i++)
                {
                    if (prices[i] > prices[i - 1])
                    {
                        totalProfit += prices[i] - prices[i - 1];
                    }
                }
            }

            Assert.Equal(expected, totalProfit);
        }

        [Theory]
        [InlineData(new[] { 2, 3, 1, 1, 4 }, true)]
        [InlineData(new[] { 3, 2, 1, 0, 4 }, false)]
        [InlineData(new[] { 2, 0 }, true)]
        [InlineData(new[] { 0 }, true)]
        public void CanJump(int[] nums, bool expected)
        {
            HashSet<int> visited = new() { 0 };
            int target = nums.Length - 1;

            bool canJump = target == 0;
            while (!canJump)
            {
                HashSet<int> visitedNext = new();

                foreach(int index in visited)
                {
                    for (int i = 1; i <= nums[index]; i++)
                    {
                        int next = index + i;

                        if (next == target)
                        {
                            canJump = true;
                            break;
                        }
                        if (next > target || visited.Contains(next))
                        {
                            continue;
                        }
                        else
                        {
                            visitedNext.Add(next);
                        }
                    }

                    if (canJump) break;
                }

                if (visitedNext.Count == 0)
                {
                    break;
                }

                visited = visitedNext;
            }

            //HashSet<int> visited = new() { 0 };

            //int target = nums.Length - 1;

            //bool canJump = nums.Length == 1;
            //for (int i = 0; !canJump && i < nums.Length; i++)
            //{
            //    if (!visited.Contains(i))
            //    {
            //        continue;
            //    }

            //    for (int j = 1; j <= nums[i]; j++)
            //    {
            //        int next = i + j;

            //        if (visited.Contains(next))
            //        {
            //            continue;
            //        }
            //        else if (next == target)
            //        {
            //            canJump = true;
            //            break;
            //        }
            //        else if (next < target)
            //        {
            //            visited.Add(next);
            //        }
            //    }
            //}
            //bool[] visited = new bool[nums.Length];
            //visited[0] = true;
            //int target = nums.Length - 1;
            //bool canJump = nums.Length == 1;
            //for (int i = 0; !canJump && i < nums.Length; i++)
            //{
            //    if (!visited[i])
            //    {
            //        continue;
            //    }

            //    for (int j = 1; j <= nums[i]; j++)
            //    {
            //        int next = i + j;

            //        if (next < visited.Length && visited[next])
            //        {
            //            continue;
            //        }
            //        else if (next == target)
            //        {
            //            canJump = true;
            //            break;
            //        }
            //        else if (next < target)
            //        {
            //            visited[next] = true;
            //        }
            //    }
            //}

            Assert.Equal(expected, canJump);
        }
    }
}