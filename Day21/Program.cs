string[] lines = File.ReadAllLines("input.sql");

(int row, int column) = lines.Where(line => line.Contains("S")).Select((line, r) => (line, r)).Where((tuple, c) => tuple.line[tuple.r].Equals('S')).Select((tuple, c) => (tuple.r, c)).First();

for(row = 0; row < lines.Length; row++)
{
    if (!lines[row].Contains('S')) continue;
    for(column = 0; column < lines[row].Length; column++)
    {
        if (lines[row][column].Equals('S'))
        {
            goto Next;
        }
    }
}

Next:

List<(int, int)> positions = new() { (row, column) } ;

for(int _ = 0; _ < 64; _++)
{
    List<(int, int)> workingPositions = new(positions);
    for (int i = 0; i < workingPositions.Count; i++)
    {
        (row, column) = workingPositions[i];
        if (row - 1 >= 0 && !lines[row - 1][column].Equals('#') && !positions.Contains((row - 1, column)))
            positions.Add((row - 1, column));
        if(column + 1 < lines[row].Length && !lines[row][column + 1].Equals('#') && !positions.Contains((row, column + 1)))
            positions.Add((row, column + 1));
        if(row + 1 < lines.Length && !lines[row + 1][column].Equals('#') && !positions.Contains((row + 1, column)))
            positions.Add((row + 1, column));
        if (column - 1 >= 0 && !lines[row][column - 1].Equals('#') && !positions.Contains((row, column - 1)))
            positions.Add((row, column - 1));

        positions.Remove(workingPositions[i]);
    }
}

Console.WriteLine(positions.Count);