using System.Diagnostics;
using static ponderthis.APSeq;

var seq = new ArithmeticProgressionSequence();

using TextWriter writer = Console.Out;

Stopwatch sw = Stopwatch.StartNew();
TimeSpan previousElapsed = TimeSpan.Zero;

for (int i = 0; i < 1000; i++)
{
    writer.Write($"{i} - {sw.Elapsed} - ");

    if (i % 10 == 0 || i == 999)
    {
        writer.Write($"{sw.Elapsed - previousElapsed} - ");
        previousElapsed = sw.Elapsed;

        bool first = true;
        foreach (var item in seq.GetNextSequence())
        {
            if (!first) writer.Write(',');
            else first = false;

            writer.Write(item);
        }

        writer.WriteLine();
    }
    else
    {
        writer.WriteLine($"{seq.GetNextSequence().First()}...");
    }
}
