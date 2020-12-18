#pragma once

#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <algorithm>
#include <map>
#include "Helpers.h"

using namespace std;

struct Cube
{
	Cube(int x, int y, int z) : X(x), Y(y), Z(z) {}
	int X = 0;
	int Y = 0;
	int Z = 0;

	friend bool operator<(const Cube& lhs, const Cube& rhs)
	{
		if (lhs.X != rhs.X) return lhs.X < rhs.X;
		if (lhs.Y != rhs.Y) return lhs.Y < rhs.Y;
		return lhs.Z < rhs.Z;
	}

	string ToString() const
	{
		return "(" + to_string(X) + ", " + to_string(Y) + ", " + to_string(Z) + ")";
	}
};

using grid = map<Cube, bool>;

class Day17
{
private:
	grid _grid;

	int ActiveNeighbours(const Cube& c, const grid& grid, vector<Cube>* newlyAdded);
	void ConwayStep();
	void TransformCube(const pair<Cube, bool>& cube, int activeNeighbours);
public:
	void ParseInput();
	int PartOne();
	int PartTwo();
};