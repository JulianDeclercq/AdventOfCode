namespace AdventOfCode2023.days;

public class Day7
{
    private record Hand(string Cards, ulong Bet);
    public void Part1()
    {
        //var hands = File.ReadAllLines("../../../input/Day7_example.txt").Select(LineToHand).OrderBy(HandScore).ToList();
        var hands = File.ReadAllLines("../../../input/Day7.txt").Select(LineToHand).OrderBy(HandScore).ToList();

        ulong answer = 0;
        for (var i = 0; i < hands.Count; ++i)
        {
            var rank = (ulong)i + 1;
            answer += hands[i].Bet * rank;
        }
        Console.WriteLine(answer);
    }

    private Hand LineToHand(string line)
    {
        var split = line.Split(' ');
        return new Hand(split[0], ulong.Parse(split[1]));
    }

    private static ulong HandScore(Hand hand) => TypeScore(hand) + ValueScores(hand);

    private static ulong TypeScore(Hand hand)
    {
        var occurrences = new Dictionary<char, int>();
        foreach (var card in hand.Cards)
        {
            var current = occurrences.GetValueOrDefault(card, 0);
            occurrences[card] = current + 1;
        }

        var highestOccurence = occurrences.MaxBy(o => o.Value);
        switch (highestOccurence.Value)
        {
            case 5:
                return 1_000_000_000_000; // 5 of a kind
            case 4:
                return 100_000_000_000; // 4 of a kind
            case 3:
                if (occurrences.Any(o => o.Value == 2)) // full house
                    return 10_000_000_000;
                
                return 1_000_000_000; // three of a kind
            case 2:
                if (occurrences.Where(o => o.Key != highestOccurence.Key).Any(o => o.Value == 2)) // two pair
                    return 100_000_000;

                return 10_000_000; // pair
            case 1:
                return 1_000_000; // high card
            default: throw new Exception("Unable to calculate score.");
        }
    }

    private static readonly Dictionary<char, int> CardValues = new()
    {
        ['A'] = 14, ['K'] = 13, ['Q'] = 12, ['J'] = 11, ['T'] = 10,
        ['9'] = 9, ['8'] = 8, ['7'] = 7, ['6'] = 6, ['5'] = 5, ['4'] = 4, ['3'] = 3, ['2'] = 2
    };
    
    private static ulong ValueScores(Hand hand)
    {
        double valueScore = 0;
        for (var i = 0; i < hand.Cards.Length; ++i)
        {
            // the earlier in the hand the stronger, 5 is hand length
            valueScore += Math.Pow(CardValues[hand.Cards[i]], 5 - i);
        }

        return (ulong) valueScore;
    }
}