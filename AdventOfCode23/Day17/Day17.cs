using AdventOfCode.Interfaces;

public class Day17 : IDay
{
    string[] lines;
    int[][] grid;

    public Day17(string inputPath)
    {
        lines = File.ReadAllLines(inputPath);
        grid = lines.Select(line => line.Select(c => int.Parse(c.ToString())).ToArray()).ToArray();
    }

    public object SolveOne()
    {
        (int[][] Distances, (int X, int Y, int DirectionX, int DirectionY, int repeats)[][] Previous) shortestPath = Dijkstra();

        Console.WriteLine("Distances:");
        Console.WriteLine(string.Join(Environment.NewLine, shortestPath.Distances.Select(line => string.Join(" ", line))));

        Console.WriteLine("Path:");
        PrintPath(shortestPath);

        return shortestPath.Distances[grid.Length - 1][grid.Length - 1];
    }

    private void PrintPath((int[][] Distances, (int X, int Y, int DirectionX, int DirectionY, int repeats)[][] Previous) shortestPath)
    {
        (int X, int Y) current = (grid.Length - 1, grid.Length - 1);

        string[] linesWithPath = lines.ToArray();

        while (current != (0, 0))
        {
            linesWithPath[current.Y] = linesWithPath[current.Y].Remove(current.X, 1).Insert(current.X, "#");
            current = (shortestPath.Previous[current.Y][current.X].X, shortestPath.Previous[current.Y][current.X].Y);
        }

        Console.WriteLine(string.Join(Environment.NewLine, linesWithPath));
    }

    public (int[][] Distances, (int X, int Y, int DirectionX, int DirectionY, int repeats)[][] Previous) Dijkstra()
    {
        int[][] distances = new int[grid.Length][];
        (int X, int Y, int DirectionX, int DirectionY, int Repeats)[][] previous = new (int X, int Y, int DirectionX, int DirectionY, int repeats)[grid.Length][];

        List<(int X, int Y)> queue = new List<(int X, int Y)>();
        List<(int X, int Y)> resolved = new List<(int X, int Y)>();

        for (int y = 0; y < grid.Length; y++)
        {
            distances[y] = new int[grid.Length];
            previous[y] = new (int X, int Y, int DirectionX, int DirectionY, int repeats)[grid.Length];

            for (int x = 0; x < grid.Length; x++)
            {
                distances[y][x] = int.MaxValue;

                queue.Add((x, y));
            }
        }

        distances[0][0] = 0;

        while (queue.Count > 0)
        {
            (int X, int Y) u = queue.OrderBy(v => distances[v.Y][v.X]).ThenBy(v => previous[v.Y][v.X].Repeats).First();
            queue.Remove(u);
            resolved.Add(u);

            foreach ((int X, int Y) v in GetNeighbors(u).Where(v => !resolved.Contains(v)))
            {
                int alt = distances[u.Y][u.X] + grid[v.Y][v.X];

                // if the new distance is shorter than the previous distance, we can update the distance                
                if (alt < distances[v.Y][v.X])
                {
                    distances[v.Y][v.X] = alt;

                    int repeatsOfSameDirection = 0;
                    repeatsOfSameDirection = previous[u.Y][u.X].DirectionX == v.X - u.X && previous[u.Y][u.X].DirectionY == v.Y - u.Y ? previous[u.Y][u.X].Repeats + 1 : 0;
                    previous[v.Y][v.X] = (u.X, u.Y, v.X - u.X, v.Y - u.Y, repeatsOfSameDirection);
                }
            }
        }

        return (distances, previous);
    }

    private List<(int X, int Y)> GetNeighbors((int X, int Y) u)
    {
        List<(int X, int Y)> neighbors = new List<(int X, int Y)>();

        if (u.X > 0)
            neighbors.Add((u.X - 1, u.Y));
        if (u.X < grid.Length - 1)
            neighbors.Add((u.X + 1, u.Y));
        if (u.Y > 0)
            neighbors.Add((u.X, u.Y - 1));
        if (u.Y < grid.Length - 1)
            neighbors.Add((u.X, u.Y + 1));

        return neighbors;
    }

    public object SolveTwo()
    {
        throw new NotImplementedException();
    }
}