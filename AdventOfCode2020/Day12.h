#pragma once

#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <algorithm>
#include <map>
#include "Helpers.h"

using namespace std;
using point = Helpers::point;
using instruction = pair<char, int>;

class Day12
{
	vector<instruction> _instructions;
	point _position = point(0, 0);
	point _direction = point(1, 0); // ship starts by facing east
	map<point, point> _rotateRight =
	{
		{ point( 0, -1), point( 1,  0) },	// N -> E
		{ point( 1,  0), point( 0,  1) },	// E -> S
		{ point( 0,  1), point(-1,  0) },	// S -> W
		{ point(-1,  0), point( 0, -1) }	// W -> N
	};
private:
	void Execute(const instruction& instruction);
public:
	void ParseInput();
	int PartOne();
	int PartTwo();
};
