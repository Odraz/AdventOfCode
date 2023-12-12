using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Interfaces;

public class Day12 : IDay
{
    private const char DAMAGED = '#';
    private const char UNDAMAGED = '.';
    private const char UNKNOWN = '?';

    private int arrangements = 0;

    private string[] lines = Array.Empty<string>();
    private IEnumerable<Row> rows = Array.Empty<Row>();

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
        // @"(^|\.|\?)([\?\#]{3})[\.\?]([\?\#]{2})[\.\?]([\?\#]{1})(\.|\?|$|\n)";
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
        
        List<string> combinations = new List<string>();
        
        GenerateCombinations(line, combinations, 0);
        Console.WriteLine($"line: {line}");

        int matches = 0;

        foreach (string combination in combinations)
        {
            Match match = Regex.Match(combination, sb.ToString());
            
            string trimDots = combination.Trim('.');

            bool isMatch = match.Success && match.Value.Trim('.') == trimDots;

            if (isMatch){
                arrangements++;
                matches++;
            }

            Console.WriteLine($"combination: {combination}, trim: {trimDots}, isMatch: {isMatch}");
        }

        Console.WriteLine($"line: {line}, matches: {matches}");

    }

    private static void GenerateCombinations(string input, List<string> combinations, int index)
    {
        if (index == input.Length)
        {
            combinations.Add(input);
            return;
        }

        if (input[index] == '?')
        {
            GenerateCombinations(input.Remove(index, 1).Insert(index, "."), combinations, index + 1);
            GenerateCombinations(input.Remove(index, 1).Insert(index, "#"), combinations, index + 1);
        }
        else
        {
            GenerateCombinations(input, combinations, index + 1);
        }
    }
    public object SolveTwo()
    {
        throw new NotImplementedException();
    }
}

class Row
{
    public string Line { get; } = string.Empty;
    public List<int> DamagedGroups { get; } = new List<int>();

    public Row(string line, IEnumerable<int> damagedGroups)
    {
        Line = line;
        DamagedGroups = damagedGroups.ToList();
    }
}