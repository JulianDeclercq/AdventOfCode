#include <string>
#include <fstream>
#include <iostream>
#include <vector>
#include <algorithm>
#include <iterator>
#include <sstream>

using namespace std;

class Day2
{
private:
	vector<string> Rows = vector<string>();

	template<typename T>
	void split(const string &s, char delim, T result)
	{
		stringstream ss(s);
		string item;
		while (getline(ss, item, delim))
			*(result++) = stoi(item);
	}

	vector<int> split(const string &s, char delim);

	// Part 1
	void ParseInput();
	int RowDifference(const string& row);

	// Part 2
	int EvenlyDivided(const string& row);

public:
	void Part1();
	void Part2();
};