// -------------------------------------------------------
// MODULO 1 : Imports
// -------------------------------------------------------

using System;

Console.WriteLine("Proving things");

string name = Ask("What is your first name? ");
string last_name = Ask("What is your last name? ");

Console.WriteLine($"Your name is {last_name}, {name} {last_name}");

// -------------------------------------------------------
// MODULO 2 : Function
// -------------------------------------------------------

static string Ask(string prompt)
{
    Console.Write(prompt);
    var message = Console.ReadLine();
    return string.IsNullOrWhiteSpace(message) ? "" : message.Trim();
};