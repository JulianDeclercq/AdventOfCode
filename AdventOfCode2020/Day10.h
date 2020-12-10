#pragma once

#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <algorithm>

using namespace std;
using arrangement = vector<int>;

class Day10
{
private:
	vector<int> _adaptors;

	// only for debug purposes
	vector<arrangement> _arrangements;

	long long _count = 0;

	void calculateArrangements(const arrangement& c, int offset);
	void calculateArrangements2(int last, int offset);
public:
	void ParseInput();
	int PartOne();
	long long PartTwo(bool fast);
};