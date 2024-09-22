using System.Collections.Immutable;

namespace dcp.DataStructures.Array
{
    public class Easy
    {
        // Problem #0001
        // Given a list of numbers and a number k, return whether any two numbers from the list add up to k.
        // For example, given[10, 15, 3, 7] and k of 17, return true since 10 + 7 is 17.
        // Bonus: Can you do this in one pass?
        // Upgrade to premium and get in-depth solutions to every problem.    
        [Theory]
        [InlineData(new int[] { 10, 15, 3, 7 }, 17, true)]
        [InlineData(new int[] { }, 1, false)]
        [InlineData(new int[] { 1 }, 1, false)]
        [InlineData(new int[] { 2, 2 }, 1, false)]
        public void SumOfTwo(int[] array, int target, bool expected)
        {
            static bool SumOfTwo_BruteForce(int[] array, int target)
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

            Assert.Equal(expected, SumOfTwo_BruteForce(array, target));

            static bool SumOfTwo_SinglePassWithSortedInput(int[] array, int target)
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

            Assert.Equal(expected, SumOfTwo_SinglePassWithSortedInput(array.AsEnumerable().ToImmutableSortedSet().ToArray(), target));
        }

        // Problem #0002
        // Given an array of integers, return a new array such that each element at index i of the new array is the product of all the numbers in the original array except the one at i.
        // For example, if our input was[1, 2, 3, 4, 5], the expected output would be[120, 60, 40, 30, 24]. If our input was [3, 2, 1], the expected output would be[2, 3, 6].
        // Follow-up: what if you can't use division?
        [Theory]
        [InlineData(new int[] { 1, 2, 3, 4, 5 }, new int[] { 120, 60, 40, 30, 24 })]
        [InlineData(new int[] { 3, 2, 1 }, new int[] { 2, 3, 6 })]
        public void ReturnArrayProductExceptOne(int[] input, int[] expected)
        {
            static int[] UsingDivision(int[] input)
            {
                int product = input.Aggregate(1, (a, b) => a * b);
                return input.Select(x => product / x).ToArray();
            }

            Assert.Equal(expected, UsingDivision(input));

            static int[] NotUsingDivision(int[] input)
            {
                int[] output = new int[input.Length];

                for (int i = 0; i < input.Length; i++) output[i] = 1;

                for (int i = 0; i < input.Length; i++)
                {
                    for (int j = 0; j < input.Length; j++)
                    {
                        if (i != j) output[i] *= input[j];
                    }
                }

                return output;
            }

            Assert.Equal(expected, NotUsingDivision(input));
        }
    }
}
