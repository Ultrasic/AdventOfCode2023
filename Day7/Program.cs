using System.Reflection;

#region PART1

using Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Day7.input.sql");
if (stream is null) return;
using StreamReader reader = new(stream);

Dictionary<string, int> hands = new();

while (!reader.EndOfStream)
{
    string? line = reader.ReadLine();
    if (line is null) return;

    hands.Add(line.Split(" ")[0], int.Parse(line.Split(" ")[1]));
}

var sortedHands = hands.OrderByDescending(kv => kv, new HandComparer());
int counter = sortedHands.Count();

Console.WriteLine(sortedHands.Aggregate(0, (acc, val) => acc + val.Value * counter--));

#endregion

#region PART2

sortedHands = hands.OrderByDescending(kv => kv, new HandComparer() { OrderedValues = "J23456789TQKA", SpecialRule = true });
counter = sortedHands.Count();

Console.WriteLine(sortedHands.Aggregate(0, (acc, val) => acc + val.Value * counter--));

#endregion

#region HandComparer

class HandComparer : IComparer<KeyValuePair<string, int>>
{
    public string OrderedValues { get; set; } = "23456789TJQKA";
    public bool SpecialRule { get; set; } = false;
    public int Compare(KeyValuePair<string, int> x, KeyValuePair<string, int> y)
    {
        int xStrength = 0;
        int yStrength = 0;
        if (!SpecialRule)
        {
            xStrength = EvaluateHandStrength(x.Key);
            yStrength = EvaluateHandStrength(y.Key);
        }
        else
        {
            var xMostRepresented = x.Key.Where(c => !c.Equals('J')).GroupBy(c => c).OrderByDescending(group => group.Count()).FirstOrDefault();
            var yMostRepresented = y.Key.Where(c => !c.Equals('J')).GroupBy(c => c).OrderByDescending(group => group.Count()).FirstOrDefault();
            string replacedXKey = x.Key.Replace('J', xMostRepresented is not null ? xMostRepresented.Key : 'J');
            string replacedYKey = y.Key.Replace('J', yMostRepresented is not null ? yMostRepresented.Key : 'J');
            xStrength = EvaluateHandStrength(replacedXKey);
            yStrength = EvaluateHandStrength(replacedYKey);
        }

        if(xStrength > yStrength) return 1;
        else if (xStrength < yStrength) return -1;
        else return CompareHandsByCards(x.Key, y.Key);
    }

    private int CompareHandsByCards(string handX, string handY)
    {
        for (int i = 0; i < handX.Length; i++)
        {
            int compareResult = OrderedValues.IndexOf(handX[i]).CompareTo(OrderedValues.IndexOf(handY[i]));

            if (compareResult != 0)
            {
                return compareResult;
            }
        }

        return 0;
    }

    private int EvaluateHandStrength(string hand)
    {
        //Only 1 type of card in hand => Five of a kind
        if (hand.Distinct().Count() == 1) return 7;
        //If there is a group of cards with 4 number of cards => Four of a kind
        if (hand.GroupBy(c => c).Any(group => group.Count() == 4)) return 6;
        //If there is a group of 3 cards and a group of 2 cards => Full house
        if (hand.GroupBy(c => c).Any(group => group.Count() == 3) && hand.GroupBy(c => c).Any(group => group.Count() == 2)) return 5;
        //If there is a group of 3 cards => Three of a kind
        if (hand.GroupBy(c => c).Any(group => group.Count() == 3)) return 4;
        //If there are two groups of 2 cards => Two pair
        if (hand.GroupBy(c => c).Count(group => group.Count() == 2) == 2) return 3;
        //If there is one group of 2 cards => One pair
        if (hand.GroupBy(c => c).Any(group => group.Count() == 2)) return 2;
        //High card
        return 1;
    }
}

#endregion