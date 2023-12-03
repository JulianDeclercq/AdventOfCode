#include "Day14.h"

void Day14::ParseInput()
{	
	//ifstream input("input/day14example.txt");
	//ifstream input("input/day14example2.txt");
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

ull Day14::ApplyMask(ull target)
{
	auto targetBits = bitset<36>(target);

	// reverse the mask, since bitset::set works from least significant to most significant
	string mask(_mask);
	reverse(mask.begin(), mask.end());

	for (size_t i = 0; i < mask.size(); ++i)
	{
		if (mask[i] == 'X')
		{
			continue;
		}
		else if (mask[i] == '0')
		{
			targetBits.set(i, false);
		}
		else if (mask[i] == '1')
		{
			targetBits.set(i, true);
		}
		else cout << "invalid character found in mask parsing: " << mask[i] << endl;
	}
	return targetBits.to_ullong();
}

string Day14::ApplyMask2(const string& mask, ull target)
{
	string targetBits = bitset<36>(target).to_string();

	for (size_t i = 0; i < mask.size(); ++i)
	{
		if (mask[i] == '0')
			continue;
		
		targetBits[i] = mask[i];
	}
	return targetBits;
}

vector<string> Day14::Possibilities(const string& s)
{
	vector<string> possibilities;
	size_t idx = s.find('X');
	if (idx == string::npos)
	{
		possibilities.push_back(s);
	}
	else
	{
		string cpy(s);

		cpy[idx] = '0';
		vector<string> zero = Possibilities(cpy);
		possibilities.insert(possibilities.end(), zero.begin(), zero.end());

		cpy[idx] = '1';
		vector<string> one = Possibilities(cpy);
		possibilities.insert(possibilities.end(), one.begin(), one.end());
	}
	return possibilities;
}

ull Day14::ValueSumAfterExecution(bool part1)
{
	_memory.clear();
	regex maskR("mask = (\\w+)"), memR("mem\\[(\\d+)\\] = (\\d+)");
	smatch match;
	for (const string& instruction : _instructions)
	{
		if (regex_search(instruction, match, maskR))
		{
			_mask = match[1];
		}
		else if (regex_search(instruction, match, memR))
		{
			size_t size = 0;
			ull target = stoull(static_cast<string>(match[2]), &size, 0);
			
			if (part1)
			{
				// write the value to the memory address
				_memory[stoi(match[1])] = ApplyMask(target);
			}
			else // part 2
			{
				// write the value to the memory address
				string mask = ApplyMask2(_mask, stoi(match[1]));
				const auto possibilities = Possibilities(mask);

				vector<ull> addresses;
				for (const string& possibility : possibilities)
					addresses.push_back(bitset<36>(possibility).to_ullong());

				for (const auto address : addresses)
					_memory[address] = target;
			}
		}
		else cout << "Invalid instruction: " << instruction << endl;
	}

	ull count = 0;

	for (const auto& pair : _memory)
		count += pair.second;

	return count;
}

ull Day14::PartOne()
{
	return ValueSumAfterExecution(true);
}

ull Day14::PartTwo()
{
	return ValueSumAfterExecution(false);
}
