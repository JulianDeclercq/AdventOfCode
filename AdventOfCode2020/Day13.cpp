#include "Day13.h"

void Day13::ParseInput()
{
	ifstream input("input/day13example6.txt");
	//ifstream input("input/day13.txt");

	if (!input)
	{
		cout << "Failed to open input." << endl;
		return;
	}

	string line = "";
	getline(input, line);
	_target = stoi(line);

	getline(input, line);
	
	int ctr = 0;
	stringstream ss(line);
	while (ss.good()) 
	{
		string substr;
		getline(ss, substr, ',');

		if (substr[0] != 'x')
		{
			int bus = stoi(substr);
			_busses.push_back(bus);
			_offsets[bus] = ctr;
		}
		++ctr;
	}
	sort(_busses.begin(), _busses.end(), [](const int a, const int b) {return a > b; });
}

int Day13::PartOne()
{
	for (int bus : _busses)
	{
		if (bus == 0)
			continue;

		int earliestAvailable = (_target / bus + 1) * bus;
		if (earliestAvailable < _earliest)
		{
			_earliest = earliestAvailable;
			_busId = bus;
		}
	}
	return _busId * (_earliest - _target);
}

long long Day13::PartTwo()
{
	// IDEA: it has to be divisable, so modulo 0
	// idea is to check all the ones from the HIGHEST busnumbers (since this one is the LEAST frequent, and thus most performant to check (i suppose)) 
	// then check if the second least frequent is offset correctly and so on until a solution has been found
	//long long current = 100000000000000;
	long long t = 0;
	t -= t % _busses[0] +_offsets[_busses[0]];
	while (true)
	{
		for (size_t i = 1; i < _busses.size(); ++i)
		{
			int bus = _busses[i];
			long long timestampWatch = t + _offsets[bus];
			if (timestampWatch % bus != 0)
				break;

			if (i == _busses.size() - 1)
				return t;
		}
		t += _busses[0]; // next iteration
	}
	return 0;
}
