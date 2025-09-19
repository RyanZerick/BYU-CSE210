// -------------------------------------------------------
// MODULE 1 : Imports
// -------------------------------------------------------
using System;

// -------------------------------------------------------
// MODULE 2 : Class Job
// Purpose: simple editable job record and display.
// -------------------------------------------------------
public class Job
{
    // Notes:
    // - Public fields on purpose (editable with dot syntax).
    // - Display prints: Title (Company) start-end

    public string _company = "";
    public string _jobTitle = "";
    public int _startYear;
    public int _endYear;

    public Job() { }

    public Job(string company, string jobTitle, int startYear, int endYear)
    {
        _company = company ?? "";
        _jobTitle = jobTitle ?? "";
        _startYear = startYear;
        _endYear = endYear;
    }

    // -------------------------------------------------------
    // MODULE 3 : Functions
    // -------------------------------------------------------
    public void Display()
    {
        Console.WriteLine($"{_jobTitle} ({_company}) {_startYear}-{_endYear}");
    }
}
