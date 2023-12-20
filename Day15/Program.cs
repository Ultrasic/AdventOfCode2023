using System.Text;

#region PART1

string[] lines = File.ReadAllLines("input.sql");

Console.WriteLine(lines[0].Split(',').Select(Hash).Sum());

#endregion

#region PART2

Dictionary<int, List<(string, int)>> boxes = Enumerable.Range(0, 256).ToDictionary(obj => obj, obj => new List<(string, int)>());

foreach (string label in lines[0].Split(','))
{
    if (label.Contains('-'))
    {
        string labelToFind = label.Replace("-", string.Empty);
        int boxNumber = Hash(labelToFind);
        int indexOfKeyInBoxes = boxes[boxNumber].FindIndex(tuple => tuple.Item1.Equals(labelToFind));
        if (indexOfKeyInBoxes > -1)
            boxes[boxNumber].RemoveAt(indexOfKeyInBoxes);
    }
    else
    {
        string[] labelAndLens = label.Split('=');
        int boxNumber = Hash(labelAndLens[0]);
        (string, int) key = (labelAndLens[0], int.Parse(labelAndLens[1]));
        int indexOfKeyInBoxes = boxes[boxNumber].FindIndex(tuple => tuple.Item1.Equals(key.Item1));
        if (indexOfKeyInBoxes < 0)
        {
            boxes[boxNumber].Add(key);
        }
        else
        {
            boxes[boxNumber][indexOfKeyInBoxes] = key;
        }
    }
}

Console.WriteLine(boxes.Select((key, index) => key.Value.Select((tuple, index) => (key.Key+1) * (index+1) * tuple.Item2).Sum()).Sum());

#endregion

static int Hash(string stringValue)
{
    return Encoding.ASCII.GetBytes(stringValue).Aggregate(0, (total, current) => (total + current) * 17 % 256);
}