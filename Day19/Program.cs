#region PART1

string[] lines = File.ReadAllText("input.sql").Split("\n\n");

Dictionary<string, string[]> workflows = new();
List<(int x, int m, int a, int s)> parts = new();
Dictionary<char, Func<int, int, bool>> operators = new() { { '<', (int a, int b) => a < b }, { '>', (int a, int b) => a > b } };

lines[0].Split("\n").Select(line => line[..^1].Split("{")).ToList().ForEach(split => workflows[split[0]] = split[1].Split(','));

lines[1][..^1].Split("\n").Select(line => line.Split(",")).ToList()
              .ForEach(split => parts.Add((int.Parse(split[0][3..]), int.Parse(split[1][2..]), int.Parse(split[2][2..]), int.Parse(split[3][2..^1]))));

int total = 0;

foreach(var (x, m, a, s) in parts)
{
    Dictionary<char, int> partValues = new(){ { 'x', x }, { 'm', m }, { 'a', a }, { 's', s } };
    List<string> workflowNames = new() { "in" };
    for(int i = 0; i < workflowNames.Count; i++)
    {
        foreach (var rule in workflows[workflowNames[i]])
        {
            string[] split = rule.Split(":");
            string nextWorkflow = split.Length == 1 ? split[0] : split[1];
            string condition = split[0];
            partValues.TryGetValue(condition[0], out int partValue);

            if (split.Length == 1 || operators[condition[1]](partValue, int.Parse(condition[2..])))
            {
                if(nextWorkflow.Equals("A"))
                    total += x + m + a + s;
                else if (!nextWorkflow.Equals("R"))
                    workflowNames.Add(nextWorkflow);
                break;
            }
        }
    }
}

Console.WriteLine(total);

#endregion

#region PART2

Console.WriteLine(Count(new() { { "x", (1, 4000) }, { "m", (1, 4000) }, { "a", (1, 4000) }, { "s", (1, 4000) }, }));

long Count(Dictionary<string, (int low, int high)> ranges, string name = "in")
{
    long total = 0;

    if (name.Equals("R"))
        return total;
    if(name.Equals("A"))
    {
        total = 1;
        foreach (var (low, high) in ranges.Values)
            total *= high - low + 1;
        return total;
    }

    bool hasBreaked = false;
    foreach(string rule in workflows[name])
    {
        if (!rule.Contains(':'))
            continue;
        (int low, int high) = ranges[rule[0].ToString()];
        string condition = rule.Split(':')[0];
        string target = rule.Split(':')[1];
        (int, int) T;
        (int, int) F;
        if (condition[1].Equals('<'))
        {
            T = (low, Math.Min(int.Parse(condition[2..]) - 1, high));
            F = (Math.Max(int.Parse(condition[2..]), low), high);
        }
        else
        {
            T = (Math.Max(int.Parse(condition[2..]) + 1, low), high);
            F = (low, Math.Min(int.Parse(condition[2..]), high));
        }

        if(T.Item1 <= T.Item2)
        {
            var copy = new Dictionary<string, (int low, int high)>(ranges);
            copy[rule[0].ToString()] = T;
            total += Count(copy, target);
        }

        if(F.Item1 <= F.Item2)
        {
            ranges = new(ranges);
            ranges[rule[0].ToString()] = F;
        }
        else
        {
            hasBreaked = true;
            break;
        }
    }
    if(!hasBreaked)
        total += Count(ranges, workflows[name].Last());

    return total;
}

#endregion