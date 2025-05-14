// RogueLegacyRandomizer - Program.cs
// Last Modified 2023-07-30 8:58 AM by
//
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
//
// Original Source - © 2011-2018, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using RogueLegacy.Util;

namespace RogueLegacy
{
    internal static class Program
    {
        public static Game Game { get; private set; }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool AllocConsole();

        [STAThread]
        private static void Main(string[] args)
        {
            // Check for specific command line args.
            foreach (var arg in args)
            {
                switch (arg)
                {
                    case "--no_logging":
                        LevelENV.RunCrashLogs = false;
                        break;
                    case "--no_saving":
                        LevelENV.DisableSaving = true;
                        LevelENV.EnableBackupSaving = false;
                        break;
                    case "--console":
                        LevelENV.RunConsole = true;
                        break;
                    case "--skip_intro":
                        LevelENV.LoadSplashScreen = false;
                        break;
                    case "--debug":
                        LevelENV.RunConsole = true;
                        LevelENV.ShowFps = true;
                        LevelENV.ShowArchipelagoStatus = true;
                        LevelENV.ShowDebugText = true;
                        // LevelENV.ShowEnemyRadii = true;
                        LevelENV.LoadSplashScreen = false;
                        break;
                    case "--debug_save_load":
                        LevelENV.ShowSaveLoadDebugText = true;
                        break;
                    case "--god_mode":
                        LevelENV.EnablePlayerDebug = true;
                        break;
                    case "--all_abilities":
                        LevelENV.UnlockAllAbilities = true;
                        break;
                    case "--weak_bosses":
                        LevelENV.WeakenBosses = true;
                        break;
                    default:
                        Console.WriteLine($"Unknown argument: {arg}");
                        break;
                }
            }

            if (LevelENV.RunConsole)
            {
                AllocConsole();
            }

            using var logger = new ConsoleLogger();
            using var game = new Game();

            if (LevelENV.RunCrashLogs)
            {
                try
                {
                    Game = game;
                    game.Run();

                    return;
                }
                catch (Exception ex)
                {
                    var str = DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss");
                    var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    var folderPath = Path.Combine(appDataPath, LevelENV.GameName, "CrashLog_" + str);
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    var exceptionLog = Path.Combine(folderPath, "exception.log");
                    using (var sw = new StreamWriter(exceptionLog, false))
                    {
                        sw.WriteLine(ex.ToString());
                    }

                    var consoleLog = Path.Combine(folderPath, "console.log");
                    using (var sw = new StreamWriter(consoleLog, false))
                    {
                        sw.WriteLine(logger.Log);
                    }

                    // Create zip and delete folder.
                    ZipFile.CreateFromDirectory(folderPath, folderPath + ".zip");
                    Directory.Delete(folderPath, true);

                    MessageBox.Show(
                        "Rogue Legacy Randomizer has run into a situation it cannot recover from and needs to " +
                        "close. If you are not Phar, please submit a #bug-report in the Archipelago Discord and include the " +
                        "crash logs so it can fix this issue.\n\nIf you are Phar, stop breaking everything.\n\n" +

                        "These log files are located at:\n" + folderPath + ".zip\n\n" +
                        
                        "Discord URL: discord.gg/archipelago",

                        "RLR: Unhandled Exception Occurred", MessageBoxButtons.OK, MessageBoxIcon.Hand);

                    return;
                }
            }

            // Run without crash logger.
            Game = game;
            game.Run();
        }
    }
}