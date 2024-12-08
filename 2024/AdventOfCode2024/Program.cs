using AdventOfCode2024;

var watch = System.Diagnostics.Stopwatch.StartNew();

Day6.Solve(part: 1);

watch.Stop();
Console.WriteLine(watch.Elapsed);

watch.Restart();
Day6.Solve(part: 2);

watch.Stop();
Console.WriteLine(watch.Elapsed);
