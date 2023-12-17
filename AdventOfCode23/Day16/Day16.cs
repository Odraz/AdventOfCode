using AdventOfCode.Interfaces;

public class Day16 : IDay
{
    string[] lines;
    string[] energizedGrid = new string[0];
    List<Beam> startingBeams = new();

    public Day16(string inputPath)
    {
        lines = File.ReadAllLines(inputPath);
    }

    private void InitializeGrid()
    {
        energizedGrid = new string[lines.Length];

        for (int i = 0; i < lines.Length; i++)
            energizedGrid[i] = new string('.', lines.Length);

        startingBeams = new List<Beam>();
    }

    public object SolveOne()
    {
        return GetEnergizedTiles(-1, 0, 1, 0);
    }

    private int GetEnergizedTiles(int x, int y, int directionX, int directionY)
    {
        int gridWidth = lines.Length;
        
        InitializeGrid();

        List<Beam> beams = new() { new Beam(x, y, (directionX, directionY)) };
        startingBeams.Add(new Beam(0, 0, (1, 0)));

        energizedGrid[0] = energizedGrid[0].Remove(0, 1).Insert(0, "#");

        while (beams.Any())
        {
            List<Beam> newBeams = new();
            List<Beam> beamsToRemove = new();

            foreach (Beam beam in beams)
            {
                beam.X += beam.Direction.X;
                beam.Y += beam.Direction.Y;

                switch (lines[beam.Y][beam.X])
                {
                    case '.':
                        break;
                    case '|':
                        if (beam.Direction.Y == 0)
                        {
                            newBeams.Add(new Beam(beam.X, beam.Y, (0, 1)));
                            newBeams.Add(new Beam(beam.X, beam.Y, (0, -1)));
                            beamsToRemove.Add(beam);
                        }
                        break;
                    case '-':
                        if (beam.Direction.X == 0)
                        {
                            newBeams.Add(new Beam(beam.X, beam.Y, (1, 0)));
                            newBeams.Add(new Beam(beam.X, beam.Y, (-1, 0)));
                            beamsToRemove.Add(beam);
                        }
                        break;
                    case '/':
                        beam.Direction = (-beam.Direction.Y, -beam.Direction.X);
                        break;
                    case '\\':
                        beam.Direction = (beam.Direction.Y, beam.Direction.X);
                        break;
                }

                energizedGrid[beam.Y] = energizedGrid[beam.Y].Remove(beam.X, 1).Insert(beam.X, "#");

                (int X, int Y, int DirectionX, int DirectionY) newPosition = (beam.X, beam.Y, beam.Direction.X, beam.Direction.Y);

                if (beam.Path.Any(p => p.X == newPosition.X && p.Y == newPosition.Y && p.DirectionX == newPosition.DirectionX && p.DirectionY == newPosition.DirectionY))
                {
                    beamsToRemove.Add(beam);
                    continue;
                }
                else
                {
                    beam.Path.Add(newPosition);
                }
            }

            List<Beam> newValidBeams = newBeams.Where(b => !HasBeenAlreadyCreated(b) && !IsOutOfBounds(b)).ToList();

            beams.AddRange(newValidBeams.ToList());
            startingBeams.AddRange(newValidBeams.Select(b => new Beam(b.X, b.Y, b.Direction)));

            beams.RemoveAll(beamsToRemove.Contains);
            beams.RemoveAll(IsOutOfBounds);
        }

        return energizedGrid.Sum(l => l.Count(c => c == '#'));
    }

    private bool HasBeenAlreadyCreated(Beam b)
    {
        return startingBeams.Any(sb => sb.X == b.X && sb.Y == b.Y && sb.Direction.X == b.Direction.X && sb.Direction.Y == b.Direction.Y);
    }

    private bool IsOutOfBounds(Beam b)
    {
        (int X, int Y) newPosition = (b.X + b.Direction.X, b.Y + b.Direction.Y);

        return newPosition.X < 0 || newPosition.X >= lines.Length || newPosition.Y < 0 || newPosition.Y >= lines.Length;
    }

    public object SolveTwo()
    {
        int max = 0;

        for (int i = 0; i < lines.Length; i++)
        {
            int energizedTiles = GetEnergizedTiles(-1, i, 1, 0);

            if (energizedTiles > max)
                max = energizedTiles;
        }

        for (int i = 0; i < lines.Length; i++)
        {
            int energizedTiles = GetEnergizedTiles(i, -1, 0, 1);

            if (energizedTiles > max)
                max = energizedTiles;
        }

        for (int i = 0; i < lines.Length; i++)
        {
            int energizedTiles = GetEnergizedTiles(lines.Length, i, -1, 0);

            if (energizedTiles > max)
                max = energizedTiles;
        }

        for (int i = 0; i < lines.Length; i++)
        {
            int energizedTiles = GetEnergizedTiles(i, lines.Length, 0, -1);

            if (energizedTiles > max)
                max = energizedTiles;
        }

        return max;
    }

    class Beam
    {
        public int X { get; set; }
        public int Y { get; set; }
        public (int X, int Y) Direction { get; set; }
        public List<(int X, int Y, int DirectionX, int DirectionY)> Path { get; set; } = new();

        public Beam(int x, int y, (int X, int Y) direction)
        {
            X = x;
            Y = y;
            Direction = direction;
            Path.Add((x, y, direction.X, direction.Y));
        }
    }
}