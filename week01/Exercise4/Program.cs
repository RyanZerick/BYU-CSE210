// -------------------------------------------------------
// MODULO 1 : Imports
// -------------------------------------------------------

using System;
using System.Collections.Generic;
using static System.Console;


WriteLine("Enter a list of numbers, type 0 when finished.");
List<int> numbers = ReadNumbersUntilZero();

if (numbers.Count == 0)
{
    WriteLine("No numbers were entered.");
}
else
{
    int sum = SumNumbers(numbers);
    double avg = Average(numbers);
    int max = MaxNumber(numbers);

    // Aditional
    int? smallestPos = SmallestPositive(numbers);
    List<int> sorted = SortedAscending(numbers);

    WriteLine($"  The sum is: {sum}");
    WriteLine($"  The average is: {avg}");
    WriteLine($"  The largest number is: {max}");
    WriteLine(smallestPos is int sp
        ? $"  The smallest positive number is: {sp}"
        : "  There is no positive number.");
    WriteLine($"  The sorted list is: {string.Join(", ", sorted)}");
}

// -------------------------------------------------------
// MODULO 2 : INPUTS AND VALIDATION
// LECTURE AND CONTROL FUNCTIONS
// -------------------------------------------------------

static List<int> ReadNumbersUntilZero()
{
    
    var data = new List<int>();
    while (true)
    {
        Write("  Enter number: ");
        string? s = ReadLine();

        if (string.IsNullOrWhiteSpace(s))
        {
            WriteLine("  Please enter a value.");
            continue;
        }
        if (s.Contains('.') || s.Contains(','))
        {
            WriteLine("  Integers only (no decimals).");
            continue;
        }
        if (!int.TryParse(s, out int n))
        {
            WriteLine("  Enter a valid integer.");
            continue;
        }

        if (n == 0) break;      // 0 => END

        data.Add(n);
    }
    return data;
}

// -------------------------------------------------------
// MODULO 3 : FUNCTIONS (SUM, AVERAGE, MAX, SMALL, SORT)
// -------------------------------------------------------

static int SumNumbers(List<int> nums)
{
    int acc = 0;
    foreach (var n in nums) acc += n;
    return acc;
}

static double Average(List<int> nums)
{
    if (nums.Count == 0) return 0.0;
    return (double)SumNumbers(nums) / nums.Count;
}

static int MaxNumber(List<int> nums)
{
    int m = nums[0];
    for (int i = 1; i < nums.Count; i++)
        if (nums[i] > m) m = nums[i];
    return m;
}

static int? SmallestPositive(List<int> nums)
{
    int? best = null;
    foreach (var n in nums)
        if (n > 0 && (!best.HasValue || n < best.Value))
            best = n;
    return best;
}

// manage a copy to dont change the original
static List<int> SortedAscending(List<int> nums)
{
    var copy = new List<int>(nums);
    copy.Sort();
    return copy;
}