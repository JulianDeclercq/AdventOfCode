using AdventOfCode2024;

var watch = System.Diagnostics.Stopwatch.StartNew();

// var day = new Day17("input/example/day17e.txt");
var day = new Day17("input/example/day17e2.txt");
// var day = new Day17("input/real/day17.txt");
// day.Part1();
day.Part2();

watch.Stop();
Console.WriteLine(watch.Elapsed);