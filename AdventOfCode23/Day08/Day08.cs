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

        ulong steps = 1;

        while (currentLocations.Any(location => !endingLocations.Contains(location)))
        {
            foreach(char instruction in instructions)
            {
                currentLocations = currentLocations.Select(location => {
                    return instruction switch
                    {
                        LEFT => nodes[location].L,
                        RIGHT => nodes[location].R,
                        _ => throw new NotImplementedException()
                    };
                }).ToArray();
                
                if (currentLocations.All(location => endingLocations.Contains(location)))
                    break;

                steps++;
            }
        }

        return steps;
    }
}