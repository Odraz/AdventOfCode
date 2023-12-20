using AdventOfCode.Interfaces;

public class Day14 : IDay
{
    private const char ROUNDED_ROCK = 'O';
    private const char CUBE_ROCK = '#';
    private const char EMPTY_SPACE = '.';

    IDictionary<string, long> Results = new Dictionary<string, long>();

    private string[] lines;

    public Day14(string path)
    {
        lines = File.ReadAllLines(path);
    }

    public object SolveOne()
    {
        long sum = 0;

        for (int i = 0; i < lines.Length - 1; i++)
        {
            for (int k = 0; k < lines.Length - 1; k++)
            {            
                lines[k] = string.Concat(lines[k].Select((c, c_i) =>
                {
                    if (c == EMPTY_SPACE && lines[k + 1][c_i] == ROUNDED_ROCK)
                    {
                        lines[k + 1] = lines[k + 1].Remove(c_i, 1).Insert(c_i, EMPTY_SPACE.ToString());
                        return ROUNDED_ROCK;
                    }
                    else
                    {
                        return c;
                    }
                }));                
            }
        }

        for (int i = 0; i < lines.Length; i++)
            sum += lines[i].Count(c => c == ROUNDED_ROCK) * (lines.Length - i);

        return sum;
    }

    public object SolveTwo()
    {
        long sum = 0;
    bool foundCycle = false;
        for (long c = 1; c <= 1000000000; c++)
        {
            Console.Write($"\rCycle:{c}");

            // ROLL NORTH
            for (int i = 0; i < lines.Length - 1; i++)
            {
                for (int k = 0; k < lines.Length - 1; k++)
                {            
                    lines[k] = string.Concat(lines[k].Select((c, c_i) =>
                    {
                        if (c == EMPTY_SPACE && lines[k + 1][c_i] == ROUNDED_ROCK)
                        {
                            lines[k + 1] = lines[k + 1].Remove(c_i, 1).Insert(c_i, EMPTY_SPACE.ToString());
                            return ROUNDED_ROCK;
                        }
                        else
                        {
                            return c;
                        }
                    }));                
                }
            }

            // ROLL WEST
            for (int i = 0; i < lines.Length; i++)
            {
                for (int k = 0; k < lines.Length; k++)
                {            
                    for (int c_i = 0; c_i < lines[k].Length - 1; c_i++)
                    {
                        char ch = lines[k][c_i];
                        if (ch == EMPTY_SPACE && lines[k][c_i + 1] == ROUNDED_ROCK)
                        {
                            lines[k] = lines[k].Remove(c_i + 1, 1).Insert(c_i + 1, EMPTY_SPACE.ToString());
                            lines[k] = lines[k].Remove(c_i, 1).Insert(c_i, ROUNDED_ROCK.ToString());
                        }
                    }
                }
            }

            // ROLL SOUTH
            for (int i = lines.Length - 1; i > 0; i--)
            {
                for (int k = lines.Length - 1; k > 0; k--)
                {            
                    lines[k] = string.Concat(lines[k].Select((c, c_i) =>
                    {
                        if (c == EMPTY_SPACE && lines[k - 1][c_i] == ROUNDED_ROCK)
                        {
                            lines[k - 1] = lines[k - 1].Remove(c_i, 1).Insert(c_i, EMPTY_SPACE.ToString());
                            return ROUNDED_ROCK;
                        }
                        else
                        {
                            return c;
                        }
                    }));                
                }
            }

            // ROLL EAST
            for (int i = 0; i < lines.Length; i++)
            {
                for (int k = 0; k < lines.Length; k++)
                {            
                    for (int c_i = lines[k].Length - 1; c_i > 0; c_i--)
                    {
                        char ch = lines[k][c_i];
                        if (ch == EMPTY_SPACE && lines[k][c_i - 1] == ROUNDED_ROCK)
                        {
                            lines[k] = lines[k].Remove(c_i - 1, 1).Insert(c_i - 1, EMPTY_SPACE.ToString());
                            lines[k] = lines[k].Remove(c_i, 1).Insert(c_i, ROUNDED_ROCK.ToString());
                        }
                    }
                }
            }

            string hash = string.Join(string.Empty, lines);
            
            if (Results.ContainsKey(hash))
            {
                if (foundCycle)
                {
                    continue;
                }
                long nextSet = c - Results[hash];
                c = 1000000000 - ((1000000000 - c) % nextSet);
                foundCycle = true;
            }    
            else
            {          
                string[] copy = new string[lines.Length];
                lines.CopyTo(copy, 0);

                Results.Add(hash, c);
            }
        }
        
        for (int i = 0; i < lines.Length; i++)
            sum += lines[i].Count(c => c == ROUNDED_ROCK) * (lines.Length - i);

        return sum;
    }
}