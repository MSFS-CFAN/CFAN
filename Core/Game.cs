
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Win32;
using System.Text;
using System.Threading;

namespace Core
{
    public static class Game
    {
        private const string configfile = @"\Packages\Microsoft.FlightSimulator_8wekyb3d8bbwe\LocalCache\UserCfg.opt";
        private const string subkey = @"Software\Microsoft\Windows\CurrentVersion\App Paths\FlightSimulator.exe";
        private const string packagepathkey = @"InstalledPackagesPath";

        public static Version Version { get; private set; }
        public static bool SuccessLoad { get; private set; }
        public static string PackageFolder { get; set; }

        public static bool Load()
        {
            // Set Successload on true. If something goes wrong along the load process, it should be put to false
            SuccessLoad = true;
            // Here we'd need to load the game's properties, like the community folder, get the game's version

            // community folder, file is made when clicking on 'update' after selecting the download folder for packages on the first run of the simulator:
            //   file: [appdata\Local]\Packages\Microsoft.FlightSimulator_8wekyb3d8bbwe\LocalCache\UserCfg.opt
            //   line: InstalledPackagesPath "F:\msfs_packages\subfolder_whynot"

            if (PackageFolder.Length == 0) // Check if not manually set
            {
                PackageFolder = ReadUserConfig();
            }

            if (!Directory.Exists(PackageFolder)) // Validate directory
            {
                SuccessLoad = false;
            }


            return SuccessLoad;
        }

        private static string ReadUserConfig()
        {
            string ret = "";
            string appdata = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string userconfig = appdata + configfile;

            if (File.Exists(userconfig))
            {
                string[] userconfigcontent = File.ReadAllLines(userconfig);
                foreach (string line in userconfigcontent)
                {
                    if (line.StartsWith(packagepathkey))
                    {
                        ret = line.Split(' ')[1].Trim('"');
                    }
                }

            }
            return ret;
        }

        private static Version GetVersion()
        {
            // Get the game version for the Windows Store type installation
            // reg keys with possible info:
            // Computer\HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\App Paths\FlightSimulator.exe
            // Computer\HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Local Settings\Software\Microsoft\Windows\CurrentVersion\AppModel\PackageRepository\Packages\Microsoft.FlightSimulator_1.7.12.0_x64__8wekyb3d8bbwe
            // Computer\HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\GamingServices\GameConfig\Microsoft.FlightSimulator_1.7.12.0_x64__8wekyb3d8bbwe
            // Computer\HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\GamingServices\GameConfig\Microsoft.FlightSimulator_1.7.12.0_x64__8wekyb3d8bbwe\Executable\FlightSimulator.exe
            // Computer\HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\FlightSimulator.exe
            // Computer\HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\App Paths\FlightSimulator.exe

            RegistryKey regkey = Registry.CurrentUser.OpenSubKey(subkey);
            if (regkey != null) // try windows store version
            {
                string value = regkey.GetValue("").ToString();
                string[] parts = value.Split('_', StringSplitOptions.RemoveEmptyEntries);
                // parts[0] : C:\Program Files\WindowsApps\Microsoft.FlightSimulator
                // parts[1] : 1.7.12.0
                // parts[3] : x64
                // parts[4] : 8wekyb3d8bbwe\FlightSimulator.exe
                Version ret = new Version(parts[1]);
                return ret;
            } else
            {
                throw new Exception("there's sowthing wrong, in the neighborhood, or we just don't have support for steam yet");
            }
            
        }
    }
}
