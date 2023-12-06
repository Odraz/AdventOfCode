using System.Text.RegularExpressions;

namespace AdventOfCode23;

public class CalibrationService
{
    private readonly Dictionary<string, string> numberMap = new()
    {
        { "one", "1" },
        { "two", "2" },
        { "three", "3" },
        { "four", "4" },
        { "five", "5" },
        { "six", "6" },
        { "seven", "7" },
        { "eight", "8" },
        { "nine", "9" },
    };

    private readonly string pattern = @"\d|one|two|three|four|five|six|seven|eight|nine";

    public int Sum()
    {
        using StreamReader streamReader = new("Day01/input.txt");

        string? line;
        int sum = 0;
        
        while ((line = streamReader.ReadLine()) != null)
        {
            int configurationFinal = ExtractConfiguration(line);

            sum += configurationFinal;

            Console.WriteLine($"{line} => {configurationFinal}");
        }

        return sum;
    }

    public int ExtractConfiguration(string line)
    {
        IEnumerable<string> digits = Regex.Matches(line, pattern).Select(match => match.Value);
            IEnumerable<string> digitsBack = Regex.Matches(line, pattern, RegexOptions.RightToLeft).Select(match => match.Value);
            (string First, string Last) configuration = (digits.First(), digitsBack.First());
            (string First, string Last) configurationDigits =
                (numberMap.ContainsKey(configuration.First) ? numberMap[configuration.First] : configuration.First,
                 numberMap.ContainsKey(configuration.Last) ? numberMap[configuration.Last] : configuration.Last);

           return int.Parse($"{configurationDigits.First}{configurationDigits.Last}");
    }
}
