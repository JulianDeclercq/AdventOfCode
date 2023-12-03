#include "Day10.h"


void Day10::ParseInput()
{
	//ifstream input("input/day10example.txt");
	//ifstream input("input/day10example2.txt");
	ifstream input("input/day10.txt");

	if (!input)
	{
		cout << "Failed to open input." << endl;
		return;
	}

	// add charging outlet
	_adaptors.push_back(0);

	string line = "";
	while (getline(input, line))
		_adaptors.push_back(stoi(line));
	
	sort(_adaptors.begin(), _adaptors.end());

	// add device's built-in adaptor
	_adaptors.push_back(_adaptors.back() + 3);
}

int Day10::PartOne()
{
	int counter1 = 0, counter3 = 0;
	for (size_t i = 1; i < _adaptors.size(); ++i)
	{
		int diff = _adaptors[i] - _adaptors[i - 1];
		if (diff == 1)
		{
			++counter1;
		}
		else if (diff == 3)
		{
			++counter3;
		}
	}
	return counter1 * counter3;
}

// works but way too slow
void Day10::CountArrangementsOld(int last, int offset)
{
	for (size_t i = offset; i < _adaptors.size(); ++i)
	{
		if (_adaptors[i] - last > 3)
			return;

		CountArrangementsOld(last, i + 1);

		last = _adaptors[i];

		// add the arrangement if it connects to the device
		if (i == _adaptors.size() - 1)
			++_count;
	}
}

// thanks to https://old.reddit.com/r/adventofcode/comments/ka8z8x/2020_day_10_solutions/gfal951/
// for the inspiration and explanation
long long Day10::CountArrangements(int step)
{
	if (_memo.find(step) != _memo.end())
		return _memo.at(step);

	long long count = 0;
	for (size_t i = 1; i < _adaptors.size(); ++i)
	{
		// avoid out of bounds
		int next = step - i;
		if (next < 0)
			break;

		// check if the adaptors are compatible
		// in case of failing this check: since the list is sorted, 
		// any adaptors "beyond" this point can be excluded as well (so leave the loop)
		if (_adaptors[step] - _adaptors[next] > 3)
			break;

		// if the adaptors are compatible, count the arrangements up until this point
		count += CountArrangements(next);
	}
	_memo[step] = count;
	return count;
}

long long Day10::PartTwo()
{
	//CountArrangementsOld({ _adaptors.front() }, 1); 
	//return _count;

	_memo[0] = 1;
	return CountArrangements(_adaptors.size() - 1);
}
