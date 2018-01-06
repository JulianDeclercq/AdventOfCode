#include "day15.h"

void Day15::Part1()
{
	Generator A(873, 16807);
	Generator B(583, 48271);

	int answer = 0;
	for (int i = 0; i < 40000000; ++i)
	{
		if (i % 5000000 == 0)
			cout << i << endl;

		A.NextValue();
		B.NextValue();

		if (A.ValueLowest16Bits().compare(B.ValueLowest16Bits()) == 0)
			++answer;
	}

	cout << "Day 15 Part 1 answer: " << answer << endl;
}