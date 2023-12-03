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

class Day11
{
private:
	map<string, Helpers::Point> _directions = map<string, Helpers::Point>
	{
		{ "n", Helpers::Point(0, -1) },{ "ne", Helpers::Point(1, -1) },{ "se", Helpers::Point(1, 0) },{ "s", Helpers::Point(0, 1) },{ "sw", Helpers::Point(-1, 1) },{ "nw", Helpers::Point(-1, 0) }
	};

	string ParseInput();
	Helpers::Point FollowPath(const string& input);
	void CalculateShortestRoute(vector<Helpers::Point>& route, const Helpers::Point& destination, Helpers::Point currentPosition);

	size_t _furthestStepsAway = 0;

	// optimization: if only part 1 is run, don't do the calculations for part 2
	bool _part1 = true;
public:
	void Part1();
	void Part2();
};
