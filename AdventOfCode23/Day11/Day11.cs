using AdventOfCode.Interfaces;

public class Day11 : IDay
{
    private readonly int MULTIPLIER = 1_000_000;

    private string[] lines = Array.Empty<string>();
    private string[] expandedMap = Array.Empty<string>();
    private string[] markedExpandedMap = Array.Empty<string>();

    public Day11(string input)
    {
        lines = File.ReadAllLines(input);
        markedExpandedMap = lines;

        for (int i = 0; i < lines.Length; i++)
        {
            string? line = lines[i];
            expandedMap = expandedMap.Append(line).ToArray();

            if (!line.Contains('#'))
            {
                expandedMap = expandedMap.Append(line).ToArray();
                markedExpandedMap[i] = string.Concat(Enumerable.Repeat('X', line.Length));
            }
        }

        for (int i = 0, k = 0; i < lines[0].Length; i++, k++)
        {
            string? column = GetColumn(k);

            if (!column.Contains('#'))
            {
                expandedMap = expandedMap.Select((line, index) => line.Insert(k, column[index].ToString())).ToArray();
                k++;

                markedExpandedMap = markedExpandedMap.Select((line, index) => line[i] == 'X' ? ReplaceAt(line, i, "O") : ReplaceAt(line, i, "Y")).ToArray();
            }
        }
    }

    private string ReplaceAt(string line, int i, string newValue)
    {
        return line.Substring(0, i) + newValue + line.Substring(i + 1);
    }

    private string GetColumn(int i)
    {
        string column = string.Empty;

        foreach (string? line in expandedMap)
            column += line[i];

        return column;
    }

    public object SolveOne()
    {
        int sum = 0;
        (int X, int Y)[] galaxies = GetGalaxies(expandedMap);

        for (int i = 0; i < galaxies.Length; i++)
        {
            for (int j = i + 1; j < galaxies.Length; j++)
            {
                (int x1, int y1) = galaxies[i];
                (int x2, int y2) = galaxies[j];

                int distance = Math.Abs(x1 - x2) + Math.Abs(y1 - y2);

                sum += distance;
            }
        }

        return sum;
    }

    private (int X, int Y)[] GetGalaxies(string[] map)
    {
        return map
            .Select((line, y) => line
                                     .Select((c, x) => (c, x))
                                     .Where(c => c.c == '#')
                                     .Select(c => (c.x, y)))
            .SelectMany(c => c)
            .ToArray();
    }

    public object SolveTwo()
    {
        long sum = 0;
        (int X, int Y)[] galaxies = GetGalaxies(markedExpandedMap);

        for (int i = 0; i < galaxies.Length; i++)
        {
            for (int j = i + 1; j < galaxies.Length; j++)
            {
                (int x1, int y1) = galaxies[i];
                (int x2, int y2) = galaxies[j];

                int distance = Math.Abs(x1 - x2) + Math.Abs(y1 - y2);

                sum += distance;

                for (int x = Math.Min(x1, x2); x < Math.Max(x1, x2); x++)
                {
                    if (markedExpandedMap[y1][x] == 'Y' || markedExpandedMap[y1][x] == 'O')
                        sum += MULTIPLIER - 1;
                }                

                for (int y = Math.Min(y1, y2); y < Math.Max(y1, y2); y++)
                {
                    if (markedExpandedMap[y][x1] == 'X' || markedExpandedMap[y][x1] == 'O')
                        sum += MULTIPLIER - 1;
                }
            }
        }

        return sum;
    }
} 