#include <iostream>
#include <string>
#include <fstream>
#include <map>
#include <vector>
#include <algorithm>

std::vector<std::pair<char, int>> SortMapByValue(const std::map<char, int>& m)
{
	std::vector<std::pair<char, int>> sorted;
	sorted.reserve(m.size());
	for (const std::pair<char, int>& p : m)
	{
		sorted.push_back(p);
	}

	//	std::sort(sorted.begin(), sorted.end(), [](const std::pair<char, int>& lhs, const std::pair<char, int>& rhs) { return lhs.second < rhs.second; }); // part 1
	std::sort(sorted.begin(), sorted.end(), [](const std::pair<char, int>& lhs, const std::pair<char, int>& rhs) { return lhs.second < rhs.second; }); // part 2

	return sorted;
}

int main()
{
	std::ifstream file("input.txt");
	//std::ifstream file("example.txt");

	std::vector<std::map<char, int>> charCount;
	charCount.resize(8);

	std::string line;
	while (std::getline(file, line))
	{
		for (size_t i = 0; i < line.size(); ++i)
		{
			charCount[i][line[i]]++;
		}
	}

	std::string answer;
	for (const auto& m : charCount)
	{
		auto result = SortMapByValue(m);
		answer.push_back(result.front().first);
	}

	std::cout << "Answer is: " << answer << std::endl;

	return 0;
}