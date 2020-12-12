#pragma once

#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <algorithm>
#include "Helpers.h"

using namespace std;
using point = Helpers::point;

class Day11
{
private:
	string _seats = "";
	int _width = 0, _height = 0;

	int PointToIdx(const point& p);
	point IdxToPoint(int idx);
	string Neighbours(const point& p, const string& seats);
	string NeighbouringSeats(const point& p, const string& seats);
	bool AddIfChair(const point& p, const string& seats, string& addTo);
	void Transform(string& seats, bool partTwo);
	void DebugPrint(const string& s);

public:
	void ParseInput();
	int PartOne();
	int PartTwo();
};
