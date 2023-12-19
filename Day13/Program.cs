#region PART1

using System.Reflection.Metadata.Ecma335;

int total = 0;

foreach(string block in File.ReadAllText("input.sql").Split("\n\n"))
{
    var grid = block.Split("\n");
    if (grid.Last().Equals(""))
    {
        grid = grid[..^1];
    }
    total += FindMirror(grid) * 100;

    // equivalent of zip(*grid) in python lol
    var columns = grid.Skip(1).Aggregate(grid.First().Select(c => c.ToString()),
            (current, next) => current.Zip(next, (c1, c2) => c1 + c2.ToString())).ToArray();

    total += FindMirror(columns);
}

Console.WriteLine(total);

#endregion

#region PART2

total = 0;

foreach (string block in File.ReadAllText("input.sql").Split("\n\n"))
{
    var grid = block.Split("\n");
    if (grid.Last().Equals(""))
    {
        grid = grid[..^1];
    }
    total += FindMirror2(grid) * 100;

    // equivalent of zip(*grid) in python lol
    var columns = grid.Skip(1).Aggregate(grid.First().Select(c => c.ToString()),
            (current, next) => current.Zip(next, (c1, c2) => c1 + c2.ToString())).ToArray();

    total += FindMirror2(columns);
}

Console.WriteLine(total);

#endregion

static int FindMirror(string[] grid)
{
    for (int i = 1; i < grid.Length; i++)
    {
        var above = grid[..i].Reverse().ToArray();
        var below = grid[i..];

        if(below.Length <= above.Length)
            above = above[..below.Length];
        if(above.Length <= below.Length)
            below = below[..above.Length];

        if (Enumerable.SequenceEqual(above,below))
            return i;
    }

    return 0;
}

static int FindMirror2(string[] grid)
{
    for (int i = 1; i < grid.Length; i++)
    {
        var above = grid[..i].Reverse().ToArray();
        var below = grid[i..];

        if (below.Length <= above.Length)
            above = above[..below.Length];
        if (above.Length <= below.Length)
            below = below[..above.Length];

        if (Enumerable.Sum(Enumerable.Zip(above, below, (x, y) => Enumerable.Sum(Enumerable.Zip(x, y, (a, b) => a.Equals(b) ? 0 : 1)))) == 1)
            return i;
    }

    return 0;
}