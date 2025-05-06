namespace dcp.DataStructures.Array
{
    public class Medium
    {
        // Problem #0099
        // This problem was asked by Microsoft.
        // Given an unsorted array of integers, find the length of the longest consecutive elements sequence.
        // For example, given[100, 4, 200, 1, 3, 2], the longest consecutive element sequence is [1, 2, 3, 4]. Return its length: 4.
        // Your algorithm should run in O(n) complexity.
        [Theory]
        [InlineData(new int[] { 100, 4, 200, 1, 3, 2 }, 4)]
        [InlineData(new int[] { 1, 3, 5, 7, 2, 4, 6, 8 }, 8)]
        public void LongestSequence(int[] input, int expected)
        {
            Dictionary<int, int> headToTail = [];
            Dictionary<int, int> tailToHead = [];

            int longest = 0;
            foreach (int entry in input)
            {
                if (headToTail.ContainsKey(entry) || tailToHead.ContainsKey(entry))
                {
                    // Ignore duplicates
                    continue;
                }

                int succ = entry + 1;
                int pred = entry - 1;
                int oldHead;
                int oldTail;

                if (tailToHead.TryGetValue(pred, out oldHead))
                {
                    // Extends previous tail
                    tailToHead[entry] = oldHead;
                    tailToHead.Remove(pred);
                    headToTail[oldHead] = entry;

                    // Merge with next sequence
                    if (headToTail.TryGetValue(succ, out oldTail))
                    {
                        headToTail[tailToHead[entry]] = oldTail;
                        tailToHead[oldTail] = oldHead;
                        headToTail.Remove(succ);
                        tailToHead.Remove(entry);
                    }
                    else
                    {
                        oldTail = entry;
                    }
                }
                else if (headToTail.TryGetValue(succ, out oldTail))
                {
                    // Extends next head
                    headToTail[entry] = oldTail;
                    headToTail.Remove(succ);
                    tailToHead[oldTail] = entry;

                    // Merge with previous sequence
                    if (tailToHead.TryGetValue(pred, out oldHead))
                    {
                        tailToHead[headToTail[entry]] = oldHead;
                        headToTail[oldHead] = oldTail;
                        tailToHead.Remove(entry);
                        headToTail.Remove(succ);
                    }
                    else
                    {
                        oldHead = entry;
                    }
                }
                else
                {
                    // Didn't exist before
                    headToTail[entry] = tailToHead[entry] = oldTail = oldHead = entry;
                }

                if (oldTail - oldHead + 1 > longest) longest = oldTail - oldHead + 1;
            }

            Assert.Equal(expected, longest);
        }

        [Theory]
        [InlineData(new int[] { 100, 4, 200, 1, 3, 2 }, 4)]
        [InlineData(new int[] { 1, 3, 5, 7, 2, 4, 6, 8 }, 8)]
        public void LongestSequence_Copilot(int[] input, int expected)
        {
            HashSet<int> numbers = new(input);

            int longest = 0;
            foreach (int number in numbers)
            {
                if (numbers.Contains(number - 1)) continue;

                int streak = 0;
                for (int candidate = number; numbers.Contains(candidate); candidate++, streak++) ;

                if (streak > longest) longest = streak;
            }

            Assert.Equal(expected, longest);
        }
    }
}