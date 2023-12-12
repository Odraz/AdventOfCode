using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Interfaces;

public class Day12 : IDay
{
    private const char DAMAGED = '#';
    private const char UNDAMAGED = '.';
    private const char UNKNOWN = '?';

    private decimal arrangements = 0;

    private string[] lines = Array.Empty<string>();
    private IEnumerable<Row> rows = Array.Empty<Row>();

    //private IDictionary<List<int>, List<string>> correctConbinations = new Dictionary<List<int>, List<string>>();

    public Day12(string path)
    {
        lines = File.ReadAllLines(path);
        rows = lines.Select(line => new Row(line.Split(' ')[0], line.Split(' ')[1].Split(',').Select(i => int.Parse(i.ToString()))));
    }

    public object SolveOne()
    {
        foreach (Row row in rows)
            CountArrangements(row.Line, row.DamagedGroups);
        
        return arrangements;
    }

    private void CountArrangements(string line, List<int> damagedGroups)
    {
        StringBuilder sb = new StringBuilder();
        // @"(\n|^|\.|\?)([\?\#]{1})[\.\?]+([\?\#]{6})[\.\?]+([\?\#]{5})([\.\?]+|$|\n)";
        sb.Append(@"(\n|^|\.|\?)([\?\#]{");

        for (int i = 0; i < damagedGroups.Count; i++)
        {
            int group = damagedGroups[i];
            sb.Append(group);

            if (i == damagedGroups.Count - 1)
                sb.Append(@"})([\.\?]+|$|\n)");
            else
                sb.Append(@"})[\.\?]+([\?\#]{");
        }
        
        int matches = 0;

        foreach (string combination in Combinations(line, 0))
        {
            Match match = Regex.Match(combination, sb.ToString());
            
            string trimDots = combination.Trim('.');

            bool isMatch = match.Success && match.Value.Trim('.') == trimDots;

            if (isMatch){
                arrangements++;
                matches++;
                //correctConbinations[damagedGroups].Add(combination);
            }

            //Console.WriteLine($"line: {line}, combination: {combination}, isMatch: {isMatch}");
        }

        Console.WriteLine($"line: {line}, matches: {matches}");

    }

    private static IEnumerable<string> Combinations(string input, int index)
    {
        if (index == input.Length)
        {
            yield return input;
        }
        else
        {
            if (input[index] == '?')
            {
                foreach (var combination in Combinations(input.Remove(index, 1).Insert(index, "."), index + 1))
                {
                    yield return combination;
                }

                foreach (var combination in Combinations(input.Remove(index, 1).Insert(index, "#"), index + 1))
                {
                    yield return combination;
                }
            }
            else
            {
                foreach (var combination in Combinations(input, index + 1))
                {
                    yield return combination;
                }
            }
        }
    }

    public object SolveTwo()
    {
        foreach (Row row in rows)
        {
            IEnumerable<int> groups = row.DamagedGroups;
            for (int i = 0; i < 4; i++)
                row.DamagedGroups = row.DamagedGroups.Concat(groups).ToList();

            CountArrangements(string.Concat(RepeatString(row.Line, 5).SkipLast(1)), row.DamagedGroups);
        }

        return arrangements;
    }

    public string RepeatString(string text, uint n)
    {
        return new StringBuilder(text.Length * (int)n)
          .Insert(0, text + '?', (int)n)
          .ToString();
    }
}

class Row
{
    public string Line { get; } = string.Empty;
    public List<int> DamagedGroups { get; set; } = new List<int>();

    public Row(string line, IEnumerable<int> damagedGroups)
    {
        Line = line;
        DamagedGroups = damagedGroups.ToList();
    }
}