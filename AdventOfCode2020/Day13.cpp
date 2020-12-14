#include "Day13.h"

void Day13::ParseInput()
{
	//ifstream input("input/day13example6.txt");
	ifstream input("input/day13.txt");
	_startAt = 100000000000000; // cheeky head start

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

	// sort the busses in descending order to maximise the step (and thus minimise iterations)
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
	long long t = _startAt;

	// ensure the start is valid by moving to the closest t that is divisibly by the first bus
	t -= t % _busses[0] + _offsets[_busses[0]];

	long long period = _busses[0];
	bool lock = false;
	while (true)
	{
		for (size_t i = 1; i < _busses.size(); ++i)
		{
			int bus = _busses[i];
			long long timeStamp = t + _offsets[bus];
			if (timeStamp % bus != 0)
				break;

			if (!lock)
			{
				auto& el = _test[_busses[i]]; // original T, COUNT
				el.second++;

				if (el.second > 1)
				{
					int diff = t - el.first;
					if (diff > period)
					{
						period = diff;
						lock = true;
					}
				}
				else
				{
					el.first = t;
				}
			}

			if (i == _busses.size() - 1)
				return t;
		}
		t += period; // next iteration
	}
	return 0;
}


