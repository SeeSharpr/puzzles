﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace leetcode
{
    public static class LeetcodeExtensions
    {
        public static IEnumerable<T> ParseSingleEnumerable<T>(this string input, Func<string, T> parse, char elementSeparator = ',')
        {
            return input
                .Split(elementSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(parse);
        }

        public static IEnumerable<IEnumerable<T>> ParseDoubleEnumerableLC<T>(this string input, Func<string, T> parse, char elementSeparator = ',', char entrySeparator = '|')
        {
            return input
                .Split(entrySeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(pair => pair.ParseSingleEnumerable<T>(parse, elementSeparator));
        }

        public static string ToString<T>(this IEnumerable<T> input, char elementSeparator = ',')
        {
            return string.Join(elementSeparator, input);
        }

        public static string ToString<T>(this IEnumerable<IEnumerable<T>> input, char elementSeparator = ',', char entrySeparator = '|')
        {
            return string.Join(entrySeparator, input.Select(entry => entry.ToString(elementSeparator)));
        }
    }
}
