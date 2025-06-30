short first = 1;
short last = 1000;

Thread thread = new(() => Console.WriteLine(SumEvenNumbersInRange(first, last)));

while (true)
{
    string command = Console.ReadLine()!;
    if (command == "show")
    {
        thread.Start();
        thread.Join();
        return;
    }
}

static long SumEvenNumbersInRange(int first, int last) =>
    Enumerable.Range(first, last - first + 1).Where(x => x % 2 == 0).Sum();