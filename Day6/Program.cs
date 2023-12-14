using System.Reflection;

#region PART1

using Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Day6.input.sql");
if (stream is null) return;
using StreamReader reader = new(stream);

string? line = reader.ReadLine();
if (line is null) return;

List<int> times = line.Split(" ").Where(s => int.TryParse(s, out _)).Select(int.Parse).ToList();

line = reader.ReadLine();
if (line is null) return;

List<int> distances = line.Split(" ").Where(s => int.TryParse(s, out _)).Select(int.Parse).ToList();

Dictionary<int, int> solutions = new();
for (int j = 0; j < times.Count && j < distances.Count; j++)
{
    for(int i = 0; i <= times[j]; i++)
    {
        if(i * (times[j] - i) > distances[j])
        {
            if(solutions.ContainsKey(j))
                solutions[j]++;
            else 
                solutions.Add(j,1);
        }
    }
}

Console.WriteLine(solutions.Values.Aggregate(1, (acc, val) => acc * val));

#endregion

#region PART2

long time = long.Parse(times.Select(i => i.ToString()).Aggregate("", (acc, val) => acc + val));
long distance = long.Parse(distances.Select(i => i.ToString()).Aggregate("", (acc, val) => acc + val));

List<long> solutionsList = new();

for (long i = 0; i <= time; i++)
{
    if (i * (time - i) > distance)
    {
        solutionsList.Add(i);
    }
}

Console.WriteLine(solutionsList.Count);

#endregion