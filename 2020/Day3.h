#pragma once

#include <iostream>
#include <regex>
#include <fstream>
#include <string>
#include <vector>
#include <algorithm>
#include "Helpers.h"

using namespace std;
using point = Helpers::point;

class Day3
{
private:
	string _treeChart = "";
	int _width = 0, _height = 0;

	bool IsTree(const point& p);
	int TreesOnSlope(const point& slope);

public:
	void ParseInput();
	int PartOne();
	unsigned int PartTwo();
};
