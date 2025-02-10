namespace leetcode
{
    public static class LeetcodeExtensions
    {
        public static IEnumerable<IEnumerable<T>> ParseNestedArrayStringLC<T>(this string input, Func<string, T> parse, char elementSeparator = ',', char entrySeparator = '|')
        {
            return input
                .Replace("], [", $"{entrySeparator}")
                .Replace("],[", $"{entrySeparator}")
                .Replace("[", "")
                .Replace("]", "")
                .ParseNestedEnumerable(parse, elementSeparator, entrySeparator);
        }

        public static IEnumerable<T> ParseArrayStringLC<T>(this string input, Func<string, T> parse, char elementSeparator = ',')
        {
            return input
                .Replace("[", "")
                .Replace("]", "")
                .ParseEnumerable(parse, elementSeparator);
        }

        public static TNode? ParseLinkedListLC<TNode, TData>(this string input, Func<TData?, TNode> createNode, Action<TNode?, TNode?> setNext, Func<TNode?, TNode?> getNext, Func<string, TData> parse, char elementSeparator = ',')
        {
            TNode dummy = createNode(default);

            TNode? ptr = dummy;
            foreach (TData u in input.ParseArrayStringLC(parse, elementSeparator))
            {
                setNext(ptr, createNode(u));
                ptr = getNext(ptr);
            }

            TNode? result = getNext(dummy);
            setNext(dummy, default);

            return result;
        }

        public static IEnumerable<T> ParseEnumerable<T>(this string input, Func<string, T> parse, char elementSeparator = ',')
        {
            return input
                .Split(elementSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(parse);
        }

        public static IEnumerable<IEnumerable<T>> ParseNestedEnumerable<T>(this string input, Func<string, T> parse, char elementSeparator = ',', char entrySeparator = '|')
        {
            return input
                .Split(entrySeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(pair => pair.ParseEnumerable<T>(parse, elementSeparator));
        }

        public static string ToLCString<T>(this IEnumerable<T> input, char elementSeparator = ',')
        {
            return string.Join(elementSeparator, input);
        }

        public static string ToLCDoubleString<T>(this IEnumerable<IEnumerable<T>> input, char elementSeparator = ',', char entrySeparator = '|')
        {
            return string.Join(entrySeparator, input.Select(entry => entry.ToLCString(elementSeparator)));
        }
    }
}
