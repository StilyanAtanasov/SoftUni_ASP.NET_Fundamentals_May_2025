int start = int.Parse(Console.ReadLine()!);
int end = int.Parse(Console.ReadLine()!);

Thread thread = new(() => PrintEvenNumbers(start, end));

thread.Start();
thread.Join();

Console.WriteLine("Thread finished work");

static void PrintEvenNumbers(int start, int end)
{
    for (int i = start; i <= end; i++)
        if (i % 2 == 0)
            Console.WriteLine(i);
}