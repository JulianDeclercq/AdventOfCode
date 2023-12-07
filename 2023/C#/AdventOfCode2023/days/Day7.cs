namespace AdventOfCode2023.days;

public static class Day7
{
    private static bool _part1 = true;

    private class Hand
    {
        public string Cards = "";
        public string TransformedCards = "";
        public ulong Bet;
    }
    
    public static void Solve(bool part1)
    {
        _part1 = part1;
        
        var hands = File.ReadAllLines("../../../input/Day7.txt").Select(LineToHand).ToList();
        hands.Sort(HandCompare);

        ulong answer = 0;
        for (var i = 0; i < hands.Count; ++i)
        {
            var rank = (ulong)i + 1;
            answer += hands[i].Bet * rank;
        }
        Console.WriteLine(answer);
    }

    private static Hand LineToHand(string line)
    {
        var split = line.Split(' ');
        return new Hand
        {
            Cards = split[0],
            Bet = ulong.Parse(split[1]),
            TransformedCards = JokerTransform(split[0])
        };
    }

    private static ulong TypeScore(Hand hand)
    {
        var occurrences = new Dictionary<char, int>();
        var cards = _part1 ? hand.Cards : hand.TransformedCards;
        foreach (var card in cards)
        {
            var current = occurrences.GetValueOrDefault(card, 0);
            occurrences[card] = current + 1;
        }
        
        var highestOccurence = occurrences.MaxBy(o => o.Value);
        return highestOccurence.Value switch
        {
            5 => 1_000_000_000_000, // 5 of a kind
            4 => 100_000_000_000, // 4 of a kind
            3 => occurrences.Any(o => o.Value == 2)
                ? (ulong)10_000_000_000 // full house
                : (ulong)1_000_000_000, // three of a kind
            2 => occurrences.Where(o => o.Key != highestOccurence.Key).Any(o => o.Value == 2)
                ? (ulong)100_000_000 // two pair
                : (ulong)10_000_000, // pair
            1 => 1_000_000, // high card
            _ => throw new Exception("Unable to calculate score.")
        };
    }

    private static readonly Dictionary<char, int> CardValues = new()
    {
        ['A'] = 14, ['K'] = 13, ['Q'] = 12, ['J'] = 11, ['T'] = 10,
        ['9'] = 9, ['8'] = 8, ['7'] = 7, ['6'] = 6, ['5'] = 5, ['4'] = 4, ['3'] = 3, ['2'] = 2
    };
    
    private static readonly Dictionary<char, int> JokerCardValues = new()
    {
        ['A'] = 14, ['K'] = 13, ['Q'] = 12, ['T'] = 10,
        ['9'] = 9, ['8'] = 8, ['7'] = 7, ['6'] = 6, ['5'] = 5, ['4'] = 4, ['3'] = 3, ['2'] = 2, ['J'] = 1
    };
    
    private static string JokerTransform(string cards)
    {
        // handle edge case
        if (cards.Equals("JJJJJ"))
            return "AAAAA";
        
        var occurrences = new Dictionary<char, int>();
        foreach (var card in cards)
        {
            if (card == 'J') // ignore joker itself
                continue;
            
            var current = occurrences.GetValueOrDefault(card, 0);
            occurrences[card] = current + 1;
        }

        // Replace the most occurring character, pick the one with highest individual value in case of a tie
        var max = occurrences.Max(o => o.Value);
        var highestOccurrence = occurrences.Where(o => o.Value == max).MaxBy(o => CardValues[o.Key]);
        return cards.Replace('J', highestOccurrence.Key);
    }
    
    private static int HandCompare(Hand lhs, Hand rhs) // 1, 0, -1
    {
        var lhsTypeScore = TypeScore(lhs);
        var rhsTypeScore = TypeScore(rhs);
        
        if (lhsTypeScore > rhsTypeScore) return 1;
        if (lhsTypeScore < rhsTypeScore) return -1;

        var lookup = _part1 ? CardValues : JokerCardValues;
        for (var i = 0; i < lhs.Cards.Length; ++i)
        {
            if (lookup[lhs.Cards[i]] > lookup[rhs.Cards[i]])
                return 1;
            
            if (lookup[lhs.Cards[i]] < lookup[rhs.Cards[i]])
                return -1;
        }

        return 0;
    }
}