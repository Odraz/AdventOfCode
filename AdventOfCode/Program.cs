﻿using AdventOfCode.Interfaces;

using System.Diagnostics;

Stopwatch stopwatch = new();

string input = "C:\\Users\\TPouzar\\source\\repos\\AdventOfCode23\\AdventOfCode23\\Day07\\input.txt"; // 97632: too low
IDay day = new Day07(input);

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
day = new Day07(input);

Console.ForegroundColor = ConsoleColor.DarkYellow;
Console.WriteLine($"> Solving 2");
Console.ForegroundColor = ConsoleColor.Black;

stopwatch.Restart();

object solution2 = day.SolveTwo();

stopwatch.Stop();

PrintResult(2, stopwatch, solution2);
#endregion

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