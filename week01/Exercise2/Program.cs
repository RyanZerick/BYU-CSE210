// -------------------------------------------------------
// MODULO 1 : Imports
// -------------------------------------------------------

using System;
using static System.Console; // to avoid writing Console 

// -------------------------------------------------------
// MODULO 1 : Main
// -------------------------------------------------------

WriteLine("Grading Program");
int score = AskScore("Enter score (0-100): ");
string grade = Grading(score);
WriteLine($"Score {score} -> Grade {grade}");

// -------------------------------------------------------
// MODULO 3 : Functions
// -------------------------------------------------------
// 1. recibe a number
// 2. process if the number is greater or lower
// 3. returns a Letter
static string Grading(int number)
{
    if (number < 0 || number > 100)
        throw new ArgumentOutOfRangeException(nameof(number), "Score must be between 0 and 100");

    return number switch
    {
        >= 97 => "A+",
        >= 93 => "A",
        >= 90 => "A-",
        >= 87 => "B+",
        >= 83 => "B",
        >= 80 => "B-",
        >= 77 => "C+",
        >= 73 => "C",
        >= 70 => "C-",
        >= 67 => "D+",
        >= 63 => "D",
        >= 60 => "D-",
        _ => "F"
    };
}


static int AskScore(string prompt)
{
    while (true)
    {
        Write(prompt);
        string? message = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(message) && (message.Contains('.') || message.Contains(',')))
        {
            WriteLine("Error: use a interger (decimals not allowed)");
            continue;
        }
        if (!int.TryParse(message, out int value))
        {
            WriteLine("Error: enter a valid number");
            continue;
        }
        if (value < 0 || value > 100)
        {
            WriteLine("Error: number must be between 0 and 100");
            continue;
        }

        return value;
    }
}