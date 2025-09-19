using System;

public class Program
{
    public static void Main()
    {
        var journal = new Journal();
        var prompts = new PromptGenerator();
        bool running = true;

        while (running)
        {
            Console.WriteLine();
            Console.WriteLine("Journal App");
            Console.WriteLine("1) Write");
            Console.WriteLine("2) Display");
            Console.WriteLine("3) Save (text)");
            Console.WriteLine("4) Load (text)");
            Console.WriteLine("5) Quit");
            Console.WriteLine("6) Filter by date (YYYY-MM-DD)");
            Console.WriteLine("7) Add prompt");
            Console.WriteLine("8) Save (JSON)");
            Console.WriteLine("9) Load (JSON)");
            Console.Write("Choose an option: ");
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                {
                    string prompt = prompts.GetRandomPrompt();
                    if (string.IsNullOrWhiteSpace(prompt))
                    {
                        Console.WriteLine("No prompts available.");
                        break;
                    }
                    Console.WriteLine($"Prompt: {prompt}");
                    Console.Write("Your entry: ");
                    string text = Console.ReadLine() ?? "";
                    string date = DateTime.Now.ToString("yyyy-MM-dd");
                    journal.AddEntry(new Entry(date, prompt, text));
                    Console.WriteLine("Entry added.");
                    break;
                }
                case "2":
                    journal.Display();
                    break;

                case "3":
                {
                    Console.Write("Filename to save (e.g., journal.txt): ");
                    string? path = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(path)) { Console.WriteLine("Invalid filename."); break; }
                    journal.SaveToFile(path);
                    break;
                }

                case "4":
                {
                    Console.Write("Filename to load (e.g., journal.txt): ");
                    string? path = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(path)) { Console.WriteLine("Invalid filename."); break; }
                    journal.LoadFromFile(path);
                    break;
                }

                case "5":
                    running = false;
                    break;

                case "6":
                {
                    Console.Write("Date (YYYY-MM-DD): ");
                    string? date = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(date)) { Console.WriteLine("Invalid date."); break; }
                    journal.DisplayByDate(date);
                    break;
                }

                case "7":
                {
                    Console.Write("New prompt: ");
                    string? p = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(p)) { Console.WriteLine("Invalid prompt."); break; }
                    prompts.AddPrompt(p);
                    Console.WriteLine("Prompt added.");
                    break;
                }

                case "8":
                {
                    Console.Write("JSON file to save (e.g., journal.json): ");
                    string? path = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(path)) { Console.WriteLine("Invalid filename."); break; }
                    journal.SaveToJson(path);
                    break;
                }

                case "9":
                {
                    Console.Write("JSON file to load (e.g., journal.json): ");
                    string? path = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(path)) { Console.WriteLine("Invalid filename."); break; }
                    journal.LoadFromJson(path);
                    break;
                }

                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }
}
