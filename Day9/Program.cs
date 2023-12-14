using System.Reflection;

#region PART1

using Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Day9.input.sql");
if (stream is null) return;
using StreamReader reader = new(stream);

long total = 0;

while (!reader.EndOfStream)
{
    string? line = reader.ReadLine();
    if (line is null) return;
    List<long[]> datasets = new()
    {
        line.Split(" ").Select(long.Parse).ToArray()
    };
    for (int i = 0; i < datasets.Count; i++)
    {
        long[] dataset = datasets[i];
        if (dataset.Where(x => x == 0).Count() == dataset.Length) break;
        long[] newDataset = new long[dataset.Length - 1];
        for(int j = 0; j < dataset.Length; j++)
        {
            if (j + 1 >= dataset.Length ) break;
            newDataset[j] = dataset[j + 1] - dataset[j];
        }
        datasets.Add(newDataset);
    }
    long extrapolation = 0;
    for (int i = datasets.Count - 1; i >= 0; i--)
    {
        if (i - 1 < 0) break;
        extrapolation += datasets[i - 1].Last();
    }
    total += extrapolation;
}

Console.WriteLine(total);

#endregion

#region PART2

stream.Position = 0;
reader.DiscardBufferedData();
total = 0;

while (!reader.EndOfStream)
{
    string? line = reader.ReadLine();
    if (line is null) return;
    List<long[]> datasets = new()
    {
        line.Split(" ").Select(long.Parse).ToArray()
    };
    for (int i = 0; i < datasets.Count; i++)
    {
        long[] dataset = datasets[i];
        if (dataset.Where(x => x == 0).Count() == dataset.Length) break;
        long[] newDataset = new long[dataset.Length - 1];
        for (int j = dataset.Length - 1; j >= 0; j--)
        {
            if (j - 1 < 0) break;
            newDataset[j - 1] = dataset[j] - dataset[j - 1];
        }
        datasets.Add(newDataset);
    }
    long extrapolation = 0;
    for (int i = datasets.Count - 1; i >= 0; i--)
    {
        if (i - 1 < 0) break;
        extrapolation = datasets[i - 1].First() - extrapolation;
    }
    total += extrapolation;
}

Console.WriteLine(total);

#endregion