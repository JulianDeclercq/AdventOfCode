#include "Day2.h"
void Day2::parse_input()
{
	//ifstream input("example/Day2.txt");
	ifstream input("Input/Day2.txt");
	if (input.fail())
	{
		cout << "Failed to open inputfile." << endl;
		return;
	}

	// the full input is on 1 single line
	string line = "";
	getline(input, line);

	// divide the line into opcodes
	// NOTE to self: I added a trailing comma to the last number in the input and example for simplicity sake
	// NOTE2 to self: Regex is overkill for just parsing comma separated numbers, but I had the structure set up before i realised opcode 99 is a dealbreaker
	smatch match;
	regex reg("\\d+,"); 

	while (regex_search(line, match, reg)) 
	{
		// add the integer to the intcode vector
		_intcode.push_back(stoi(match[0])); // match on index 0 = full match content

		// update the string to search (start search after current match)
		line = match.suffix().str();
	}

	/*
	Once you have a working computer, the first step is to restore the gravity
	assist program (your puzzle input) to the "1202 program alarm" state it had just before
	the last computer caught fire. To do this, before running the program,
	replace position 1 with the value 12 and replace position 2 with the value 2.
	*/
	_intcode[1] = 12;
	_intcode[2] = 2;
}

void Day2::progress()
{
	//TODO: bounds check needed?
	// check the next opcode
	int opcode = _intcode[_idx];

	// execute the opcode
	switch (opcode)
	{
		case 1: // addition
			_intcode[_intcode[_idx + 3]] = _intcode[_intcode[_idx + 1]] + _intcode[_intcode[_idx + 2]];
			break;
		case 2:
			_intcode[_intcode[_idx + 3]] = _intcode[_intcode[_idx + 1]] * _intcode[_intcode[_idx + 2]];
			break;
		case 99: // program should immediately halt
			return;
		default: cout << "Invalid opcode: " << opcode << endl;
			break;
	}

	// progress to the next operation
	_idx += 4;
	progress();
}

void Day2::part1()
{
	parse_input();

	// start traversion
	progress();

	//debug: print the intcode
	for (int i : _intcode)
		cout << i << " ";
	cout << endl;

	cout << "The answer to day 1 part 1 is: " << _intcode[0] << endl;
}


