using AdventOfCode.Interfaces;

using System.Text.RegularExpressions;

public class Day08 : IDay
{
    private const char LEFT = 'L';
    private const char RIGHT = 'R'; 

    private const string START_LOCATION = "AAA";
    private const string FINAL_LOCATION = "ZZZ";

    private const string WORD = @"[A-Z]+";

    private string[] lines;

    private string instructions;
    private IDictionary<string, (string L, string R)> nodes = new Dictionary<string, (string L, string R)>();

    public Day08(string filePath)
    {
        lines = File.ReadAllLines(filePath);
        
        instructions = lines[0];
    }

    public object SolveOne()
    {
        lines.Skip(2).Select(line => Regex.Matches(line, WORD)).ToList().ForEach(match => {
            nodes.Add(match[0].Value, (match[1].Value, match[2].Value));
        });

        int steps = 1;
        string currentLocation = START_LOCATION;

        while (currentLocation != FINAL_LOCATION)
        {            
            instructions.ToList().ForEach(instruction => {
                switch (instruction)
                {
                    case LEFT:
                        currentLocation = nodes[currentLocation].L;
                        break;
                    case RIGHT:
                        currentLocation = nodes[currentLocation].R;
                        break;
                }

                if (currentLocation == FINAL_LOCATION)
                    return;

                steps++;
            }); 
        }

        return steps;
    }

    public object SolveTwo()
    {
        instructions = lines[0];

        lines.Skip(2).Select(line => Regex.Matches(line, WORD)).ToList().ForEach(match => {
            nodes.Add(match[0].Value, (match[1].Value, match[2].Value));
        });

        
        string[] currentLocations = nodes.Where(node => node.Key.EndsWith("A")).Select(node => node.Key).ToArray();

        string[] endingLocations = nodes.Where(node => node.Key.EndsWith("Z")).Select(node => node.Key).ToArray();

        long[] steps = Enumerable.Repeat(1L, currentLocations.Length).ToArray();

        for (int i = 0; i < currentLocations.Length; i++)
        {
            string currentLocation = currentLocations[i];
            while (!endingLocations.Contains(currentLocation))
            {      
                foreach (char instruction in instructions)
                {
                    switch (instruction)
                    {
                        case LEFT:
                            currentLocation = nodes[currentLocation].L;
                            break;
                        case RIGHT:
                            currentLocation = nodes[currentLocation].R;
                            break;
                    }

                    if (endingLocations.Contains(currentLocation))
                        break;

                    steps[i]++;
                } 
            }
        }

        return steps.Aggregate(LCM);
    }

    private long LCM(long a, long b) =>
        a * b / GCD(a, b);

    private long GCD(long a, long b) =>
        b == 0 ? a : GCD(b, a % b);
}