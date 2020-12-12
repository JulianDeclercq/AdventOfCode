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

char Day11::SeatAt(const Point& p)
{
	//int col = p.X % _width; // wrap
	int col = p.X;
	int row = p.Y * _width;
	return _seats[col + row];
}

int Day11::PointToIdx(const Point& p)
{
	int col = p.X;
	int row = p.Y * _width;
	return col + row;
}

Point Day11::IdxToPoint(int idx)
{
	return Point({idx % _width, idx / _width});
}

string Day11::Neighbours(const Point& p, const string& seats)
{
	string neighbours = "";
	bool hasNeighboursLeft = p.X > 0;
	bool hasNeighboursRight = p.X < _width - 1;
	bool hasNeighboursUp = p.Y > 0;
	bool hasNeighboursDown = p.Y < _height - 1;
	
	if (hasNeighboursLeft)
	{
		neighbours += seats[PointToIdx({ p.X - 1, p.Y })];

		if (hasNeighboursUp)
			neighbours += seats[PointToIdx({ p.X - 1, p.Y - 1 })];

		if (hasNeighboursDown)
			neighbours += seats[PointToIdx({ p.X - 1, p.Y + 1 })];
	}

	if (hasNeighboursRight)
	{
		neighbours += seats[PointToIdx({ p.X + 1, p.Y })];

		if (hasNeighboursUp)
			neighbours += seats[PointToIdx({ p.X + 1, p.Y - 1 })];

		if (hasNeighboursDown)
			neighbours += seats[PointToIdx({ p.X + 1, p.Y + 1 })];
	}

	if (hasNeighboursUp)
		neighbours += seats[PointToIdx({ p.X, p.Y - 1 })];

	if (hasNeighboursDown)
		neighbours += seats[PointToIdx({ p.X, p.Y + 1 })];

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
		
		const Point p = IdxToPoint(i);
		if (p.X == 3 && p.Y == 0)
		{
			int brkpt = 5;
		}

		const auto& neighbours = Neighbours(p, copy);
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
	string previous = string(_seats);
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
