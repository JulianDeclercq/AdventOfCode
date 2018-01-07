#include "Day16.h"
#include <time.h>

int main()
{
	clock_t clkStart = clock();

	Day16().Part1();
	cout << "Part 1 execution time: " << (clock() - clkStart) << "ms" << endl;

	// Reset the clock
	clkStart = clock();

	Day16().Part2();
	cout << "Part 2 execution time: " << (clock() - clkStart) << "ms" << endl;

	return 0;
}