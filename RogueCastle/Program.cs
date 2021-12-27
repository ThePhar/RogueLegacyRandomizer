// 
// RogueLegacyArchipelago - Program.cs
// Last Modified 2021-12-27
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using RogueCastle.Structs;

namespace RogueCastle
{
    internal static class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool AllocConsole();
        public static Game Game;

        private static void Main(string[] args)
        {
            // Start our console window.
            AllocConsole();

            if (LevelENV.CreateRetailVersion)
            {
                LevelENV.ShowEnemyRadii = false;
                LevelENV.EnablePlayerDebug = false;
                LevelENV.UnlockAllAbilities = false;
                LevelENV.TestRoomLevelType = LevelType.Castle;
                LevelENV.TestRoomReverse = false;
                LevelENV.RunTestRoom = false;
                LevelENV.ShowDebugText = false;
                LevelENV.LoadTitleScreen = false;
                LevelENV.LoadSplashScreen = true;
                LevelENV.ShowSaveLoadDebugText = true;
                LevelENV.DeleteSaveFile = false;
                LevelENV.CloseTestRoomDoors = false;
                LevelENV.RunTutorial = false;
                LevelENV.RunDemoVersion = false;
                LevelENV.DisableSaving = false;
                LevelENV.RunCrashLogs = true;
                LevelENV.WeakenBosses = false;
                LevelENV.EnableBackupSaving = true;
                LevelENV.EnableOffscreenControl = false;
                LevelENV.ShowFps = false;
                LevelENV.SaveFrames = false;
            }

            if (args.Length == 1 && !LevelENV.CreateRetailVersion)
            {
                using (var game = new Game(args[0]))
                {
                    Game = game;
                    LevelENV.RunTestRoom = true;
                    LevelENV.DisableSaving = true;
                    game.Run();
                    return;
                }
            }
            if (LevelENV.RunCrashLogs)
            {
                try
                {
                    using (var game2 = new Game())
                    {
                        Game = game2;
                        game2.Run();
                    }
                    return;
                }
                catch (Exception ex)
                {
                    var str = DateTime.Now.ToString("dd-mm-yyyy_HH-mm-ss");
                    var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    var path = Path.Combine(folderPath, "Rogue Legacy Archipelago");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    var path2 = Path.Combine(folderPath, "Rogue Legacy Archipelago", "CrashLog_" + str + ".log");
                    using (var streamWriter = new StreamWriter(path2, false))
                    {
                        streamWriter.WriteLine(ex.ToString());
                    }
                    MessageBox.Show(
                        "Your game has encountered an error and a crash log has been generated.  Please view the Readme.txt file for more information.",
                        "Game Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
            }
            using (var game3 = new Game())
            {
                Game = game3;
                game3.Run();
            }
        }

        public static List<T> Clone<T>(this List<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
    }
}
