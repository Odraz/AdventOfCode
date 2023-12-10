using AdventOfCode.Interfaces;

enum Direction
{
    None,
    North,
    South,
    East,
    West
}

public class Day10 : IDay
{
    private const char START_POSITION = 'S';
    private const char EMPTY_SPACE = '.';
    private const char LOOP = 'X';

    private IDictionary<Direction, (int X, int Y)> directions = new Dictionary<Direction, (int, int)>
    {
        {Direction.None, (0, 0)},
        {Direction.North, (0, -1)},
        {Direction.South, (0, 1)},
        {Direction.East, (1, 0)},
        {Direction.West, (-1, 0)},
    };

    private IDictionary<Direction, Direction> oppositeDirections = new Dictionary<Direction, Direction>
    {
        {Direction.North, Direction.South},
        {Direction.South, Direction.North},
        {Direction.East, Direction.West},
        {Direction.West, Direction.East},
    };

    private IDictionary<char, (Direction, Direction)> pipes = new Dictionary<char, (Direction, Direction)>
    {
        {'S', (Direction.None, Direction.None)},
        {'|', (Direction.North, Direction.South)},
        {'-', (Direction.East, Direction.West)},
        {'L', (Direction.North, Direction.East)},
        {'7', (Direction.South, Direction.West)},
        {'J', (Direction.North, Direction.West)},
        {'F', (Direction.South, Direction.East)}
    };

    private string[] lines = Array.Empty<string>();
    private string[] loopLines = Array.Empty<string>();

    public Day10(string input)
    {
        lines = File.ReadAllLines(input);
        loopLines = File.ReadAllLines(input);
    }

    public object SolveOne()
    {
        return DrawLoop();
    }

    private int DrawLoop()
    {
        (int X, int Y) startPosition = lines.Select((line, index) => (line, index))
                    .Where(line => line.line.Contains(START_POSITION))
                    .Select(line => (line.line.IndexOf(START_POSITION), line.index))
                    .First();

        ((int X, int Y), Direction) currentPosition = (startPosition, Direction.None);

        Direction startDirection;

        foreach (Direction direction in directions.Keys.Where(direction => direction != Direction.None))
        {
            (int X, int Y) nextPosition = (currentPosition.Item1.X + directions[direction].X, currentPosition.Item1.Y + directions[direction].Y);

            if (nextPosition.X < 0 || nextPosition.X >= lines[0].Length || nextPosition.Y < 0 || nextPosition.Y >= lines.Length)
                continue;

            char nextChar = lines[nextPosition.Y][nextPosition.X];

            if (nextChar == START_POSITION)
            {
                throw new Exception("Found second start position.");
            }
            else if (nextChar == EMPTY_SPACE)
            {
                continue;
            }
            else if (direction == oppositeDirections[pipes[nextChar].Item1] || direction == oppositeDirections[pipes[nextChar].Item2])
            {
                Direction nextDirection = pipes[nextChar].Item1 == direction ? pipes[nextChar].Item2 : pipes[nextChar].Item1;
                currentPosition = (nextPosition, nextDirection);
                break;
            }
        }

        int steps = 1;

        do
        {
            char nextChar = lines[currentPosition.Item1.Y][currentPosition.Item1.X];
            Direction nextDirection = pipes[nextChar].Item1 == currentPosition.Item2 ? pipes[nextChar].Item2 : pipes[nextChar].Item1;

            char[] lineChars = loopLines[currentPosition.Item1.Y].ToCharArray();
            lineChars[currentPosition.Item1.X] = LOOP;
            loopLines[currentPosition.Item1.Y] = new string(lineChars);

            (int X, int Y) nextPosition = (currentPosition.Item1.X + directions[nextDirection].X, currentPosition.Item1.Y + directions[nextDirection].Y);

            currentPosition = (nextPosition, oppositeDirections[nextDirection]);

            steps++;
        } while (currentPosition.Item1 != startPosition);

        return steps / 2;
    }

    public object SolveTwo()
    {
        DrawLoop();

        int tilesEnclosed = 0;

        for (int l_i = 0; l_i < lines.Length; l_i++)
        {
            string line = lines[l_i];
            bool isInsideLoop = false;

            char? previousCorner = null;

            for (int c_i = 0; c_i < line.Length; c_i++)
            {
                char c = line[c_i];

                bool isJunk = loopLines[l_i][c_i] != LOOP;

                if (c == '|' && !isJunk)
                {
                    isInsideLoop = !isInsideLoop;
                }
                else if ((c == 'F' || c == '7' || c == 'L' || c == 'J' || c == 'S') && !isJunk)
                {
                    if (previousCorner != null &&
                        (previousCorner == 'F' && c == 'J' || previousCorner == 'L' && c == '7' || previousCorner == 'S' && c == 'J'))
                    {
                        isInsideLoop = !isInsideLoop;
                        previousCorner = null;
                    }
                    else
                        previousCorner = c;

                }
                else if (isInsideLoop && isJunk && c != START_POSITION)
                {
                    tilesEnclosed++;
                }
            }
        }

        return tilesEnclosed;
    }
}