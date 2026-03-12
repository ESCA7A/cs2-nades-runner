using System;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
            string libraryPath = Directory.GetCurrentDirectory().TrimEnd(Path.DirectorySeparatorChar);
            string libraryRootName = new DirectoryInfo(libraryPath).Name;
            string counterStrikePath = new DirectoryInfo(libraryPath).Parent?.FullName ?? throw new Exception("counter strike directory not found");
            string counterStrikeDirectoryName = (new DirectoryInfo(libraryPath).Parent?.Name) ?? throw new Exception("counter strike directory not found");
            string from = Path.Combine(libraryPath, "training", "example.training.cfg");
            string to = Path.Combine(counterStrikePath, "training.cfg");
            if (File.Exists(to))
            {
                throw new Exception($"File training.cfg is exists in {to}. Replace the name or remove them");
            }
            File.Copy(from, to, true);
            string txt = File.ReadAllText(to);
            txt = txt.Replace("exec training/main.cfg;", $"exec {libraryRootName}/training/main.cfg;");
            txt = txt.Replace("exec_async training/helloworld.cfg;", $"exec_async {libraryRootName}/training/helloworld.cfg;");
            File.WriteAllText(to, txt);
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("=== UNHANDLED EXCEPTION ===");
            Console.WriteLine(ex.GetType().FullName);
            Console.WriteLine(ex.Message);
            Console.WriteLine();
            Console.WriteLine("StackTrace:");
            Console.WriteLine(ex.StackTrace);
            Console.ResetColor();

            System.IO.File.AppendAllText("crash.log",
                $"[{DateTime.UtcNow:u}] {ex}{Environment.NewLine}");

            Console.WriteLine("\nPress any key to exit …");
            Console.ReadKey();
        }
    }
}