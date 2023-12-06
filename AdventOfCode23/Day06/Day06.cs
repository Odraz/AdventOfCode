using System.Text.RegularExpressions;

public class Day06 : IDay
{
    private string[] lines;

    private List<(long Time, long Distance)> BestRecords = new List<(long Time, long Distance)>();

    public Day06(string filePath){
        lines = File.ReadAllLines(filePath);
    }

    public object SolveOne()
    {
        long[] times = Regex.Matches(lines[0], @"\d+").Select(m => long.Parse(m.Value)).ToArray();
        long[] distances = Regex.Matches(lines[1], @"\d+").Select(m => long.Parse(m.Value)).ToArray();

        return Solve(times, distances);
    }

    private long Solve(long[] times, long[] distances)
    {
        Console.WriteLine($"Times: {string.Join(", ", times)}");
        Console.WriteLine($"Distances: {string.Join(", ", distances)}");

        BestRecords = new List<(long Time, long Distance)>();

        for (long i = 0; i < times.Length; i++)
        {
            BestRecords.Add((times[i], distances[i]));
        }

        return BestRecords.Select(GetWaysToWin).Aggregate((a, b) => a * b);
    }

    private long GetWaysToWin((long Time, long Distance) record)
    {
        return LongEnumerable(1, record.Time).Where(buttonTime => GetDistance(record.Time - buttonTime, buttonTime) > record.Distance).Count();
    }

    private IEnumerable<long> LongEnumerable(long start, long end)
    {
        for (long i = start; i <= end; i++)
        {
            yield return i;
        }
    }

    private long GetDistance(long time, long speed)
    {
        return time * speed;
    }

    public object SolveTwo()
    {
        long[] times = Regex.Matches(lines[0].Replace(" ", string.Empty), @"\d+").Select(m => long.Parse(m.Value)).ToArray();
        long[] distances = Regex.Matches(lines[1].Replace(" ", string.Empty), @"\d+").Select(m => long.Parse(m.Value)).ToArray();

        return Solve(times, distances);
    }
}