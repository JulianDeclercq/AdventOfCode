namespace AdventOfCode2021.days;

public class Day14
{
    public void Part1()
    {
        var lines = File.ReadAllLines(@"..\..\..\input\day14.txt");
        var polymerTemplate = lines.First();

        var insertionRules = lines.Skip(2).Select(line => line.Remove(line.IndexOf('-'), 1).Split('>'))
            .ToDictionary(split => split[0].Trim(), split =>
            {
                var pair = split[0].Trim();
                var toInsert = split[1].Trim()[0];
                return (toInsert, $"{pair[0]}{toInsert}", $"{toInsert}{pair[1]}");
            });

        //foreach(var el in insertionRules) Console.WriteLine($"{el.Key}|{el.Value}");

        var sequence = new Dictionary<string, int>();
        var occurenceCounter = new Dictionary<char, int>();

        for (var i = 0; i < polymerTemplate.Length; ++i)
        {
            occurenceCounter.TryGetValue(polymerTemplate[i], out var count);
            occurenceCounter[polymerTemplate[i]] = count + 1;
            
            if (i < polymerTemplate.Length - 1)
                sequence.Add($"{polymerTemplate[i]}{polymerTemplate[i+1]}", 1);
        }
        
        const int steps = 10;
        var newSequence = sequence.ToDictionary(x => x.Key, x => x.Value);
        for (var i = 0; i < steps; ++i)
        {
            foreach (var (pair, amount) in sequence)
            {
                if (amount == 0 || !insertionRules.TryGetValue(pair, out var resultingPairs))
                    continue;

                // count the occurrences
                occurenceCounter.TryGetValue(resultingPairs.toInsert, out var count1);
                occurenceCounter[resultingPairs.toInsert] = count1 + amount;
                
                // handle the insertion
                newSequence[pair] -= amount;
                
                newSequence.TryGetValue(resultingPairs.Item2, out var count2);
                newSequence[resultingPairs.Item2] = count2 + amount;
                
                newSequence.TryGetValue(resultingPairs.Item3, out var count3);
                newSequence[resultingPairs.Item3] = count3 + amount;
                
                
            }
            sequence = newSequence.ToDictionary(x => x.Key, x => x.Value);
        }
        
        foreach(var e in occurenceCounter)
            Console.WriteLine($"{e.Key}|{e.Value}");
        
        Console.WriteLine($"Day 14 part 1: {occurenceCounter.Values.Max() - occurenceCounter.Values.Min()}");
        
        

        //        foreach(var el in map) Console.WriteLine($"{el.Key}|{el.Value}");

    }
}