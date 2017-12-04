#include "Helpers.h"

std::vector<int> Helpers::SplitInteger(const std::string &s, char delim)
{
	std::vector<int> elems;
	SplitInteger(s, delim, std::back_inserter(elems));
	return elems;
}

void Helpers::SplitInteger(const std::string &s, char delim, std::back_insert_iterator<std::vector<int>> result)
{
	std::stringstream ss(s);
	std::string item;
	while (std::getline(ss, item, delim))
		*(result++) = std::stoi(item);
}