using AdventOfCode2024;

var watch = System.Diagnostics.Stopwatch.StartNew();

// var day = new Day21("input/example/day21e.txt");
var day = new Day21("input/real/day21.txt");
day.Part1();
// Console.WriteLine(day.Part2());

watch.Stop();
Console.WriteLine(watch.Elapsed);
