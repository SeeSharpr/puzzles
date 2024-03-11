
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;

namespace ponderthis
{
    public class Primes
    {
        private readonly SortedSet<ulong> _primes = new SortedSet<ulong>() { 3 };
        private int locked = 0;
        private ulong _currentSqrt = 1;

        public bool IsPrime(ulong value)
        {
            UpTo(value);

            return _primes.Contains(value) || value == 1 || value == 2;
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

    public class Sequence
    {
        private readonly Primes _primes = new Primes();
        private readonly SortedList<ulong, int> _sequences = new() { { 1, 1 } };
        private readonly ulong[] _values;

        public Sequence(int maxSize)
        {
            this._values = new ulong[maxSize + 1];
        }

        public IEnumerable<ulong> GetSequence(ulong initialValue, int length)
        {
            ulong nextValue = initialValue;
            for (uint i = 0; i < length; i++)
            {
                nextValue += i;

                yield return nextValue;
            }
        }

        public IEnumerable<KeyValuePair<ulong, int>> GetNextSequence()
        {
            ulong candidate = _sequences.Keys[_sequences.Keys.Count - 1];
            int length = _sequences.Values[_sequences.Values.Count - 1] + 1;

            while (length < _values.Length)
            {
                while (_primes.IsPrime(candidate)) candidate++;

                // Load values to test in parallel
                Parallel.For(fromInclusive: 0, toExclusive: length + 1, body: index => _values[index] = candidate + (ulong)((index + 1) * index / 2));

                // Ensures we have enough primes in cache
                _primes.UpTo(_values[length]);

                // Primality check
                _primes.Lock();
                bool hasPrime = _values.Take(length).AsParallel().Any(_primes.IsPrime);
                _primes.Unlock();

                if (hasPrime)
                {
                    // There is a prime, continue searching
                    candidate++;
                    continue;
                }

                // There is no prime, try to keep looking for the same candidate
                while (length < _values.Length)
                {
                    yield return new KeyValuePair<ulong, int>(candidate, length);

                    length++;

                    _values[length] = _values[length - 1] + (ulong)length;
                    _primes.UpTo(_values[length]);
                    _primes.Lock();
                    hasPrime = _primes.IsPrime(_values[length - 1]);
                    _primes.Unlock();

                    if (hasPrime)
                    {
                        _sequences.Add(candidate, length-1);
                        candidate++;
                        break;
                    }
                }
            }

            yield break;
        }

        public void SerializeLatest(TextWriter writer, int length, TimeSpan timestamp, IEnumerable<ulong> sequence, int fullDumpEvery, int targetLength)
        {
            writer.Write($"{length} - {timestamp} - ");

            if (length % fullDumpEvery == 0 || length == targetLength)
            {
                bool first = true;
                foreach (var item in sequence)
                {
                    if (!first)
                    {
                        writer.Write(',');
                    }
                    else
                    {
                        first = false;
                    }

                    writer.Write(item);
                }

                writer.WriteLine();
            }
            else
            {
                var first = sequence.First();

                writer.WriteLine($"{first}...");
            }
        }
    }

    [TestClass]
    public class SequenceTest
    {
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
        public void GetSequence()
        {
            var seq = new Sequence(1001);

            Assert.AreEqual("1", string.Join(',', seq.GetSequence(1, 1)), "X1");
            Assert.AreEqual("8,9", string.Join(',', seq.GetSequence(8, 2)), "X2");
            Assert.AreEqual("9,10,12", string.Join(',', seq.GetSequence(9, 3)), "X3");
            Assert.AreEqual("9,10,12,15", string.Join(',', seq.GetSequence(9, 4)), "X4");
            Assert.AreEqual("15,16,18,21,25", string.Join(',', seq.GetSequence(15, 5)), "X5");
        }

        [TestMethod]
        public void SerializationDeserialization()
        {
            var seq = new Sequence(10);

            var input = string.Join(Environment.NewLine, new[] { "7 - 10:00 - 123", "8 - 11:00 - 234", });

            using StringReader reader = new(input);
            using StringWriter writer = new();
        }
    }
}
