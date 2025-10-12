using AdventOfCode2024;

var watch = System.Diagnostics.Stopwatch.StartNew();

var day = new Day17("input/example/day17e.txt");
day.Solve();

watch.Stop();
Console.WriteLine(watch.Elapsed);