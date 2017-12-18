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

void Day10::Part2()
{
	const int listSize = 256;
	string input = "14,58,0,116,179,16,1,104,2,254,167,86,255,55,122,244";
	const string standardLengthSuffix = "17,31,73,47,23";

	// Fill the list
	vector<int> numbers = vector<int>(listSize);
	for (size_t i = 0; i < numbers.size(); ++i)
		numbers[i] = i;

	// Convert the input to a string of ASCII characters
	string asciiInput = "";
	for (char c : input)
	{
		asciiInput += to_string(static_cast<int>(c));
		asciiInput += ',';
	}

	// Append the suffix
	asciiInput.append(standardLengthSuffix);

	// Run 64 rounds using the same length sequence in each round
	// The current position and skip size should be preserved between rounds
	for (int i = 0; i < 64; ++i)
	{
		for (const string& lengthString : Helpers::Split(asciiInput, ','))
		{
			int length = stoi(lengthString);

			// Reverse the length
			ReverseLength(numbers, length);

			// The current position moves forward by the length plus the skip size
			_currentPosition = (_currentPosition + length + _skipSize) % listSize;

			// Increment the skip size
			++_skipSize;
		}
	}

	// Now that the sparse hash has been calculated, calculate the dense hash
	vector<int> denseHash = vector<int>();

	/*
		// TEMP TEMP TEMP DEBUG
		vector<int> lel = vector<int>{ 65, 27, 9, 1, 4, 3, 40, 50, 91, 7, 6, 0, 2, 5, 68, 22 };
		numbers.clear();
		for (int i = 0; i < 16; ++i)
			numbers.insert(numbers.end(), lel.begin(), lel.end());*/

	int element = numbers[0];
	for (size_t i = 1; i < numbers.size(); ++i)
	{
		// XOR the element with the next one
		element ^= numbers[i];

		// Start a new chunk of 16
		if ((i + 1) % 16 == 0)
		{
			// Add the element to the dense hash
			denseHash.push_back(element);

			// Increment loop counter
			++i;

			// Bounds check
			if (i == numbers.size())
				break;

			// Reset the element to the first of the new chunk
			element = numbers[i];
		}
	}

	// Convert the dense hash to hex notation
	stringstream sstream;
	for (int element : denseHash)
		sstream << setfill('0') << setw(2) << hex << element;

	cout << "Day 10 Part 2 answer: " << sstream.str() << endl;
}