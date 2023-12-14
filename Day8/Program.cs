using System.Reflection;

#region PART1

using Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Day8.input.sql");
if (stream is null) return;
using StreamReader reader = new(stream);

string? line = reader.ReadLine();
if (line is null) return;

string instructions = line;
reader.ReadLine();

Dictionary<string, (string, string)> nodes = new();

while (!reader.EndOfStream)
{
    line = reader.ReadLine();
    if (line is null) return;
    string[] split = line.Split(" = ");
    nodes.Add(split[0], (split[1].Substring(1, 3), split[1].Substring(6, 3)));
}

string currentNode = "AAA";
int steps = 0;
while (!currentNode.Equals("ZZZ"))
{
    char direction = instructions.ElementAt(steps % instructions.Length);
    currentNode = direction.Equals('R') ? nodes[currentNode].Item2 : nodes[currentNode].Item1;
    steps++;
}

Console.WriteLine(steps);

#endregion

#region PART2

string[] currentNodes = nodes.Keys.Where(s => s.EndsWith("A")).ToArray();

long[] stepsNeeded = new long[currentNodes.Length];
for (int i = 0; i < currentNodes.Length; i++)
{
    steps = 0;
    while (!currentNodes[i].EndsWith("Z"))
    {
        char direction = instructions.ElementAt(steps % instructions.Length);
        currentNodes[i] = direction.Equals('R') ? nodes[currentNodes[i]].Item2 : nodes[currentNodes[i]].Item1;
        steps++;
    }
    stepsNeeded[i] = steps;
}

Console.WriteLine(LCM(stepsNeeded));

static long LCM(long[] numbers)
{
    return numbers.Aggregate(lcm);
}
static long lcm(long a, long b)
{
    return Math.Abs(a * b) / GCD(a, b);
}
static long GCD(long a, long b)
{
    return b == 0 ? a : GCD(b, a % b);
}

#endregion