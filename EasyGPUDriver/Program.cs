using HtmlAgilityPack;
using System;
using System.Configuration;
using System.Linq;
using System.Management;
using System.Net;

namespace EasyGPUDriver
{
    internal class Program
    {
        private static string downloadURL;

        static void Main(string[] args)
        {
            Console.Title = "EasyGPUDriver";

            Version currentVersion = GetCurrentVersion();
            Version lastVersion = GetLastVersion(psid: 98, pfid: 756, osid: 57, lid: 12, dtcid: 1, ctk: 0);

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

        private static Version GetLastVersion(int psid, int pfid, int osid, int lid, int dtcid, int ctk)
        {
            string version = "0.0";

            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load($"https://www.nvidia.com/Download/processDriver.aspx?psid={psid}&pfid={pfid}&osid={osid}&lid={lid}&dtcid={dtcid}&ctk={ctk}");
            string versionPageURL = doc.DocumentNode.OuterHtml;

            doc = web.Load(versionPageURL);
            string driverVersion = doc.DocumentNode.SelectSingleNode("//td[@id='tdVersion']").InnerText;
            version = driverVersion.Trim().Substring(0, 6); // "497.29 WHQL" format to "497.29" format
            string downloadPageURL = "https://www.nvidia.com" + doc.DocumentNode.SelectSingleNode("//a[@id='lnkDwnldBtn']").Attributes["href"].Value.Trim();

            doc = web.Load(downloadPageURL);
            downloadURL = "https:" + doc.DocumentNode.SelectSingleNode("//td/a").Attributes["href"].Value.Trim();

            return new Version(version);
        }

        private static void DownloadDriver()
        {
            string downloadPath = ConfigurationManager.AppSettings.Get("downloadPath");

            Console.Write("\nDownloading... ");

            WebClient client = new WebClient();
            client.DownloadFile(new Uri(downloadURL), downloadPath + downloadURL.Split('/').Last());

            Console.WriteLine("Done.");
        }
    }
}
