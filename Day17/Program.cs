#region PART1

string[] lines = File.ReadAllLines("input.sql");

int[][] graph = lines.Select(s => s.ToCharArray().Select(c => (int)char.GetNumericValue(c)).ToArray()).ToArray();
//(heatloss, row, column, delta row, delta column, n), priority => heatloss
PriorityQueue<(int, int, int, int, int, int), int> queue = new();
HashSet<(int, int, int, int, int)> seen = new();

queue.Enqueue((0, 0, 0, 0, 0, 0), 0);

while (queue.Count > 0)
{
    (int heatLoss, int row, int column, int deltaRow, int deltaColumn, int n) = queue.Dequeue();

    if(row == graph.Length - 1 && column == graph[0].Length - 1)
    {
        Console.WriteLine(heatLoss);
        break;
    }

    if (seen.Contains((row, column, deltaRow, deltaColumn, n))) continue;

    seen.Add((row, column, deltaRow, deltaColumn, n));

    if(n < 3 && (deltaRow, deltaColumn) != (0, 0))
    {
        int nr = row + deltaRow;
        int nc = column + deltaColumn;
        if(0 <= nr && nr < graph.Length && 0 <= nc && nc < graph[0].Length)
            queue.Enqueue((heatLoss + graph[nr][nc], nr, nc, deltaRow, deltaColumn, n + 1), heatLoss + graph[nr][nc]);
    }

    foreach((int ndr, int ndc) in new List<(int, int)> { (0, 1), (1, 0), (0, -1), (-1, 0) }) 
    {
        if((ndr, ndc) != (deltaRow, deltaColumn) && (ndr, ndc) != (-deltaRow, -deltaColumn))
        {
            int nr = row + ndr;
            int nc = column + ndc;
            if (0 <= nr && nr < graph.Length && 0 <= nc && nc < graph[0].Length)
                queue.Enqueue((heatLoss + graph[nr][nc], nr, nc, ndr, ndc, 1), heatLoss + graph[nr][nc]);
        }
    }
}

#endregion

#region PART2

queue.Clear();
seen.Clear();

queue.Enqueue((0, 0, 0, 0, 0, 0), 0);

while (queue.Count > 0)
{
    (int heatLoss, int row, int column, int deltaRow, int deltaColumn, int n) = queue.Dequeue();

    if (row == graph.Length - 1 && column == graph[0].Length - 1 && n >= 4)
    {
        Console.WriteLine(heatLoss);
        break;
    }

    if (seen.Contains((row, column, deltaRow, deltaColumn, n))) continue;

    seen.Add((row, column, deltaRow, deltaColumn, n));

    if (n < 10 && (deltaRow, deltaColumn) != (0, 0))
    {
        int nr = row + deltaRow;
        int nc = column + deltaColumn;
        if (0 <= nr && nr < graph.Length && 0 <= nc && nc < graph[0].Length)
            queue.Enqueue((heatLoss + graph[nr][nc], nr, nc, deltaRow, deltaColumn, n + 1), heatLoss + graph[nr][nc]);
    }

    if(n >= 4 || (deltaRow, deltaColumn) == (0, 0))
    {
        foreach ((int ndr, int ndc) in new List<(int, int)> { (0, 1), (1, 0), (0, -1), (-1, 0) })
        {
            if ((ndr, ndc) != (deltaRow, deltaColumn) && (ndr, ndc) != (-deltaRow, -deltaColumn))
            {
                int nr = row + ndr;
                int nc = column + ndc;
                if (0 <= nr && nr < graph.Length && 0 <= nc && nc < graph[0].Length)
                    queue.Enqueue((heatLoss + graph[nr][nc], nr, nc, ndr, ndc, 1), heatLoss + graph[nr][nc]);
            }
        }
    }
}

#endregion