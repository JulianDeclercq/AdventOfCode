#include "Day13.h"

void Day13::ParseInput()
{
	//ifstream input("input/day13example.txt");
	ifstream input("input/day13.txt");

	if (!input)
	{
		cout << "Failed to open input." << endl;
		return;
	}

	string line = "";
	getline(input, line);
	_target = stoi(line);

	getline(input, line);
	
	stringstream ss(line);
	while (ss.good()) 
	{
		string substr;
		getline(ss, substr, ',');
		if(substr[0] != 'x')
			_busses.push_back(stoi(substr));
	}
}

int Day13::PartOne()
{
	for (int bus : _busses)
	{
		int earliestAvailable = (_target / bus + 1) * bus;
		if (earliestAvailable < _earliest)
		{
			_earliest = earliestAvailable;
			_busId = bus;
		}
	}
	return _busId * (_earliest - _target);
}

int Day13::PartTwo()
{
	return 0;
}
