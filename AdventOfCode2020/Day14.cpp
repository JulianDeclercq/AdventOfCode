#include "Day14.h"

void Day14::ParseInput()
{	
	//ifstream input("input/day14example.txt");
	ifstream input("input/day14.txt");

	if (!input)
	{
		cout << "Failed to open input." << endl;
		return;
	}

	string line = "";
	while (getline(input, line))
		_instructions.push_back(line);	
}

void Day14::SetMask(string& mask)
{
	// reverse the string, since bitset::set works from least significant to most significant
	//cout << "mask set to:  " << mask << endl;
	reverse(mask.begin(), mask.end());
	_mask = mask;
}

ull Day14::ApplyMask(ull target)
{
	auto targetBits = bitset<36>(target);
	//cout << "TargetBits B: " << targetBits.to_string() << "(" << target << ")" << endl;

	for (size_t i = 0; i < _mask.size(); ++i)
	{
		if (_mask[i] == 'X')
		{
			continue;
		}
		else if (_mask[i] == '0')
		{
			targetBits.set(i, false);
		}
		else if (_mask[i] == '1')
		{
			targetBits.set(i, true);
		}
		else cout << "invalid character found in mask parsing: " << _mask[i] << endl;
	}

	const auto result = targetBits.to_ullong();
	//cout << "TargetBits A: " << targetBits.to_string() << "(" << result << ")" << endl << endl;
	return result;
}

void Day14::WriteMemory(int at, ull value)
{
	_memory[at] = value;
}

ull Day14::PartOne()
{
	regex maskR("mask = (\\w+)"), memR("mem\\[(\\d+)\\] = (\\d+)");
	smatch match;
	for (const string& instruction : _instructions)
	{
		if (regex_search(instruction, match, maskR))
		{
			string mask = static_cast<string>(match[1]);
			SetMask(mask);
		}
		else if (regex_search(instruction, match, memR))
		{
			size_t size = 0;
			unsigned long long target = stoull(static_cast<string>(match[2]), &size, 0);

			WriteMemory(stoi(match[1]), ApplyMask(target));
		}
		else cout << "Invalid instruction: " << instruction << endl;
	}

	ull count = 0;

	for (const auto& pair : _memory)
		count += pair.second;

	return count;
}

ull Day14::PartTwo()
{
	return 0;
}
