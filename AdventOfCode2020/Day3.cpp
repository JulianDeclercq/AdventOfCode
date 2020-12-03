#include "Day3.h"

void Day3::ParseInput()
{
	if (_inputParsed)
		return;

	//ifstream input("input/day3example.txt");
	ifstream input("input/day3.txt");

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

		_treeChart.append(line);
		++_height;
	}

	_inputParsed = true;
}

char Day3::AtCoord(Vector2 p) // DEBUG
{
	int col = p.X % _width;
	int row = p.Y * _width;
	char c = _treeChart[col + row];
	return c;
}

bool Day3::IsTree(Vector2 p)
{
	return AtCoord(p) == '#'; // TEMP UNTIL ATCOORD IS NO LONGER IN USE
}

int Day3::Part1()
{
	ParseInput();

	Vector2 slope(3, 1);
	Vector2 current(0, 0);
	int treeCount = 0;
	while (current.Y < _height)
	{
		if (IsTree(current))
		{
			++treeCount;
			//cout << "Found tree at " << current.ToString() << endl;
		}
		//else cout << "Found NO tree at " << current.ToString() << endl;

		// slide the slope
		current += slope;
	}

	return treeCount;
}


