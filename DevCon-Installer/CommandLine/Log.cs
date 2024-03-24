using System;

namespace DevConInstaller.CommandLine
{
    internal class Logger
    {
        public static bool Timestamps = false;
        public static void Write(string text, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
        {
            Console.ForegroundColor = foregroundColor ?? Console.ForegroundColor;
            Console.BackgroundColor = backgroundColor ?? Console.BackgroundColor;
            Console.Write(text);
            Console.ResetColor();
        }

        public static void Log(string message = "", ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
        {
            if (string.IsNullOrEmpty(message))
            {
                Console.WriteLine();
            }
            else
            {
                if (Timestamps)
                {
                    Write($"{DateTime.Now.ToLongTimeString()}: ", Console.ForegroundColor);
                }
                Write(message, foregroundColor, backgroundColor);
                Console.WriteLine();
            }
        }

        public static void Info(string message)
        {
            Log(message, ConsoleColor.Blue, Console.BackgroundColor);
        }
        public static void Success(string message)
        {
            Log(message, ConsoleColor.Green, Console.BackgroundColor);
        }
        public static void Warning(string message)
        {
            Log(message, ConsoleColor.Yellow, Console.BackgroundColor);
        }
        public static void Error(string message)
        {
            Log($"[ERROR] {message}", ConsoleColor.Red, Console.BackgroundColor);
        }
    }
}