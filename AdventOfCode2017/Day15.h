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
	long long Value;
	int Factor;
	int Criterion;

public:
	Generator(long long value, int factor, int criterion = 0) : Value(value), Factor(factor), Criterion(criterion)
	{
	}

	long long NextValue()
	{
		Value = (Value * Factor) % 2147483647;
		return Value;
	}

	void NextValidValue()
	{
		// Keep calculating the value until it is dividable by the criterion
		while (NextValue() % Criterion != 0);
	}

	string ValueLowest16Bits()
	{
		bitset<bitSize> answer(Value);
		string fullBits = answer.to_string();
		return fullBits.substr(fullBits.size() - 16);
	}
};

class Day15
{
public:
	void Part1();
	void Part2();
};