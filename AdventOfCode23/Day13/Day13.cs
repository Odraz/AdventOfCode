using AdventOfCode.Interfaces;

public class Day13 : IDay
{
    string[] lines = Array.Empty<string>();
    IEnumerable<string[]> patterns = Array.Empty<string[]>();

    IDictionary<int, long> verticalReflections = new Dictionary<int, long>(); 
    IDictionary<int, long> horizontalReflections = new Dictionary<int, long>();

    public Day13(string path)
    {
        lines = File.ReadAllLines(path);

        string[] tempPattern = Array.Empty<string>();
        foreach (string line in lines)
        {
            if (line == string.Empty)
            {
                patterns = patterns.Append(tempPattern);
                tempPattern = Array.Empty<string>();

                continue;
            }

            tempPattern = tempPattern.Append(line).ToArray();
        }

        patterns = patterns.Append(tempPattern);
    }

    public object SolveOne()
    {
        long result = 0; 

        for (int i = 0; i < patterns.Count(); i++)
        {
            string[] pattern = patterns.ElementAt(i);

            long verticalReflectionsNum = VerticalReflectionLine(i, pattern, true);
            if (verticalReflectionsNum == 0)
            {
                long horizontalReflectionsNum = HorizontalReflectionLine(i, pattern);
                result += 100 * horizontalReflectionsNum;
                horizontalReflections.Add(i, horizontalReflectionsNum);
            }
            else
            {
                result += verticalReflectionsNum;
                verticalReflections.Add(i, verticalReflectionsNum);
            }
        }

        return result;
    }

    private long VerticalReflectionLine(int patternId, string[] lines, bool isVerticalSearch, int? rowFlip = null, int? columnFlip = null)
    {
        Dictionary<int, bool> verticalReflectionsTmp = new Dictionary<int, bool>();
        for (int l = 0; l < lines.Length; l++)
        {
            string line = lines[l];

            if (rowFlip.HasValue && l == rowFlip.Value)
            {
                char oldChar = line[columnFlip!.Value];
                char newChar = oldChar == '.' ? '#' : '.';
                line = line.Remove(columnFlip.Value, 1).Insert(columnFlip.Value, newChar.ToString());
            }

            for (int i = 1; i < line.Length; i++)
            {
                string leftReversePart = string.Concat(line.Take(i).Reverse());
                string rightPart = string.Concat(line.Skip(i));

                if (leftReversePart.Length > rightPart.Length)
                    leftReversePart = string.Concat(leftReversePart.Take(rightPart.Length));
                else
                    rightPart = string.Concat(rightPart.Take(leftReversePart.Length));

                bool isVerticalReflection = leftReversePart == rightPart; 

                if (!verticalReflectionsTmp.ContainsKey(i))
                {
                    verticalReflectionsTmp.Add(i, isVerticalReflection);
                }
                else if (isVerticalReflection && verticalReflectionsTmp.ContainsKey(i) && verticalReflectionsTmp[i])
                {
                    continue;
                }
                else if (!isVerticalReflection && verticalReflectionsTmp.ContainsKey(i) && verticalReflectionsTmp[i])
                {
                    verticalReflectionsTmp[i] = false;
                }
            }
        }

            IEnumerable<KeyValuePair<int, bool>> foundLines = verticalReflectionsTmp
            .Where(x => x.Value &&
                        (rowFlip == null ||
                        (!isVerticalSearch || !verticalReflections.ContainsKey(patternId) || verticalReflections[patternId] != x.Key) &&
                        (isVerticalSearch || !horizontalReflections.ContainsKey(patternId) || horizontalReflections[patternId] != x.Key)));
        if (foundLines.Count() == 1)
            return foundLines.SingleOrDefault().Key;
        else
            return 0;
    }

    private long HorizontalReflectionLine(int patternId, string[] lines, int? rowFlip = null, int? columnFlip = null)
    {
        string[] flippedLines = new string[lines[0].Length];
        foreach (string line in lines)
        {
            for (int i = 0; i < line.Length; i++)
            {
                flippedLines[i] += line[i];
            }
        }

        return VerticalReflectionLine(patternId, flippedLines, false, rowFlip, columnFlip);
    }

    public object SolveTwo()
    {
        long result = 0; 

        for (int p = 0; p < patterns.Count(); p++)
        {
            long p_result = 0;

            string[] pattern = patterns.ElementAt(p);

            long verticalReflectionsNum = 0;

            for (int i = 0; i < pattern.Length; i++)
            {
                for (int k = 0; k < pattern[i].Length; k++)
                {
                    verticalReflectionsNum = VerticalReflectionLine(p, pattern, true, i, k);
                    
                    if (verticalReflectionsNum != 0 && !(verticalReflections.ContainsKey(p) && verticalReflections[p] == verticalReflectionsNum))
                        break;
                }

                if (verticalReflectionsNum != 0 && !(verticalReflections.ContainsKey(p) && verticalReflections[p] == verticalReflectionsNum))
                    break;
                
                if (i == pattern.Length - 1)
                    verticalReflectionsNum = 0;
            }

            if (verticalReflectionsNum != 0)
            {
                p_result = verticalReflectionsNum;

                Console.WriteLine($"Pattern {p + 1}: {p_result}");

                result += p_result;
                continue;
            }

            long horizontalReflectionsNum = 0;
            
            for (int i = 0; i < pattern.Length; i++)
            {
                for (int k = 0; k < pattern[i].Length; k++)
                {
                    horizontalReflectionsNum = HorizontalReflectionLine(p, pattern, k, i);
                    
                    if (horizontalReflectionsNum != 0 && !(horizontalReflections.ContainsKey(p) && horizontalReflections[p] == horizontalReflectionsNum))
                        break;
                }

                if (horizontalReflectionsNum != 0 && !(horizontalReflections.ContainsKey(p) && horizontalReflections[p] == horizontalReflectionsNum))
                    break;
                
                if (i == pattern.Length - 1)
                    horizontalReflectionsNum = 0;
            }

            p_result = 100 * horizontalReflectionsNum;
            
            Console.WriteLine($"Pattern {p + 1}: {p_result}");

            result += p_result;
        }

        return result;
    }
} 