using System.Text.RegularExpressions;

public class EngineValidator
{
    // Regex for any number of digits
    private readonly string NUMBER_REGEX = @"[0-9]+";
    
    // Regex for any number of digits with optional negative sign
    // private readonly string NUMBER_REGEX_WITH_NEGATIVE = @"-?[0-9]+";

    // Regex for any symbol that is not a dot nor number
    private readonly string SYMBOL_REGEX = @"[^0-9\.]";

    // private readonly string SYMBOL_REGEX_NOT_NEGATIVE_NUMBER = @"-(?!\d)|[^-0-9\.]";

    public int Sum()
    {
        using StreamReader streamReader = new("Day03/input01.txt");
        
        string? line;
        int sum = 0;

        Engine engine = new();

        while ((line = streamReader.ReadLine()) != null)
        {
            Regex.Matches(line, NUMBER_REGEX).ToList().ForEach(match =>
            {
                engine.Parts.Add(new EngineNumber
                {
                    Line = engine.Lines.Count,
                    StartPosition = match.Index,
                    Number = match.Value
                });
            });
            
            Regex.Matches(line, SYMBOL_REGEX).ToList().ForEach(match =>
            {
                engine.Parts.Add(new EngineSymbol
                {
                    Line = engine.Lines.Count,
                    StartPosition = match.Index,
                    Symbol = match.Value
                });
            });
            
            engine.Lines.Add(line);
        }

        foreach (EnginePart part in engine.Parts)
        {
            // Console.WriteLine($"Line: {part.Line}, StartPosition: {part.StartPosition}, Value: {part}");

            // Part 1
            // if (part is EngineNumber partNumber && engine.IsValidEngineNumber(partNumber))
            // {
            //     Console.WriteLine($"Line: {part.Line}, Valid number: {partNumber.Number}");
            //     sum += int.Parse(partNumber.Number);
            // }

            // Part 2
            if (part is EngineSymbol partSymbol && partSymbol.Symbol == "*")
            {
                sum += engine.TryMultiply(partSymbol);
            }
        }

        return sum;
    }
}

public class Engine
{
    public List<string> Lines { get; set; } = new();
    public List<EnginePart> Parts { get; set; } = new();

    public bool IsValidEngineNumber(EngineNumber engineNumber)
    {
        return Parts.Any(part => part is EngineSymbol engineSymbol && 
                                 (part.Line == engineNumber.Line || part.Line == engineNumber.Line + 1 || part.Line == engineNumber.Line - 1) &&
                                 part.StartPosition >= engineNumber.StartPosition - 1 && part.StartPosition <= engineNumber.StartPosition + engineNumber.Number.Length);
    }

    public int TryMultiply(EngineSymbol symbol)
    {
        List<EngineNumber> adjacentNumbers = Parts.Where(part => part is EngineNumber number &&
                                                                 (number.Line == symbol.Line || number.Line == symbol.Line + 1 || number.Line == symbol.Line - 1) &&
                                                                 symbol.StartPosition >= number.StartPosition - 1 && symbol.StartPosition <= number.StartPosition + number.Number.Length)
                                                                 .OfType<EngineNumber>()
                                                                 .ToList();


        if (adjacentNumbers.Count != 2)
            return 0;

        Console.WriteLine($"Line: {symbol.Line}, Valid number: {adjacentNumbers.First().Number} * {adjacentNumbers.Last().Number}");
        return int.Parse(adjacentNumbers.First().Number) * int.Parse(adjacentNumbers.Last().Number);
    }
}

public abstract class EnginePart
{
    public int Line { get; set; }
    public int StartPosition { get; set; }

    public abstract override string ToString();
}

public class EngineNumber : EnginePart
{
    public required string Number { get; set; }

    public override string ToString() => Number;
}

public class EngineSymbol : EnginePart
{
    public required string Symbol { get; set; }

    public override string ToString() => Symbol;
}