using System.Text;

namespace AdventOfCode2018.days;

public class Day2
{
    public void Part1()
    {
        var input = File.ReadAllLines(@"..\..\..\input\day2.txt");
        List<string> exactlyTwo = new(), exactlyThree = new();
        foreach (var line in input)
        {
            var map = new Dictionary<char, int>();
            foreach (var c in line)
            {
                map.TryGetValue(c, out var count);
                map[c] = count + 1;
            }
            
            if (map.Values.Any(x => x == 2))
                exactlyTwo.Add(line);
            
            if (map.Values.Any(x => x == 3))
                exactlyThree.Add(line);
        }
        Console.WriteLine($"Day 2 part 1: {exactlyTwo.Count * exactlyThree.Count}");
    }
    
    public void Part2()
    {
        var input = File.ReadAllLines(@"..\..\..\input\day2.txt").ToList();
        var idLength = input[0].Length;
        foreach (var lhs in input)
        {
            foreach (var rhs in input)
            {
                int differences = 0, differenceIdx = 0;
                for (var i = 0; i < idLength; ++i)
                {
                    if (lhs[i] == rhs[i]) 
                        continue;
                    
                    differences++;
                    differenceIdx = i;
                }

                if (differences != 1) 
                    continue;
                
                var answer = new StringBuilder(lhs).Remove(differenceIdx, 1);
                Console.WriteLine($"Day 2 part 2: {answer}");
                return;
            }
        }
    }
}