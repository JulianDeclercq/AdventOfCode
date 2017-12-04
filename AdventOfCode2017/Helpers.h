#pragma once
#include <vector>
#include <sstream>
#include <string>
#include <iterator>

namespace Helpers
{
	template<typename T>
	void Split(const std::string &s, char delim, T result)
	{
		std::stringstream ss(s);
		std::string item;
		while (std::getline(ss, item, delim))
			*(result++) = item;
	}

	template<typename T>
	std::vector<T> Split(const std::string &s, char delim)
	{
		std::vector<int> elems;
		split(s, delim, std::back_inserter(elems));
		return elems;
	}

	// Integer specific methods
	void SplitToInteger(const std::string &s, char delim, std::back_insert_iterator<std::vector<int>> result);
	std::vector<int> SplitToInteger(const std::string &s, char delim);
}