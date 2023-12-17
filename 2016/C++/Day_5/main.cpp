#include <iostream>
#include <string>
#include "md5.h"

int main()
{
	const std::string input = "uqwqemis";
	//const std::string input = "abc";

	bool found = false;

	int resultCount = 0;
	std::string answer = "////////";

	//for (int i = 3231928; resultCount < 8; ++i) // Start at this number, found be doing earlier tests
	for (int i = 4515058; resultCount < 8; ++i) // Start at this number, found be doing earlier tests
	{
		std::string nrInput(input + std::to_string(i));

		const std::string hashed = md5(nrInput);
		if (hashed.substr(0, 5).compare("00000") == 0)
		{
			int idx = static_cast<int>(hashed[5] - '0');
			if (idx >= 0 && idx < 8 && answer[idx] == '/')
			{
				answer[idx] = hashed[6];
				std::cout << "Found letter " << hashed[6] << " and put it on index " << idx << " of answer " << " resultcount: (" << resultCount << ')' << std::endl;
				std::cout << "Answer looks like this now: ~" << answer << "~" << std::endl;
				++resultCount;
			}
		}
	}
	std::cout << answer << std::endl;
	std::cin.get();
	return 0;
}