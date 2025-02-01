using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using System.Windows.Markup;
using Xunit.Sdk;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                    string part = path[left..right];

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
            MinStack minStack = new();
            minStack.Push(-2);
            minStack.Push(0);
            minStack.Push(-3);
            Assert.Equal(-3, minStack.GetMin());
            minStack.Pop();
            Assert.Equal(0, minStack.Top());
            Assert.Equal(-2, minStack.GetMin());
        }

        // You are given an array of strings tokens that represents an arithmetic expression in a Reverse Polish Notation.
        // Evaluate the expression.Return an integer that represents the value of the expression.

        [Theory]
        [InlineData("2, 1, +, 3, *", 9)]
        [InlineData("4, 13, 5, /, +", 6)]
        [InlineData("10, 6, 9, 3, +, -1, *, /, *, 17, +, 5, +", 22)]
        public void EvalRPN(string inputTokens, int expected)
        {
            string[] tokens = inputTokens.ParseArrayStringLC<string>(s => s).ToArray();

            Stack<int> stack = new();

            foreach (string token in tokens)
            {
                {
                    switch (token)
                    {
                        case "+":
                        case "-":
                        case "*":
                        case "/":
                            _ = stack.TryPop(out int op2);
                            _ = stack.TryPop(out int op1);

                            stack.Push(
                                token == "+" ? op1 + op2 :
                                token == "-" ? op1 - op2 :
                                token == "*" ? op1 * op2 :
                                token == "/" ? op1 / op2 :
                                throw new InvalidOperationException(token)
                                );

                            break;
                        default:
                            stack.Push(int.Parse(token));
                            break;
                    }
                }
            }

            _ = stack.TryPop(out int result);

            Assert.Equal(expected, result);
        }

        // Given a string s representing a valid expression, implement a basic calculator to evaluate it, and return the result of the evaluation.
        // Note: You are not allowed to use any built-in function which evaluates strings as mathematical expressions, such as eval().
        [Theory]
        [InlineData("1 + 1", 2)]
        [InlineData(" 2-1 + 2 ", 3)]
        [InlineData("(1+(4+5+2)-3)+(6+8)", 23)]
        [InlineData("2147483647", 2147483647)]
        [InlineData("1-(     -2)", 3)]
        [InlineData("- (3 + (4 + 5))", -12)]
        public void Calculate(string s, int expected)
        {
            static int Precedence(char op)
            {
                return op switch
                {
                    '+' or '-' => 1,
                    '*' or '/' => 2,
                    _ => 0,
                };
            }

            static bool IsOperator(char op)
            {
                return op switch
                {
                    '+' or '-' or '*' or '/' => true,
                    _ => false,
                };
            }

            static void Evaluate(Stack<int> vals, Stack<char> ops)
            {
                _ = vals.TryPop(out int op2);
                _ = vals.TryPop(out int op1);
                _ = ops.TryPop(out char op);

                vals.Push(op == '+' ? op1 + op2 : op == '-' ? op1 - op2 : op == '*' ? op1 * op2 : op == '/' ? op1 / op2 : throw new InvalidOperationException(op.ToString()));
            }

            Stack<int> vals = new();
            Stack<char> ops = new();

            int pars = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (char.IsDigit(s[i]))
                {
                    int val = s[i] - '0';
                    while (i + 1 < s.Length && char.IsDigit(s[i + 1]))
                    {
                        val = val * 10 + (s[i + 1] - '0');
                        i++;
                    }

                    vals.Push(val);
                }
                else if (s[i] == '(')
                {
                    pars++;
                    ops.Push(s[i]);
                }
                else if (IsOperator(s[i]) || s[i] == ')')
                {
                    if (s[i] == '-' && ops.Count - pars >= vals.Count)
                    {
                        vals.Push(0);
                    }

                    while (ops.TryPeek(out char op) && op != '(' && Precedence(s[i]) <= Precedence(op))
                    {
                        Evaluate(vals, ops);
                    }

                    if (s[i] != ')')
                    {
                        // Push actual operator
                        ops.Push(s[i]);
                    }
                    else
                    {
                        // Pop the '('
                        ops.Pop();
                        pars--;
                    }
                }

                while (vals.Count > 1 + pars && ops.TryPeek(out char op) && op != '(')
                {
                    Evaluate(vals, ops);
                }
            }

            _ = vals.TryPop(out int result);

            Assert.Equal(expected, result);
        }
    }
}