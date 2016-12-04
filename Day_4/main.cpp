#include <iostream>
#include <string>
#include <fstream>
#include <vector>
#include <regex>
#include <map>
#include <algorithm>

std::vector<std::pair<char, int>> SortMapByValue(const std::map<char, int>& m)
{
	std::vector<std::pair<char, int>> sorted;
	sorted.reserve(m.size());
	for (const std::pair<char, int>& p : m)
	{
		sorted.push_back(p);
	}

	std::sort(sorted.begin(), sorted.end(), [](const std::pair<char, int>& lhs, const std::pair<char, int>& rhs) { return lhs.second > rhs.second; });

	return sorted;
}

bool Checksum(const std::map<char, int>& m, const std::string& checkSum)
{
	auto sortedMap = SortMapByValue(m);
	std::string sortedCheckSum = "";
	for (size_t i = 0; i < checkSum.length(); ++i)
	{
		sortedCheckSum += sortedMap[i].first;
	}

	return (sortedCheckSum.compare(checkSum) == 0);
}

int main()
{
	std::ifstream file("input.txt");

	std::string line;
	std::vector<std::string> lines;
	while (std::getline(file, line))
	{
		lines.push_back(line);
	}
	file.close();

	//std::regex re("[^\\d]*(\\d*)\\[(\\w*)\\]");
	std::regex re("(\\d*)\\[(\\w*)\\]");
	std::smatch match;

	int sectorIDSum = 0;
	for (std::string& str : lines)
	{
		int sectorID = 0;
		std::string checkSum = "";
		if (std::regex_search(str, match, re))
		{
			//std::cout << "Prefix is: " << match.prefix().length() << std::endl;
			std::string l = match[1].str();
			sectorID = std::stoi(l);
			checkSum = match[2].str();

			str = str.substr(0, match.prefix().length());
			std::map<char, int> charCount;
			for (const char c : str)
			{
				if (isalpha(c))
				{
					++charCount[c];
				}
			}

			if (Checksum(charCount, checkSum))
			{
				sectorIDSum += sectorID;
			}
		}
	}

	std::cout << "Sum of sectorIDs is: " << sectorIDSum << std::endl;
	return 0;
}