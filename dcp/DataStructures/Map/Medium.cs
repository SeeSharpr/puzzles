
using static dcp.DataStructures.Map.Medium;

namespace dcp.DataStructures.Map
{
    public class Medium
    {
        // Problem # 0097
        // This problem was asked by Stripe.
        // Write a map implementation with a get function that lets you retrieve the value of a key at a particular time.
        // It should contain the following methods:
        // set(key, value, time): sets key to value for t = time. get(key, time): gets the key at t = time.
        // The map should work like this. If we set a key at a particular time, it will maintain that value forever or until it gets set at a later time.In other words, when we get a key at a time, it should return the value that was set for that key set at the most recent time.
        public interface IMapWithTime
        {
            void Set(int key, int value, int time);
            int? Get(int key, int time);
        }

        public class MapWithTime : IMapWithTime
        {
            private class TimeComparer : IComparer<int>
            {
                public int Compare(int x, int y)
                {
                    return x - y;
                }
            }

            private readonly Dictionary<int, SortedList<int, int>> map = [];

            public void Set(int key, int value, int time)
            {
                if (!map.TryGetValue(key, out var values))
                {
                    map[key] = values = new SortedList<int, int>(new TimeComparer());
                }

                values[time] = value;
            }

            public int? Get(int key, int time)
            {
                if (!map.TryGetValue(key, out var values) || values.Count == 0 || time < values.Keys[0])
                {
                    return null;
                }

                int left = 0;
                int right = values.Keys.Count;

                while (left < right)
                {
                    int pivot = (left + right) / 2;

                    if (time < values[values.Keys[pivot]])
                    {
                        right = pivot - 1;
                    }
                    else
                    {
                        left = pivot + 1;
                    }
                }

                return values[values.Keys[Math.Min(left, values.Keys.Count-1)]];
            }
        }

        public class MapWithTimeTests<T> where T : IMapWithTime, new()
        {
            private static Tuple<SetParams[], GetParams[]>[] tuples = [
                    new Tuple<SetParams[], GetParams[]>([new SetParams(1, 1, 0), new SetParams(1, 2, 2),], [new GetParams(1, 1, 1), new GetParams(1, 3, 2),]),
                new Tuple<SetParams[], GetParams[]>([new SetParams(1, 1, 5),], [new GetParams(1, 0, null), new GetParams(1, 10, 1),]),
                new Tuple<SetParams[], GetParams[]>([new SetParams(1, 1, 0), new SetParams(1, 2, 0),], [new GetParams(1, 0, 2)]),
            ];

            private class SetParams(int key, int value, int time) { public int Key { get; } = key; public int Value { get; } = value; public int Time { get; } = time; }

            private class GetParams(int key, int time, int? expectedValue) { public int Key { get; } = key; public int Time { get; } = time; public int? ExpectedValue { get; } = expectedValue; }

            public void Run()
            {
                foreach (var tuple in tuples)
                {
                    T map = new();

                    foreach (var sets in tuple.Item1)
                    {
                        map.Set(sets.Key, sets.Value, sets.Time);
                    }

                    foreach (var gets in tuple.Item2)
                    {
                        Assert.Equal(gets.ExpectedValue, map.Get(gets.Key, gets.Time));
                    }
                }
            }
        }

        [Fact]
        public void MapWithTimeTest()
        {
            new MapWithTimeTests<MapWithTime>().Run();
        }
    }
}

