#include "Day14.h"

string Day14::HexToBinary(const std::string& hex)
{
	string bin;
	for_each(hex.begin(), hex.end(), [&](const char c) {bin.append(_hexLookup.at(toupper(c))); });
	return bin;
}

void Day14::Part1()
{
	//const string input = "flqrgnkx";
	const string input = "ljoxqyyw";

	int used = 0;
	for (int i = 0; i < 128; ++i)
	{
		string hash = Day10().KnotHash(input + '-' + to_string(i));
		string binary = HexToBinary(hash);
		used += count_if(binary.begin(), binary.end(), [](const char c) {return c == '1'; });
	}

	cout << "Day 14 Part 1 answer: " << used << endl;
}

void Day14::Part2()
{
}