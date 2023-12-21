#region PART1

string[] inputs = File.ReadAllLines("input.sql");

List<(int X, int Y)> points = new() { new(0,0) };

Dictionary<string, (int, int)> directions = new() { { "R", (0, 1) }, { "L", (0, -1) }, { "U", (-1, 0) }, { "D", (1, 0) } };

int b = 0;

foreach (string input in inputs)
{
    string[] split = input.Split(" ");
    int factor = int.Parse(split[1]);
    (int row, int column) = points.Last();
    (int directionRow, int directionColumn) = directions[split[0]];
    b += factor;
    points.Add((row + directionRow * factor, column + directionColumn * factor));
}

int area = Math.Abs(Enumerable.Range(0, points.Count).Select(i => points[i].X * (points[i - 1 >= 0 ? i - 1 : ^1].Y - points[(i + 1) % points.Count].Y)).Sum()) / 2;
int i = area - b / 2 + 1;

Console.WriteLine(i + b);

#endregion

#region PART2

List<(long X, long Y)> pointsLong = new() { new(0, 0) };
long longB = 0;

foreach (string input in inputs)
{
    string colorCode = input.Split(" ")[2][2..^1];
    long factor = long.Parse(colorCode[..^1], System.Globalization.NumberStyles.HexNumber);
    (long row, long column) = pointsLong.Last();
    (int directionRow, int directionColumn) = directions["RDLU"[(int)char.GetNumericValue(colorCode[^1])].ToString()];
    longB += factor;
    pointsLong.Add((row + directionRow * factor, column + directionColumn * factor));
}

long areaLong = Math.Abs(Enumerable.Range(0, pointsLong.Count).Select(i => pointsLong[i].X * (pointsLong[i - 1 >= 0 ? i - 1 : ^1].Y - pointsLong[(i + 1) % pointsLong.Count].Y)).Sum()) / 2;
long iLong = areaLong - longB / 2 + 1;

Console.WriteLine(iLong + longB);

#endregion