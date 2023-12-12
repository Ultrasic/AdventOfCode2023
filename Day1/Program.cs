using System.Reflection;
using System.Text.RegularExpressions;

#region PART1

using Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Day1.input.sql");
if (stream is null) return;
using StreamReader reader = new(stream);

Regex rx = new(@"\d", RegexOptions.Compiled | RegexOptions.IgnoreCase);
int total = 0;

while (!reader.EndOfStream)
{
    string? line = reader.ReadLine();
    if (line is null) continue;
    MatchCollection matches = rx.Matches(line);
    if(matches.Count > 0)
        total += int.Parse(matches[0].Value + matches[^1].Value);
}

Console.WriteLine(total);

#endregion

#region PART2

stream.Position = 0;
reader.DiscardBufferedData();

Dictionary<string, string> lettersToNumbers = new Dictionary<string, string>() 
{
    { "one", "1" }, { "two", "2" }, { "three",  "3" }, { "four", "4" }, { "five", "5" }, { "six", "6" }, { "seven", "7" }, { "eight", "8" }, { "nine", "9" },
    { "1", "1" }, { "2", "2" }, { "3",  "3" }, { "4", "4" }, { "5", "5" }, { "6", "6" }, { "7", "7" }, { "8", "8" }, { "9", "9" },
};

rx = new(@"(?=(\d|one|two|three|four|five|six|seven|eight|nine))", RegexOptions.Compiled | RegexOptions.IgnoreCase);
total = 0;

while (!reader.EndOfStream)
{
    string? line = reader.ReadLine();
    if (line is null) continue;
    MatchCollection matches = rx.Matches(line);
    if (matches.Count > 0)
        total += int.Parse(lettersToNumbers[matches[0].Groups[1].Value] + lettersToNumbers[matches[^1].Groups[1].Value]);
}

Console.WriteLine(total);

#endregion