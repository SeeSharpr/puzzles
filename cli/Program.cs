using ponderthis;
using System.Diagnostics;

internal class Program
{
    private static void Main(string[] args)
    {
        var sequence = new Sequence(2024);

        TimeSpan startingTimestamp = TimeSpan.Zero;

        using TextWriter writer = new StreamWriter($"execution_{DateTime.UtcNow.ToString("yyyy_MM_dd_hh_mm_ss")}.txt");

        Stopwatch sw = Stopwatch.StartNew();

        foreach (var pair in sequence.GetNextSequence())
        {
            sequence.SerializeLatest(writer, pair.Value, sw.Elapsed + startingTimestamp, sequence.GetSequence(pair.Key, pair.Value), 10, 999);
            sequence.SerializeLatest(Console.Out, pair.Value, sw.Elapsed + startingTimestamp, sequence.GetSequence(pair.Key, pair.Value), 1000, 999);
        }
    }
}