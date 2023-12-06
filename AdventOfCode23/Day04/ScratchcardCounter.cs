using System.Text.RegularExpressions;

public class ScratchcardCoutner
{
    private readonly string NUMBER_REGEX = @"[0-9]+";
    
    public int Sum()
    {
        using StreamReader streamReader = new("Day04/input01.txt");
        
        string? line;

        ScratchcardPile scratchcardPile = new();
            
        while ((line = streamReader.ReadLine()) != null)
        {
            string[] parts = line.Split(':');
            int numberId = Regex.Matches(parts[0], NUMBER_REGEX).Select(match => int.Parse(match.Value)).Single();

            string[] numberSeries = parts[1].Trim().Split('|');
            int[] winningNumbers = Regex.Matches(numberSeries[0], NUMBER_REGEX).Select(match => int.Parse(match.Value)).ToArray();
            int[] playingNumbers = Regex.Matches(numberSeries[1], NUMBER_REGEX).Select(match => int.Parse(match.Value)).ToArray();

            scratchcardPile.Scratchcards.Add(new Scratchcard(numberId, winningNumbers, playingNumbers));
        }

        int scratchcardCount = scratchcardPile.Scratchcards.Count;
        for(int i = 1; i <= scratchcardCount; i++)
        {
            IEnumerable<Scratchcard> scratchcards = new List<Scratchcard>(scratchcardPile.Scratchcards.Where(s => s.Id == i));

            foreach(Scratchcard scratchcard in scratchcards)
            {
                int winningScratchcards = scratchcard.WinningNumbersCount;

                for(int k = 1; k <= winningScratchcards; k++)
                {
                    Scratchcard scratchcardCopy = scratchcardPile.Scratchcards.First(s => s.Id == scratchcard.Id + k);

                    if (scratchcardCopy != null)
                    {
                        scratchcardPile.Scratchcards.Add(new Scratchcard(scratchcardCopy.Id, scratchcardCopy.WinningNumbers, scratchcardCopy.PlayingNumbers));
                    }
                }                    
            }
        }

        return scratchcardPile.Scratchcards.Count;
    }

    // Part 1
    // private int CalculateScratchcardPrice(int winningNumbersCount)
    // {
    //     if (winningNumbersCount == 0)
    //         return 0;
    //     else if (winningNumbersCount == 1)
    //         return 1;
    //     else
    //         return CalculateScratchcardPrice(winningNumbersCount - 1) * 2;
    // }
}

public class ScratchcardPile
{
    public List<Scratchcard> Scratchcards { get; set; }= new();
}

public class Scratchcard
{
    public int Id { get; }
    public List<int> WinningNumbers { get; }
    public List<int> PlayingNumbers { get; }
    public int WinningNumbersCount { get; }

    public Scratchcard(int id, IEnumerable<int> winningNumbers, IEnumerable<int> playingNumbers)
    {
        Id = id;
        WinningNumbers = new List<int>(winningNumbers);
        PlayingNumbers = new List<int>(playingNumbers);
        WinningNumbersCount = PlayingNumbers.Count(number => WinningNumbers.Contains(number));
    }
}