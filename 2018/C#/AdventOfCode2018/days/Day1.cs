namespace AdventOfCode2018.days;

public class Day1
{
    public void Part1()
    {
        var input = File.ReadAllLines(@"..\..\..\input\day1.txt").Select(int.Parse);
        Console.WriteLine($"Day 1 part 1: {input.Sum()}");
    }

    public void Part2()
    {
        var input = File.ReadAllLines(@"..\..\..\input\day1.txt").Select(int.Parse).ToArray();
        var current = 0;
        var frequencies = new List<int>(){current};
        for (;;)
        {
            foreach (var number in input)
            {
                current += number;

                if (frequencies.Contains(current))
                {
                    Console.WriteLine($"Day 1 part 2: {current}");
                    return;
                }
                
                frequencies.Add(current);
            }

        }
    }
}