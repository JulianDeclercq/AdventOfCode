using System.Text;

namespace AdventOfCode2021.days
{
    // Thanks to https://www.redblobgames.com/pathfinding/a-star/introduction.html for the amazing pathfinding article!
    public class Day15
    {
        // returns path with lowest risk
        private static int Solve(IReadOnlyList<string> input)
        {
            int width = input[0].Length, height = input.Count;
            var grid = new Grid<int>(width, height, 
                input.SelectMany(l => l.ToCharArray()).Select(Helpers.ToInt), int.MinValue);

            var first = new Point(0, 0);
            var last = new Point(width - 1, height - 1);

            var frontier = new PriorityQueue<Point, int>();
            frontier.Enqueue(first, 0);
            var cameFrom = new Dictionary<Point, Point>();
            var costSoFar = new Dictionary<Point, int>() {{first, 0}};

            while (frontier.Count > 0)
            {
                var current = frontier.Dequeue();
                
                if (current.Equals(last))
                    break;
                        
                foreach (var next in grid.NeighbouringPoints(current, includeDiagonals: false))
                {
                    var newCost = costSoFar[current] + grid.At(current);
                    
                    // if the new cost is higher than the cheapest cost so far, ignore this path
                    if (costSoFar.ContainsKey(next) && newCost >= costSoFar[next])
                        continue;

                    costSoFar[next] = newCost;
                    frontier.Enqueue(next, newCost);
                    cameFrom[next] = current;
                }
            }

            var path = new List<Point>();
            var cur = last;
            while (!cur.Equals(first))
            {
                path.Add(cur);
                cur = cameFrom[cur];
            }

            return path.Select(grid.At).Sum();

            // debug printing
            //path.Reverse(); path.Print(); path.Select(grid.At).Print();
        }
        
        public void Part1() => Console.WriteLine($"Day 15 part 1: {Solve(File.ReadAllLines(@"..\..\..\input\day15.txt"))}");
        
        public void Part2()
        {
            var lines = File.ReadAllLines(@"..\..\..\input\day15.txt");
            const int timesLarger = 5;

            var last = lines;
            
            // index is iteration of transformation (1 = 1x transformed etc.)
            var transformations = new List<string[]>(Enumerable.Repeat(lines, 9));
            for (var i = 1; i < 9; ++i)
            {
                last = last.Select(Transform).ToArray();
                transformations[i] = last;
            }

            int singleWidth = lines[0].Length, singleHeight = lines.Length;
            var cave = new List<string>(Enumerable.Repeat(string.Empty, timesLarger * singleHeight));

            for (var i = 0; i < singleHeight; ++i)
            {
                var idx = i;
                cave[i] = transformations.Take(timesLarger).SelectMany(x => x[idx]).Stringify();
                cave[i + singleHeight] = transformations.Skip(1).Take(timesLarger).SelectMany(x => x[idx]).Stringify();
                cave[i + singleHeight * 2] = transformations.Skip(2).Take(timesLarger).SelectMany(x => x[idx]).Stringify();
                cave[i + singleHeight * 3] = transformations.Skip(3).Take(timesLarger).SelectMany(x => x[idx]).Stringify();
                cave[i + singleHeight * 4] = transformations.Skip(4).Take(timesLarger).SelectMany(x => x[idx]).Stringify();
            }
            
            Console.WriteLine($"Day 15 part 2: {Solve(cave)}");
        }

        private static string Transform(string original)
        {
            var sb = new StringBuilder(original);
            for (var i = 0; i < original.Length; ++i)
            {
                var current = int.Parse(sb[i].ToString());
                var next = (current + 1) % 10;
                sb[i] = Helpers.DigitToChar(next == 0 ? 1 : next);
            }
            return sb.ToString();
        }
    }
}