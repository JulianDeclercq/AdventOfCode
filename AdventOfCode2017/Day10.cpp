#include "Day10.h"

void Day10::ReverseLength(vector<int>& sequence, int length)
{
	// Reversing with length 1 has no effect
	if (length == 1)
		return;

	// Offset the begin iterator with the current position
	auto curPosIt = sequence.begin() + _currentPosition;

	// Check if this is a wrap-case
	int beforeWrap = sequence.size() - _currentPosition;

	// In case it does not wrap
	if (length <= beforeWrap)
	{
		// Fill the vector with the to be reversed elements
		vector<int> reversed = vector<int>(curPosIt, curPosIt + length);

		// Reverse the vector
		reverse(reversed.begin(), reversed.end());

		// Reconstruct the numbers vector
		for (int i = _currentPosition; i < _currentPosition + length; ++i)
		{
			// Calculate the true loop index by extracting the starting value of i
			sequence[i] = reversed[i - _currentPosition];
		}

		// Exit the method
		return;
	}

	// In case it wraps
	int afterWrap = length - beforeWrap;

	// Add the elements before the wrap (end of sequence)
	vector<int> reversed = vector<int>(curPosIt, curPosIt + beforeWrap);

	// Add the elements after the wrap (beginning of sequence)
	reversed.insert(reversed.end(), sequence.begin(), sequence.begin() + afterWrap);

	// Reverse the reverse vector
	reverse(reversed.begin(), reversed.end());

	// Reconstruct the numbers vector
	// Unwrapped part (end of sequence)
	for (size_t i = _currentPosition; i < sequence.size(); ++i)
	{
		// Calculate the true loop index by extracting the starting value of i
		sequence[i] = reversed[i - _currentPosition];
	}

	// Wrapped part (beginning of sequence)
	for (int i = 0; i < afterWrap; ++i)
	{
		// Offset the position for reversed with the number of numbers that have been used in the previous loop
		sequence[i] = reversed[i + beforeWrap];
	}
}

void Day10::Part1()
{
	const string input = "14,58,0,116,179,16,1,104,2,254,167,86,255,55,122,244";
	const int listSize = 256;

	// Fill the list
	vector<int> numbers = vector<int>(listSize);
	for (size_t i = 0; i < numbers.size(); ++i)
		numbers[i] = i;

	// Parse the lengths
	for (const string& lengthString : Helpers::Split(input, ','))
	{
		int length = stoi(lengthString);

		// Reverse the length
		ReverseLength(numbers, length);

		// The current position moves forward by the length plus the skip size
		_currentPosition = (_currentPosition + length + _skipSize) % listSize;

		// Increment the skip size
		++_skipSize;
	}

	// Calculate the answer
	cout << "Day 10 Part 1 answer: " << numbers[0] * numbers[1] << endl;
}