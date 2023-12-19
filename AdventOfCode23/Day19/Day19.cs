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
        // Get workflows
        // Example input:
        // in{s<1351:px,qqz}
        // qqz{s>2770:qs,m<1801:hdj,R}
        // gd{a>3333:R,R}
        // hdj{m>838:A,pv}
        var workflows = new Dictionary<string, Workflow>();

        int i = 0;
        for (; i < lines.Length; i++)
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

        // Get ratings
        // Example input:
        // {x=787,m=2655,a=1222,s=2876}
        // {x=1679,m=44,a=2067,s=496}
        // {x=2036,m=264,a=79,s=2244}
        // {x=2461,m=1339,a=466,s=291}
        // {x=2127,m=1623,a=2188,s=1013}
        List<(int X, int M, int A, int S)> ratings = new List<(int, int, int, int)>();

        for (i++; i < lines.Length; i++)
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

    public object SolveTwo()
    {
        throw new NotImplementedException();
    }

    
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
        private char? category;
        private int? value;
        private char? operation;
        private string result;

        public Rule(char category, int value, char operation, string result)
        {
            this.category = category;
            this.value = value;
            this.operation = operation;            
            this.result = result;
        }

        public Rule(string result)
        {
            this.result = result;
        }

        public string? GetResult((int X, int M, int A, int S) part)
        {
            if (operation == null)
                return result;
            else if (category == PART_X)
            {
                if (operation == OPERATION_LESS)
                    return part.X < value ? result : null;
                else if (operation == OPERATION_GREATER)
                    return part.X > value ? result : null;
            }
            else if (category == PART_M)
            {
                if (operation == OPERATION_LESS)
                    return part.M < value ? result : null;
                else if (operation == OPERATION_GREATER)
                    return part.M > value ? result : null;
            }
            else if (category == PART_A)
            {
                if (operation == OPERATION_LESS)
                    return part.A < value ? result : null;
                else if (operation == OPERATION_GREATER)
                    return part.A > value ? result : null;
            }
            else if (category == PART_S)
            {
                if (operation == OPERATION_LESS)
                    return part.S < value ? result : null;
                else if (operation == OPERATION_GREATER)
                    return part.S > value ? result : null;
            }

            return null;
        }
    }
}

