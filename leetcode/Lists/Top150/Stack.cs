namespace leetcode.Lists.Top150
{
    public class Stack
    {
        //Given a string s containing just the characters '(', ')', '{', '}', '[' and ']', determine if the input string is valid.
        // An input string is valid if:
        // Open brackets must be closed by the same type of brackets.
        // Open brackets must be closed in the correct order.
        // Every close bracket has a corresponding open bracket of the same type.
        [Theory]
        [InlineData("()", true)]
        [InlineData("()[]{}", true)]
        [InlineData("(}", false)]
        [InlineData("([])", true)]
        [InlineData("]", false)]
        [InlineData("[", false)]
        public void IsValid(string s, bool expected)
        {
            Stack<char> stack = new();

            bool isValid = true;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '(' || s[i] == '[' || s[i] == '{')
                {
                    stack.Push(s[i]);
                }
                else if (stack.TryPeek(out char c) &&
                        ((s[i] == ')' && c == '(') ||
                         (s[i] == ']' && c == '[') ||
                         (s[i] == '}' && c == '{')))
                {
                    stack.Pop();
                }
                else
                {
                    isValid = false;
                    break;
                }
            }

            isValid = isValid && stack.Count == 0;

            Assert.Equal(expected, isValid);
        }
    }
}
