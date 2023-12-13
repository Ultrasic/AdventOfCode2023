using System.Reflection;

#region PART1

using Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Day5.input.sql");
if (stream is null) return;
using StreamReader reader = new(stream);

Dictionary<string, List<Tuple<long, long, long>>> maps = new();

List<SeedLong> seeds = new();

string[] mapsNames = new string[] { "seed-to-soil", "soil-to-fertilizer", "fertilizer-to-water", "water-to-light", "light-to-temperature", "temperature-to-humidity", "humidity-to-location", "" };

for(int i = 0; i < mapsNames.Length; i++)
{
    if (mapsNames[i].Equals("")) break;
    maps.Add(mapsNames[i], File.ReadLines("input.sql")
                               .SkipWhile(line => !line.Contains(mapsNames[i] + " map:"))
                               .Skip(1)
                               .TakeWhile(line => !line.Contains(mapsNames[i + 1] + " map:"))
                               .Where(s => !string.IsNullOrEmpty(s))
                               .Select(s =>
                               {
                                   string[] split = s.Split(" ");
                                   return new Tuple<long, long, long>(long.Parse(split[0]), long.Parse(split[1]), long.Parse(split[2]));
                               })
                               .ToList());
}

string? line = reader.ReadLine();
if (line is null) return;
foreach(string seed in line.Split(": ")[1].Split(" "))
{
    SeedLong s = new();
    s.Numbers[0] = long.Parse(seed);
    for(int i = 0; i < mapsNames.Length - 1; i++)
    {
        foreach (Tuple<long, long, long> map in maps[mapsNames[i]])
        {
            if (s.Numbers[i] >= map.Item2 && s.Numbers[i] <= map.Item2 + map.Item3)
            {
                s.Numbers[i+1] = map.Item1 + (s.Numbers[i] - map.Item2);
                break;
            }
        }
        if (s.Numbers[i + 1] == 0)
        {
            s.Numbers[i + 1] = s.Numbers[i];
        }
    }
    seeds.Add(s);
}

Console.WriteLine(seeds.Min(s => s.Numbers[7]));

#endregion

#region PART2

List<SeedRange> startSeedRanges = line.Split(": ")[1].Split(" ")
                                       .Select((value, index) => new { value, index })
                                       .GroupBy(pair => pair.index / 2)
                                       .Select(group => (long.Parse(group.ElementAt(0).value), long.Parse(group.ElementAt(1).value)))
                                       .Select(t => new SeedRange() { Start = t.Item1, End = t.Item1 + t.Item2 }).ToList();

for(int j = 0; j < mapsNames.Length - 1; j++)
{
    for(int i = 0; i < startSeedRanges.Count; i++)
    {
        SeedRange seed = startSeedRanges[i];
        foreach (Tuple<long, long, long> map in maps[mapsNames[j]])
        {
            long linearTransformation = map.Item1 - map.Item2;
            long mapStart = map.Item2;
            long mapEnd = mapStart + map.Item3;
            // ( .. ) => Seed
            // [ .. ] => Map Range
            //Seed is not in range ( .. ) [ .. ] OR [ .. ] ( .. )
            if (seed.End < mapStart || seed.Start >= mapEnd) continue;
            //Seed fully in range [ .. ( .. ) .. ] 
            if (seed.Start >= mapStart && seed.End <= mapEnd)
            {
                seed.Start += linearTransformation;
                seed.End += linearTransformation;
                break;
            }
            //Seed is partially in range
            //low ( .. [ .. ) .. ]
            else if (seed.Start <= mapStart && seed.End >= mapStart && seed.End <= mapEnd)
            {
                startSeedRanges.Add(new() { Start = seed.Start, End = mapStart - 1 });
                seed.Start = mapStart + linearTransformation;
                seed.End += linearTransformation;
                break;
            }
            //middle ( .. [ .. ] .. )
            else if (seed.Start < mapStart && seed.End > mapStart && seed.End > mapEnd)
            {
                startSeedRanges.Add(new() { Start = seed.Start, End = mapStart - 1 });
                startSeedRanges.Add(new() { Start = mapEnd, End = seed.End });
                seed.Start = mapStart + linearTransformation;
                seed.End = mapEnd - 1 + linearTransformation;
                break;
            }
            //high [ .. ( .. ] .. )
            else if (seed.Start >= mapStart && seed.Start <= mapEnd && seed.End > mapEnd)
            {
                startSeedRanges.Add(new() { Start = mapEnd, End = seed.End });
                seed.Start += linearTransformation;
                seed.End = mapEnd - 1 + linearTransformation;
                break;
            }
        }
    }
}

Console.WriteLine(startSeedRanges.Min(s => s.Start));

#endregion

#region SeedClass

class SeedLong
{
    public long[] Numbers { get; set; } = new long[8];
}

class SeedRange
{
    public long Start { get; set; } = 0;
    public long End { get; set; } = 0;
}

#endregion