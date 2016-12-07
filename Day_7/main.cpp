#include <iostream>
#include <string>
#include <vector>
#include <fstream>
#include <algorithm>

bool HasABBA(const std::string& str)
{
	std::string cpy(str.substr(1, str.size() - 2));
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
		if (sub.compare(rev) == 0)
		{
			return true;
		}
	}
	return false;
}

int main()
{
	//	std::ifstream file("input.txt");
	std::ifstream file("example.txt");

	if (file.fail())
	{
		std::cout << "Failed to open inputfile\n";
		return -1;
	}

	std::string line;
	while (std::getline(file, line))
	{
		std::vector<std::string> enclosedParts, nonenclosedParts;
		size_t findPos = 0, closePos = 0;
		bool registeredFirst = false;
		std::string cpy(line);

		do
		{
			findPos = cpy.substr(closePos).find('[');
			if (!registeredFirst)
			{
				nonenclosedParts.push_back(cpy.substr(0, findPos));
				registeredFirst = true;
			}
			if (findPos == std::string::npos)
			{
				break;
			}
			closePos = cpy.find(']') + 1; // no check if invalid, assume every '[' has a closing ']', add 1 so it is included in the next line's substr

			std::string nonecnl = cpy.substr(closePos, findPos);
			nonenclosedParts.push_back(nonecnl);

			enclosedParts.push_back(cpy.substr(findPos, (closePos - findPos)));
		} while (findPos != std::string::npos);

		for (const std::string& part : enclosedParts)
		{
			HasABBA(part);
		}
		std::cout << "LINE\n";
	}

	file.close();

	std::cin.get();
	return 0;
}