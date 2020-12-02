#include "Day1.h"

void Day1::ParseInput()
{
	if (_inputParsed)
		return;

	//ifstream input("input/day1example.txt");
	ifstream input("input/day1.txt");

	if (!input)
	{
		cout << "Failed to open input." << endl;
		return;
	}

	string line = "";
	while (getline(input, line))
		_expenses.push_back(stoi(line));

	_inputParsed = true;
}

int Day1::Part1()
{
	ParseInput();
	for (size_t i = 0; i < _expenses.size(); ++i)
	{
		for (size_t j = 0; j < _expenses.size(); ++j)
		{
			if (_expenses[i] + _expenses[j] == 2020)
				return _expenses[i] * _expenses[j];
		}
	}

	cout << "Error: no solution found for Day1::Part1.";
	return -1;
}

int Day1::Part2()
{
	ParseInput();
	for (size_t i = 0; i < _expenses.size(); ++i)
	{
		for (size_t j = 0; j < _expenses.size(); ++j)
		{
			for (size_t k = 0; k < _expenses.size(); ++k)
			{
				if (_expenses[i] + _expenses[j] + _expenses[k] == 2020)
					return _expenses[i] * _expenses[j] * _expenses[k];
			}
		}
	}

	cout << "Error: no solution found for Day1::Part1.";
	return -1;
}
