namespace AdventOfCode2022.Days;

public class Day3
{
    public void Solve()
    {
        // var lines = File.ReadAllLines(@"..\..\..\input\day3_example.txt");
        var lines = File.ReadAllLines(@"..\..\..\input\day3.txt");
        var result = 0;
        foreach (var line in lines)
        {
            var half = line.Length / 2;
            var compartment1 = line[..half];
            var compartment2 = line[^half..];
            var sharedItem = compartment1.Intersect(compartment2).Single();
            var score = char.ToLower(sharedItem) - 'a' + (char.IsLower(sharedItem) ? 1 : 27);
            result += score;
        }
        Console.WriteLine(result);
    }
}