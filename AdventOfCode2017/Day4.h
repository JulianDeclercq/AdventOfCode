#pragma once

#include <vector>
#include <string>
#include <fstream>
#include <iostream>
#include "Helpers.h"

using namespace std;

class Day4
{
private:
	vector<string> _words = vector<string>();
	void ParseInput();

public:
	void Part1();
	void Part2();
};