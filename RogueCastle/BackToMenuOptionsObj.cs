// 
//  Rogue Legacy Randomizer - BackToMenuOptionsObj.cs
//  Last Modified 2022-01-25
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using RogueCastle.Enums;
using RogueCastle.Screens;

namespace RogueCastle
{
    public class BackToMenuOptionsObj : OptionsObj
    {
        public BackToMenuOptionsObj(OptionsScreen parentScreen) : base(parentScreen, "Quit to Title Screen") { }

        public override bool IsActive
        {
            get { return base.IsActive; }
            set
            {
                base.IsActive = value;
                if (IsActive)
                {
                    var rCScreenManager = m_parentScreen.ScreenManager as RCScreenManager;
                    rCScreenManager.DialogueScreen.SetDialogue("Back to Menu");
                    rCScreenManager.DialogueScreen.SetDialogueChoice("ConfirmTest1");
                    rCScreenManager.DialogueScreen.SetConfirmEndHandler(this, "GoBackToTitle");
                    rCScreenManager.DialogueScreen.SetCancelEndHandler(this, "CancelCommand");
                    rCScreenManager.DisplayScreen(13, false);
                }
            }
        }

        public override void Initialize()
        {
            if (Game.PlayerStats.TutorialComplete)
            {
                m_nameText.Text = "Quit to Title Screen";
            }
            else
            {
                m_nameText.Text = "Quit to Title Screen (skip tutorial)";
            }

            base.Initialize();
        }

        public void GoBackToTitle()
        {
            IsActive = false;
            var levelScreen = Game.ScreenManager.GetLevelScreen();
            if (levelScreen != null &&
                (levelScreen.CurrentRoom is CarnivalShoot1BonusRoom ||
                 levelScreen.CurrentRoom is CarnivalShoot2BonusRoom))
            {
                if (levelScreen.CurrentRoom is CarnivalShoot1BonusRoom)
                {
                    (levelScreen.CurrentRoom as CarnivalShoot1BonusRoom).UnequipPlayer();
                }

                if (levelScreen.CurrentRoom is CarnivalShoot2BonusRoom)
                {
                    (levelScreen.CurrentRoom as CarnivalShoot2BonusRoom).UnequipPlayer();
                }
            }

            if (levelScreen != null)
            {
                if (levelScreen.CurrentRoom is ChallengeBossRoomObj challengeBossRoomObj)
                {
                    (m_parentScreen.ScreenManager.Game as Game).SaveManager.LoadFiles(levelScreen,
                        SaveType.UpgradeData);
                    levelScreen.Player.CurrentHealth = challengeBossRoomObj.StoredHP;
                    levelScreen.Player.CurrentMana = challengeBossRoomObj.StoredMP;
                }
            }

            (m_parentScreen.ScreenManager.Game as Game).SaveManager.SaveFiles(SaveType.PlayerData,
                SaveType.UpgradeData);
            if (Game.PlayerStats.TutorialComplete && levelScreen != null && levelScreen.CurrentRoom.Name != "Start" &&
                levelScreen.CurrentRoom.Name != "Ending" && levelScreen.CurrentRoom.Name != "Tutorial")
            {
                (m_parentScreen.ScreenManager.Game as Game).SaveManager.SaveFiles(SaveType.MapData);
            }

            Game.ScreenManager.DisplayScreen(3, true);
        }

        public void CancelCommand()
        {
            IsActive = false;
        }
    }
}
