using AdventOfCode.Interfaces;

public class Day21 : IDay
{
    private string[] lines;

    public Day21(string path)
    {
        lines = File.ReadAllLines(path);
    }

    public object SolveOne()
    {
        // Start at position 'S' in grid
        // Example input:
        // ...........
        // .....###.#.
        // .###.##..#.
        // ..#.#...#..
        // ....#.#....
        // .##..S####.
        // .##..#...#.
        // .......##..
        // .##.#.####.
        // .##..##.##.
        // ...........
        (int X, int Y) startingPosition = lines
            .Select((line, y) => (line, y))
            .SelectMany(line => line.line.Select((c, x) => (c, x)))
            .First(c => c.c == 'S');
        
        return 0;
    }

    public object SolveTwo()
    {
        throw new NotImplementedException();
    }
}