// 
//  Rogue Legacy Randomizer - Program.cs
//  Last Modified 2022-04-05
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RogueCastle
{
    internal static class Program
    {
        public static Game Game { get; private set; }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool AllocConsole();

        [STAThread]
        private static void Main()
        {
            if (LevelENV.RunConsole)
            {
                AllocConsole();
            }

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
                    var str = DateTime.Now.ToString("dd-mm-yyyy_HH-mm-ss");
                    var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    var path = Path.Combine(folderPath, LevelENV.GameName);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    var path2 = Path.Combine(folderPath, LevelENV.GameName, "CrashLog_" + str + ".log");
                    using (var sw = new StreamWriter(path2, false))
                    {
                        sw.WriteLine(ex.ToString());
                    }

                    MessageBox.Show(
                        "Rogue Legacy has run into a situation it cannot recover from and needs to close. If you are" +
                        " not a Randomizer developer, please reach out to Phar#4444 in Discord and include the crash logs so " +
                        "they can fix this issue.\n\nIf you're Phar, stop breaking things and fix it.\n\n" +
                        "You can find these log files in:\n" + path,
                        "Unexpected Exception Occurred", MessageBoxButtons.OK, MessageBoxIcon.Hand);

                    return;
                }
            }

            // Run without crash logger.
            Game = game;
            game.Run();
        }
    }
}
