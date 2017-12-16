#pragma once
#include <iostream>
#include <string>
#include <vector>
#include <sstream>
#include <iterator>
#include <map>
#include <fstream>
#include <stdlib.h>
#include "Helpers.h"

using namespace std;

struct Point
{
public:
	Point() : X(0), Y(0) {}
	Point(int x, int y) : X(x), Y(y)
	{
	}
	int X;
	int Y;

public:
	Point& operator+=(const Point& rhs)
	{
		X += rhs.X;
		Y += rhs.Y;
		return *this; // return the result by reference
	}

	Point& operator-=(const Point& rhs)
	{
		X -= rhs.X;
		Y -= rhs.Y;
		return *this; // return the result by reference
	}

	friend Point operator+(Point lhs, const Point& rhs)
	{
		return lhs += rhs;
	}

	friend Point operator-(Point lhs, const Point& rhs)
	{
		return lhs -= rhs;
	}
};

class Day11
{
private:
	map<string, Point> _directions = map<string, Point>
	{
		{ "n", Point(0, -1) },{ "ne", Point(1, -1) },{ "se", Point(1, 0) },{ "s", Point(0, 1) },{ "sw", Point(-1, 1) },{ "nw", Point(-1, 0) }
	};

	string ParseInput();
	Point FollowPath(const string& input);
	void CalculateShortestRoute(vector<Point>& route, const Point& destination, Point currentPosition);

	size_t _furthestStepsAway = 0;

	// optimization: if only part 1 is run, don't do the calculations for part 2
	bool _part1 = true;
public:
	void Part1();
	void Part2();
};
