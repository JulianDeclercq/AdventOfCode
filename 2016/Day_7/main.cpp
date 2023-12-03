#include <iostream>
#include <string>
#include <vector>
#include <fstream>
#include <algorithm>

bool OneCharString(const std::string& s)
{
	return s.find_first_not_of(s[0]) == std::string::npos;
}

bool CheckABBA(const std::string& str, bool encl)
{
	std::string cpy(str);
	if (encl) // if string was an enclosed string, get the brackets out
	{
		cpy = (str.substr(1, str.size() - 2));
	}

	// Break whole string in 4sized strings
	std::vector<std::string> parts;
	for (size_t i = 0; i < cpy.size() - 3; ++i)
	{
		parts.push_back(cpy.substr(i, 4));
	}

	for (const std::string& part : parts)
	{
		const std::string sub = part.substr(0, 2);
		std::string rev(part.substr(2, 2));
		std::reverse(rev.begin(), rev.end());
		if (sub.compare(rev) == 0 && !OneCharString(sub))
		{
			return true;
		}
	}
	return false;
}

void GetABAs(const std::string& str, std::vector<std::string>& abas)
{
	std::string cpy(str);

	// Break whole string in 3sized strings
	std::vector<std::string> parts;
	for (size_t i = 0; i < cpy.size() - 2; ++i)
	{
		parts.push_back(cpy.substr(i, 3));
	}

	for (const std::string& part : parts)
	{
		if ((part[0] == part[2]) && (part[0] != part[1]))
		{
			abas.push_back(part);
		}
	}
}

bool CheckBABs(const std::string& str, const std::vector<std::string>& abas)
{
	// Remove brackets
	std::string cpy = (str.substr(1, str.size() - 2));

	std::vector<std::string> parts;
	for (size_t i = 0; i < cpy.size() - 2; ++i)
	{
		parts.push_back(cpy.substr(i, 3));
	}

	// For all existing aba's, check if a bab exists
	for (const std::string& part : parts)
	{
		// Optimisation: if the part is not a possible aba then don't even check it
		if (part[0] != part[2])
		{
			continue;
		}
		for (const std::string& aba : abas)
		{
			if ((cpy[0] == aba[1]) && (cpy[2] == aba[1]) && (cpy[1] == aba[0]))
			{
				return true;
			}
		}
	}
	return false;
}

void ExtractParts(const std::string& str, std::vector<std::string>& parts, std::vector<std::string>& enclParts)
{
	size_t findPos = 0, closePos = 0;
	findPos = str.substr(closePos).find('[');

	// Recursion ending condition
	if (findPos == str.npos)
	{
		parts.push_back(str);
		return;
	}

	closePos = str.find(']') + 1; // no check if invalid, assume every '[' has a closing ']'

	parts.push_back(str.substr(0, findPos));
	enclParts.push_back(str.substr(findPos, (closePos - findPos)));

	ExtractParts(str.substr(closePos), parts, enclParts);
}

bool ValidIPv7(const std::vector<std::string>& parts, const std::vector<std::string>& enclParts)
{
	bool hasABBA = false, failedABBA = false;

	for (const std::string& part : parts)
	{
		if (CheckABBA(part, false))
		{
			hasABBA = true;
			break;
		}
	}

	for (const std::string& part : enclParts)
	{
		if (CheckABBA(part, true))
		{
			failedABBA = true;
			break;
		}
	}
	return(hasABBA && (!failedABBA));
}

bool ValidIPv7Part2(const std::vector<std::string>& parts, const std::vector<std::string>& enclParts)
{
	std::vector<std::string> abas;
	for (const std::string& part : parts)
	{
		GetABAs(part, abas);
	}

	for (const std::string& part : enclParts)
	{
		if (CheckBABs(part, abas))
		{
			return true;
		}
	}
	return false;
}

int main()
{
	std::ifstream file("input2.txt");
	//std::ifstream file("example.txt");

	if (file.fail())
	{
		std::cout << "Failed to open inputfile\n";
		return -1;
	}

	std::string line, answer;
	int validCtr = 0;
	while (std::getline(file, line))
	{
		std::vector<std::string> parts, enclParts;
		std::string cpy(line);

		ExtractParts(cpy, parts, enclParts);

		//if (ValidIPv7(parts, enclParts))
		if (ValidIPv7Part2(parts, enclParts))
		{
			//std::cout << "Valid line: " << cpy << std::endl;
			++validCtr;
		}
		else
		{
			//std::cout << "Invalid line: " << cpy << std::endl;
		}
	}

	std::cout << validCtr << " valid IPv7 addresses\n";

	file.close();

	std::cin.get();
	return 0;
}