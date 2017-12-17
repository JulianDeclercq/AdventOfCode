#pragma once
#include <fstream>
#include <string>
#include <iostream>
#include <regex>
#include <map>
#include <set>
#include <algorithm>
#include "Helpers.h"

using namespace std;

class Day12
{
private:
	void ParseInput();
	void ExploreConnections(set<int>& connections, int pipeID);

	map<int, set<int>> _pipeNetwork = map<int, set<int>>();

public:
	void Part1();
};
