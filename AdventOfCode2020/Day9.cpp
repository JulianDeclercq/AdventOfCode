#include "Day9.h"

void Day9::ParseInput()
{
	//ifstream input("input/day9example.txt"); _preamble = 5;
	ifstream input("input/day9.txt");

	if (!input)
	{
		cout << "Failed to open input." << endl;
		return;
	}

	string line = "";
	while (getline(input, line))
		_numbers.push_back(stoll(line));
}

long long Day9::PartOne()
{
	int startIdx = 0, endIdx = _preamble - 1;
	for (size_t i = _preamble; i < _numbers.size(); ++i)
	{
		bool found = false;
		for (int j = startIdx; j < endIdx && !found; ++j)
		{
			auto target = _numbers[i] - _numbers[j];
			for (int k = startIdx; k <= endIdx && !found; ++k)
			{
				// self doesn't count
				if (j == k)
					continue;

				if (_numbers[k] == target)
					found = true;
			}
		}

		if (!found)
		{
			_part1Cached = _numbers[i];
			return _part1Cached;
		}

		++startIdx;
		++endIdx;
	}
	return -1;
}

long long Day9::PartTwo()
{
	if (_part1Cached == -1)
		PartOne();

	size_t startIdx = 0;
	long long target = _part1Cached, sum = 0, first = -1, largest = 0;
	for (size_t i = startIdx; i < _numbers.size(); ++i)
	{
		if (first == -1)
			first = _numbers[i];

		if (_numbers[i] > largest)
			largest = _numbers[i];

		sum += _numbers[i];

		if (sum == target)
		{
			return first + largest;
		}
		else if (sum > target)
		{
			// reset and start looking again
			++startIdx;
			i = startIdx;
			first = -1;
			sum = 0;
		}
	}
	return -1;
}
