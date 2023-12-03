#pragma once
#include <string>
#include <vector>
#include <fstream>
#include <iostream>
#include <algorithm>
#include <map>
#include "Helpers.h"

using namespace std;

class Day6
{
private:
	string State(const vector<int>& memoryBanks);

	// Method to re-use the logic from part 1 for part 2
	int Solve(bool part2 = false);
public:
	void Part1();
	void Part2();
};
