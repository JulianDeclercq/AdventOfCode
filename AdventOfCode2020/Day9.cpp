#include "Day9.h"

void Day9::ParseInput()
{
	//ifstream input("input/day9example.txt");
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
				// skip self
				if (j == k)
					continue;

				if (_numbers[k] == target)
				{
					//cout << current << ": was valid with " << _numbers[j] << " and " << _numbers[k] << endl;
					found = true;
				}
			}
		}

		if (!found)
			return _numbers[i];

		++startIdx;
		++endIdx;
	}
	return -1;
}

int Day9::PartTwo()
{
	return 0;
}
