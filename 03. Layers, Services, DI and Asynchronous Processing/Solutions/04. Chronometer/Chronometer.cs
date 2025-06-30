using System.Diagnostics;

namespace _04._Chronometer;

public class Chronometer : IChronometer
{
    private readonly Stopwatch _timer = new();

    public Chronometer() => Laps = new();

    public string GetTime => FormatTime(_timer);

    public List<string> Laps { get; }

    public void Start() => _timer.Start();

    public void Stop() => _timer.Stop();

    public string Lap()
    {
        string lap = GetTime;
        Laps.Add(lap);
        return lap;
    }

    public void Reset()
    {
        _timer.Reset();
        Laps.Clear();
    }

    private string FormatTime(Stopwatch s) =>
        $"{s.Elapsed.Minutes:D2}:{s.Elapsed.Seconds:D2}:{s.Elapsed.Milliseconds:D4}";
}