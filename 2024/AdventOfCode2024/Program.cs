using AdventOfCode2024;

var watch = System.Diagnostics.Stopwatch.StartNew();

// var day = new Day18("input/example/day18e.txt");
var day = new Day18("input/real/day18.txt");
day.Solve();

watch.Stop();
Console.WriteLine(watch.Elapsed);
