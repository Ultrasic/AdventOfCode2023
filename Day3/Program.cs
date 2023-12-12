using System.Reflection;
using System.Text.RegularExpressions;

#region PART1

using Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Day3.input.sql");
if (stream is null) return;
using StreamReader reader = new(stream);

Regex rx = new(@"\d+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
int total = 0;
int lineNumber = 0;

List<Tuple<int, int, int, int>> numbers = new();

string[] fileData = reader.ReadToEnd().Split("\n");

stream.Position = 0;
reader.DiscardBufferedData();

while (!reader.EndOfStream)
{
    string? line = reader.ReadLine();
    if (line is null) continue;
    MatchCollection matches = rx.Matches(line);
    foreach(Match match in matches)
    {
        //line, index start, length, value
        numbers.Add(new(lineNumber, match.Index, match.Length, int.Parse(match.Value)));
    }
    lineNumber++;
}

foreach(Tuple<int, int, int, int> number in numbers)
{
    for(int j = number.Item1 - 1; j <= number.Item1 + 1; j++)
    {
        if (j >= 0 && j < fileData.Length)
        {
            for (int i = number.Item2 - 1; i < number.Item2 + number.Item3 + 1; i++)
            {
                if (i >= 0 && i < fileData[j].Length)
                {
                    char symbol = fileData[j][i];
                    if (!char.IsNumber(symbol) && !symbol.Equals('.'))
                    {
                        total += number.Item4;
                        continue;
                    }
                }
            }
        }
    }
}

Console.WriteLine(total);

#endregion

#region PART2

stream.Position = 0;
reader.DiscardBufferedData();
total = 0;
lineNumber = 0;

rx = new(@"[*]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

List<Tuple<int, int>> stars = new();

while (!reader.EndOfStream)
{
    string? line = reader.ReadLine();
    if (line is null) continue;
    MatchCollection matches = rx.Matches(line);
    foreach (Match match in matches)
    {
        //line, index start
        stars.Add(new(lineNumber, match.Index));
    }
    lineNumber++;
}

foreach(Tuple<int, int> star in stars)
{
    List<Tuple<int, int, int, int>> numbersNearStar = new();
    for (int j = star.Item1 - 1; j <= star.Item1 + 1; j++)
    {
        if (j >= 0 && j < fileData.Length)
        {
            for (int i = star.Item2 - 1; i < star.Item2 + 2; i++)
            {
                if (i >= 0 && i < fileData[j].Length && char.IsNumber(fileData[j][i]))
                {
                    foreach(Tuple<int, int, int, int> number in numbers)
                    {
                        if(number.Item1 == j && number.Item2 <= i && number.Item2 + number.Item3 >= i && !numbersNearStar.Contains(number))
                        {
                            numbersNearStar.Add(number);
                            break;
                        }
                    }
                }
            }
        }
    }
    if(numbersNearStar.Count == 2)
    {
        total += numbersNearStar[0].Item4 * numbersNearStar[1].Item4;
    }
}

Console.WriteLine(total);

#endregion