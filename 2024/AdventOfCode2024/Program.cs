using AdventOfCode2024;

var watch = System.Diagnostics.Stopwatch.StartNew();

// var day = new Day15("input/real/day15.txt");
var day = new Day15("input/example/day15e6.txt", part: 2, transformGridPart2: false);
day.Solve();

watch.Stop();
Console.WriteLine(watch.Elapsed);