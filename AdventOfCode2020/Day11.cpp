#include "Day11.h"

void Day11::ParseInput()
{
	//ifstream input("input/day11example.txt");
	ifstream input("input/day11.txt");

	if (!input)
	{
		cout << "Failed to open input." << endl;
		return;
	}

	string line = "";
	while (getline(input, line))
	{
		if (_width == 0)
			_width = line.size();

		_seats.append(line);
		++_height;
	}
}

string Day11::Neighbours(int idx, const string& seats)
{
	string neighbours = "";
	bool hasNeighboursLeft = idx % _width != 0;
	bool hasNeighboursRight = idx % _width != _width - 1;
	bool hasNeighboursUp = idx > _width;
	bool hasNeighboursDown = idx + _width < seats.size();
	
	if (hasNeighboursLeft)
	{
		neighbours += seats[idx - 1];

		if (hasNeighboursUp)
			neighbours += seats[idx - _width - 1];

		if (hasNeighboursDown)
			neighbours += seats[idx + _width - 1];
	}

	if (hasNeighboursRight)
	{
		neighbours += seats[idx + 1];

		if (hasNeighboursUp)
			neighbours += seats[idx - _width + 1];

		if (hasNeighboursDown)
			neighbours += seats[idx + _width + 1];
	}

	if (hasNeighboursUp)
		neighbours += seats[idx - _width];

	if (hasNeighboursDown)
		neighbours += seats[idx + _width];

	return neighbours;
}

void Day11::Transform(string& seats)
{
	const string copy = string(seats);
	for (size_t i = 0; i < copy.size(); ++i)
	{
		// skip floor
		if (copy[i] == '.')
			continue;
		
		const auto& neighbours = Neighbours(i, copy);
		if (copy[i] == 'L')
		{
			if (find(neighbours.begin(), neighbours.end(), '#') == neighbours.end())
				seats[i] = '#';
		}
		else if (copy[i] == '#')
		{
			if (count(neighbours.begin(), neighbours.end(), '#') >= 4)
				seats[i] = 'L';
		}
	}
}

void Day11::DebugPrint()
{
	for (size_t i = 0; i < _seats.size(); ++i)
	{
		if (i % _width == 0 && i != 0)
			cout << endl;

		cout << _seats[i];
	}
	cout << endl << endl;
}


int Day11::PartOne()
{
	const auto& n = Neighbours(11, _seats);

	string previous = string(_seats);
	//DebugPrint();
	do
	{
		previous = string(_seats);
		Transform(_seats);
		//DebugPrint();
	} while (_seats.compare(previous) != 0);
	
	return count(_seats.begin(), _seats.end(), '#');
}

int Day11::PartTwo()
{
	return 0;
}
