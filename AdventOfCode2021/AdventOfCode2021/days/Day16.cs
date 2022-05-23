using System.Collections;
using System.Text;

namespace AdventOfCode2021.days;

public class Day16
{
    private static Dictionary<char, string> _lookup = new()
    {
        {'0', "0000"}, {'1', "0001"}, {'2', "0010"}, {'3', "0011"},
        {'4', "0100"}, {'5', "0101"}, {'6', "0110"}, {'7', "0111"},
        {'8', "1000"}, {'9', "1001"}, {'A', "1010"}, {'B', "1011"},
        {'C', "1100"}, {'D', "1101"}, {'E', "1110"}, {'F', "1111"}
    };

    private static int _part1 = 0;

    public void Part1()
    {
        Helpers.Verbose = false;
        //const string hexInput = "A0016C880162017C3686B18A3D4780";
        var hexInput = File.ReadAllText(@"..\..\..\input\day16.txt");
        ParsePacket(ToBinary(hexInput));
        
        Helpers.WriteLine($"Day 16 part 1: {_part1}");
    }
    
    public void Part2()
    {
        Helpers.Verbose = true;
        const string hexInput = "04005AC33890";
        //var hexInput = File.ReadAllText(@"..\..\..\input\day16.txt");
        var info = ParsePacket(ToBinary(hexInput));
        
        Helpers.WriteLine($"Day 16 part 2: {info.Value}");
    }

    private static string ToBinary(string s)
    {
        var sb = new StringBuilder();
        
        foreach(var c in s)
            sb.Append(_lookup[c]);

        return sb.ToString();
    }

    private class ParseInfo
    {
        public string Remainder = "";
        public long Value = long.MinValue;
        public int TotalParsed = 0;

        public override string ToString() => $"Remainder: {Remainder}\nLiteral {Value}\nTotalParsed: {TotalParsed}";
    }

    private static ParseInfo ParsePacket(string packet)
    {
        var offset = 0;
        var packetVersion = Convert.ToInt32(packet[offset..3], 2);

        _part1 += packetVersion;
        
        offset += 3;
        var typeId = Convert.ToInt32(packet[offset..6], 2);
        offset += 3;
        Helpers.WriteLine($"packet version {packetVersion}, type id {typeId}", true);

        // parse literal
        if (typeId == 4)
        {
            var info = ParseLiteral(packet[offset..]);
            info.TotalParsed += offset;
            Helpers.WriteLine($"Parsed literal {info}", true);
            return info;
        }

        var subPacketValues = new List<long>();
        var lengthTypeId = packet[offset++];
        switch (lengthTypeId)
        {
            // total length in bits of the sub-packets contained by this packet.
            case '0':
            {
                var totalSubpacketLength = Convert.ToInt32(packet[offset..(offset + 15)], 2);
                offset += 15;
            
                // parse the next packet for now
                var targetOffset = offset + totalSubpacketLength;
                while (offset < targetOffset)
                {
                    var info = ParsePacket(packet[offset..]);
                    offset += info.TotalParsed;
                    subPacketValues.Add(info.Value);
                }

                break;
            }
            
            // number of sub-packets immediately contained by this packet.
            case '1':
            {
                var nrOfSubpackets = Convert.ToInt32(packet[offset..(offset + 11)], 2);
                offset += 11;
                for (var i = 0; i < nrOfSubpackets; ++i)
                {
                    var info = ParsePacket(packet[offset..]);
                    offset += info.TotalParsed;
                    subPacketValues.Add(info.Value);
                }
                break;
            }
            default:
                throw new Exception($"Invalid lengthTypeId: {lengthTypeId}");
        }

        var value = long.MaxValue;
        
        // TODO: check for any?
        switch (typeId)
        {
            case 0: // sum packet
                value = subPacketValues.Sum(); 
                break;
            case 1:
                value = subPacketValues.Aggregate((total, next) => total * next);
                break;
            case 2: break;
            case 3: break;
            case 5: break;
            case 6: break;
            case 7: break;
            default: throw new Exception($"Invalid type id {typeId}");
        }
        
        return new ParseInfo
        {
            Remainder = "endofmethod",
            Value = value,
            TotalParsed = offset // not sure if this is correct
        };
    }

    private static ParseInfo ParseLiteral(string packet)
    {
        const int groupLength = 5; // literal groups are 4 bits + 1 bit prefix

        var offset = 0;
        var sb = new StringBuilder();
        for (var i = 0;; ++i)
        {
            var group = packet.Skip(i * groupLength).Take(groupLength).ToArray();
            sb.Append(group.Skip(1).Stringify()); // skip the prefix
            offset += groupLength;

            // check if this was the last group
            if (group.First() == '0')
                break;
        }

        return new ParseInfo
        {
            Remainder = packet[offset..],
            Value = Convert.ToInt64(sb.ToString(), 2),
            TotalParsed = offset
        };
    }
}