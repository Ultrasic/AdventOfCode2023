#region PART1

string[] inputs = File.ReadAllLines("input.sql");

List<(int X, int Y)> points = new() { new(0,0) };

Dictionary<string, (int, int)> directions = new() { { "R", (0, 1) }, { "L", (0, -1) }, { "U", (-1, 0) }, { "D", (1, 0) } };

int b = 0;

foreach (string input in inputs)
{
    string direction = input.Split(" ")[0];
    int factor = int.Parse(input.Split(" ")[1]);
    (int row, int column) = points.Last();
    (int directionRow, int directionColumn) = directions[direction];
    b += factor;
    points.Add((row + directionRow * factor, column + directionColumn * factor));
}

int area = Math.Abs(points.Select((point, i) => points[i].X * (points[i - 1 >= 0 ? i - 1 : ^1].Y - points[(i + 1) % points.Count].Y)).Sum()) / 2;
int i = area - b / 2 + 1;

Console.WriteLine(i + b);

#endregion

#region PART2

List<(long X, long Y)> pointsLong = new() { new(0, 0) };
long longB = 0;

foreach (string input in inputs)
{
    string x = input.Split(" ")[2];
    x = x[2..^1];
    long factor = long.Parse(x[..^1], System.Globalization.NumberStyles.HexNumber);
    (long row, long column) = pointsLong.Last();
    (int directionRow, int directionColumn) = directions[char.ToString("RDLU"[(int)char.GetNumericValue(x[^1])])];
    longB += factor;
    pointsLong.Add((row + directionRow * factor, column + directionColumn * factor));
}

long areaLong = Math.Abs(Enumerable.Range(0, pointsLong.Count).Select(i => pointsLong[i].X * (pointsLong[i - 1 >= 0 ? i - 1 : ^1].Y - pointsLong[(i + 1) % pointsLong.Count].Y)).Sum()) / 2;
long iLong = areaLong - longB / 2 + 1;

Console.WriteLine(iLong + longB);

#endregion