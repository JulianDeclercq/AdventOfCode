namespace AdventOfCode2023.days;

public class Day5
{
    private class RangeMap
    {
        public RangeMap(long destinationStart, long sourceStart, long length)
        {
            _destinationStart = destinationStart;
            _sourceStart = sourceStart;
            _length = length;    
        }
        
        private readonly long _destinationStart;
        private readonly long _sourceStart;
        private readonly long _length;

        public (bool inRange, long result) Map(long sourceNr)
        {
            if (sourceNr < _sourceStart || sourceNr > _sourceStart + _length)
                return new(false, sourceNr);

            return new(true, _destinationStart + (sourceNr - _sourceStart));
        }

    }
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
        
        var seedToSoil = lines.Skip(offset).TakeWhile(l => !string.IsNullOrWhiteSpace(l)).ToArray();
        var seedToSoilMaps = seedToSoil
            .Select(l => l.Split(' '))
            .Select(x => new RangeMap(long.Parse(x[0]), long.Parse(x[1]), long.Parse(x[2])))
            .ToArray();
        offset += seedToSoil.Length + 2; // skip the empty line and the "X-to-X map:" line

        var soilToFertilizer = lines.Skip(offset).TakeWhile(l => !string.IsNullOrWhiteSpace(l)).ToArray();
        var soilToFertilizerMaps = soilToFertilizer
            .Select(l => l.Split(' '))
            .Select(x => new RangeMap(long.Parse(x[0]), long.Parse(x[1]), long.Parse(x[2])))
            .ToArray();
        offset += soilToFertilizer.Length + 2;
        
        var fertilizerToWater = lines.Skip(offset).TakeWhile(l => !string.IsNullOrWhiteSpace(l)).ToArray();
        var fertilizerToWaterMaps = fertilizerToWater
            .Select(l => l.Split(' '))
            .Select(x => new RangeMap(long.Parse(x[0]), long.Parse(x[1]), long.Parse(x[2])))
            .ToArray();
        offset += fertilizerToWater.Length + 2;
        
        var waterToLight = lines.Skip(offset).TakeWhile(l => !string.IsNullOrWhiteSpace(l)).ToArray();
        var waterToLightMaps = waterToLight
            .Select(l => l.Split(' '))
            .Select(x => new RangeMap(long.Parse(x[0]), long.Parse(x[1]), long.Parse(x[2])))
            .ToArray();
        offset += waterToLight.Length + 2;
        
        var lightToTemperature = lines.Skip(offset).TakeWhile(l => !string.IsNullOrWhiteSpace(l)).ToArray();
        var lightToTemperatureMaps = lightToTemperature
            .Select(l => l.Split(' '))
            .Select(x => new RangeMap(long.Parse(x[0]), long.Parse(x[1]), long.Parse(x[2])))
            .ToArray();
        offset += lightToTemperature.Length + 2;
        
        var temperatureToHumidity = lines.Skip(offset).TakeWhile(l => !string.IsNullOrWhiteSpace(l)).ToArray();
        var temperatureToHumidityMaps = temperatureToHumidity
            .Select(l => l.Split(' '))
            .Select(x => new RangeMap(long.Parse(x[0]), long.Parse(x[1]), long.Parse(x[2])))
            .ToArray();
        offset += temperatureToHumidity.Length + 2;
        
        var humidityToLocation = lines.Skip(offset).TakeWhile(l => !string.IsNullOrWhiteSpace(l)).ToArray();
        var humidityToLocationMaps = humidityToLocation
            .Select(l => l.Split(' '))
            .Select(x => new RangeMap(long.Parse(x[0]), long.Parse(x[1]), long.Parse(x[2])))
            .ToArray();

        // AssertEqual(81, GetValueFromMaps(79, seedToSoilMaps));
        // AssertEqual(14, GetValueFromMaps(14, seedToSoilMaps));
        // AssertEqual(57, GetValueFromMaps(55, seedToSoilMaps));
        // AssertEqual(13, GetValueFromMaps(13, seedToSoilMaps));
        //
        // AssertEqual(82, SeedToLocation(79, seedToSoilMaps, soilToFertilizerMaps, fertilizerToWaterMaps, waterToLightMaps, lightToTemperatureMaps, temperatureToHumidityMaps, humidityToLocationMaps));
        // AssertEqual(43, SeedToLocation(14, seedToSoilMaps, soilToFertilizerMaps, fertilizerToWaterMaps, waterToLightMaps, lightToTemperatureMaps, temperatureToHumidityMaps, humidityToLocationMaps));
        // AssertEqual(86, SeedToLocation(55, seedToSoilMaps, soilToFertilizerMaps, fertilizerToWaterMaps, waterToLightMaps, lightToTemperatureMaps, temperatureToHumidityMaps, humidityToLocationMaps));
        // AssertEqual(35, SeedToLocation(13, seedToSoilMaps, soilToFertilizerMaps, fertilizerToWaterMaps, waterToLightMaps, lightToTemperatureMaps, temperatureToHumidityMaps, humidityToLocationMaps));

        var answer = long.MaxValue;
        foreach (var seed in seeds)
        {
            if (_iterations % 100 == 0)
                Console.WriteLine($"{_iterations}/{seeds.Count} {(float)_iterations/seeds.Count}%");
            
            var location = SeedToLocation(seed, seedToSoilMaps, soilToFertilizerMaps, fertilizerToWaterMaps,
                waterToLightMaps, lightToTemperatureMaps, temperatureToHumidityMaps, humidityToLocationMaps);

            if (location < answer)
                answer = location;
        }
        Console.WriteLine(answer);
    }

    private int _iterations = 0;
    private long SeedToLocation(long seed, IEnumerable<RangeMap> sts, IEnumerable<RangeMap> stf, IEnumerable<RangeMap> ftw,
        IEnumerable<RangeMap> wtl, IEnumerable<RangeMap> ltt, IEnumerable<RangeMap> tth, IEnumerable<RangeMap> htl)
    {
        var soil = GetValueFromMaps(seed, sts);
        var fertilizer = GetValueFromMaps(soil, stf);
        var water = GetValueFromMaps(fertilizer, ftw);
        var light = GetValueFromMaps(water, wtl);
        var temperature = GetValueFromMaps(light, ltt);
        var humidity = GetValueFromMaps(temperature, tth);
        var location = GetValueFromMaps(humidity, htl);

        _iterations++;
        return location;
    }

    private static void AssertEqual(long expected, long value)
    {
        if (value != expected)
            throw new Exception($"got: {value}, expected: {expected}");
    }

    private static long GetValueFromMaps(long value, IEnumerable<RangeMap> maps)
    {
        var inRange = maps.Select(x => x.Map(value)).FirstOrDefault(x => x.inRange);
        return inRange == default ? value : inRange.result;
    }
    
    public void Part2(){}
}