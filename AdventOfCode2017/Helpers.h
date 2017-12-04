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

	std::vector<std::string> Split(const std::string &s, char delim);
}