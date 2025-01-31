namespace leetcode.Lists.Top150
{
    public class Stack
    {
        // Given a string s containing just the characters '(', ')', '{', '}', '[' and ']', determine if the input string is valid.
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

        // You are given an absolute path for a Unix-style file system, which always begins with a slash '/'. Your task is to transform this absolute path into its simplified canonical path.
        // The rules of a Unix-style file system are as follows:
        // A single period '.' represents the current directory.
        // A double period '..' represents the previous/parent directory.
        // Multiple consecutive slashes such as '//' and '///' are treated as a single slash '/'.
        // Any sequence of periods that does not match the rules above should be treated as a valid directory or file name. For example, '...' and '....' are valid directory or file names.
        // The simplified canonical path should follow these rules:
        // The path must start with a single slash '/'.
        // Directories within the path must be separated by exactly one slash '/'.
        // The path must not end with a slash '/', unless it is the root directory.
        // The path must not have any single or double periods ('.' and '..') used to denote current or parent directories.
        // Return the simplified canonical path.
        [Theory]
        [InlineData("/home/", "/home")]
        [InlineData("/home//foo/", "/home/foo")]
        [InlineData("/home/user/Documents/../Pictures", "/home/user/Pictures")]
        [InlineData("/../", "/")]
        [InlineData("/.../a/../b/c/../d/./", "/.../b/d")]
        [InlineData("/a/../../b/../c//.//", "/c")]
        [InlineData("/...", "/...")]
        [InlineData("/hello../world", "/hello../world")]
        [InlineData("/", "/")]
        [InlineData("/a/./b///../c/../././../d/..//../e/./f/./g/././//.//h///././/..///", "/e/f/g")]
        public void SimplifyPath(string path, string expected)
        {
            List<string> list = [];

            for (int left = 0; left < path.Length; left++)
            {
                while (left < path.Length && path[left] == '/') left++;

                int right = left + 1;
                while (right < path.Length && path[right] != '/') right++;

                if (left < path.Length)
                {
                    string part = path.Substring(left, right - left);

                    switch (part)
                    {
                        case "":
                        case ".":
                            // Ignore
                            break;
                        case "..":
                            if (list.Count > 0) list.RemoveAt(list.Count - 1);
                            break;
                        default:
                            list.Add(part);
                            break;
                    }
                }

                left = right;
            }

            string result = "/" + string.Join("/", list);

            Assert.Equal(expected, result);
        }

        // Design a stack that supports push, pop, top, and retrieving the minimum element in constant time.
        // Implement the MinStack class:
        // MinStack() initializes the stack object.
        // void push(int val) pushes the element val onto the stack.
        // void pop() removes the element on the top of the stack.
        // int top() gets the top element of the stack.
        // int getMin() retrieves the minimum element in the stack.
        // You must implement a solution with O(1) time complexity for each function.
        public class MinStack
        {
            private readonly Stack<int> _stack = new();
            private readonly Stack<int> _minSoFar = new();

            public MinStack()
            {
            }

            public void Push(int val)
            {
                _stack.Push(val);
                if (_minSoFar.Count == 0 || val <= _minSoFar.Peek())
                {
                    _minSoFar.Push(val);
                }
            }

            public void Pop()
            {
                if (_stack.TryPop(out int val) && (val == _minSoFar.Peek()))
                {
                    _minSoFar.Pop();
                }
            }

            public int Top()
            {
                return _stack.Peek();
            }

            public int GetMin()
            {
                return _minSoFar.Peek();
            }
        }

        [Fact]
        public void MinStackTest()
        {
            MinStack minStack = new MinStack();
            minStack.Push(-2);
            minStack.Push(0);
            minStack.Push(-3);
            Assert.Equal(-3, minStack.GetMin());
            minStack.Pop();
            Assert.Equal(0, minStack.Top());
            Assert.Equal(-2, minStack.GetMin());
        }
    }
}