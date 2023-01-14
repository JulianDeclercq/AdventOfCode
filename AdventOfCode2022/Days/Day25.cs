namespace AdventOfCode2022.Days;

public class Day25
{
    private record Possibility(char Operand, long Value, long? DiffWithTarget);

    private readonly Dictionary<char, long> _lookup = new()
    {
        ['2'] = 2,
        ['1'] = 1,
        ['0'] = 0,
        ['-'] = -1,
        ['='] = -2,
    };

    private List<long> _powersOf5 = new();

    public void Solve()
    {
        var answer = File.ReadAllLines(@"..\..\..\input\day25.txt").Select(ToDecimal).Sum();
        
        checked
        {
            _powersOf5 = Enumerable.Range(0, 21).Select(x => (long) Math.Pow(5, x)).Reverse().ToList();
        }
        
        new List<long>
        {
            1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 15, 20, 2022, 12345, 314159265
        }.ForEach(n => Console.WriteLine($"Decimal {n} in snafu is {ToSnafu(n)}"));

        Console.WriteLine($"Day 25 part 1: {ToSnafu(answer)}");
        
        /*
            I couldn't figure out the code for decimal to snafu  at first so I did it manually..
            Found the answer, then came back and implemented ToSnafu!
            
            ////// EXAMPLE //////
            Powers of 5:
            15625, 3125, 625, 125, 25, 5, 1

            Target: 4890

            2 		* 3125 	=> 6250		=> 6250
            -2(=) 	* 625 	=> -1250 	=> 5000
            -1(-) 	* 125 	=> -125 	=> 4875
            1 		* 25 	=> 25 		=> 4900
            -2(=) 	* 5 	=> 10	 	=> 4890
            0 		* 1 	=> 0 		=> 4890

            Answer: take all first column from above and put them in sequence
            2=-1=0 

            ////// Part 1 //////
            Powers of 5:
            19073486328125, 3814697265625, 762939453125, 152587890625, 30517578125, 6103515625, 1220703125, 244140625,
            48828125, 9765625, 1953125, 390625, 78125, 15625, 3125, 625, 125, 25, 5, 1

            Target: 34,561,628,468,940

            2		* 19,073,486,328,125    => 38,146,972,656,250    => 38,146,972,656,250
            -1(-)	* 3,814,697,265,625     => -3,814,697,265,625    => 34,332,275,390,625
            0		* 762,939,453,125       => 0                     => 34,332,275,390,625
            2		* 152,587,890,625       => 305,175,781,250       => 34,637,451,171,875
            -2(=)	* 30,517,578,125        => -61,035,156,250       => 34,576,416,015,625
            -2(=)	* 6,103,515,625         => -12,207,031,250       => 34,564,208,984,375
            -2(=)	* 1,220,703,125         => -2,441,406,250        => 34,561,767,578,125
            -1(-)	* 244,140,625           => -244,140,625          => 34,561,523,437,500
            2		* 48,828,125            => 97,656,250            => 34,561,621,093,750
            1		* 9,765,625             => 9,765,625             => 34,561,630,859,375
            -1(-)	* 1,953,125             => -1,953,125            => 34,561,628,906,250
            -1(-)	* 390,625               => -390,625              => 34,561,628,515,625
            -1(-)	* 78,125                => -78,125               => 34,561,628,437,500
            2		* 15,625                => 31,250                => 34,561,628,468,750
            0		* 3,125                 => 0                     => 34,561,628,468,750
            0		* 625                   => 0                     => 34,561,628,468,750
            2		* 125                   => 250                   => 34,561,628,469,000
            -2(=)	* 25                    => -50                   => 34,561,628,468,950
            -2(=)	* 5                     => -10                   => 34,561,628,468,940
            0		* 1                     => 0                     => 34,561,628,468,940

            Answer: take all first column from above and put them in sequence
            2-02===-21---2002==0
         */
    }

    private long ToDecimal(string snafu)
    {
        checked
        {
            return snafu.Reverse().Select((t, i) => (long) Math.Pow(5, i) * _lookup[t]).Sum();
        }
    }

    private long _lastPower = long.MinValue;
    private string ToSnafu(long number)
    {
        var firstHigherPower = _powersOf5.LastOrDefault(p => p > number);
        if (firstHigherPower == default)
            throw new Exception("Not enough power of 5's generated.");
                
        var closestHigher = ClosestPossibility(firstHigherPower, number);

        var firstLowerPower = _powersOf5.FirstOrDefault(p => p < number);
        if (firstLowerPower == default)
            return "1";
        
        var closestLower = ClosestPossibility(firstLowerPower, number);
        
        var correct = new[] { closestLower, closestHigher }.MinBy(p => p.DiffWithTarget)!;
        
        // e.g. input 2022:
        // If closestLower is closer than closestHigher but the difference is bigger than 2 (max operand) times
        // the next lower power you still have to go with closestHigher since you can't make it otherwise
        if (GetNextPower(firstLowerPower) * 2 < closestLower.DiffWithTarget)
            correct = closestHigher;

        _lastPower = correct.Value;

        var answer = correct.Operand.ToString(); 
        var curr = NextCorrectPossibility(number, _lookup[correct.Operand] * correct.Value)!;
        for(;;)
        {
            if (curr == null)
                break;
            
            answer += curr.Operand;
            curr = NextCorrectPossibility(number, curr.Value);
        }

        return answer.TrimStart('0');
    }

    private long GetNextPower(long currentPower) => _powersOf5.FirstOrDefault(p => p < currentPower);
    
    private Possibility? NextCorrectPossibility(long target, long currentValue)
    {
        var nextPower = GetNextPower(_lastPower);

        if (nextPower == default)
            return null;
        
        // take all possibilities and add their value to the current number
        // then check their distance, closest candidate wins
        var possibilities = Possibilities(nextPower).Select(p => p with
        {
            Value = currentValue + p.Value,
            DiffWithTarget = Math.Abs(target - (currentValue + p.Value))
        });

        _lastPower = nextPower;
        return possibilities.MinBy(p => p.DiffWithTarget);
    }
    
    private Possibility ClosestPossibility(long power, long closestTo)
    {
        var possibilities = _lookup.Select(x => new Possibility(x.Key, power, Math.Abs(closestTo - (x.Value * power))));
        return possibilities.MinBy(p => p.DiffWithTarget)!;
    }

    private IEnumerable<Possibility> Possibilities(long number)
    {
        return _lookup.Select(x => new Possibility(x.Key, x.Value * number, null));
    }
}