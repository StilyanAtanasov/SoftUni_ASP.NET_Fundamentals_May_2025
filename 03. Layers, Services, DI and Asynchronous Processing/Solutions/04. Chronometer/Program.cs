using _04._Chronometer;

Chronometer chronometer = new();

while (true)
{
    string command = Console.ReadLine()!;
    switch (command)
    {
        case "exit": return;
        case "start":
            chronometer.Start();
            break;
        case "stop":
            chronometer.Stop();
            break;
        case "lap":
            chronometer.Lap();
            Console.WriteLine(chronometer.GetTime);
            break;
        case "laps":
            string[] laps = chronometer.Laps.ToArray();

            if (laps.Length == 0)
            {
                Console.WriteLine("Laps: no laps");
                break;
            }

            Console.WriteLine("Laps:");
            for (int i = 0; i < laps.Length; i++) Console.WriteLine($"{i}: {laps[i]}");
            break;
        case "time":
            Console.WriteLine(chronometer.GetTime);
            break;
        case "reset":
            chronometer.Reset();
            break;
    }
}