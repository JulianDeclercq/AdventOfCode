#include <string>
#include <fstream>
#include <iostream>
#include <vector>
#include <algorithm>
#include "Helpers.h"

using namespace std;

class Day2
{
private:
	vector<string> Rows = vector<string>();

	// Part 1
	void ParseInput();
	int RowDifference(const string& row);

	// Part 2
	int EvenlyDivided(const string& row);

public:
	void Part1();
	void Part2();
};