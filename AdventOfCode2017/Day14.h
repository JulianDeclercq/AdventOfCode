#pragma once
#include <iostream>
#include <vector>
#include <string>
#include <bitset>
#include <sstream>
#include <algorithm>
#include <map>
#include <fstream>
#include <set>
#include "Day10.h"

using namespace std;

struct Cell
{
	Cell(bool used, size_t idx) : Visible(used), Index(idx)
	{
	}

public:
	size_t Index;
	bool Visible = false;
	int Group = -1;
	bool GroupChecked = false;
};

class Day14
{
private:
	string HexToBinary(const string& hex);
	void AssignGroupToCell(vector<Cell>& cells, Cell& cell, int currentGroup);

	size_t _width = 128;

	map<char, string> _hexLookup{ { '0', "0000" },{ '1', "0001" },{ '2', "0010" },{ '3', "0011" },{ '4', "0100" },{ '5', "0101" },{ '6', "0110" },{ '7', "0111" },{ '8', "1000" },{ '9', "1001" },{ 'A', "1010" },{ 'B', "1011" },{ 'C', "1100" },{ 'D', "1101" },{ 'E', "1110" },{ 'F', "1111" } };

public:
	void Part1();
	void Part2();
};