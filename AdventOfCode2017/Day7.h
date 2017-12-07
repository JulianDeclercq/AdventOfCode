#pragma once
#include <iostream>
#include <string>
#include <vector>
#include <fstream>
#include <regex>
#include <algorithm>
#include <map>
#include "Helpers.h"

using namespace std;

struct Program
{
	// Default constructor
	Program() {	}
	Program(string name, int weight, vector<Program*> children) : Name(name), Weight(weight), Children(children)
	{
	}

	string Name;
	int Weight;
	vector<Program*> Children;
};

class Day7
{
public:
	void Part1();
	void Part2();
};
