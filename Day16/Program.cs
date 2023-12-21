#region PART1

string[] lines = File.ReadAllLines("input.sql");

Console.WriteLine(CalculateNumberOfEnergizedCells(0, -1, 0, 1));

#endregion

#region PART2

int maxValue = 0;

for(int i = 0; i < lines.Length; i++)
{
    maxValue = Math.Max(maxValue, CalculateNumberOfEnergizedCells(i, -1, 0, 1));
    maxValue = Math.Max(maxValue, CalculateNumberOfEnergizedCells(i, lines[0].Length, 0, 1));
    maxValue = Math.Max(maxValue, CalculateNumberOfEnergizedCells(-1, i, 1, 0));
    maxValue = Math.Max(maxValue, CalculateNumberOfEnergizedCells(lines.Length, i, -1, 0));
}

Console.WriteLine(maxValue);

#endregion

#region METHODS

int CalculateNumberOfEnergizedCells(int r, int c, int dr, int dc)
{
    //row, column, delta row, delta column
    Queue<int[]> beams = new();
    //row, column
    HashSet<(int, int, int, int)> seen = new();

    beams.Enqueue(new int[] { r, c, dr, dc });

    while (beams.Count > 0)
    {
        int[] beam = beams.Dequeue();

        beam[0] += beam[2];
        beam[1] += beam[3];

        if (beam[0] < 0 || beam[0] >= lines.Length || beam[1] < 0 || beam[1] >= lines[0].Length)
            continue;

        char character = lines[beam[0]][beam[1]];

        if (character.Equals('.') || (character.Equals('-') && beam[2] == 0) || (character.Equals('|') && beam[3] == 0))
        {
            (int, int, int, int) key = (beam[0], beam[1], beam[2], beam[3]);
            if (!seen.Contains(key))
            {
                seen.Add(key);
                beams.Enqueue(new int[] { beam[0], beam[1], beam[2], beam[3] });
            }
        }
        else if (character.Equals('/'))
        {
            (beam[2], beam[3]) = (-beam[3], -beam[2]);
            (int, int, int, int) key = (beam[0], beam[1], beam[2], beam[3]);
            if (!seen.Contains(key))
            {
                seen.Add(key);
                beams.Enqueue(new int[] { beam[0], beam[1], beam[2], beam[3] });
            }
        }
        else if (character.Equals('\\'))
        {
            (beam[2], beam[3]) = (beam[3], beam[2]);
            (int, int, int, int) key = (beam[0], beam[1], beam[2], beam[3]);
            if (!seen.Contains(key))
            {
                seen.Add(key);
                beams.Enqueue(new int[] { beam[0], beam[1], beam[2], beam[3] });
            }
        }
        else
        {
            if (character.Equals('|'))
            {
                beam[3] = 0;
                for (beam[2] = -1; beam[2] <= 1; beam[2] += 2)
                {
                    (int, int, int, int) key = (beam[0], beam[1], beam[2], beam[3]);
                    if (!seen.Contains(key))
                    {
                        seen.Add(key);
                        beams.Enqueue(new int[] { beam[0], beam[1], beam[2], beam[3] });
                    }
                }
            }
            else
            {
                beam[2] = 0;
                for (beam[3] = -1; beam[3] <= 1; beam[3] += 2)
                {
                    (int, int, int, int) key = (beam[0], beam[1], beam[2], beam[3]);
                    if (!seen.Contains(key))
                    {
                        seen.Add(key);
                        beams.Enqueue(new int[] { beam[0], beam[1], beam[2], beam[3] });
                    }
                }
            }
        }
    }

    HashSet<(int, int)> coords = seen.Select(t => (t.Item1, t.Item2)).ToHashSet();
    return coords.Count();
}

#endregion