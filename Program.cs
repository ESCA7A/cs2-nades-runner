using System;
using System.IO;

class Program
{
    public const string _trainingDirectoryName = "training";
    public const string _trainingConfigName = "training.cfg";
    public const string _exampleTrainingConfigName = "example.training.cfg";

    private static readonly DirectoryInfo _libraryRootDirectoryPath = new DirectoryInfo(Directory.GetCurrentDirectory());
    private static readonly DirectoryInfo _trainingDirectoryPath = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), _trainingDirectoryName));
    private static readonly DirectoryInfo _counterStrikeDirectoryPath = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent 
        ?? throw new Exception("counter strike directory not found");
    
    static void Main()
    {
        try
        {
            copyConfigToCounterStrikeScope();
            copyTrainingDirToCounterStrikeScope();
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

            Console.WriteLine("\nSomething with wrong. Please add issue https://github.com/ESCA7A/training-cs2-nades …");
            Console.ReadKey();
        }
    }

    private static void copyTrainingDirToCounterStrikeScope()
    {
        string inCounterStrikeTrainingDirectoryPath = Path.Combine(_counterStrikeDirectoryPath.FullName, _trainingDirectoryName);
    
        if (Directory.Exists(Path.Combine(inCounterStrikeTrainingDirectoryPath, _trainingDirectoryName)))
        {
            throw new Exception();
        }

        CopyDirectory(_trainingDirectoryPath.FullName, inCounterStrikeTrainingDirectoryPath, true);
    }

    private static void copyConfigToCounterStrikeScope()
    {
        string exampleTrainingCfgPath = Path.Combine(_trainingDirectoryPath.FullName, _exampleTrainingConfigName);
        string trainingCfgPath = Path.Combine(_counterStrikeDirectoryPath.FullName, _trainingConfigName);
        
        if (File.Exists(trainingCfgPath))
        {
            throw new Exception($"File `training.cfg` is exists in {trainingCfgPath}. Replace the name or remove them");
        }

        File.Copy(exampleTrainingCfgPath, trainingCfgPath, true);
    }

    private static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
    {
        DirectoryInfo dir = new DirectoryInfo(sourceDir);

        if (!dir.Exists)
            throw new DirectoryNotFoundException($"Исходная папка не найдена: {dir.FullName}");

        Directory.CreateDirectory(destinationDir);

        foreach (FileInfo file in dir.GetFiles())
        {
            string targetFilePath = Path.Combine(destinationDir, file.Name);
            file.CopyTo(targetFilePath, true);
        }

        if (recursive)
        {
            foreach (DirectoryInfo subDir in dir.GetDirectories())
            {
                string targetSubDir = Path.Combine(destinationDir, subDir.Name);
                CopyDirectory(subDir.FullName, targetSubDir, true);
            }
        }
    }
}