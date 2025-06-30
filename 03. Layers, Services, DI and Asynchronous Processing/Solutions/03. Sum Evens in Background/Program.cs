using System.Numerics;

short start = 1;
long end = 1_000_000_000;
BigInteger sum = 0;
CancellationTokenSource cts = new();

Task sumEvens = SumEvenNumbersInRangeAsync(start, end, cts.Token);

while (true)
{
    string command = Console.ReadLine()!;
    if (command == "show")
        Console.WriteLine(sumEvens.IsCompleted ? sum : $"Calculating... As of now: {sum}");
    if (command == "exit")
    {
        cts.Cancel();
        try
        {
            await sumEvens;
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Calculation was successfully cancelled.");
        }
        return;
    }
}

async Task SumEvenNumbersInRangeAsync(long first, long last, CancellationToken ct)
{
    await Task.Run(() =>
    {
        for (long i = first; i <= last; i++)
        {
            if (i % 2 == 0) sum += i;
            if (ct.IsCancellationRequested)
            {
                Console.WriteLine($"Calculation cancelled at i = {i}. Current sum: {sum}.");
                ct.ThrowIfCancellationRequested();
            }
        }
    }, ct);
}