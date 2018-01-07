#include "Day16.h"
#include <time.h>

int main()
{
	clock_t clkStart = clock();

	Day16().Part2();
	cout << "Execution time OPTIMIZED: " << (clock() - clkStart) << "ms" << endl;

	return 0;
}