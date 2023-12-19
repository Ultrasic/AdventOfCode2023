#region PART1

string[] springLines = File.ReadAllLines("input.sql");

long total = 0;
Dictionary<(string, int[]), long> cache = new();

foreach (string line in springLines)
{
    string[] springsAndConditions = line.Split(' ');
    int[] conditions = springsAndConditions[1].Split(',').Select(int.Parse).ToArray();
    string currentInput = springsAndConditions[0];
    total += CountPossibilities(currentInput, conditions);
}

Console.WriteLine(total);

#endregion

#region PART2

total = 0;

foreach (string line in springLines)
{
    string[] springsAndConditions = line.Split(' ');
    List<int> conditions = springsAndConditions[1].Split(',').Select(int.Parse).ToList();
    for (int i = 0; i < 4; i++) conditions.AddRange(springsAndConditions[1].Split(',').Select(int.Parse).ToList());
    string currentInput = springsAndConditions[0];
    for (int i = 0; i < 4; i++) currentInput += "?" + springsAndConditions[0];
    total += CountPossibilities(currentInput, conditions.ToArray());
}

Console.WriteLine(total);

#endregion

#region METHODS

long CountPossibilities(string springs, int[] conditions)
{
    if (springs.Equals(""))
        return conditions.Length == 0 ? 1 : 0;

    if (conditions.Length == 0)
        return springs.Contains('#') ? 0 : 1;

    (string, int[]) key = (springs, conditions);
    foreach(var k in cache.Keys)
        if(k.Item1.Equals(key.Item1) && Enumerable.SequenceEqual(k.Item2, key.Item2))
            return cache[k];

    long result = 0;

    if (".?".Contains(springs[0]))
        result += CountPossibilities(springs[1..], conditions);
    if ("#?".Contains(springs[0]))
        if (conditions[0] <= springs.Length && !springs[..conditions[0]].Contains('.') && (conditions[0] == springs.Length || !springs[conditions[0]].Equals('#')))
            if (conditions[0] == springs.Length)
                result += CountPossibilities(springs[conditions[0]..], conditions[1..]);
            else
                result += CountPossibilities(springs[(conditions[0] + 1)..], conditions[1..]);

    cache.Add(key, result);
    return result;
}

#endregion