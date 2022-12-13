namespace AdventOfCode2022.Days;

public class Day13
{
    private class Element
    {
        public int Value = 0;
        public List<Element> Contents = new();
        public ListElementType Type = ListElementType.None;

        public override string ToString()
        {
            return Type switch
            {
                ListElementType.None => "INVALID",
                ListElementType.Integer => Value.ToString(),
                ListElementType.List => $"[{string.Join(',', Contents)}]",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    private enum ListElementType
    {
        None = 0,
        Integer = 1,
        List = 2
    }
    
    public static void Solve()
    {
        var pairs = File.ReadAllLines(@"..\..\..\input\day13.txt")
            .Where(l => !string.IsNullOrEmpty(l))
            .Select(ParseElement)
            .Chunk(2)
            .ToArray();

        var orderedPairsIndicesSum = pairs
            .Select((p, i) => IsOrdered(p.First(), p.Last())!.Value ? i + 1 : 0)
            .Sum();
        
        Console.WriteLine($"Day 13 part 1: {orderedPairsIndicesSum}");
    }
    
    public static void Solve2()
    {
        var comparer = new ElementComparer();
        const string divider1 = "[[2]]", divider2 = "[[6]]";
        
        var packets = File.ReadAllLines(@"..\..\..\input\day13.txt")
            .Where(l => !string.IsNullOrEmpty(l))
            .Concat(new[] {divider1, divider2})
            .Select(ParseElement)
            .OrderBy(x => x, comparer)
            .Select(x => x.ToString())
            .ToList();
        
        var decoderKey = (packets.IndexOf(divider1) + 1) * (packets.IndexOf(divider2) + 1);
        Console.WriteLine($"Day 13 part 2: {decoderKey}");
    }
    
    private class ElementComparer : IComparer<Element>
    {
        public int Compare(Element left, Element right)
        {
            if (left.Type is ListElementType.Integer && right.Type is ListElementType.Integer)
            {
                if (left.Value < right.Value)
                    return -1;
            
                if (left.Value > right.Value)
                    return 1;
        
                return 0;
            }
        
            if (left.Type is ListElementType.List && right.Type is ListElementType.List)
            {
                var loopLength = Math.Max(left.Contents.Count, right.Contents.Count);
                for (var i = 0; i < loopLength; ++i)
                {
                    // if undecided and left runs out of items first, the inputs are in the right order
                    if (i >= left.Contents.Count)
                        return -1;
                
                    // if undecided and right runs out of items first, the inputs are not in the right order
                    if (i >= right.Contents.Count)
                        return 1;
                
                    var ordered = Compare(left.Contents[i], right.Contents[i]);
                    if (ordered != 0)
                        return ordered;
                }
            
                // if the lists are the same length and no comparison makes a decision about the order,
                // continue checking the next part of the input.
                return 0;
            }
        
            // if exactly one value is an integer, convert the integer to a list which contains that integer as
            // its only value, then retry the comparison.
        
            if (left.Type is ListElementType.Integer)
                return Compare(ConvertIntegerToList(left), right);
        
            if (right.Type is ListElementType.Integer)
                return Compare(left, ConvertIntegerToList(right));
        
            return 1;
        }
    }

    private static bool? IsOrdered(Element left, Element right)
    {
        return new ElementComparer().Compare(left, right) switch
        {
            -1 => true,
            1 => false,
            _ => null
        };
    }
    
    private static Element ConvertIntegerToList(Element element)
    {
        if (element.Type is not ListElementType.Integer)
            throw new Exception("Can't convert non-integer element.");
        
        return new Element
        {
            Type = ListElementType.List,
            Contents = new List<Element> { element }
        };
    }

    private static Element ParseElement(string element)
    {
        var stack = new Stack<int>();
        var elements = new List<Element>();
        var depth = 0;
        var currentInt = "";
        for (var i = 0; i < element.Length; ++i)
        {
            var c = element[i];
            switch (c)
            {
                case '[':
                    stack.Push(i);
                    depth++;
                    break;
                case ']': 
                    var itemStart = stack.Pop();
                    depth--;
                    if (depth == 1)
                        elements.Add(ParseElement(element[itemStart..(i+1)]));
                    break;
                case ',':
                    if (string.IsNullOrEmpty(currentInt))
                        break;

                    if (depth == 1)
                    {
                        elements.Add(new Element
                        {
                            Type = ListElementType.Integer,
                            Value = int.Parse(currentInt) 
                        });
                    }
                    currentInt = "";
                    break;
                default:
                    if (depth == 1 && char.IsDigit(c))
                        currentInt += c;
                    break;
            }
        }

        // Add the last element, since there's no trailing comma for the last one
        if (currentInt.Any())
        {
            elements.Add(new Element
            {
                Type = ListElementType.Integer,
                Value = int.Parse(currentInt) 
            });
        }

        return new Element
        {
            Type = ListElementType.List,
            Contents = elements
        };
    }
}