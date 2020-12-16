#include "Day12.h"

void Day12::ParseInput()
{
	//ifstream input("input/day12example.txt");
	ifstream input("input/day12.txt");

	if (!input)
	{
		cout << "Failed to open input." << endl;
		return;
	}

	string line = "";
	while (getline(input, line))
		_instructions.push_back({ line[0], stoi(line.substr(1)) });
}

void Day12::Execute(const instruction& instruction)
{
	/*
		0,0 1,0 2,0
		0,1 1,1 2,1
		0,2 1,2 2,2
	*/
	int count = 0;
	switch (instruction.first)
	{
	case 'N':
		_position += instruction.second * point(0, -1);
		break;
	case 'E':
		_position += instruction.second * point(1, 0);
		break;
	case 'S':
		_position += instruction.second * point(0, 1);
		break;
	case 'W':
		_position += instruction.second * point(-1, 0);
		break;
	case 'R':
		count = instruction.second / 90;
		for (int i = 0; i < count; ++i)
			_direction = _rotateRight[_direction];
		break;
	case 'L':
		count = (instruction.second / 90) * 3; // rotating left is rotating right 3 times <:)
		for (int i = 0; i < count; ++i)
			_direction = _rotateRight[_direction];
		break;
	case 'F':
		_position += instruction.second * _direction;
		break;
	default: cout << "Invalid instruction: " << instruction.first << instruction.second << endl;
		break;
	}
}

void Day12::ExecutePart2(const instruction& instruction)
{
	float angle = 0.0f;
	switch (instruction.first)
	{
	case 'N':
		_waypoint += instruction.second * point(0, -1);
		break;
	case 'E':
		_waypoint += instruction.second * point(1, 0);
		break;
	case 'S':
		_waypoint += instruction.second * point(0, 1);
		break;
	case 'W':
		_waypoint += instruction.second * point(-1, 0);
		break;
	case 'R':
		angle = static_cast<float>(instruction.second);
		_waypoint = RotatePoint(_waypoint, angle);
		break;
	case 'L':
		angle = static_cast<float>(instruction.second);
		_waypoint = RotatePoint(_waypoint, -angle);
		break;
	case 'F':
	{
		_position += instruction.second * _waypoint;
	}
		break;
	default: cout << "Invalid instruction: " << instruction.first << instruction.second << endl;
		break;
	}
}

// derived from a theoretical explanation from https://math.stackexchange.com/questions/346672/2d-rotation-of-point-about-origin
point Day12::RotatePoint(const point& p, float angle)
{
	int s = static_cast<int>(sin(angle * M_PI / 180.0)); // convert degrees to radians
	int c = static_cast<int>(cos(angle * M_PI / 180.0));

	int x = c * p.X + (-s * p.Y);
	int y = s * p.X + c * p.Y;

	return point(x, y);
}

int Day12::PartOne()
{
	_position = point(0, 0);
	for (const instruction& instruction : _instructions)
		Execute(instruction);

	return Helpers::point::ManhattanDistance({ 0, 0 }, _position);
}

int Day12::PartTwo()
{
	_position = point(0, 0);
	for (const instruction& instruction : _instructions)
		ExecutePart2(instruction);

	return Helpers::point::ManhattanDistance({ 0, 0 }, _position);
}
