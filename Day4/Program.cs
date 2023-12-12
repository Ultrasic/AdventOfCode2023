using System.Reflection;

#region PART1

using Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Day4.input.sql");
if (stream is null) return;
using StreamReader reader = new(stream);

int total = 0;

while (!reader.EndOfStream)
{
    string? line = reader.ReadLine();
    if (line is null) continue;
    string card = line.Split(": ")[1];
    string[] winningNumbers = card.Split(" | ")[0].Split(" ").Where(x => !string.IsNullOrEmpty(x)).ToArray();
    IEnumerable<string> actualCard = card.Split(" | ")[1].Split(" ").Where(x => !string.IsNullOrEmpty(x));
    int pointsForCard = 0;
    actualCard.Where(winningNumbers.Contains).ToList().ForEach(x => pointsForCard = pointsForCard == 0 ? 1 : pointsForCard * 2);
    total += pointsForCard;
}

Console.WriteLine(total);

#endregion

#region PART2

Dictionary<int, int> cards = new();

stream.Position = 0;
reader.DiscardBufferedData();
total = 0;
int currentCard = 1;

while (!reader.EndOfStream)
{
    string? line = reader.ReadLine();
    if (line is null) continue;
    if (cards.ContainsKey(currentCard))
    {
        cards[currentCard]++;
    }
    else
    {
        cards.Add(currentCard, 1);
    }
    string card = line.Split(": ")[1];
    string[] winningNumbers = card.Split(" | ")[0].Split(" ").Where(x => !string.IsNullOrEmpty(x)).ToArray();
    IEnumerable<string> actualCard = card.Split(" | ")[1].Split(" ").Where(x => !string.IsNullOrEmpty(x));
    int nextCardCounter = currentCard + 1;
    actualCard.Where(winningNumbers.Contains).ToList().ForEach(x => {
        if (cards.ContainsKey(nextCardCounter))
        {
            cards[nextCardCounter] += cards[currentCard];
        }
        else
        {
            cards.Add(nextCardCounter, cards[currentCard]);
        }
        nextCardCounter++;
    });
    currentCard++;
}

Console.WriteLine(cards.Sum(x => { return x.Value; }));

#endregion