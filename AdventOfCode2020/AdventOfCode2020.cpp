#include <iostream>
#include "Helpers.h"
#include "Day7.h"

using namespace std;

int main()
{
    auto day = Day7();
    auto stopwatch = Helpers::StopWatch();

    stopwatch.Start();
    cout << "Solution to day 7, part one: " << day.PartOne();
    stopwatch.Stop();
    cout << stopwatch.Formatted() << endl;

    stopwatch.Start();
    cout << "Solution to day 7, part two: " << day.PartTwo();
    stopwatch.Stop();
    cout << stopwatch.Formatted() << endl;
}
