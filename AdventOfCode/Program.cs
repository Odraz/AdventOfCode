using AdventOfCode.Interfaces;

using System.Diagnostics;

Stopwatch stopwatch = new();

//string input = @"../AdventOfCode23/Day12/input.txt";
string input = @"Day12/example.txt";
IDay day = new Day12(input);

Console.ForegroundColor = ConsoleColor.DarkYellow;
Console.WriteLine($"> Solving input: '{input}'");
Console.WriteLine();

#region SolveOne
Console.WriteLine($"> Solving 1");
Console.ForegroundColor = ConsoleColor.Black;

stopwatch.Start();

object solution1 = day.SolveOne();

stopwatch.Stop();

PrintResult(1, stopwatch, solution1);
#endregion

#region SolveTwo
day = new Day12(input);

Console.ForegroundColor = ConsoleColor.DarkYellow;
Console.WriteLine($"> Solving 2");
Console.ForegroundColor = ConsoleColor.Black;

stopwatch.Restart();

object solution2 = day.SolveTwo();

stopwatch.Stop();

PrintResult(2, stopwatch, solution2);
#endregion

Console.ForegroundColor = ConsoleColor.DarkYellow;
Console.WriteLine("Press any key to exit...");
Console.ReadKey();

static void PrintResult(int solutionNumber, Stopwatch stopwatch, object solution2)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"> Solution {solutionNumber}: {solution2}");
    
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine($"> Time taken: {stopwatch.ElapsedMilliseconds}ms");

    Console.ForegroundColor = ConsoleColor.Black;
    Console.WriteLine();
}