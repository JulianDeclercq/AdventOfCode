namespace AdventOfCode2022.Days;

public class Day3
{
    private static int Score(char input) => char.ToLower(input) - 'a' + (char.IsLower(input) ? 1 : 27);
    public static void Solve()
    {
        var lines = File.ReadAllLines(@"..\..\..\input\day3.txt");
        var result = 0;
        foreach (var line in lines)
        {
            var half = line.Length / 2;
            var sharedItem = line[..half].Intersect(line[^half..]).Single();
            result += Score(sharedItem);
        }
        Console.WriteLine(result);
    }
    
    public static void Solve2()
    {
        var lines = File.ReadAllLines(@"..\..\..\input\day3.txt");
        var result = 0;
        for (var i = 0 ; i < lines.Length - 2; i += 3)
        {
            var sharedItem = lines[i + 0].Intersect(lines[i + 1]).Intersect(lines[i + 2]).Single();
            result += Score(sharedItem);
        }
        Console.WriteLine(result);
    }
}