using AdventOfCode2024;

var watch = System.Diagnostics.Stopwatch.StartNew();

// var day = new Day15("input/real/day15.txt");
var day = new Day15("input/real/day15.txt", part: 2);
day.Solve();

watch.Stop();
Console.WriteLine(watch.Elapsed);