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

int main()
{
	std::ifstream file("input.txt");
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

		if (ValidIPv7(parts, enclParts))
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