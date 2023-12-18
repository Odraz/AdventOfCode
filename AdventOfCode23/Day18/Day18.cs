using AdventOfCode.Interfaces;

public class Day18 : IDay
{
    const char DIRECTION_UP = 'U';
    const char DIRECTION_DOWN = 'D';
    const char DIRECTION_LEFT = 'L';
    const char DIRECTION_RIGHT = 'R';
    const char TRENCH = '#';
    const char OPEN = '.';

    string[] lines;

    public Day18(string path)
    {
        lines = File.ReadAllLines(path);
    }

    public object SolveOne()
    {
        return GetInteriorCubicMeters(lines);
    }

    private object GetInteriorCubicMeters(string[] lines)
    {
        return GetPolygonArea(lines);

        // Original solution for part one using flood fill but unusable for part two
        // List<string> map = new List<string>() { "..." , ".#.", "..."};

        // (int x, int y) currentPosition = (1, 1);

        // foreach (string line in lines)
        // {
        //     string[] parts = line.Split(' ');
        //     char direction = parts[0][0];
        //     int length = int.Parse(parts[1]);

        //     for (int i = 0; i < length; i++)
        //     {
        //         switch (direction)
        //         {
        //             case DIRECTION_UP:
        //                 currentPosition.y--;
        //                 break;
        //             case DIRECTION_DOWN:
        //                 currentPosition.y++;
        //                 break;
        //             case DIRECTION_LEFT:
        //                 currentPosition.x--;
        //                 break;
        //             case DIRECTION_RIGHT:
        //                 currentPosition.x++;
        //                 break;
        //         }

        //         if (map.Count - 1 == currentPosition.y)
        //         {
        //             map.Insert(map.Count - 1, new string(OPEN, map[0].Length));
        //         }
        //         else if (currentPosition.y == 0)
        //         {
        //             map.Insert(1, new string(OPEN, map[0].Length));

        //             currentPosition.y = 1;
        //         }
        //         else if (currentPosition.x == 0)
        //         {
        //             for (int i1 = 0; i1 < map.Count; i1++)
        //             {
        //                 map[i1] = map[i1].Insert(1, OPEN.ToString());
        //             }

        //             currentPosition.x = 1;
        //         }
        //         else if (currentPosition.x == map[0].Length - 1)
        //         {
        //             for (int r = 0; r < map.Count; r++)
        //                 map[r] = map[r].Insert(map[r].Length - 1, OPEN.ToString());
        //         }

        //         map[currentPosition.y] = map.ElementAt(currentPosition.y).Remove(currentPosition.x, 1).Insert(currentPosition.x, TRENCH.ToString());
        //     }
        // }

        // FloodFill(map);

        // // Print map
        // Console.WriteLine(string.Join(Environment.NewLine, map));

        // return map.Sum(x => x.LongCount(y => y == OPEN || y == TRENCH));
    }

    private void FloodFill(List<string> map)
    {
        Queue<(int x, int y)> queue = new Queue<(int x, int y)>();
        queue.Enqueue((0, 0));

        while (queue.Count > 0)
        {
            (int x, int y) = queue.Dequeue();

            if (map[y][x] == OPEN)
            {
                map[y] = map[y].Remove(x, 1).Insert(x, 'X'.ToString());

                if (x > 0)
                    queue.Enqueue((x - 1, y));

                if (x < map[0].Length - 1)
                    queue.Enqueue((x + 1, y));

                if (y > 0)
                    queue.Enqueue((x, y - 1));

                if (y < map.Count - 1)
                    queue.Enqueue((x, y + 1));
            }
        }
    }

    public object SolveTwo()
    {
        long[] distances = lines.Select(x => long.Parse(string.Concat(x.Split(' ')[2].Skip(2).SkipLast(2)), System.Globalization.NumberStyles.HexNumber)).ToArray();
        char[] directions = lines.Select(x => {
            char number = x.SkipLast(1).Last();
            if (number == '0')
                return DIRECTION_RIGHT;
            else if (number == '1')
                return DIRECTION_DOWN;
            else if (number == '2')
                return DIRECTION_LEFT;
            else
                return DIRECTION_UP;
        }).ToArray();

        string[] instructions = new string[distances.Length];
        
        for (int i = 0; i < distances.Length; i++)
            instructions[i] = $"{directions[i]} {distances[i]}";

        return GetPolygonArea(instructions);
    }

    private long GetPolygonCircumference(string[] instructions)
    {
        return instructions.Select(x => long.Parse(x.Split(' ')[1])).Sum() / 2;
    }

    private long GetPolygonArea(string[] instructions)
    {
        List<(long x, long y)> points = new List<(long x, long y)>() { (0, 0) };

        (long x, long y) currentPosition = (0, 0);

        foreach (string instruction in instructions)
        {
            string[] parts = instruction.Split(' ');
            char direction = parts[0][0];
            int length = int.Parse(parts[1]);

            switch (direction)
            {
                case DIRECTION_UP:
                    points.Add(new (currentPosition.x, currentPosition.y - length));
                    break;
                case DIRECTION_DOWN:
                    points.Add(new (currentPosition.x, currentPosition.y + length));
                    break;
                case DIRECTION_LEFT:
                    points.Add(new (currentPosition.x - length, currentPosition.y));
                    break;
                case DIRECTION_RIGHT:
                    points.Add(new (currentPosition.x + length, currentPosition.y));
                    break;
            }

            currentPosition = points.Last();
        }

        points = points.Distinct().ToList();

        // Print points
        Console.WriteLine(string.Join(Environment.NewLine, points));

        var area = Math.Abs(points.Take(points.Count - 1).Select((p, i) => (points[i + 1].x - p.x) * (points[i + 1].y + p.y)).Sum() / 2);

        return area + GetPolygonCircumference(instructions) + 1;
    }
}