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
        //const string hexInput = File.ReadAllText(@"..\..\..\input\day16.txt");
        //const string hexInput = "D2FE28";
        //const string hexInput = "38006F45291200";
        //const string hexInput = "EE00D40C823060";
        const string hexInput = "A0016C880162017C3686B18A3D4780";
        ParsePacket(ToBinary(hexInput));
        
        Console.WriteLine($"Day 16 part 1: {_part1}");
    }

    private static string ToBinary(string s)
    {
        var sb = new StringBuilder();
        
        foreach(var c in s)
            sb.Append(_lookup[c]);

        return sb.ToString();
    }

    private static int ParsePacket(string packet)
    {
        var offset = 0;
        var packetVersion = Convert.ToInt32(packet[offset..3], 2);

        _part1 += packetVersion;
        
        offset += 3;
        var typeId = Convert.ToInt32(packet[offset..6], 2);
        offset += 3;
        Console.WriteLine($"packet version {packetVersion}, type id {typeId}");

        if (typeId == 4)
        {
            var l = ParseLiteral(packet[offset..]);
            Console.WriteLine($"Parsed literal {l}");
            return l;
        }

        // total length in bits of the sub-packets contained by this packet.
        const int subpacketLength = 11; // this is wrong
        var subpackets = new List<string>();
        if (packet[offset] == '0')
        {
            offset++;
            
            var totalSubpacketLength = Convert.ToInt32(packet[offset..(offset + 15)], 2);
            offset += 15;
            Console.WriteLine(totalSubpacketLength);

            var cycles = totalSubpacketLength / subpacketLength;
            for (var i = 0; i < cycles; ++i)
            {
                // parse the last packet
                if (i == cycles - 1)
                {
                    subpackets.Add(packet[offset..(offset + subpacketLength + totalSubpacketLength % subpacketLength)]);
                    break;
                }

                subpackets.Add(packet[offset..(offset + subpacketLength)]);
                offset += subpacketLength;
            }
        }
        else // number of sub-packets immediately contained by this packet.
        {
            offset++;

            var nrOfSubpackets = Convert.ToInt32(packet[offset..(offset + 11)], 2);
            offset += 11;
            for (var i = 0; i < nrOfSubpackets; ++i)
            {
                subpackets.Add(packet[offset..(offset + subpacketLength)]);
                offset += subpacketLength;
            }
        }
        
        // TODO: Don't just print the parsed packet, do something with it :P
        foreach (var sp in subpackets) 
            Console.WriteLine(ParsePacket(sp));

        return int.MinValue;
    }

    private static int ParseLiteral(string packet)
    {
        const int groupLength = 5; // literal groups are 4 bits + 1 bit prefix
        var cycles = packet.Length / groupLength;

        var sb = new StringBuilder();
        for (var i = 0; i < cycles; ++i)
            sb.Append(packet.Skip(i * groupLength + 1).Take(groupLength - 1).Stringify()); // +-1 for prefix

        return Convert.ToInt32(sb.ToString(), 2);
    }
}