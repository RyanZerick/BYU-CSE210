using System;

public class Entry
{
    private string _date;
    private string _prompt;
    private string _text;

    public Entry(string date, string prompt, string text)
    {
        _date = date;
        _prompt = prompt;
        _text = text;
    }

    public void Display()
    {
        Console.WriteLine($"{_date} | Prompt: {_prompt}");
        Console.WriteLine(_text);
        Console.WriteLine(new string('-', 40));
    }

    public string GetDate() => _date;
    public string GetPrompt() => _prompt;
    public string GetText() => _text;
}
