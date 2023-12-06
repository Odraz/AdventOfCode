public class GameValidator
{
    // private readonly int RED_CUBES_MAX = 12;
    // private readonly int GREEN_CUBES_MAX = 13;
    // private readonly int BLUE_CUBES_MAX = 14;

    public int Sum()
    {
        using StreamReader streamReader = new("Day02/input01.txt");

        string? line;
        int sum = 0;

        // Example of a line:"Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green"
        while ((line = streamReader.ReadLine()) != null)
        {
            Game game = ExtractGame(line);

            int redCubesMax = game.Draws.Max(d => d.RedCubes);
            int greenCubesMax = game.Draws.Max(d => d.GreenCubes);
            int blueCubesMax = game.Draws.Max(d => d.BlueCubes);

            int power = redCubesMax * greenCubesMax * blueCubesMax;

            sum += power;
        }

        return sum;
    }

    public Game ExtractGame(string line)
    {
        string[] parts = line.Split(":");
        string[] draws = parts[1].Split(";");

        Game game = new()
        {
            Id = int.Parse(parts[0].Split(" ")[1])
        };

        foreach (string draw in draws)
        {
            Draw drawObject = new();

            string[] cubes = draw.Split(",");

            foreach (string cube in cubes)
            {
                string[] cubeParts = cube.Trim().Split(" ");

                switch (cubeParts[1])
                {
                    case "red":
                        drawObject.RedCubes = int.Parse(cubeParts[0]);
                        break;
                    case "green":
                        drawObject.GreenCubes = int.Parse(cubeParts[0]);
                        break;
                    case "blue":
                        drawObject.BlueCubes = int.Parse(cubeParts[0]);
                        break;
                }
            }

            game.Draws.Add(drawObject);
        }

        return game;
    }
}

public class Game
{
    public int Id { get; set; }
    public List<Draw> Draws { get; set; } = new List<Draw>();
}

public class Draw
{
    public int RedCubes { get; set; }
    public int GreenCubes { get; set; }
    public int BlueCubes { get; set; }
}