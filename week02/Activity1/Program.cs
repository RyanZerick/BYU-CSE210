// -------------------------------------------------------
// MODULE 1 : Imports
// -------------------------------------------------------
using System;

// -------------------------------------------------------
// MODULE 2 : Main Program (CLI)
// Purpose: read user data, build Resume, display summary.
// -------------------------------------------------------
public static class Program
{
    // Steps:
    // 1) Read name.
    // 2) Loop to read jobs.
    // 3) Print Resume.

    public static void Main(string[] args)
    {
        Console.Title = "Resume - Activity1";

        Console.Write("Enter your name: ");
        var name = Console.ReadLine() ?? "";
        var resume = new Resume(name);

        Console.WriteLine();
        Console.WriteLine("Enter your jobs. Leave Company empty to finish.");

        while (true)
        {
            Console.Write("  Company: ");
            var company = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(company))
                break;

            Console.Write("  Title: ");
            var title = Console.ReadLine() ?? "";

            int start = ReadInt("  Start year");
            int end = ReadInt("  End year");

            var job = new Job
            {
                _company = company,
                _jobTitle = title,
                _startYear = start,
                _endYear = end
            };

            // Example “dot” edit:
            // job.JobTitle = "Software Engineer";

            resume.Jobs.Add(job);

            Console.Write("Add another job? [y/N]: ");
            var more = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();
            if (more != "y" && more != "yes") break;

            Console.WriteLine();
        }

        Console.WriteLine();
        resume.Display();

        Console.WriteLine();
        Console.WriteLine("Done. Press ENTER to exit.");
        Console.ReadLine();
    }

    // -------------------------------------------------------
    // MODULE 3 : Helpers (CLI)
    // -------------------------------------------------------
    private static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write($"{prompt}: ");
            var raw = Console.ReadLine();
            if (int.TryParse(raw, out int value))
                return value;
            Console.WriteLine("  -> Please enter a valid integer.");
        }
    }
}
