using System;
using System.Collections.Generic;

public class PromptGenerator
{
    private List<string> _prompts = new List<string>
    {
        "Who was the most interesting person you interacted with today?",
        "What was the best part of your day?",
        "How did you see the hand of the Lord in your life today?",
        "What was the strongest emotion you felt today?",
        "If you had one thing you could do over today, what would it be?",
        "What was your favorite meal today and why?",
        "What is something new you learn today?",
        "What was the most challeging thing today to overcome?",
        "Which song or music let you thinking today, and why?"
    };

    private Random _random = new Random();

    public string GetRandomPrompt()
    {
        if (_prompts.Count == 0) return "";
        int i = _random.Next(_prompts.Count);
        return _prompts[i];
    }

    public void AddPrompt(string prompt)
    {
        if (!string.IsNullOrWhiteSpace(prompt)) _prompts.Add(prompt);
    }
}
