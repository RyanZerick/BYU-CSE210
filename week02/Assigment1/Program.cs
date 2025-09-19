// -------------------------------------------------------
// MODULE 1 : Imports
// -------------------------------------------------------
using System;
using System.Collections.Generic;
using static System.Console;

// -------------------------------------------------------
// MODULE 2 : Domain model - Job
// Encapsulates a single job with validation.
// -------------------------------------------------------
public sealed class Job
{
    // Observations:
    // 1) Public getters with private setters for encapsulation.
    // 2) Validate strings and year range in constructor.
    // 3) Immutable from outside (no public setters).

    public string Company { get; private set; }
    public string Title   { get; private set; }
    public int StartYear  { get; private set; }
    public int EndYear    { get; private set; }

    public Job(string company, string title, int startYear, int endYear)
    {
        if (string.IsNullOrWhiteSpace(company))
            throw new ArgumentException("Company is required.", nameof(company));
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required.", nameof(title));
        if (startYear > endYear)
            throw new ArgumentException("StartYear cannot be greater than EndYear.");

        Company  = company.Trim();
        Title    = title.Trim();
        StartYear = startYear;
        EndYear   = endYear;
    }

    // Purpose:
    // 1) Quick, reusable console display for debugging or simple outputs.
    public void Display()
    {
        WriteLine($"Company: {Company} | Title: {Title} | {StartYear}–{EndYear}");
    }

    public override string ToString() => $"{Title} @ {Company} ({StartYear}–{EndYear})";
}

// -------------------------------------------------------
// MODULE 3 : Aggregate - Resume
// Holds a person's name and their list of jobs. Prevents overlap.
// -------------------------------------------------------
public sealed class Resume
{
    // Observations:
    // 1) Store jobs privately; expose read-only view.
//  2) AddJob enforces “no overlap” rule; touching borders is allowed.
//  3) Sorted by StartYear for stable output.

    private readonly List<Job> _jobs = new();

    public string Name { get; private set; } = "";

    public Resume(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));
        Name = name.Trim();
    }

    public IReadOnlyList<Job> Jobs => _jobs.AsReadOnly();

    // Purpose:
    // 1) Validate new job does not overlap any existing job.
    // 2) Keep jobs sorted by StartYear.
    public void AddJob(Job job)
    {
        foreach (var existing in _jobs)
        {
            if (Overlaps(existing, job))
            {
                throw new InvalidOperationException(
                    $"The period {job.StartYear}–{job.EndYear} overlaps with {existing.StartYear}–{existing.EndYear}."
                );
            }
        }
        _jobs.Add(job);
        _jobs.Sort((a, b) => a.StartYear.CompareTo(b.StartYear));
    }

    // Purpose:
    // 1) Interval overlap helper: we allow touching borders.
    private static bool Overlaps(Job a, Job b)
    {
        // No overlap if a ends before (or exactly at) b starts, or vice versa.
        bool noOverlap = a.EndYear <= b.StartYear || b.EndYear <= a.StartYear;
        return !noOverlap;
    }
}

// -------------------------------------------------------
// MODULE 4 : Presentation - Summary styles and printer
// Reusable formatter for resume output (CLI today, GUI tomorrow).
// -------------------------------------------------------
public enum SummaryStyle
{
    Simple,
    Boxed
}

public static class ResumePrinter
{
    // Observations:
    // 1) Dynamic inner width based on longest line (title or job lines).
    // 2) Proper centering and padding; no overflow.
//  3) Easy to extend with wrapping if needed in the future.

    public static void Print(Resume resume, SummaryStyle style = SummaryStyle.Boxed)
    {
        switch (style)
        {
            case SummaryStyle.Simple:
                PrintSimple(resume);
                break;
            case SummaryStyle.Boxed:
            default:
                PrintBoxed(resume);
                break;
        }
    }

    private static void PrintSimple(Resume resume)
    {
        Console.WriteLine($"Resume for: {resume.Name}");
        foreach (var j in resume.Jobs)
            Console.WriteLine($" - {j.Title} @ {j.Company} ({j.StartYear}–{j.EndYear})");
    }

    private static void PrintBoxed(Resume resume)
    {
        // 1) Build lines to measure the true max width.
        string title = $"Resume for {resume.Name}";
        var contentLines = new List<string>();

        if (resume.Jobs.Count == 0)
        {
            contentLines.Add("No jobs registered.");
        }
        else
        {
            foreach (var j in resume.Jobs)
            {
                contentLines.Add($"{j.Title} @ {j.Company}");
                contentLines.Add($"Years: {j.StartYear}–{j.EndYear}");
            }
        }

        // 2) Compute inner width (no borders). Keep a reasonable minimum (30).
        int maxContent = contentLines.Count > 0 ? contentLines.Max(s => s.Length) : 0;
        int innerWidth = Math.Max(30, Math.Max(title.Length, maxContent));

        // 3) Optional: clamp to console width to avoid wrapping by the terminal itself
        try
        {
            int maxConsoleInner = Math.Max(20, Console.WindowWidth - 4); // room for "║  ║"
            innerWidth = Math.Min(innerWidth, maxConsoleInner);
        }
        catch
        {
            // ignore if not supported (e.g., redirected output)
        }

        string bar = new string('═', innerWidth + 2); // +2 for side spaces

        // 4) Draw box
        Console.WriteLine($"╔{bar}╗");
        Console.WriteLine($"║ {Center(title, innerWidth)} ║");
        Console.WriteLine($"╠{bar}╣");

        if (contentLines.Count == 0)
        {
            Console.WriteLine($"║ {Center("No jobs registered.", innerWidth)} ║");
        }
        else
        {
            for (int i = 0; i < contentLines.Count; i++)
            {
                Console.WriteLine($"║ {Pad(contentLines[i], innerWidth)} ║");
                // Insert separator after each job pair (2 lines per job), but not after the last block
                bool isLastLine = (i == contentLines.Count - 1);
                bool endOfJobBlock = (i % 2 == 1); // line 1:title@company, line 2:years
                if (endOfJobBlock && !isLastLine)
                    Console.WriteLine($"╟{bar}╢");
            }
        }

        Console.WriteLine($"╚{bar}╝");
    }

