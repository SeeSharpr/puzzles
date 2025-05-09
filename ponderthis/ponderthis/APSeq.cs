namespace ponderthis
{
    public class APSeq
    {
        private class Primes
        {
            private readonly SortedSet<ulong> _primes = new SortedSet<ulong>() { 3 };
            private ulong _currentSqrt = 1;

            public bool IsPrime(ulong value)
            {
                UpTo(value);

                return _primes.Contains(value);
            }

            public void UpTo(ulong value)
            {
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
        }

        public class ArithmeticProgressionSequence
        {
            private readonly Primes _primes = new Primes();
            private readonly SortedSet<ulong> _sequences = new SortedSet<ulong>() { 1 };

            public IEnumerable<ulong> GetSequence(ulong initialValue, int length)
            {
                for (int i = 0; i < length; i++)
                {
                    yield return initialValue + (ulong)i;
                }
            }

            public IEnumerable<ulong> GetNextSequence()
            {
                uint nextLength = (uint)_sequences.Count;

                for (ulong candidate = _sequences.Max + 1; ; candidate++)
                {
                    if (_primes.IsPrime(candidate)) continue;

                    bool found = true;
                    ulong next = candidate;
                    for (uint i = 0; i < nextLength; i++)
                    {
                        next += i + 1;

                        if (_primes.IsPrime(next))
                        {
                            found = false;
                            break;
                        }
                    }

                    if (found)
                    {
                        _sequences.Add(candidate);
                        break;
                    }
                }

                return GetSequence(_sequences.Max, _sequences.Count);
            }
        }

        [Fact]
        public void UpTo()
        {
            var primes = new Primes();

            Assert.False(primes.IsPrime(7918ul));
            Assert.Equal(1000, primes.GetCount());
            Assert.True(primes.IsPrime(7919ul));
            Assert.Equal(1001, primes.GetCount());
            Assert.False(primes.IsPrime(7920ul));
            Assert.Equal(1001, primes.GetCount());
        }

        [Fact]
        public void ExampleSequences()
        {
            var seq = new ArithmeticProgressionSequence();

            Assert.Equal("1", string.Join(',', seq.GetSequence(1, 1)));
            Assert.Equal("8,9", string.Join(',', seq.GetSequence(8, 2)));
            Assert.Equal("9,10,12", string.Join(',', seq.GetSequence(9, 3)));
            Assert.Equal("15,16,18,21", string.Join(',', seq.GetSequence(15, 4)));
        }
    }
}