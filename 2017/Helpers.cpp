#include "Helpers.h"

std::vector<std::string> Helpers::Split(const std::string &s, char delim)
{
	std::vector<std::string> elems;
	Split(s, delim, std::back_inserter(elems));
	return elems;
}