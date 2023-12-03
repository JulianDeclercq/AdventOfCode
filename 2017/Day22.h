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

	enum State
	{
		Clean = 0,
		Weakened = 1,
		Infected = 2,
		Flagged = 3
	};

	// Define the forward movement for each facing direction
	const map <Facing, Point> _forwardMovement{ { North, Point(0, 1) },{ East, Point(1, 0) },{ South, Point(0, -1) },{ West, Point(-1, 0) } };

	// Use pointers so nullptr can be passed for the cluster that does not apply to current part
	void ParseInput(map<Point, bool>* clusterP1, map<Point, State>* clusterP2);
	void TurnRight();
	void TurnLeft();
	void Inverse();

public:
	void Part1();
	void Part2();
};