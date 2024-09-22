
using System.Collections.Immutable;

namespace dcp.DataStructures.Array
{
    // This problem was recently asked by Google.
    // Given a list of numbers and a number k, return whether any two numbers from the list add up to k.
    // For example, given[10, 15, 3, 7] and k of 17, return true since 10 + 7 is 17.
    // Bonus: Can you do this in one pass?
    // Upgrade to premium and get in-depth solutions to every problem.    
    public class Easy
    {
        [Theory]
        [InlineData(new int[] { 10, 15, 3, 7 }, 17, true)]
        [InlineData(new int[] { }, 1, false)]
        [InlineData(new int[] { 1 }, 1, false)]
        [InlineData(new int[] { 2, 2 }, 1, false)]
        public void SumOfTwo(int[] array, int target, bool expected)
        {
            Assert.Equal(expected, SumOfTwo_BruteForce(array, target));
            Assert.Equal(expected, SumOfTwo_SinglePassWithSortedInput(array.AsEnumerable().ToImmutableSortedSet().ToArray(), target));
        }

        private static bool SumOfTwo_BruteForce(int[] array, int target)
        {
            for (int j = 1; j < array.Length; j++)
            {
                for (int i = 0; i < j; i++)
                {
                    if (array[i] + array[j] == target) return true;
                }
            }

            return false;
        }

        private static bool SumOfTwo_SinglePassWithSortedInput(int[] array, int target)
        {
            HashSet<int> set = new HashSet<int>();

            foreach (int n in array)
            {
                if (set.Contains(target - n))
                {
                    return true; 
                }
                else
                {
                    set.Add(n);
                }
            }

            return false;
        }
    }
}
