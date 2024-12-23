namespace AdventOfCode2024.Day23;
public class Network : IEquatable<Network>
{
    public HashSet<string> Nodes { get; init; } = []; // not sure that my naming is ok for purists here :)
    public string LastAddedNode { get; private set; } = "";

    public string GetPassword() => string.Join(",", Nodes.Order());

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