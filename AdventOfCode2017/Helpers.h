#pragma once
#include <vector>
#include <sstream>
#include <string>
#include <iterator>

namespace Helpers
{
	template<typename T>
	void split(const std::string &s, char delim, T result)
	{
		std::stringstream ss(s);
		std::string item;
		while (getline(ss, item, delim))
			*(result++) = stoi(item);
	}

	std::vector<int> split(const std::string &s, char delim);
}