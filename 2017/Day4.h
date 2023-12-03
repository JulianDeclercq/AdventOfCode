#pragma once

#include <vector>
#include <string>
#include <fstream>
#include <iostream>
#include <algorithm>
#include "Helpers.h"

using namespace std;

class Day4
{
private:
	// Method to re-use the logic for both part1 and part2
	int Solve(bool part2 = false);
public:
	void Part1();
	void Part2();
};