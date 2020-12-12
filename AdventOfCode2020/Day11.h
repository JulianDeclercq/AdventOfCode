#pragma once

#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <algorithm>
#include "Helpers.h"

using namespace std;
using Point = Helpers::Point;

class Day11
{
private:
	string _seats = "";
	int _width = 0, _height = 0;

	int PointToIdx(const Point& p);
	Point IdxToPoint(int idx);
	string Neighbours(const Point& p, const string& seats);
	string NeighbouringSeats(const Point& p, const string& seats);
	bool AddIfChair(const Point& p, const string& seats, string& addTo);
	void Transform(string& seats, bool partTwo);
	void DebugPrint(const string& s);

public:
	void ParseInput();
	int PartOne();
	int PartTwo();
};
