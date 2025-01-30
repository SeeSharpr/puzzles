using static System.Runtime.InteropServices.JavaScript.JSType;
using System.IO;
using System;
using System.Text;

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
    }
}
