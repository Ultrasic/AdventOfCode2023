#region PART1

using System.Drawing;

string[] universe = File.ReadAllLines("input.sql");

List<int> lines = new();
List<int> columns = new();
int numberOfColumns = universe[0].Length;

for(int i = 0; i < universe.Length; i++)
{
    if (universe[i].Distinct().Count() == 1)
    {
        lines.Add(i);
    }
}

for(int j = 0; j < numberOfColumns; j++)
{
    bool isDistinct = true;
    for (int i = 0; i < universe.Length; i++)
    {
        if (universe[i][j].Equals('#'))
        {
            isDistinct = false;
            break;
        }
    }
    if(isDistinct)
    {
        columns.Add(j);
    }
}

List<string> universeList = universe.ToList();
for(int i = 0; i < lines.Count; i++)
{
    universeList.Insert(lines[i]+i, new string('.', numberOfColumns).PadLeft(numberOfColumns, '.'));
}

for(int i = 0; i < columns.Count; i++)
{
    for(int j = 0; j < universeList.Count; j++)
    {
        universeList[j] = universeList[j].Insert(columns[i]+i, ".");
    }
}

List<Point> galaxies = new();

for (int row = 0; row < universeList.Count; row++)
{
    for(int column = 0; column < universeList[row].Length; column++)
    {
        if (universeList[row][column].Equals('#'))
            galaxies.Add(new(column,row));
    }
}

int total = 0;

for(int i = 0; i < galaxies.Count; i++)
{
    for(int j = i + 1; j < galaxies.Count; j++)
    {
        total += Math.Abs(galaxies[i].X - galaxies[j].X) + Math.Abs(galaxies[i].Y- galaxies[j].Y);
    }
}

Console.WriteLine(total);

#endregion

#region PART2

universeList = universe.ToList();

galaxies.Clear();

for (int row = 0; row < universeList.Count; row++)
{
    for (int column = 0; column < universeList[row].Length; column++)
    {
        if (universeList[row][column].Equals('#'))
            galaxies.Add(new(column, row));
    }
}

int expansion = 1000000 - 1;

for(int k = 0; k < galaxies.Count; k++)
{
    for (int i = 0; i < lines.Count; i++)
    {
        Point galaxy = galaxies[k];
        if(galaxy.Y >= lines[i] + i * expansion)
        {
            galaxy.Y += expansion;
            galaxies[k] = galaxy;
        }
    }

    for (int i = 0; i < columns.Count; i++)
    {
        Point galaxy = galaxies[k];
        if(galaxy.X >= columns[i] + i * expansion)
        {
            galaxy.X += expansion;
            galaxies[k] = galaxy;
        }
    }
}

long longTotal = 0;

for (int i = 0; i < galaxies.Count; i++)
{
    for (int j = i + 1; j < galaxies.Count; j++)
    {
        longTotal += Math.Abs(galaxies[i].X - galaxies[j].X) + Math.Abs(galaxies[i].Y - galaxies[j].Y);
    }
}

Console.WriteLine(longTotal);

#endregion