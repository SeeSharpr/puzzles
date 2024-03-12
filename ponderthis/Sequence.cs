
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace ponderthis
{
    public class Primes
    {
        private readonly SortedSet<ulong> _primes = [3, 5, 7];
        private uint _candidateSqrt = 5;

        public bool IsPrime(ulong value)
        {
            if (value < 1) return false;
            else if (value == 1 || value == 2 || value == 3) return true;
            else if (value % 2 == 0 || value % 3 == 0) return false;
            else
            {
                UpTo(value);
                return _primes.Contains(value);
            }
        }

        public void UpTo(ulong value)
        {
            ulong candidate = _primes.Max;

            while (_primes.Max <= value)
            {
                do candidate += 2; while (candidate % 3 == 0 || candidate % 5 == 0);

                while (_candidateSqrt * _candidateSqrt < candidate)
                {
                    _candidateSqrt += 2;
                }

                bool isPrime = true;
                for (uint i = 5; i <= _candidateSqrt; i += 6)
                {
                    if (candidate % i == 0 || candidate % (i + 2) == 0)
                    {
                        isPrime = false;
                        break;
                    }
                }

                if (!isPrime) continue;

                _primes.Add(candidate);
            }
        }

        public int GetCount()
        {
            return 1 + _primes.Count;
        }
    }

    public class Sequence
    {
        private readonly Primes _primes = new Primes();
        private readonly SortedList<ulong, int> _sequences = new() { { 1, 1 } };
        private readonly int maxLength;

        public Sequence(int maxLength)
        {
            this.maxLength = maxLength;
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
            yield return new KeyValuePair<ulong, int>(1, 1);

            ulong candidate = _sequences.Keys[_sequences.Keys.Count - 1];
            int length = _sequences.Values[_sequences.Values.Count - 1] + 1;

            while (length <= maxLength)
            {
                ulong value = candidate;
                bool hasPrime = false;

                _primes.UpTo(candidate + (ulong)((1 + maxLength) * maxLength / 2));

                for (int i = 1; i <= length; i++)
                {
                    if (_primes.IsPrime(value))
                    {
                        hasPrime = true;
                        break;
                    }

                    value += (ulong)i;
                }

                while (!hasPrime && length <= maxLength)
                {
                    yield return new KeyValuePair<ulong, int>(candidate, length);

                    _sequences[candidate] = length;
                    hasPrime = _primes.IsPrime(value);
                    length++;

                    if (hasPrime)
                    {
                        break;
                    }
                    else
                    {
                        value += (ulong)length;
                    }
                }

                candidate++;
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
        public void GetNextSequence()
        {
            var seq = new Sequence(1001);
            var result = seq.GetNextSequence().Take(5).ToArray();

            Assert.AreEqual("1", string.Join(',', seq.GetSequence(result[0].Key, result[0].Value)), "X1");
            Assert.AreEqual("8,9", string.Join(',', seq.GetSequence(result[1].Key, result[1].Value)), "X2");
            Assert.AreEqual("9,10,12", string.Join(',', seq.GetSequence(result[2].Key, result[2].Value)), "X3");
            Assert.AreEqual("9,10,12,15", string.Join(',', seq.GetSequence(result[3].Key, result[3].Value)), "X4");
            Assert.AreEqual("15,16,18,21,25", string.Join(',', seq.GetSequence(result[4].Key, result[4].Value)), "X5");
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
