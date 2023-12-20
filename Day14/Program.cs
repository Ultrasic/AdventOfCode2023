#region PART1

string[] lines = File.ReadAllLines("input.sql");

int total = 0;

for(int i = 0; i < lines.Length; i++)
{
    for(int j = 0; j < lines[i].Length; j++)
    {
        if (lines[i][j].Equals('O'))
        {
            int currentLine = i;
            while (currentLine > 0 && lines[currentLine - 1][j].Equals('.'))
            {
                lines[currentLine] = lines[currentLine].Remove(j, 1).Insert(j, ".");
                currentLine--;
                lines[currentLine] = lines[currentLine].Remove(j, 1).Insert(j, "O");
            }
            total += lines.Length - currentLine;
        }
    }
}

Console.WriteLine(total);

#endregion

#region PART2

lines = File.ReadAllLines("input.sql");
HashSet<string[]> seenGrids = new() { lines };
List<string[]> arrays = new() { lines };

int iteration = 0;

while (true)
{
    iteration++;
    Cycle();
    if (seenGrids.Where(grid => Enumerable.SequenceEqual(grid, lines)).FirstOrDefault() is not null)
        break;
    seenGrids.Add(lines);
    arrays.Add(lines);
}

int first = arrays.Select((array, index) => (array, index)).Where(tuple => Enumerable.SequenceEqual(tuple.array, lines)).First().index;

lines = arrays[(1000000000 - first) % (iteration - first) + first];

Console.WriteLine(lines.Select((line, index) => line.Where(c => c.Equals('O')).Count() * (lines.Length - index)).Sum());

#endregion

void Cycle()
{
    for (int _ = 0; _ < 4; _++)
    {
        lines = lines.Skip(1).Aggregate(lines.First().Select(c => c.ToString()), (current, next) => current.Zip(next, (c1, c2) => c1 + c2.ToString())).ToArray();
        for(int i = 0; i < lines.Length; i++)
        {
            lines[i] = string.Join("#", lines[i].Split('#').Select(s => new string(s.OrderByDescending(c => c).ToArray())));
        }
        for(int i = 0;i < lines.Length; i++)
        {
            lines[i] = new string(lines[i].Reverse().ToArray());
        }
    }
}