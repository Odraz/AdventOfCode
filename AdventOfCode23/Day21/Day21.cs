using AdventOfCode.Interfaces;

public class Day21 : IDay
{
    private string[] lines;
    private int gridWidth;

    private int startX;
    private int startY;

    public Day21(string path)
    {
        lines = File.ReadAllLines(path);
        gridWidth = lines.Length;

        startY = lines.Single(l => l.Contains('S')).IndexOf('S');
        startX = lines[startY].IndexOf('S');
    }

    public object SolveOne()
    {
        return BFS(startX, startY, 64);
    }

    private int BFS(int startX, int startY, int steps)
    {   
        List<(int x, int y, int steps)> visited = new();
        List<(int x, int y)> reached = new();        
        Queue<(int x, int y, int steps)> queue = new();
        
        queue.Enqueue((startX, startY, steps));
        visited.Add((startX, startY, steps));

        while (queue.Count > 0)
        {
            (int x, int y, int currSteps) = queue.Dequeue();

            if (currSteps == 0)
            {
                if (!reached.Contains((x, y)))
                    reached.Add((x, y));

                continue;
            }

            if (y > 0 && lines[y - 1][x] != '#' && !visited.Contains((x, y - 1, currSteps - 1)))
            {
                queue.Enqueue((x, y - 1, currSteps - 1));
                visited.Add((x, y - 1, currSteps - 1));
            }
            if (y < lines.Length - 1 && lines[y + 1][x] != '#' && !visited.Contains((x, y + 1, currSteps - 1)))
            {
                queue.Enqueue((x, y + 1, currSteps - 1));
                visited.Add((x, y + 1, currSteps - 1));
            }
            if (x > 0 && lines[y][x - 1] != '#' && !visited.Contains((x - 1, y, currSteps - 1)))
            {
                queue.Enqueue((x - 1, y, currSteps - 1));
                visited.Add((x - 1, y, currSteps - 1));
            }
            if (x < lines[0].Length - 1 && lines[y][x + 1] != '#' && !visited.Contains((x + 1, y, currSteps - 1)))
            {
                queue.Enqueue((x + 1, y, currSteps - 1));
                visited.Add((x + 1, y, currSteps - 1));
            }
        }

        return reached.Count;
    }

    // For 100 steps, this finishes in 88716ms
    public object SolveTwo()
    {
        return BFSInfiniteGrid(startX, startY, 100);
    }

    private int BFSInfiniteGrid(int startX, int startY, int steps)
    {   
        List<(int x, int y, int steps)> visited = new();
        List<(int x, int y)> reached = new();        
        Queue<(int x, int y, int steps)> queue = new();
        
        queue.Enqueue((startX, startY, steps));
        visited.Add((startX, startY, steps));

        int lastStep = steps;
        while (queue.Count > 0)
        {
            (int x, int y, int currSteps) = queue.Dequeue();

            if (currSteps == 0)
            {
                if (!reached.Contains((x, y)))
                    reached.Add((x, y));

                continue;
            }

            if (lines[Translate(y - 1)][Translate(x)] != '#' && !visited.Contains((x, y - 1, currSteps - 1)))
            {
                queue.Enqueue((x, y - 1, currSteps - 1));
                visited.Add((x, y - 1, currSteps - 1));
            }
            if (lines[Translate(y + 1)][Translate(x)] != '#' && !visited.Contains((x, y + 1, currSteps - 1)))
            {
                queue.Enqueue((x, y + 1, currSteps - 1));
                visited.Add((x, y + 1, currSteps - 1));
            }
            if (lines[Translate(y)][Translate(x - 1)] != '#' && !visited.Contains((x - 1, y, currSteps - 1)))
            {
                queue.Enqueue((x - 1, y, currSteps - 1));
                visited.Add((x - 1, y, currSteps - 1));
            }
            if (lines[Translate(y)][Translate(x + 1)] != '#' && !visited.Contains((x + 1, y, currSteps - 1)))
            {
                queue.Enqueue((x + 1, y, currSteps - 1));
                visited.Add((x + 1, y, currSteps - 1));
            }

            if (lastStep != currSteps)
            {
                lastStep = currSteps;
                Console.WriteLine($"Steps left: {currSteps}, Queue size: {queue.Count}, Visited unique size: {visited.DistinctBy(x => (x.x, x.y)).Count()}");
            }
        }

        return reached.Count;
    }

    private int Translate(int c) =>
    c < 0 ? (gridWidth - Math.Abs(c) % gridWidth) % gridWidth : Math.Abs(c) % gridWidth;
}