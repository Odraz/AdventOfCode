using AdventOfCode.Interfaces;

public class Day09 : IDay
{
    private readonly string[] lines;

    public Day09(string input)
    {
        lines = File.ReadAllLines(input);
    }

    public object SolveOne()
    {
        IEnumerable<int?[]> histories = GetHistories();

        int lastIndex = histories.First().Length - 1;

        return histories.Select(h => PredictNext(lastIndex, h)).Sum();
    }

    private IEnumerable<int?[]> GetHistories() =>
        lines.Select(l => l.Split(' ').Select(match => (int?)int.Parse(match)).ToArray());

    private long PredictNext(int lastIndex, int?[] history)
    {
        if (history.All(r => r == 0 || r == null))
            return 0;
        else
            return PredictNext(lastIndex, history.Select((r, index) => GetDiff(r, index, lastIndex, history)).ToArray()) +
                   history.Last(r => r != null)!.Value;
    }

    public object SolveTwo()
    {
        IEnumerable<int?[]> histories = GetHistories();

        int lastIndex = histories.First().Length - 1;

        return histories.Select(h => PredictPrevious(lastIndex, h)).Sum();
    }

    private long PredictPrevious(int lastIndex, int?[] history)
    {
        if (history.All(r => r == 0 || r == null))
            return 0;
        else
            return history.First()!.Value -
                   PredictPrevious(lastIndex, history.Select((r, index) => GetDiff(r, index, lastIndex, history)).ToArray());
    }

    private static int? GetDiff(int? r, int index, int lastIndex, int?[] history) =>
        index == lastIndex || (history[index + 1] == null) ?
            null :
            history[index + 1] - r;
}