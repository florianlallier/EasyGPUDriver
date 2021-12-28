using HtmlAgilityPack;
using System;
using System.Management;

namespace EasyGPUDriver
{
    internal class Program
    {
        private static string downloadURL;

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

            foreach (ManagementObject mo in new ManagementObjectSearcher("SELECT * FROM Win32_VideoController").Get())
            {
                if (mo["Description"].ToString().ToLower().Contains("nvidia"))
                {
                    string driverVersion = mo["DriverVersion"].ToString();
                    version = driverVersion.Substring(driverVersion.Length - 6, 6).Replace(".", string.Empty).Insert(3, "."); // "30.0.14.9729" format to "497.29" format

                    break;
                }
            }

            return new Version(version);
        }

        private static Version GetLastVersion()
        {
            string version = "0.0";

            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load("http://www.nvidia.com/Download/driverResults.aspx/184729/fr");

            string driverVersion = doc.DocumentNode.SelectSingleNode("//td[@id='tdVersion']").InnerText;
            version = driverVersion.Trim().Substring(0, 6); // "497.29 WHQL" format to "497.29" format

            string downloadPageURL = "https://www.nvidia.com" + doc.DocumentNode.SelectSingleNode("//a[@id='lnkDwnldBtn']").Attributes["href"].Value.Trim();
            doc = web.Load(downloadPageURL);
            downloadURL = "https:" + doc.DocumentNode.SelectSingleNode("//td/a").Attributes["href"].Value.Trim();

            return new Version(version);
        }

        private static void DownloadDriver()
        {
            // TODO
        }
    }
}
