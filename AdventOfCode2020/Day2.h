#pragma once

#include <iostream>
#include <regex>
#include <fstream>
#include <string>
#include <vector>
#include <algorithm>

using namespace std;

class Day2
{
private:
	struct Password
	{
		Password(int min, int max, char req, string content) : Minimum(min), Maximum(max), Requirement(req), Contents(content)
		{
		}

		int Minimum = 0;
		int Maximum = 0;
		char Requirement = ' ';
		string Contents = "";
	};

	vector<Password> _passwords = vector<Password>();

	bool IsValidPartOne(const Password& password);
	bool IsValidPartTwo(const Password& password);

public:
	void ParseInput();
	int PartOne();
	int PartTwo();
};