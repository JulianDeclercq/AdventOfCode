#include "day15.h"

void Day15::Part1()
{
	Generator A(873, 16807);
	Generator B(583, 48271);

	int answer = 0;
	for (int i = 0; i < 40000000; ++i)
	{
		A.NextValue();
		B.NextValue();

		if (A.ValueLowest16Bits().compare(B.ValueLowest16Bits()) == 0)
			++answer;
	}

	cout << "Day 15 Part 1 answer: " << answer << endl;
}

void Day15::Part2()
{
	Generator A(873, 16807, 4);
	Generator B(583, 48271, 8);

	int answer = 0;
	for (int i = 0; i < 5000000; ++i)
	{
		A.NextValidValue();
		B.NextValidValue();

		if (A.ValueLowest16Bits().compare(B.ValueLowest16Bits()) == 0)
			++answer;
	}

	cout << "Day 15 Part 2 answer: " << answer << endl;
}