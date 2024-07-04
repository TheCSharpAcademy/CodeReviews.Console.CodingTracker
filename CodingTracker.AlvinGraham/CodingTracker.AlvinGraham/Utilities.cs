﻿namespace CodingTracker;

internal static class Utilities
{

	internal static void ClearScreen(string message)
	{
		Console.Clear();
		Console.WriteLine(message);
		Console.WriteLine("--------------------------------------");
	}

	internal static string titleText = @"
░░      ░░░░      ░░░       ░░░        ░░   ░░░  ░░░      ░░░░░░░░    
▒  ▒▒▒▒  ▒▒  ▒▒▒▒  ▒▒  ▒▒▒▒  ▒▒▒▒▒  ▒▒▒▒▒    ▒▒  ▒▒  ▒▒▒▒▒▒▒▒▒▒▒▒▒    
▓  ▓▓▓▓▓▓▓▓  ▓▓▓▓  ▓▓  ▓▓▓▓  ▓▓▓▓▓  ▓▓▓▓▓  ▓  ▓  ▓▓  ▓▓▓   ▓▓▓▓▓▓▓    
█  ████  ██  ████  ██  ████  █████  █████  ██    ██  ████  ███████    
██      ████      ███       ███        ██  ███   ███      ████████    
                                                                      
░        ░░       ░░░░      ░░░░      ░░░  ░░░░  ░░        ░░       ░░
▒▒▒▒  ▒▒▒▒▒  ▒▒▒▒  ▒▒  ▒▒▒▒  ▒▒  ▒▒▒▒  ▒▒  ▒▒▒  ▒▒▒  ▒▒▒▒▒▒▒▒  ▒▒▒▒  ▒
▓▓▓▓  ▓▓▓▓▓       ▓▓▓  ▓▓▓▓  ▓▓  ▓▓▓▓▓▓▓▓     ▓▓▓▓▓      ▓▓▓▓       ▓▓
████  █████  ███  ███        ██  ████  ██  ███  ███  ████████  ███  ██
████  █████  ████  ██  ████  ███      ███  ████  ██        ██  ████  █
                                                                      
";

}
