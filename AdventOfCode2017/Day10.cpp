#include "Day10.h"

void Day10::ReverseLength(vector<int>& sequence, int length)
{
	vector<int> selection = vector<int>();

	// Add the selection
	for (int i = 0; i < length; ++i)
		selection.push_back(sequence[(i + _currentPosition) % sequence.size()]);

	// Reverse the selection
	reverse(selection.begin(), selection.end());

	// Re-insert the reversed selection
	for (int i = 0; i < length; ++i)
		sequence[(i + _currentPosition) % sequence.size()] = selection[i];
}

void Day10::KnotHashRound(vector<int>& sequence, const string& input)
{
	for (const string& lengthString : Helpers::Split(input, ','))
	{
		int length = stoi(lengthString);

		// Reverse the length
		ReverseLength(sequence, length);

		// The current position moves forward by the length plus the skip size
		_currentPosition = (_currentPosition + length + _skipSize) % _listSize;

		// Increment the skip size
		++_skipSize;
	}
}

void Day10::Part1()
{
	const string input = "14,58,0,116,179,16,1,104,2,254,167,86,255,55,122,244";

	// Fill the list
	vector<int> numbers = vector<int>(_listSize);
	for (size_t i = 0; i < numbers.size(); ++i)
		numbers[i] = i;

	// Parse the lengths
	KnotHashRound(numbers, input);

	// Calculate the answer
	cout << "Day 10 Part 1 answer: " << numbers[0] * numbers[1] << endl;
}

void Day10::Part2()
{
	string input = "14,58,0,116,179,16,1,104,2,254,167,86,255,55,122,244";
	const string standardLengthSuffix = "17,31,73,47,23";

	// Fill the list
	vector<int> numbers = vector<int>(_listSize);
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
	for (int i = 0; i < 64; ++i)
		KnotHashRound(numbers, asciiInput);

	// Now that the sparse hash has been calculated, calculate the dense hash
	vector<int> denseHash = vector<int>();
	for (int i = 0; i < 16; ++i)
	{
		// Set the new element as the first int of the new chunck
		int element = numbers[i * 16];

		// XOR with each element of the next chunck (except for itself so start at 1)
		for (int j = 1; j < 16; ++j)
			element ^= numbers[i * 16 + j];

		// Add the element to the dense hash
		denseHash.push_back(element);
	}

	// Convert the dense hash to hex notation
	stringstream sstream;
	for (int element : denseHash)
		sstream << setfill('0') << setw(2) << hex << element;

	cout << "Day 10 Part 2 answer: " << sstream.str() << endl;
}