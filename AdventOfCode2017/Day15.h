#pragma once
#include <iostream>
#include <vector>
#include <string>
#include <bitset>

using namespace std;

struct Generator
{
private:
	static const int bitSize = sizeof(long long) * 8;

public:
	Generator(long long value, int factor) : Value(value), Factor(factor)
	{
	}

	long long NextValue()
	{
		Value = (Value * Factor) % 2147483647;
		return Value;
	}

	string ValueLowest16Bits()
	{
		bitset<bitSize> answer(Value);
		string fullBits = answer.to_string();
		return fullBits.substr(fullBits.size() - 16);
	}

	long long Value;
	int Factor;
};

class Day15
{
public:
	void Part1();
};