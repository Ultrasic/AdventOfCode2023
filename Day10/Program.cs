using System.Drawing;

string[] sketch = File.ReadAllLines("input.sql");

#region PART1

char[] rightChars = new char[] { 'J', '-', '7' };
char[] leftChars = new char[] { 'L', 'F', '-' };
char[] upChars = new char[] { '7', 'F', '|' };
char[] downChars = new char[] { '|', 'L', 'J' };

List<(char, int, int)> path = new();

int currentIndex = 0;
int currentLine = 0;

for(int i = 0; i < sketch.Length; i++)
{
    if (!sketch[i].Contains('S')) continue;
    currentIndex = sketch[i].IndexOf('S');
    currentLine = i;
    path.Add(('S', currentLine, currentIndex));
    break;
}

for(int i = 0; i < path.Count; i++)
{
    currentLine = path[i].Item2;
    currentIndex = path[i].Item3;
    if(rightChars.Contains(path[i].Item1) || path[i].Item1.Equals('S')) 
    {
        char left = sketch[currentLine][currentIndex - 1];
        if (leftChars.Contains(left) && !path.Contains((left, currentLine, currentIndex - 1)))
        {
            path.Add((left, currentLine, currentIndex - 1));
            continue;
        }
    }
    if (leftChars.Contains(path[i].Item1) || path[i].Item1.Equals('S'))
    {
        char right = sketch[currentLine][currentIndex + 1];
        if (rightChars.Contains(right) && !path.Contains((right, currentLine, currentIndex + 1)))
        {
            path.Add((right, currentLine, currentIndex + 1));
            continue;
        }
    }
    if(upChars.Contains(path[i].Item1) || path[i].Item1.Equals('S'))
    {
        char down = sketch[currentLine + 1][currentIndex];
        if (downChars.Contains(down) && !path.Contains((down, currentLine + 1, currentIndex)))
        {
            path.Add((down, currentLine + 1, currentIndex));
            continue;
        }
    }
    if(downChars.Contains(path[i].Item1)|| path[i].Item1.Equals('S'))
    {
        char up = sketch[currentLine - 1][currentIndex];
        if (upChars.Contains(up) && !path.Contains((up, currentLine - 1, currentIndex)))
        {
            path.Add((up, currentLine - 1, currentIndex));
            continue;
        }
    }
}

Console.WriteLine(path.Count/2);

#endregion

#region PART2

int total = 0;

for(int lineNumber = 0; lineNumber < sketch.Length; lineNumber++)
{
    for(int indexNumber = 0; indexNumber < sketch[lineNumber].Length; indexNumber++)
    {
        char c = sketch[lineNumber][indexNumber];
        if (IsPointInPolygon(new Point(indexNumber, lineNumber), path, c))
        {
            total++;
        }
    }
}

Console.WriteLine(total);

bool IsPointInPolygon(Point point, List<(char, int, int)> path, char c)
{
    int crossings = 0;
    if (!path.Contains((c, point.Y, point.X)))
    {
        for (int i = 0, j = path.Count - 1; i < path.Count; j = i++)
        {
            if (((path[i].Item2 > point.Y) != (path[j].Item2 > point.Y)) &&
                (point.X < (path[j].Item3 - path[i].Item3) * (point.Y - path[i].Item2) / (path[j].Item2 - path[i].Item2) + path[i].Item3))
            {
                crossings++;
            }
        }
    }

    return (crossings % 2) == 1;
}

#endregion