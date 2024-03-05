using System.Diagnostics;
using static ponderthis.APSeq;

var seq = new ArithmeticProgressionSequence();

Stopwatch sw = Stopwatch.StartNew();
for (int i = 0; i < 1000; i++)
{
    Console.Write($"{sw.Elapsed} {i} - ");

    if (i % 10 == 0 || i == 999)
    {
        bool first = true;
        foreach (var item in seq.GetNextSequence())
        {
            if (!first) Console.Write(',');
            else first = false;

            Console.Write(item);
        }

        Console.WriteLine();
    }
    else
    {
        Console.WriteLine($"{seq.GetNextSequence().First()}...");
    }
}
