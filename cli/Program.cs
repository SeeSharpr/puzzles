using System.Diagnostics;
using static ponderthis.APSeq;

var sequence = new ArithmeticProgressionSequence(1001);

using TextWriter writer = new StreamWriter($"execution_{DateTime.UtcNow.ToString("yyyy_MM_dd_hh_mm_ss")}.txt");

Stopwatch sw = Stopwatch.StartNew();
TimeSpan previousElapsed = TimeSpan.Zero;

for (int i = 0; i < 1000; i++)
{
    Console.Write($"{i} - {sw.Elapsed} - ");
    writer.Write($"{i} - {sw.Elapsed} - ");

    var result = sequence.GetNextSequence();

    if (i % 10 == 0 || i == 999)
    {
        writer.Write($"{sw.Elapsed - previousElapsed} - ");
        previousElapsed = sw.Elapsed;

        bool first = true;
        foreach (var item in result)
        {
            if (!first)
            {
                writer.Write(',');
            }
            else
            {
                Console.WriteLine(item);
                first = false;
            }

            writer.Write(item);
        }

        writer.WriteLine();
    }
    else
    {
        var first = result.First();

        Console.WriteLine(first);
        writer.WriteLine($"{first}...");
    }
}
