namespace ponderthis
{
    [TestClass]
    public class APSeq
    {
        private class Primes
        {
            private readonly SortedSet<ulong> _primes = new SortedSet<ulong>() { 3 };
            private int locked = 0;
            private ulong _currentSqrt = 1;

            public bool IsPrime(ulong value)
            {
                UpTo(value);

                return _primes.Contains(value);
            }

            public void UpTo(ulong value)
            {
                if (_primes.Max < value && locked == 1) throw new InvalidOperationException("Locked");

                while (_primes.Max <= value)
                {
                    ulong candidate = _primes.Max + 2;
                    while (true)
                    {
                        for (ulong nextSqrt = _currentSqrt + 1; nextSqrt * nextSqrt <= candidate; nextSqrt++)
                        {
                            _currentSqrt = nextSqrt;
                        }

                        if (_primes.Any(prime => prime <= _currentSqrt && candidate % prime == 0))
                        {
                            candidate += 2;
                        }
                        else
                        {
                            break;
                        }
                    }

                    _primes.Add(candidate);
                }
            }

            public int GetCount()
            {
                return 1 + _primes.Count;
            }

            internal void Lock()
            {
                if (Interlocked.CompareExchange(ref locked, 1, 0) == 1) throw new InvalidOperationException("Already locked");
            }

            internal void Unlock()
            {
                if (Interlocked.CompareExchange(ref locked, 0, 1) == 0) throw new InvalidOperationException("Already unlocked");
            }
        }

        public class ArithmeticProgressionSequence
        {
            private readonly Primes _primes = new Primes();
            private readonly SortedSet<ulong> _sequences = new SortedSet<ulong>() { 1 };
            private readonly ulong[] _values;

            public ArithmeticProgressionSequence(int maxSize)
            {
                this._values = new ulong[maxSize];
            }

            public IEnumerable<ulong> GetSequence(ulong initialValue, int length)
            {
                for (int i = 0; i < length; i++)
                {
                    yield return initialValue + (ulong)i;
                }
            }

            public IEnumerable<ulong> GetNextSequence()
            {
                int nextLength = _sequences.Count + 1;

                for (ulong candidate = _sequences.Max + 1; ; candidate++)
                {
                    while (_primes.IsPrime(candidate) || _primes.IsPrime(candidate + 1)) candidate += 2;

                    Parallel.For(0, nextLength + 1, i => _values[i] = candidate + (ulong)((i + 1) * i / 2));

                    // Ensures the prime cache contains at least up to the value past the last
                    _primes.UpTo(_values[nextLength]);
                    _primes.Lock();
                    bool hasPrime = _values.Take(nextLength).AsParallel().Any(_primes.IsPrime);
                    _primes.Unlock();

                    if (!hasPrime)
                    {
                        _sequences.Add(candidate);
                        break;
                    }

                }

                return GetSequence(_sequences.Max, _sequences.Count);
            }
        }

        [TestMethod]
        public void UpTo()
        {
            var primes = new Primes();

            Assert.IsFalse(primes.IsPrime(7918ul));
            Assert.AreEqual(1000, primes.GetCount());
            Assert.IsTrue(primes.IsPrime(7919ul));
            Assert.AreEqual(1001, primes.GetCount());
            Assert.IsFalse(primes.IsPrime(7920ul));
            Assert.AreEqual(1001, primes.GetCount());
        }

        [TestMethod]
        public void ExampleSequences()
        {
            var seq = new ArithmeticProgressionSequence(1001);

            Assert.AreEqual("1", string.Join(',', seq.GetSequence(1, 1)), "X1");
            Assert.AreEqual("8,9", string.Join(',', seq.GetSequence(8, 2)), "X2");
            Assert.AreEqual("9,10,12", string.Join(',', seq.GetSequence(9, 3)), "X3");
            Assert.AreEqual("15,16,18,21", string.Join(',', seq.GetSequence(15, 4)), "X4");
        }
    }
}