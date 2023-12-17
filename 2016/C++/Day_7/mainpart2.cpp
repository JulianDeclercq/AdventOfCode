#include <iostream>
#include <string>
#include <vector>
#include <fstream>
#include <algorithm>

void GetABAs(const std::string& str, std::vector<std::string>& abas)
{
	// Break whole string in 3sized strings
	std::vector<std::string> parts;
	for (size_t i = 0; i < str.size() - 2; ++i)
	{
		parts.push_back(str.substr(i, 3));
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
	std::string bracketText = (str.substr(1, str.size() - 2));

	std::vector<std::string> babs;
	for (size_t i = 0; i < bracketText.size() - 2; ++i)
	{
		std::string possibleBab = bracketText.substr(i, 3);

		// Check if it is a bab
		if (possibleBab[0] == possibleBab[2] && possibleBab[0] != possibleBab[1])
		{
			babs.push_back(possibleBab);
		}
	}

	// For all existing aba's, check if a bab exists
	// Aba is outside brackets, bab is inside
	for (const std::string& bab : babs)
	{
		for (const std::string& aba : abas)
		{
			// Check if the aba and bab are compatible
			if ((bab[0] == aba[1]) && (bab[1] == aba[0]) && (bab[2] == aba[1]))
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

	// Recursion, for brackets within brackets
	ExtractParts(str.substr(closePos), parts, enclParts);
}

bool SSLCompatible(const std::vector<std::string>& parts, const std::vector<std::string>& enclParts)
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
	// Open the file
	std::ifstream file("input.txt");

	// File fail check
	if (file.fail())
	{
		std::cout << "Failed to open inputfile\n";
		return -1;
	}

	int validCtr = 0;
	std::string line;
	std::vector<std::string> parts, enclParts;

	while (std::getline(file, line))
	{
		// Clear the vectors
		parts.clear();
		enclParts.clear();

		// Puts the strings between brackets in enclParts and the others in parts
		ExtractParts(line, parts, enclParts);

		// Check if the IP addresses are SSL compatible
		if (SSLCompatible(parts, enclParts))
		{
			++validCtr;
		}
	}

	std::cout << "Answer is: " << validCtr << std::endl;
	file.close();

	return 0;
}