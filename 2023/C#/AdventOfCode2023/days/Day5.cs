namespace AdventOfCode2023.days;

public class Day5
{
    private class SubMap
    {
        public SubMap(long destinationStart, long sourceStart, long length)
        {
            _destinationStart = destinationStart;
            _sourceStart = sourceStart;
            _length = length;    
        }
        
        private readonly long _destinationStart;
        private readonly long _sourceStart;
        private readonly long _length;

        public (bool inRange, long result) MapToDestination(long sourceNr)
        {
            if (sourceNr < _sourceStart || sourceNr >= _sourceStart + _length)
                return new(false, sourceNr);

            return new(true, _destinationStart + (sourceNr - _sourceStart));
        }

        public bool InBounds(long sourceNr)
        {
            if (sourceNr < _sourceStart || sourceNr >= _sourceStart + _length)
                return false;

            return true;
        }

        public long MinimumSource => _sourceStart;
        public long MaximumSource => _sourceStart + _length - 1;
    }

    private class Map
    {
        public Map(List<SubMap> orderedSubmaps)
        {
            OrderedSubMaps = orderedSubmaps;
        }
        
        private long MinElement => OrderedSubMaps.First().MinimumSource;
        private long MaxElement => OrderedSubMaps.Last().MaximumSource;

        public bool InRange(long nr) => MinElement <= nr && nr <= MaxElement;
        public List<SubMap> OrderedSubMaps;

        public SubMap? AdaptedBinarySearchCorrectMap(long value)
        {
            var left = 0;
            var right = OrderedSubMaps.Count - 1;
            while (left <= right)
            {
                var m = (left + right) / 2;

                var subMap = OrderedSubMaps[m];
                if (subMap.MinimumSource < value)
                {
                    if (subMap.MaximumSource >= value)
                        return subMap;

                    left = m + 1;
                } else if (subMap.MinimumSource > value)
                {
                    right = m - 1;
                }
                else return subMap; // only checks for match with minimumscore
            }
            return null;
        }
    }

    private static Map ParseMap(IEnumerable<string> lines, ref int offset)
    {
        var subMaps = lines
            .Skip(offset) // skip previous maps
            .TakeWhile(l => !string.IsNullOrWhiteSpace(l)) // go until next map
            .Select(l => l.Split(' '))
            .Select(x => new SubMap(long.Parse(x[0]), long.Parse(x[1]), long.Parse(x[2])))
            .OrderBy(x => x.MinimumSource) // this sort is safe because no ranges overlap in the input.
            .ToList();

        offset += subMaps.Count + 2; // +2 to skip the empty line and the "X-to-X map:" line
        return new Map(subMaps);
    }

    private Map? _seedToSoil, _soilToFertilizer, _fertilizerToWater, _waterToLight, _lightToTemperature,
        _temperatureToHumidity, _humidityToLocation;
    public void Solve()
    {
        var offset = 0;
        //var lines = File.ReadAllLines("../../../input/Day5_example.txt");
        var lines = File.ReadAllLines("../../../input/Day5.txt");
        var seedRanges = lines.First()["seeds: ".Length..].Split(' ').Select(long.Parse).ToArray();
        offset += 3;
        
        var seeds = new List<long>();
        
        for (long i = 0; i < seedRanges[1]; ++i)
            seeds.Add(seedRanges[0] + i);
        
        for (long i = 0; i < seedRanges[3]; ++i)
            seeds.Add(seedRanges[2] + i);

        _seedToSoil = ParseMap(lines, ref offset);
        _soilToFertilizer = ParseMap(lines, ref offset);
        _fertilizerToWater = ParseMap(lines, ref offset);
        _waterToLight = ParseMap(lines, ref offset);
        _lightToTemperature = ParseMap(lines, ref offset);
        _temperatureToHumidity = ParseMap(lines, ref offset);
        _humidityToLocation = ParseMap(lines, ref offset);

        var answer = long.MaxValue;
        foreach (var seed in seeds)
        {
            if (_iterations % 1_000_000 == 0)
                Console.WriteLine($"{_iterations}/{seeds.Count} {((float)_iterations/seeds.Count)*100}%");

            var location = SeedToLocation(seed);

            if (location < answer)
                answer = location;
        }
        Console.WriteLine(answer);
    }

    private int _iterations = 0;
    
    private long SeedToLocation(long seed)
    {
        var soil = GetValue(_seedToSoil!, seed);
        var fertilizer = GetValue(_soilToFertilizer!, soil);
        var water = GetValue(_fertilizerToWater!, fertilizer);
        var light = GetValue(_waterToLight!, water);
        var temperature = GetValue(_lightToTemperature!, light);
        var humidity = GetValue(_temperatureToHumidity!, temperature);
        var location = GetValue(_humidityToLocation!, humidity);

        _iterations++;
        return location;
    }
    
    private static long GetValue(Map map, long value)
    {
        if (!map.InRange(value))
            return value;

        var submap = map.AdaptedBinarySearchCorrectMap(value);
        if (submap == null)
            return value;
        
        var (inRange, result) = submap.MapToDestination(value);

        if (!inRange) throw new Exception("Incorrect submap");

        return result;
    }
    
    private static void AssertEqual<T>(T expected, T value)
    {
        if (!value.Equals(expected))
            throw new Exception($"got: {value}, expected: {expected}");
    }
}