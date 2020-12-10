#include "Day10.h"


void Day10::ParseInput()
{
	//ifstream input("input/day10example.txt");
	ifstream input("input/day10example2.txt");
	//ifstream input("input/day10.txt");

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

void Day10::calculateArrangements(const arrangement& c, int offset)
{
	arrangement currentArrangement = c;
	for (size_t i = offset; i < _adaptors.size(); ++i)
	{
		int diff = _adaptors[i] - currentArrangement.back();
		if (diff <= 3)
		{ 
			// recursion
			calculateArrangements(currentArrangement, i + 1);

			// add to current
			currentArrangement.push_back(_adaptors[i]);

			// add the arrangement if it connects to the device
			if (i == _adaptors.size() - 1)
				_arrangements.push_back(currentArrangement);
		}
	}
}

void Day10::calculateArrangements2(int last, int offset)
{
	for (size_t i = offset; i < _adaptors.size(); ++i)
	{
		if (_adaptors[i] - last <= 3)
		{
			// recursion
			calculateArrangements2(last, i + 1);

			// add to current
			last = _adaptors[i];

			// add the arrangement if it connects to the device
			if (i == _adaptors.size() - 1)
				++_count;
		}
	}
}

long long Day10::PartTwo(bool fast)
{
	if (fast)
	{
		_count = 0;
		calculateArrangements2({ _adaptors.front() }, 1);
		return _count;
	}
	else
	{
		calculateArrangements({ _adaptors.front() }, 1);
		return _arrangements.size();
	}
	return -1;
}
