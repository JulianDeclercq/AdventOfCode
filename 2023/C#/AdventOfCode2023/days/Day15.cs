namespace AdventOfCode2023.days;

public class Day15
{
    public void Solve()
    {
        var input = File.ReadAllLines("../../../input/Day15.txt").First().Split(',');
        Console.WriteLine(input.Sum(Hash));
    }

    private static int Hash(string input)
    {
        var hash = 0;
        foreach (var c in input)
        {
            hash += c;
            hash *= 17;
            hash %= 256;
        }

        return hash;
    }
}