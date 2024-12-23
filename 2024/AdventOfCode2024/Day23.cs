namespace AdventOfCode2024;

public class Day23
{
    public static void Solve(int part)
    {
        if (part != 1 && part != 2)
            throw new Exception($"Invalid part {part}");
        
        var lines = File.ReadAllLines("input/day23.txt");
        Dictionary<string, HashSet<string>> connections = [];
        foreach (var line in lines)
        {
            var split = line.Split('-');
            AddConnection(split[0], split[1], connections);
            AddConnection(split[1], split[0], connections);
        }

        if (part is 1) Part1(connections);
        else Part2(connections);
    }
    
    private static void Part1(Dictionary<string, HashSet<string>> connections)
    {
        // find sets of three connected computers where each computer in the set is connected to the other two computers
        HashSet<TriLink> triLinks = [];
        foreach (var computer in connections.Keys)
        {
            foreach (var connections1 in connections[computer])
            {
                foreach (var connections2 in connections[connections1])
                {
                    var connections3 = connections[connections2];
                    if (connections3.Contains(computer))
                    {
                        triLinks.Add(new TriLink
                        {
                            First = computer,
                            Second = connections1,
                            Third = connections2
                        });
                    }
                }
            }
        }
        
        var answer = triLinks.Count(triLink => triLink.HasComputerThatStartsWithT());
        Console.WriteLine(answer);
    }
    
    // contains partial networks (while they're still in the making) but that shouldn't matter for the solution
    private static List<Network> ValidNetworks = [];

    private static void Part2(Dictionary<string, HashSet<string>> lookup)
    {
        foreach (var computer in lookup.Keys)
        {
            var network = new Network();
            network.Add(computer);
            Part2Logic(network, lookup);
        }

        var answer = ValidNetworks.MaxBy(network => network.Nodes.Count);
        Console.WriteLine(answer);
    }
    
    private static void Part2Logic(Network network, Dictionary<string, HashSet<string>> lookup)
    {
        // current is a hashset of computers that are currently all connected
        // now I need to find the next node that is also connected to all others
        var neighbours = lookup[network.LastAddedNode];
        foreach (var neighbour in neighbours)
        {
            // for each computer at the LAN party,
            // that computer will have a connection to every other computer at the LAN party
            if (network.Nodes.All(n => lookup[n].Contains(neighbour))) // still valid
            {
                network.Nodes.Add(neighbour);
                ValidNetworks.Add(network.DeepCopy());
                
                // search for further connections
                Part2Logic(network, lookup);
            }
        }
    }

    private class Network : IEquatable<Network>
    {
        public HashSet<string> Nodes { get; init; } = []; // not sure that my naming is ok for purists here :)
        public string LastAddedNode { get; private set; } = "";

        public override string ToString() => string.Join(",", Nodes.Order());

        public Network DeepCopy()
        {
            return new Network
            {
                Nodes = Nodes.ToHashSet(),
                LastAddedNode = LastAddedNode
            };
        }

        public void Add(string node)
        {
            Nodes.Add(node);
            LastAddedNode = node;
        }

        public bool Equals(Network? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Nodes.SetEquals(other.Nodes);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Network)obj);
        }

        public override int GetHashCode()
        {
            return Nodes.GetHashCode();
        }
    }

    private static void AddConnection(string first, string second, Dictionary<string, HashSet<string>> connections)
    {
        var hashset = connections.TryGetValue(first, out var current) ? current : [];
        hashset.Add(second);
        connections[first] = hashset;
    }

    private class TriLink : IEquatable<TriLink>
    {
        public string First { get; init; }
        public string Second { get; init; }
        public string Third { get; init; }

        private string Alphabetical => string.Join("", new List<string> { First, Second, Third }.Order());

        public bool HasComputerThatStartsWithT() => First[0] == 't' || Second[0] == 't' || Third[0] == 't';

        public override string ToString() => $"{First},{Second},{Third}";

        public bool Equals(TriLink? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Alphabetical == other.Alphabetical;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TriLink)obj);
        }

        public override int GetHashCode()
        {
            return Alphabetical.GetHashCode();
        }
    }
}