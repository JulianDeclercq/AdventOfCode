#pragma once

#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <algorithm>
#include <map>
#include <set>
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

struct Cube4D
{
	Cube4D(int x, int y, int z, int w) : X(x), Y(y), Z(z), W(w) {}
	int X = 0;
	int Y = 0;
	int Z = 0;
	int W = 0;

	friend bool operator<(const Cube4D& lhs, const Cube4D& rhs)
	{
		if (lhs.X != rhs.X) return lhs.X < rhs.X;
		if (lhs.Y != rhs.Y) return lhs.Y < rhs.Y;
		if (lhs.Z != rhs.Z) return lhs.Z < rhs.Z;
		return lhs.W < rhs.W;
	}

	string ToString() const
	{
		return "(" + to_string(X) + ", " + to_string(Y) + ", " + to_string(Z) + ", " + to_string(W) + ")";
	}
};

using grid = map<Cube, bool>;
using grid4D = map<Cube4D, bool>;

class Day17
{
private:
	grid _grid;
	grid4D _grid4D;

	void ConwayStep();
	int ActiveNeighbours(const Cube& c, const grid& grid, set<Cube>* newlyAdded);
	void TransformCube(const pair<Cube, bool>& cube, int activeNeighbours);

	void ConwayStep4D();
	int ActiveNeighbours4D(const Cube4D& c, const grid4D& grid, set<Cube4D>* newlyAdded);
	void TransformCube4D(const pair<Cube4D, bool>& cube, int activeNeighbours);

public:
	void ParseInput();
	int PartOne();
	int PartTwo();
};