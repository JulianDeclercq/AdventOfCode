namespace AdventOfCode2021.days
{
    public class Day15
    {
        // Thanks to https://www.redblobgames.com/pathfinding/a-star/introduction.html for the amazing pathfinding article!
        public void Part1()
        {
            var lines = File.ReadAllLines(@"..\..\..\input\day15.txt");
            int width = lines[0].Length, height = lines.Length;
            var grid = new Grid<int>(width, height, 
                lines.SelectMany(l => l.ToCharArray()).Select(Helpers.ToInt), int.MinValue);

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
            
            Console.WriteLine($"Day 15 part 1: {path.Select(grid.At).Sum()}");

            // debug printing
            //path.Reverse(); path.Print(); path.Select(grid.At).Print();
        }
    }
}