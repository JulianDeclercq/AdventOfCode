#pragma once
#include <iostream>
#include <string>
#include <vector>
#include <fstream>
#include <regex>
#include <map>
#define u16 unsigned short

using namespace std;

class Day7
{
private:
	map<string, u16> _wireMapping = map<string, u16>();

	void ExecuteCommand(const string& command);

public:
	void Part1();
	void Part2();
};