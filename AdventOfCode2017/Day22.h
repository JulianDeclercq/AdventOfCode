#pragma once
#include <iostream>
#include <fstream>
#include <vector>
#include <string>
#include <algorithm>
#include <map>
#include "Helpers.h"

using Point = Helpers::Point;
using namespace std;

class Day22
{
private:
	int _gridHeight = -1;
	int _gridWidth = -1;

	enum Facing
	{
		North = 0,
		East = 1,
		South = 2,
		West = 3
	};

	Facing _facing = North;

	void TurnRight();
	void TurnLeft();

public:
	void Part1();
};