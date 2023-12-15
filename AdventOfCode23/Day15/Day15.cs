using AdventOfCode.Interfaces;

public class Day15 : IDay
{
    string line;

    public Day15(string path)
    {
        line = File.ReadAllText(path);
    }

    public object SolveOne()
    {
        return line.Split(',').Select(GetHash).Sum();
    }

    private int GetHash(string step) =>
        step.Aggregate(0, (currentValue, c) => (currentValue + c) * 17 % 256);

    public object SolveTwo()
    {
        Dictionary<int, List<(string Label, int FocalLengthId)>> boxes = new Dictionary<int, List<(string Label, int FocalLengthId)>>();

        foreach (string step in line.Split(','))
        {
            string[] parts = step.Split('=');
            
            if (parts.Length == 2)
            {
                string label = parts[0];
                int boxId = GetHash(label);
                int focalLength = int.Parse(parts[1]);

                if (boxes.ContainsKey(boxId) && boxes[boxId].Any(l => l.Label == label))
                {
                    int lenIndex = boxes[boxId].FindIndex(l => l.Label == label);
                    boxes[boxId][lenIndex] = (label, focalLength);
                }
                else if (boxes.ContainsKey(boxId))
                    boxes[boxId].Add((label, focalLength));
                else
                    boxes.Add(boxId, new List<(string Label, int FocalLengthId)> { (label, focalLength) });
            }
            else
            {
                string label = string.Concat(parts[0].SkipLast(1));
                int boxId = GetHash(label);

                if (boxes.ContainsKey(boxId) && boxes[boxId].Any(l => l.Label == label))
                    boxes[boxId].RemoveAll(l => l.Label == label);
            }
        }

        return boxes.Aggregate(0, (sum, b) => sum + b.Value.Sum(len => (b.Key + 1) * (b.Value.IndexOf(len) + 1) * len.FocalLengthId));
    }
}