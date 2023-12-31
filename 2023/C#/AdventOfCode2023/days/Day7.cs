namespace AdventOfCode2023.days;

public static class Day7
{
    private static bool _part1 = true;

    private class Hand
    {
        public string Cards = "";
        public string TransformedCards = "";
        public ulong Bid;
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
            answer += hands[i].Bid * rank;
        }
        Console.WriteLine(answer);
    }

    private static Hand LineToHand(string line)
    {
        var split = line.Split(' ');
        return new Hand
        {
            Cards = split[0],
            Bid = ulong.Parse(split[1]),
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
            2 => occurrences.Any(o => o.Key != highestOccurence.Key && o.Value == 2)
                ? (ulong)100_000_000 // two pair
                : (ulong)10_000_000, // pair
            1 => 1_000_000, // high card
            _ => throw new Exception("Unable to calculate score.")
        };
    }

    private static int CardValue(char card)
    {
        return card switch
        {
            'A' => 14,
            'K' => 13,
            'Q' => 12,
            'J' => _part1 ? 11 : 1,
            'T' => 10,
            _ => int.Parse($"{card}")
        };
    }
    
    private static int HandCompare(Hand lhs, Hand rhs) // 1, 0, -1
    {
        var lhsTypeScore = TypeScore(lhs);
        var rhsTypeScore = TypeScore(rhs);
        
        if (lhsTypeScore > rhsTypeScore) return 1;
        if (lhsTypeScore < rhsTypeScore) return -1;

        for (var i = 0; i < lhs.Cards.Length; ++i)
        {
            if (CardValue(lhs.Cards[i]) > CardValue(rhs.Cards[i]))
                return 1;
            
            if (CardValue(lhs.Cards[i]) < CardValue(rhs.Cards[i]))
                return -1;
        }

        return 0;
    }
    
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
        var highestOccurrence = occurrences.Where(o => o.Value == max).MaxBy(o => CardValue(o.Key));
        return cards.Replace('J', highestOccurrence.Key);
    }
}