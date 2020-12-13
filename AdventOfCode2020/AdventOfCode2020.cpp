#include <iostream>
#include "Helpers.h"
#include "Day13.h"

using namespace std;

int main()
{
    auto day = Day13();
    auto stopwatch = Helpers::StopWatch();

    stopwatch.Start();
    day.ParseInput();
    stopwatch.Stop();
    cout << "Parsed input\t\t\t" << stopwatch.Formatted() << endl;

    stopwatch.Start();
    cout << "Solution to day 13, part one: " << day.PartOne();
    stopwatch.Stop();
    cout << stopwatch.Formatted() << endl;

    stopwatch.Start();
    cout << "Solution to day 13, part two: " << day.PartTwo();
    stopwatch.Stop();
    cout << stopwatch.Formatted() << endl << endl;
}
