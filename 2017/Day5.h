#pragma once
#include <iostream>
#include <string>
#include <fstream>
#include <vector>

using namespace std;

class Day5
{
private:
	// Method to re-use the logic for both part1 and part2
	int Solve(bool part2 = false);
public:
	void Part1();
	void Part2();
};
