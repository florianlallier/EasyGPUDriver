using System;

namespace EasyGPUDriver
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "EasyGPUDriver";

            Version currentVersion = GetCurrentVersion();
            Version lastVersion = GetLastVersion();

            Console.WriteLine($"Current version: {currentVersion}");
            Console.WriteLine($"Last version: {lastVersion}");

            if (currentVersion.CompareTo(lastVersion) == -1)
            {
                Console.Write("\nNew version available! Do you want to download it? [y/N] ");

                ConsoleKeyInfo cki = Console.ReadKey();
                Console.WriteLine();

                if (cki.Key.ToString() == "Y")
                {
                    DownloadDriver();
                }
            }
            else
            {
                Console.WriteLine("\nUp-to-date.");
            }

            Console.Write("\nPress any key to exit... ");
            Console.ReadKey();
        }

        private static Version GetCurrentVersion()
        {
            string version = "0.0";

            // TODO

            return new Version(version);
        }

        private static Version GetLastVersion()
        {
            string version = "0.0";

            // TODO

            return new Version(version);
        }

        private static void DownloadDriver()
        {
            // TODO
        }
    }
}
