// -------------------------------------------------------
// MODULO 1 : Imports
// -------------------------------------------------------

using System;

// -------------------------------------------------------
// MODULO 2 : main
// -------------------------------------------------------

string name = Ask("What is your first name? ");
string last_name = Ask("What is your last name? ");

Console.WriteLine($"Your name is {last_name}, {name} {last_name}");

// -------------------------------------------------------
// MODULO 3 : Function
// -------------------------------------------------------

static string Ask(string prompt)
{
    Console.Write(prompt);
    var message = Console.ReadLine();
    return string.IsNullOrWhiteSpace(message) ? "" : message.Trim();
};