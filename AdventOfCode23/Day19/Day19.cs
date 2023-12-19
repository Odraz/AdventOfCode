using AdventOfCode.Interfaces;

public class Day19 : IDay
{    
    const char PART_X = 'x';
    const char PART_M = 'm';
    const char PART_A = 'a';
    const char PART_S = 's';

    const char OPERATION_LESS = '<';
    const char OPERATION_GREATER = '>';

    string[] lines;

    public Day19(string path)
    {
        lines = File.ReadAllLines(path);
    }

    public object SolveOne()
    {
        Dictionary<string, Workflow> workflows = GetWorkflows();

        List<(int X, int M, int A, int S)> ratings = new List<(int, int, int, int)>();

        for (int i = workflows.Count + 1; i < lines.Length; i++)
        {
            string? line = lines[i];

            string[] parts = string.Concat(line.Skip(1).SkipLast(1)).Split(',');

            int x = int.Parse(parts[0].Substring(2));
            int m = int.Parse(parts[1].Substring(2));
            int a = int.Parse(parts[2].Substring(2));
            int s = int.Parse(parts[3].Substring(2));

            ratings.Add((x, m, a, s));
        }

        List<(int X, int M, int A, int S)> acceptedRatings = new List<(int, int, int, int)>();

        foreach ((int X, int M, int A, int S) rating in ratings)
        {
            string? result = "in";

            do
            {
                if (result == null)
                    break;

                foreach (Rule rule in workflows[result].Rules)
                {
                    result = rule.GetResult(rating);

                    if (result != null)
                        break;
                }   
            } while (result != "A" && result != "R");

            if (result == "A")
                acceptedRatings.Add(rating);
        }

        return acceptedRatings.Sum(r => r.X + r.M + r.A + r.S);
    }

    private Dictionary<string, Workflow> GetWorkflows()
    {
        var workflows = new Dictionary<string, Workflow>();

        for (int i = 0; i < lines.Length; i++)
        {
            string? line = lines[i];

            if (line == string.Empty)
                break;

            string[] parts = line.Split('{');
            string name = parts[0];
            string[] rules = string.Concat(parts[1].SkipLast(1)).Split(',');

            var workflow = new Workflow(name);

            foreach (string rule in rules)
            {
                if (rule.Contains(':'))
                {
                    string[] ruleParts = rule.Split(':');
                    char category = ruleParts[0][0];
                    int value = int.Parse(ruleParts[0].Substring(2));
                    char operation = ruleParts[0][1];
                    string result = ruleParts[1];
                    workflow.Rules.Add(new Rule(category, value, operation, result));
                }
                else
                {
                    workflow.Rules.Add(new Rule(rule));
                }
            }

            workflows.Add(name, workflow);
        }

        return workflows;
    }

    public object SolveTwo()
    {
        Dictionary<string, Workflow> workflows = GetWorkflows();

        List<(int Min, int Max)> combinations = new List<(int, int)>()
        {
            (1, 4000),
            (1, 4000),
            (1, 4000),
            (1, 4000)
        };

        Dictionary<string, long> numberOfCombinations = new Dictionary<string, long>()
        {
            { "in", GetNumberOfCombinations(combinations) },
            { "A", 0 },
            { "R", 0 }
        };

        Calculate(workflows["in"], combinations, numberOfCombinations);

        foreach (KeyValuePair<string, long> kvp in numberOfCombinations)
            Console.WriteLine($"{kvp.Key}: {kvp.Value}");

        return numberOfCombinations["A"];
        
