// -------------------------------------------------------
// MODULE 1 : Imports
// -------------------------------------------------------
using System;
using System.Collections.Generic;

// -------------------------------------------------------
// MODULE 2 : Class Resume
// Purpose: holds a name and a list of jobs; prints summary.
// -------------------------------------------------------
public class Resume
{
    // Notes:
    // - Display iterates all jobs.

    public string _name = "";
    public List<Job> Jobs = new List<Job>();

    public Resume() { }
    public Resume(string name) => _name = name ?? "";

    // -------------------------------------------------------
    // MODULE 3 : Functions
    // -------------------------------------------------------
    public void Display()
    {
        Console.WriteLine($"Name: {_name}");
        Console.WriteLine("Jobs:");
        foreach (var job in Jobs)
        {
            job.Display();
        }
    }
}
