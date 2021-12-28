namespace AdventOfCode2018.days;

public class Day1
{
    public void Part1()
    {
        var input = File.ReadAllLines(@"..\..\..\input\day1.txt").Select(int.Parse);
        Console.WriteLine($"Day 1 part 1: {input.Sum()}");
    }
}