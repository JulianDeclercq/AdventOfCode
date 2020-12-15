#include "Day14.h"

void Day14::ParseInput()
{	
	//ifstream input("input/day14example.txt");
	ifstream input("input/day14example2.txt");
	//ifstream input("input/day14.txt");

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
	//cout << "TargetBits B: " << targetBits.to_string() << "(" << target << ")" << endl;

	// reverse the string, since bitset::set works from least significant to most significant
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

	const auto result = targetBits.to_ullong();
	//cout << "TargetBits A: " << targetBits.to_string() << "(" << result << ")" << endl << endl;
	return result;
}

string Day14::ApplyMask2(const string& mask, ull target)
{
	string targetBits = bitset<36>(target).to_string();
	//reverse(targetBits.begin(), targetBits.end());

	for (size_t i = 0; i < mask.size(); ++i)
	{
		if (mask[i] == '0')
			continue;
		
		targetBits[i] = mask[i];
	}
	return targetBits;
}

vector<int> Day14::CalculateAddresses(const string& s)
{
	vector<string> addresses;
	string cpy(s);

	size_t idx = cpy.find('X');
	while (idx != string::npos)
	{
		cpy[idx] = 0;

		idx = s.find('X');
	}
	//replace(cpy.begin(), cpy.end(), 'X', '0');
	//addresses.push_back(static_cast<int>(bitset<36>(targetBits).to_ullong()));

	return vector<int>();
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

ull Day14::PartOne()
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
			unsigned long long target = stoull(static_cast<string>(match[2]), &size, 0);

			// write the value to the memory address
			_memory[stoi(match[1])] = ApplyMask(target);
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
			unsigned long long target = stoull(static_cast<string>(match[2]), &size, 0);

			// write the value to the memory address
			string mask = ApplyMask2(_mask, stoi(match[1]));
			const auto possibilities = Possibilities(mask);

			vector<ull> addresses;
			for (const string& possibility : possibilities)
			{
				string cpy(possibility);
				addresses.push_back(bitset<36>(cpy).to_ullong());
			}
			
			//const auto& addresses = CalculateAddresses(mask);
			for(const auto address : addresses)
				_memory[address] = target;
		}
		else cout << "Invalid instruction: " << instruction << endl;
	}

	ull count = 0;

	for (const auto& pair : _memory)
		count += pair.second;

	return count;
}
