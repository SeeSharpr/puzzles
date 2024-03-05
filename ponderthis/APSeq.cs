using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ponderthis
{
    [TestClass]
    public class APSeq
    {
        private class Primes
        {
            private readonly SortedSet<ulong> _primes = new SortedSet<ulong>() { 3 };
            private ulong _currentSqrt = 1;
            private ulong _currentSqr = 1;

            public ulong UpTo(ulong value)
            {
                if (value <= 2) return 2;

                if (_primes.TryGetValue(value, out ulong result))
                {
                    return result;
                }

                result = _primes.Max;
                while (_primes.Max < value)
                {
                    result = _primes.Max;

                    ulong candidate = _primes.Max + 2;
                    while (true)
                    {
                        for (ulong nextSqrt = _currentSqrt + 1; nextSqrt * nextSqrt <= candidate; nextSqrt++)
                        {
                            _currentSqrt = nextSqrt;
                            _currentSqr = _currentSqrt * _currentSqrt;
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

                return result;
            }
        }

        private class ArithmeticProgressionSequence
        {
            private ulong GetTerm(ulong initialValue, uint index)
            {
                return initialValue + (index * (index + 1ul) / 2ul);
            }

            public IEnumerable<ulong> GetSequence(ulong initialValue, uint length)
            {
                for (uint i = 0; i < length; i++)
                {
                    yield return GetTerm(initialValue, i);
                }
            }
        }

        [TestMethod]
        public void UpTo()
        {
            var primes = new Primes();

            Assert.AreEqual(7907ul, primes.UpTo(7918ul));
            Assert.AreEqual(7919ul, primes.UpTo(7919ul));
            Assert.AreEqual(7919ul, primes.UpTo(7920ul));
        }

        [TestMethod]
        public void ExampleSequences()
        {
            var seq = new ArithmeticProgressionSequence();

            Assert.AreEqual("1", string.Join(',', seq.GetSequence(1, 1)), "X1");
            Assert.AreEqual("8,9", string.Join(',', seq.GetSequence(8, 2)), "X2");
            Assert.AreEqual("15,16,18", string.Join(',', seq.GetSequence(15, 3)), "X3");
        }
    }
}