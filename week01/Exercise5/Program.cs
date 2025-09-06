// -------------------------------------------------------
// MODULO 1 : IMPORTS + MAIN (Top-level)
// -------------------------------------------------------

using System;
using static System.Console;

DisplayWelcome();
string name = PromptUserName();
int favNum  = PromptUserNumber();
int squared = SquareNumber(favNum);
DisplayResult(name, squared);


// -------------------------------------------------------
// MODULO 2 : FUNCTIONS
// -------------------------------------------------------

static void DisplayWelcome()
{
    WriteLine("Welcome to the Program!");
}

static string PromptUserName()
{
    Write("Please enter your name: ");
    var s = ReadLine();
    return string.IsNullOrWhiteSpace(s) ? "" : s.Trim();
}

static int PromptUserNumber()
{
    while (true)
    {
        Write("Please enter your favorite number: ");
        var s = ReadLine();

        if (string.IsNullOrWhiteSpace(s))
        {
            WriteLine("Please enter a value.");
            continue;
        }
        if (s.Contains('.') || s.Contains(','))
        {
            WriteLine("Integers only (no decimals).");
            continue;
        }
        if (!int.TryParse(s, out int n))
        {
            WriteLine("Enter a valid integer.");
            continue;
        }
        return n;
    }
}

static int SquareNumber(int x)
{
    return x * x;
}

static void DisplayResult(string userName, int squared)
{
    WriteLine($"{userName}, the square of your number is {squared}");
}