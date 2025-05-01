using AdventOfCode2024;

var watch = System.Diagnostics.Stopwatch.StartNew();

var day = new Day16("input/example/day16e.txt");
day.Solve();

watch.Stop();
Console.WriteLine(watch.Elapsed);