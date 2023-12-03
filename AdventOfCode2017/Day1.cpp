#include "Day1.h"

void Day1::Part1()
{
	int sum = 0;

	// Take a copy of the input string to make sure different parts do not affect each other
	std::string inputString(_inputString);

	// Add the last character to the beginning as a way of looping.
	inputString.insert(0, 1, inputString.back());

	for (size_t i = 0; i < inputString.size() - 1; ++i)
	{
		if (inputString[i] == inputString[i + 1])
			sum += inputString[i] - '0'; // manually "cast" char to its integer value
	}

	std::cout << "Day 1 Part 1 answer is: " << sum << std::endl;
}

bool Day1::Corresponds(size_t currentIdx)
{
	size_t correspondingIdx = (currentIdx + _circularOffset) % _inputString.size();
	return _inputString[currentIdx] == _inputString[correspondingIdx];
}

void Day1::Part2()
{
	int sum = 0;

	// Set the circular offset
	_circularOffset = _inputString.size() / 2;

	// Loop through the string and add the value of the character if it corresponds with the circular offset
	for (size_t i = 0; i < _inputString.size(); ++i)
	{
		if (Corresponds(i))
			sum += _inputString[i] - '0'; // manually "cast" char to its integer value
	}

	std::cout << "Day 1 Part 2 answer is: " << sum << std::endl;
}