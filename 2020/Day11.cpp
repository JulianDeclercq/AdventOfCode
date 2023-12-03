#include "Day11.h"

void Day11::ParseInput()
{
	//ifstream input("input/day11example.txt");
	//ifstream input("input/day11example2.txt");
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

int Day11::PointToIdx(const point& p)
{
	return p.X + (p.Y * _width); // col + row
}

point Day11::IdxToPoint(int idx)
{
	return point({idx % _width, idx / _width});
}

string Day11::Neighbours(const point& p, const string& seats)
{
	string neighbours = "";
	bool hasNeighboursLeft = p.X > 0;
	bool hasNeighboursRight = p.X < _width - 1;
	bool hasNeighboursUp = p.Y > 0;
	bool hasNeighboursDown = p.Y < _height - 1;
	
	if (hasNeighboursLeft)
	{
		if (hasNeighboursUp)
			neighbours += seats[PointToIdx({ p.X - 1, p.Y - 1 })];
		
		neighbours += seats[PointToIdx({ p.X - 1, p.Y })];

		if (hasNeighboursDown)
			neighbours += seats[PointToIdx({ p.X - 1, p.Y + 1 })];
	}

	if (hasNeighboursUp)
		neighbours += seats[PointToIdx({ p.X, p.Y - 1 })];

	if (hasNeighboursDown)
		neighbours += seats[PointToIdx({ p.X, p.Y + 1 })];

	if (hasNeighboursRight)
	{
		if (hasNeighboursUp)
			neighbours += seats[PointToIdx({ p.X + 1, p.Y - 1 })];
		
		neighbours += seats[PointToIdx({ p.X + 1, p.Y })];

		if (hasNeighboursDown)
			neighbours += seats[PointToIdx({ p.X + 1, p.Y + 1 })];
	}

	return neighbours;
}

string Day11::NeighbouringSeats(const point& p, const string& seats)
{
	string neighbours = "";

	point n = p;
	// find the first visible chair on the straight left
	while (n.X > 0)
	{
		--n.X;
		if (AddIfChair(n, seats, neighbours))
			break;
	}

	// straight right
	n = p;
	while (n.X < _width - 1) // TODO: DOUBLE CHECK
	{
		++n.X;
		if (AddIfChair(n, seats, neighbours))
			break;
	}

	// straight up
	n = p;
	while (n.Y > 0)
	{
		--n.Y;
		if (AddIfChair(n, seats, neighbours))
			break;
	}

	// straight down
	n = p;
	while (n.Y < _height - 1)
	{
		++n.Y;
		if (AddIfChair(n, seats, neighbours))
			break;
	}

	// diagonals
	// left-up
	n = p;
	while (n.X > 0 && n.Y > 0)
	{
		--n.X;
		--n.Y;
		if (AddIfChair(n, seats, neighbours))
			break;
	}

	// left-down
	n = p;
	while (n.X > 0 && n.Y < _height - 1)
	{
		--n.X;
		++n.Y;
		if (AddIfChair(n, seats, neighbours))
			break;
	}

	// right-up
	n = p;
	while (n.X < _width - 1 && n.Y > 0)
	{
		++n.X;
		--n.Y;
		if (AddIfChair(n, seats, neighbours))
			break;
	}

	// right-down
	n = p;
	while (n.X < _width - 1 && n.Y < _height - 1)
	{
		++n.X;
		++n.Y;
		if (AddIfChair(n, seats, neighbours))
			break;
	}

	return neighbours;
}

bool Day11::AddIfChair(const point& p, const string& seats, string& addTo)
{
	char c = seats[PointToIdx(p)];
	if (c != '.') // '.' == floor
	{
		addTo += c;
		return true;
	}
	return false;
}

void Day11::Transform(string& seats, bool partTwo)
{
	const string copy = string(seats);
	for (size_t i = 0; i < copy.size(); ++i)
	{
		// skip floor
		if (copy[i] == '.')
			continue;
		
		const point p = IdxToPoint(i);
		const auto& neighbours = partTwo ? NeighbouringSeats(p, copy) : Neighbours(p, copy);
		if (copy[i] == 'L')
		{
			if (find(neighbours.begin(), neighbours.end(), '#') == neighbours.end())
				seats[i] = '#';
		}
		else if (copy[i] == '#')
		{
			if (count(neighbours.begin(), neighbours.end(), '#') >= (partTwo ? 5 : 4))
				seats[i] = 'L';
		}
	}
}

void Day11::DebugPrint(const string& s)
{
	for (size_t i = 0; i < s.size(); ++i)
	{
		if (i % _width == 0 && i != 0)
			cout << endl;

		cout << s[i];
	}
	cout << endl << endl;
}

int Day11::PartOne()
{
	string copy = string(_seats); // avoid changing seats so other part can use it after this part has been called
	
	string previous = string(copy);
	do
	{
		previous = string(copy);
		Transform(copy, false);
	} while (copy.compare(previous) != 0);
	
	return count(copy.begin(), copy.end(), '#');
}

int Day11::PartTwo()
{
	string copy = string(_seats); // avoid changing seats so other part can use it after this part has been called
	
	string previous = string(copy);
	do
	{
		previous = string(copy);
		Transform(copy, true);
	} while (copy.compare(previous) != 0);

	return count(copy.begin(), copy.end(), '#');
}
