using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

public class Journal
{
    private List<Entry> _entries = new List<Entry>();

    public void AddEntry(Entry entry)
    {
        _entries.Add(entry);
    }

    public void Display()
    {
        if (_entries.Count == 0)
        {
            Console.WriteLine("(No entries)");
            return;
        }
        foreach (var e in _entries) e.Display();
    }

    public void DisplayByDate(string date)
    {
        var items = _entries.Where(e => e.GetDate() == date);
        bool any = false;
        foreach (var e in items) { e.Display(); any = true; }
        if (!any) Console.WriteLine("(No entries for that date)");
    }

    public void SaveToFile(string path)
    {
        const string SEP = "~|~";
        var lines = _entries.Select(e => string.Join(SEP, new[] { e.GetDate(), e.GetPrompt(), e.GetText() }));
        File.WriteAllLines(path, lines);
        Console.WriteLine($"Saved {_entries.Count} entries to {path}");
    }

    public void LoadFromFile(string path)
    {
        const string SEP = "~|~";
        if (!File.Exists(path)) { Console.WriteLine("File not found."); return; }

        var tmp = new List<Entry>();
        foreach (var line in File.ReadAllLines(path))
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            var parts = line.Split(SEP);
            if (parts.Length >= 3)
            {
                string date = parts[0];
                string prompt = parts[1];
                string text = string.Join(SEP, parts.Skip(2));
                tmp.Add(new Entry(date, prompt, text));
            }
            else
            {
                Console.WriteLine("Skipping malformed line.");
            }
        }
        _entries.Clear();
        _entries.AddRange(tmp);
        Console.WriteLine($"Loaded {_entries.Count} entries from {path}");
    }

    public void SaveToJson(string path)
    {
        var dto = _entries.Select(e => new EntryDto
        {
            Date = e.GetDate(),
            Prompt = e.GetPrompt(),
            Text = e.GetText()
        }).ToList();

        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(dto, options);
        File.WriteAllText(path, json);
        Console.WriteLine($"Saved {_entries.Count} entries to {path}");
    }

    public void LoadFromJson(string path)
    {
        if (!File.Exists(path)) { Console.WriteLine("File not found."); return; }

        try
        {
            string json = File.ReadAllText(path);
            var dto = JsonSerializer.Deserialize<List<EntryDto>>(json) ?? new List<EntryDto>();
            var tmp = new List<Entry>();
            foreach (var d in dto)
            {
                if (d is null) continue;
                string date = d.Date ?? "";
                string prompt = d.Prompt ?? "";
                string text = d.Text ?? "";
                tmp.Add(new Entry(date, prompt, text));
            }
            _entries.Clear();
            _entries.AddRange(tmp);
            Console.WriteLine($"Loaded {_entries.Count} entries from {path}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Invalid JSON: {ex.Message}");
        }
    }

    private class EntryDto
    {
        public string? Date { get; set; }
        public string? Prompt { get; set; }
        public string? Text { get; set; }
    }
}
