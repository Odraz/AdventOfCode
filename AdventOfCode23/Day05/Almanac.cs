using System.Data;
using System.Text.RegularExpressions;

public class Almanac
{
    private const string NUMBER_REGEX = @"\d+";
    private const string NUMBER_PAIR_REGEX = @"\d+ \d+";

    public long GetLowestLocation()
    {
        StreamReader streamReader = new("Day05/input01.txt");

        string? line;
        
        List<(long, long)> seedPairs = new();
        List<Section> sections = new();

        while ((line = streamReader.ReadLine()) != null)
        {
            if (string.IsNullOrEmpty(line))
                continue;
            else if (line.StartsWith("seeds"))
                seedPairs = GetSeeds(line);
            else if (line.Contains("map"))
                sections.Add(new Section { Id = line.Split(' ')[0].Trim() });
            else
                sections.Last().Maps.Add(new Map
                {
                    DestinationStart = long.Parse(Regex.Matches(line, NUMBER_REGEX).ElementAt(0).Value),
                    SourceStart = long.Parse(Regex.Matches(line, NUMBER_REGEX).ElementAt(1).Value),
                    Length = long.Parse(Regex.Matches(line, NUMBER_REGEX).ElementAt(2).Value)
                });
        }

        List<long> locations = new();
        List<(long, long)> shortcuts = new();
        foreach ((long, long) seedPair in seedPairs)
        {
            foreach (long seed in SeedEnumeration(seedPair))
            {
                (long, long) shortcut = shortcuts.FirstOrDefault(s => s.Item1 == seed);
                if (shortcut != default)
                {
                    locations.Add(shortcut.Item2);
                    continue;
                }

                long element = seed;
                foreach (Section section in sections)
                    element = section.GetDestination(element);

                locations.Add(element);
                shortcuts.Add((seed, element));
            }
        }

        return locations.Min();
    }

    private List<(long, long)> GetSeeds(string line)
    {
        return Regex.Matches(line, NUMBER_PAIR_REGEX).Select(match => (long.Parse(match.Value.Split(' ')[0]), long.Parse(match.Value.Split(' ')[1]))).ToList();
    }

    public IEnumerable<long> SeedEnumeration ((long, long) seed)
    {
        long element = seed.Item1;
        while (element < seed.Item1 + seed.Item2 - 1)
        {
            yield return element;
            element++;
        }
    }
}

public class Section{
    public string Id { get; set; } = string.Empty;
    public List<Map> Maps { get; set; } = new();
    
    public long GetDestination(long element)
    {
        foreach (Map map in Maps)
        {
            if (map.IsInSourceRange(element))
                return element - map.Diff;
        }

        return element;
    }
}

public class Map
{
    public long SourceStart { get; set; }
    public long DestinationStart { get; set; }
    public long Length { get; set; }

    public long SourceEnd => SourceStart + Length - 1;
    public bool IsInSourceRange(long element) => element >= SourceStart && element <= SourceEnd;
    public long Diff => SourceStart - DestinationStart;
}