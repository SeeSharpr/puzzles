namespace basics.os
{
    public class MutexTests
    {
        private class DiningPhilosophers(int count)
        {
            private readonly Mutex csLocals = new();
            private readonly bool[] eating = new bool[count];
            private readonly int[] forks = new int[count];
            private readonly int[] iterations = new int[count];

            public int[] Run(TimeSpan duration)
            {
                Thread[] threads = new Thread[count];

                try
                {
                    for (int i = 0; i < count; i++)
                    {
                        forks[i] = 1;
                        threads[i] = new(this.OnePhilosopher);
                        threads[i].Name = $"Philosopher {i}";
                        threads[i].Start(new Tuple<int, TimeSpan>(i, duration));
                    }
                }
                finally
                {
                    foreach (var thread in threads)
                    {
                        thread.Join();
                    }
                }

                return iterations;
            }

            private void OnePhilosopher(object? data)
            {
                Random random = new();
                Tuple<int, TimeSpan> tuple = (Tuple<int, TimeSpan>)data!;
                int me = tuple.Item1;
                int left = (me - 1 + count) % count;
                int right = (me + 1 + count) % count;
                DateTime limit = DateTime.UtcNow + tuple.Item2;

                while (DateTime.UtcNow < limit)
                {
                    // Think
                    Thread.Sleep(random.Next(100, 200));

                    // Exclusive lock to acquire forks
                    try
                    {
                        csLocals.WaitOne();

                        if (!eating[left] && !eating[right])
                        {
                            // Eating
                            eating[me] = true;
                            forks[left]--;
                            forks[right]--;

                            iterations[me]++;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    finally
                    {
                        csLocals.ReleaseMutex();
                    }

                    // Eat
                    Thread.Sleep(random.Next(100, 200));

                    // Exclusive lock to release forks
                    try
                    {
                        csLocals.WaitOne();

                        // Back to thinking
                        forks[right]++;
                        forks[left]++;
                        eating[me] = false;
                    }
                    finally
                    {
                        csLocals.ReleaseMutex();
                    }
                }
            }
        }


        [Theory]
        [InlineData(2, 10)]
        [InlineData(5, 30)]
        public void DiningPhilosophersTest(int count, int limitInSeconds)
        {
            Assert.True(new DiningPhilosophers(count).Run(TimeSpan.FromSeconds(limitInSeconds)).All(x => x > 0));
        }
    }
}
