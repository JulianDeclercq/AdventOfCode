namespace AdventOfCode2022.Days;

public class Day1
{
    public void Solve()
    {
        //var lines = File.ReadAllLines(@"..\..\..\input\day1_example.txt");
        var lines = File.ReadAllLines(@"..\..\..\input\day1.txt");
        var totalWeightPerPerson = new List<int>();
        var current = 0;
        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                totalWeightPerPerson.Add(current);
                current = 0;
                continue;
            }

            current += int.Parse(line);
        }
        Console.WriteLine($"Day 1 Part 1: {totalWeightPerPerson.Max()}");
        Console.WriteLine($"Day 1 Part 2: {totalWeightPerPerson.OrderByDescending(x => x).Take(3).Sum()}");
    }
}