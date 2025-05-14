//  RogueLegacyRandomizer - RetireOptionsObj.cs
//  Last Modified 2023-10-25 8:36 PM
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2018, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using DS2DEngine;
using RogueLegacy.Screens;

namespace RogueLegacy
{
    public class RetireOptionsObj : OptionsObj
    {
        public RetireOptionsObj(OptionsScreen parentScreen) : base(parentScreen, "Retire Character") { }

        public override bool IsActive
        {
            get { return base.IsActive; }
            set
            {
                var rCScreenManager = Game.ScreenManager as RCScreenManager;
                var levelScreen = rCScreenManager.GetLevelScreen();
                if (levelScreen.CurrentRoom is StartingRoomObj)
                {
                    SoundManager.PlaySound("Error_Spell");
                    return;
                }
                
                base.IsActive = value;
                if (IsActive)
                {
                    DialogueManager.AddText("Retire Character", new[] { "Retire?" }, new[]
                    {
                        "Are you sure you want to immediately end your current character's suffering?" +
                        (Program.Game.ArchipelagoManager.DeathLink
                            ? " Be warned, this counts as a death and will punish all those with DeathLink enabled."
                            : ""),
                    });

                    rCScreenManager.DialogueScreen.SetDialogue("Retire Character");
                    rCScreenManager.DialogueScreen.SetDialogueChoice("ConfirmTest1");
                    rCScreenManager.DialogueScreen.SetConfirmEndHandler(this, "RetireCommand");
                    rCScreenManager.DialogueScreen.SetCancelEndHandler(this, "CancelCommand");
                    rCScreenManager.DisplayScreen(13, false);
                }
            }
        }

        public void RetireCommand()
        {
            IsActive = false;
            _parentScreen.ExitTransition();
            Game.ScreenManager.GetLevelScreen().UnpauseScreen();
            Game.ScreenManager.HideCurrentScreen();
            Game.Retired = true;
        }

        public void CancelCommand()
        {
            IsActive = false;
        }
    }
}
