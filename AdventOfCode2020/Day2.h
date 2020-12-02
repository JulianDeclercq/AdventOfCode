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

	bool _inputParsed = false;
	vector<Password> _passwords = vector<Password>();
	void ParseInput();

	bool IsValidPart1(const Password& password);
	bool IsValidPart2(const Password& password);
public:
	Day2(){}
	int Part1();
	int Part2();
};