        void Calculate(Workflow workflow, List<(int Min, int Max)> combinations, Dictionary<string, long> numberOfCombinations)
        {
            List<(int Min, int Max)> currentCombinations = new List<(int, int)>(combinations);

            foreach (Rule rule in workflow.Rules)
            {
                if (rule.Operation == null && rule.Result == "A")
                {
                    numberOfCombinations["A"] += GetNumberOfCombinations(currentCombinations);
                    return;
                }
                else if (rule.Operation == null && rule.Result == "R")
                {
                    numberOfCombinations["R"] += GetNumberOfCombinations(currentCombinations);
                    return;
                }
                else
                {
                    List<(int Min, int Max)> newCombinations = new List<(int, int)>(currentCombinations);

                    if (rule.Operation == OPERATION_GREATER)
                    {
                        switch (rule.Category)
                        {
                            case PART_X:
                                currentCombinations[0] = (newCombinations[0].Min, Math.Min(newCombinations[0].Max, rule.Value.Value));
                                newCombinations[0] = (Math.Max(newCombinations[0].Min, rule.Value.Value + 1), newCombinations[0].Max);
                                break;
                            case PART_M:
                                currentCombinations[1] = (newCombinations[1].Min, Math.Min(newCombinations[1].Max, rule.Value.Value));
                                newCombinations[1] = (Math.Max(newCombinations[1].Min, rule.Value.Value + 1), newCombinations[1].Max);
                                break;
                            case PART_A:
                                currentCombinations[2] = (newCombinations[2].Min, Math.Min(newCombinations[2].Max, rule.Value.Value));
                                newCombinations[2] = (Math.Max(newCombinations[2].Min, rule.Value.Value + 1), newCombinations[2].Max);
                                break;
                            case PART_S:
                                currentCombinations[3] = (newCombinations[3].Min, Math.Min(newCombinations[3].Max, rule.Value.Value));
                                newCombinations[3] = (Math.Max(newCombinations[3].Min, rule.Value.Value + 1), newCombinations[3].Max);
                                break;
                        }
                    }
                    else if (rule.Operation == OPERATION_LESS)
                    {
                        switch (rule.Category)
                        {
                            case PART_X:
                                currentCombinations[0] = (Math.Max(newCombinations[0].Min, rule.Value.Value), newCombinations[0].Max);
                                newCombinations[0] = (newCombinations[0].Min, Math.Min(newCombinations[0].Max, rule.Value.Value - 1));
                                break;
                            case PART_M:
                                currentCombinations[1] = (Math.Max(newCombinations[1].Min, rule.Value.Value), newCombinations[1].Max);
                                newCombinations[1] = (newCombinations[1].Min, Math.Min(newCombinations[1].Max, rule.Value.Value - 1));
                                break;
                            case PART_A:
                                currentCombinations[2] = (Math.Max(newCombinations[2].Min, rule.Value.Value), newCombinations[2].Max);
                                newCombinations[2] = (newCombinations[2].Min, Math.Min(newCombinations[2].Max, rule.Value.Value - 1));
                                break;
                            case PART_S:
                                currentCombinations[3] = (Math.Max(newCombinations[3].Min, rule.Value.Value), newCombinations[3].Max);
                                newCombinations[3] = (newCombinations[3].Min, Math.Min(newCombinations[3].Max, rule.Value.Value - 1));
                                break;
                        }
                    }

                    if (rule.Result == "A" || rule.Result == "R")
                    {
                        numberOfCombinations[rule.Result] += GetNumberOfCombinations(newCombinations);
                    }
                    else
                    {
                        numberOfCombinations[rule.Result] = GetNumberOfCombinations(newCombinations);                            
                        Calculate(workflows[rule.Result], newCombinations, numberOfCombinations);
                    }
                }
            }   
        }
    }

    private long GetNumberOfCombinations(List<(int Min, int Max)> combinations) =>
        combinations.Aggregate(1L, (acc, c) => acc * (c.Max - c.Min + 1));

    public class Workflow
    {
        public string Name { get; set; }
        public List<Rule> Rules { get; set; }

        public Workflow(string name)
        {
            Name = name;
            Rules = new List<Rule>();
        }
    }

    public class Rule
    {
        public char? Category { get; }
        public int? Value { get; }
        public char? Operation { get; }
        public string Result { get; }

        public Rule(char category, int value, char operation, string result)
        {
            this.Category = category;
            this.Value = value;
            this.Operation = operation;            
            this.Result = result;
        }

        public Rule(string result)
        {
            this.Result = result;
        }

        public string? GetResult((int X, int M, int A, int S) part)
        {
            if (Operation == null)
                return Result;
            else if (Category == PART_X)
            {
                if (Operation == OPERATION_LESS)
                    return part.X < Value ? Result : null;
                else if (Operation == OPERATION_GREATER)
                    return part.X > Value ? Result : null;
            }
            else if (Category == PART_M)
            {
                if (Operation == OPERATION_LESS)
                    return part.M < Value ? Result : null;
                else if (Operation == OPERATION_GREATER)
                    return part.M > Value ? Result : null;
            }
            else if (Category == PART_A)
            {
                if (Operation == OPERATION_LESS)
                    return part.A < Value ? Result : null;
                else if (Operation == OPERATION_GREATER)
                    return part.A > Value ? Result : null;
            }
            else if (Category == PART_S)
            {
                if (Operation == OPERATION_LESS)
                    return part.S < Value ? Result : null;
                else if (Operation == OPERATION_GREATER)
                    return part.S > Value ? Result : null;
            }

            return null;
        }
    }
}

