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

int Day10::PartTwo()
{
	return 0;
}
