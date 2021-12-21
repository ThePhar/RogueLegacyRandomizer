/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators.
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using System;
using System.IO;
using System.Windows.Forms;

namespace RogueCastle
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var flag = true;
            if (flag)
            {
                if (LevelEV.CREATE_RETAIL_VERSION)
                {
                    LevelEV.SHOW_ENEMY_RADII = false;
                    LevelEV.ENABLE_PLAYER_DEBUG = false;
                    LevelEV.UNLOCK_ALL_ABILITIES = false;
                    LevelEV.TESTROOM_LEVELTYPE = GameTypes.LevelType.CASTLE;
                    LevelEV.TESTROOM_REVERSE = false;
                    LevelEV.RUN_TESTROOM = false;
                    LevelEV.SHOW_DEBUG_TEXT = false;
                    LevelEV.LOAD_TITLE_SCREEN = false;
                    LevelEV.LOAD_SPLASH_SCREEN = true;
                    LevelEV.SHOW_SAVELOAD_DEBUG_TEXT = false;
                    LevelEV.DELETE_SAVEFILE = false;
                    LevelEV.CLOSE_TESTROOM_DOORS = false;
                    LevelEV.RUN_TUTORIAL = false;
                    LevelEV.RUN_DEMO_VERSION = false;
                    LevelEV.DISABLE_SAVING = false;
                    LevelEV.RUN_CRASH_LOGS = true;
                    LevelEV.WEAKEN_BOSSES = false;
                    LevelEV.ENABLE_BACKUP_SAVING = true;
                    LevelEV.ENABLE_OFFSCREEN_CONTROL = false;
                    LevelEV.SHOW_FPS = false;
                    LevelEV.SAVE_FRAMES = false;
                }

                if (LevelEV.DEBUG_MODE)
                {
                    // LevelEV.SHOW_DEBUG_TEXT = true;
                    // LevelEV.WEAKEN_BOSSES = true;
                    // LevelEV.SHOW_FPS = true;
                    LevelEV.DELETE_SAVEFILE = true;
                    LevelEV.DISABLE_SAVING = true;
                }
                
                if (args.Length == 1 && !LevelEV.CREATE_RETAIL_VERSION)
                {
                    using (var game = new Game(args[0]))
                    {
                        LevelEV.RUN_TESTROOM = true;
                        LevelEV.DISABLE_SAVING = true;
                        game.Run();
                        return;
                    }
                }
                if (LevelEV.RUN_CRASH_LOGS)
                {
                    try
                    {
                        using (var game2 = new Game())
                        {
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
                    game3.Run();
                }
            }
        }
    }
}