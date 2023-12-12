using System.Reflection;

#region PART1

using Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Day2.input.sql");
if (stream is null) return;
using StreamReader reader = new(stream);

Dictionary<string, int> rules = new Dictionary<string, int>() 
{
    { "red", 12 }, {"green", 13}, {"blue",  14}
};

int total = 0;

while (!reader.EndOfStream)
{
    string? line = reader.ReadLine();
    if (line is null) continue;

    string[] gameAndCubes = line.Split(": ");
    bool isValid = true;
    foreach (string draw in gameAndCubes[1].Split("; "))
    {
        foreach(string cube in draw.Split(", "))
        {
            string[] cubeNumberAndName = cube.Split(" ");
            if (int.Parse(cubeNumberAndName[0]) > rules[cubeNumberAndName[1]])
            {
                isValid = false;
                break;
            }
        }
        if(!isValid) break;
    }
    if (isValid)
        total += int.Parse(gameAndCubes[0].Split(" ")[1]);
}

Console.WriteLine(total);

#endregion

#region PART2

stream.Position = 0;
reader.DiscardBufferedData();

total = 0;

while (!reader.EndOfStream)
{
    string? line = reader.ReadLine();
    if (line is null) continue;

    string[] gameAndCubes = line.Split(": ");

    Dictionary<string, int> minimum = new Dictionary<string, int>()
    {
        { "red", 0 }, {"green", 0}, {"blue",  0}
    };

    foreach (string draw in gameAndCubes[1].Split("; "))
    {

        foreach (string cube in draw.Split(", "))
        {
            string[] cubeNumberAndName = cube.Split(" ");
            if (int.Parse(cubeNumberAndName[0]) > minimum[cubeNumberAndName[1]])
            {
                minimum[cubeNumberAndName[1]] = int.Parse(cubeNumberAndName[0]);
            }
        }
    }
    total += minimum["red"] * minimum["green"] * minimum["blue"];
}

Console.WriteLine(total);

#endregion