    private static string Center(string text, int width)
    {
        if (text.Length >= width) return text.Substring(0, width);
        int left = (width - text.Length) / 2;
        int right = width - text.Length - left;
        return new string(' ', left) + text + new string(' ', right);
    }

    private static string Pad(string text, int width)
    {
        if (text.Length >= width) return text.Substring(0, width);
        return text + new string(' ', width - text.Length);
    }
}

// -------------------------------------------------------
// MODULE 5 : CLI Helpers
// Small utilities for input validation and yes/no prompts.
// -------------------------------------------------------
public static class Cli
{
    // Observations:
    // 1) Strong input guards to avoid crashes.
    // 2) Separate helpers keep Main clean and testable.

    public static string ReadNonEmpty(string prompt)
    {
        while (true)
        {
            Write($"{prompt}: ");
            string? s = ReadLine();
            if (!string.IsNullOrWhiteSpace(s))
                return s.Trim();
            WriteLine("  -> Please enter a non-empty value.");
        }
    }

    public static int ReadInt(string prompt, int? min = null, int? max = null)
    {
        while (true)
        {
            Write($"{prompt}: ");
            string? raw = ReadLine();
            if (int.TryParse(raw, out int value))
            {
                if (min.HasValue && value < min.Value)
                {
                    WriteLine($"  -> Must be >= {min.Value}.");
                    continue;
                }
                if (max.HasValue && value > max.Value)
                {
                    WriteLine($"  -> Must be <= {max.Value}.");
                    continue;
                }
                return value;
            }
            WriteLine("  -> Please enter a valid integer.");
        }
    }

    public static bool ReadYesNo(string prompt, bool defaultYes = true)
    {
        string suffix = defaultYes ? "[Y/n]" : "[y/N]";
        while (true)
        {
            Write($"{prompt} {suffix}: ");
            string? raw = ReadLine();
            if (string.IsNullOrWhiteSpace(raw)) return defaultYes;
            raw = raw.Trim().ToLowerInvariant();
            if (raw is "y" or "yes") return true;
            if (raw is "n" or "no") return false;
            WriteLine("  -> Please answer y/yes or n/no.");
        }
    }
}

// -------------------------------------------------------
// MODULE 6 : Main (orchestration)
// Asks user's name and jobs, enforces no-overlap, prints summary.
// -------------------------------------------------------
public static class Program
{
    // Internal steps:
    // 1) Ask for user's name.
    // 2) Loop: ask for company, title, start year, end year; try to add job.
    // 3) On overlap/validation errors, show message and let user retry or skip.
    // 4) Ask printing style and display the formatted summary.

    public static void Main(string[] args)
    {
        Title = "Resume Builder - Week02.Assignment1";

        string name = Cli.ReadNonEmpty("Enter your name");
        var resume = new Resume(name);

        WriteLine();
        WriteLine("Let's enter your jobs. Years may touch (e.g., 2020–2022 and 2022–2024), but they cannot overlap.");

        while (true)
        {
            WriteLine();
            WriteLine("Add a new job:");

            string company = Cli.ReadNonEmpty("  Company");
            string title   = Cli.ReadNonEmpty("  Title");

            // Optional: constrain year ranges as you wish (e.g., 1900..2100)
            int start = Cli.ReadInt("  Start year", min: 1900, max: 2100);
            int end   = Cli.ReadInt("  End year",   min: start, max: 2100);

            try
            {
                var job = new Job(company, title, start, end);
                resume.AddJob(job);
                WriteLine("  -> Job added.");
            }
            catch (Exception ex)
            {
                ForegroundColor = ConsoleColor.Yellow;
                WriteLine($"  -> Could not add job: {ex.Message}");
                ResetColor();
            }

            if (!Cli.ReadYesNo("Do you want to add another job?", defaultYes: false))
                break;
        }

        WriteLine();
        var useBox = Cli.ReadYesNo("Use boxed summary style?", defaultYes: true);
        var style = useBox ? SummaryStyle.Boxed : SummaryStyle.Simple;

        WriteLine();
        ResumePrinter.Print(resume, style);

        WriteLine();
        WriteLine("Done. Press ENTER to exit.");
        ReadLine();
    }
}
