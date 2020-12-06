#include <iostream>
#include "Helpers.h"
#include "Day6.h"

using namespace std;

int main()
{
    auto day = Day6();
    auto stopwatch = Helpers::StopWatch();

    stopwatch.Start();
    cout << "Solution to day 6, part one: " << day.PartOne();
    stopwatch.Stop();
    cout << stopwatch.Formatted() << endl;

    stopwatch.Reset();

    stopwatch.Start();
    cout << "Solution to day 6, part two: " << day.PartTwo();
    stopwatch.Stop();
    cout << stopwatch.Formatted() << endl;
}
