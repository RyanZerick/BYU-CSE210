// -------------------------------------------------------
// MODULO 1 : Imports + Main
// -------------------------------------------------------

using System;
using static System.Console; // to avoid writing Console 

// args by default
var opts = Cli.ParseArgs(args, defaults: new Options(Min: 1, Max: 10, Tries: 0, Seed: null, Help: false, ManualSecret: true));
if (opts.Help)
{
    Cli.PrintHelp();
    return;
}

// if args are different
var error = Cli.Validate(opts);
if (error is not null)
{
    ForegroundColor = ConsoleColor.Red;
    Error.WriteLine($"Error: {error}");
    ResetColor();
    Environment.ExitCode = 2;
    return;
}

// runs under option 
Game.Run(opts);

// -------------------------------------------------------
// MODULO 2 : CLI 
// -------------------------------------------------------

// recibe inputs from console
public readonly record struct Options(int Min, int Max, int Tries, int? Seed, bool Help, bool ManualSecret = true);


public static class Cli
{
    public static Options ParseArgs(string[] args, Options defaults)
    {
        int min = defaults.Min;
        int max = defaults.Max;
        int tries = defaults.Tries;
        int? seed = defaults.Seed;
        bool help = defaults.Help;
        bool manualSecret = defaults.ManualSecret;

        for (int i = 0; i < args.Length; i++)
        {
            var a = args[i];

            // for --arg=value or --arg value
            string? v = null; // value
            if (a.Contains('='))
            {
                var parts = a.Split('=', 2, StringSplitOptions.TrimEntries);
                a = parts[0];
                v = parts[1];
            }
            else if (i + 1 < args.Length && !args[i + 1].StartsWith("--"))
            {
                v = args[i + 1];
                i++; // advance in the args.
            }

            switch (a.ToLowerInvariant())
            {
                case "--min": min = StrictInt(v); break;
                case "--max": max = StrictInt(v); break;
                case "--tries": tries = StrictInt(v); break;
                case "--seed": seed = StrictInt(v); break;
                case "--help":
                case "-h":
                case "/?":
                    help = true; break;
                case "--manual-secret": manualSecret = false; break;
            }
        }
        return new Options(min, max, tries, seed, help);

        static int StrictInt(string? s)
        {
            if (string.IsNullOrWhiteSpace(s)) throw new ArgumentException("missing value");
            if (s.Contains('.') || s.Contains(',')) throw new ArgumentException("integers only (no decimals)");
            if (!int.TryParse(s, out var n)) throw new ArgumentException("invalid integer");
            return n;
        }
    }

    public static string? Validate(Options o)
    {
        if (o.Min < 0) return "min cannot be negative";
        if (o.Max < o.Min) return "max must be >= min";
        if (o.Tries < 0) return "tries must be >= 0 (0 = unlimited)";
        return null;
    }

    public static void PrintHelp()
    {
        WriteLine("""
        Guess Game (CLI only for options)
        Usage:
          dotnet run -- --min 1 --max 10 --tries 0 --seed 123
        Flags:
          --min   <int>   lower bound (default 1)
          --max   <int>   upper bound inclusive (default 10)
          --tries <int>   0 = unlimited (default 0)
          --seed  <int>   RNG seed (optional, reproduce runs)
          --help          show this help
          --ManualSecret  <bool> 2 playes or 1 player
        """);
    }
}

// -------------------------------------------------------
// MODULO 3 : GAME CORE
// -------------------------------------------------------

public static class Game
{
    // Observaciones (funciones):
    // 3.1 Run: (1) pide secreto, (2) loop de guesses, (3) da pistas por umbral, (4) imprime tries.
    // 3.2 ReadSecret: valida entero no negativo y dentro del rango.
    // 3.3 ReadGuess: valida entero no negativo y dentro del rango.
    public static void Run(Options o)
    {
        WriteLine("What is the magic number?");
        int secret;
        if (o.ManualSecret)
        {
            secret = ReadSecret("> ", o.Min, o.Max);
        }
        else
        {
            var rng = o.Seed is int s ? new Random(s) : new Random();
            secret = rng.Next(o.Min, o.Max + 1);
            WriteLine($"I'm thinking of a number between {o.Min} and {o.Max}.");
        }

        int attempt = 0;
        while (true)
        {
            int guess = ReadGuess("What is your guess? ", o.Min, o.Max);
            attempt++;

            if (guess == secret)
            {
                WriteLine("You guessed it!");
                WriteLine($"tries: {attempt}");
                return;
            }

            int diff = Math.Abs(secret - guess);
            bool needHigher = guess < secret;

            string hint = diff switch
            {
                > 1000 => needHigher ? "Too Much Higher" : "Too Much Lower",
                > 100  => needHigher ? "Too Higher"      : "Too Lower",
                > 10   => needHigher ? "Higher"          : "Lower",
                _      => needHigher ? "higher"          : "lower"
            };

            WriteLine(hint);
        }
    }

    private static int ReadSecret(string prompt, int min, int max)
    {
        while (true)
        {
            Write(prompt);
            var s = ReadLine();

            if (string.IsNullOrWhiteSpace(s)) { WriteLine("Enter a value."); continue; }
            if (s.Contains('.') || s.Contains(',')) { WriteLine("Integers only (no decimals)."); continue; }
            if (!int.TryParse(s, out int n)) { WriteLine("Enter a valid integer."); continue; }
            if (n < 0) { WriteLine("Negative numbers are not allowed."); continue; }
            if (n < min || n > max) { WriteLine($"Out of range. Use {min}..{max}."); continue; }
            return n;
        }
    }

    public static int ReadGuess(string prompt, int min, int max)
    {
        while (true)
        {
            Write(prompt);
            var s = ReadLine();

            if (string.IsNullOrWhiteSpace(s)) { WriteLine("Enter a value."); continue; }
            if (s.Contains('.') || s.Contains(',')) { WriteLine("Integers only (no decimals)."); continue; }
            if (!int.TryParse(s, out int n)) { WriteLine("Enter a valid integer."); continue; }
            if (n < 0) { WriteLine("Negative numbers are not allowed."); continue; }
            if (n < min || n > max) { WriteLine($"Out of range. Use {min}..{max}."); continue; }
            return n;
        }
    }
}