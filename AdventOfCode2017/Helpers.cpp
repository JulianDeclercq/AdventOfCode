#include "Helpers.h"

std::vector<int> Helpers::split(const std::string &s, char delim)
{
	std::vector<int> elems;
	split(s, delim, std::back_inserter(elems));
	return elems;
}