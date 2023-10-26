//  RogueLegacyRandomizer - BackToMenuOptionsObj.cs
//  Last Modified 2023-10-25 8:58 PM
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2018, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using RogueLegacy.Enums;
using RogueLegacy.Screens;

namespace RogueLegacy;

public class BackToMenuOptionsObj : OptionsObj
{
    public BackToMenuOptionsObj(OptionsScreen parentScreen) : base(parentScreen, "Quit to Title Screen") { }

    public override bool IsActive
    {
        get => base.IsActive;
        set
        {
            base.IsActive = value;
            if (!IsActive)
            {
                return;
            }

            var rcScreenManager = (RCScreenManager) _parentScreen.ScreenManager;
            rcScreenManager.DialogueScreen.SetDialogue("Back to Menu");
            rcScreenManager.DialogueScreen.SetDialogueChoice("ConfirmTest1");
            rcScreenManager.DialogueScreen.SetConfirmEndHandler(this, "GoBackToTitle");
            rcScreenManager.DialogueScreen.SetCancelEndHandler(this, "CancelCommand");
            rcScreenManager.DisplayScreen(ScreenType.Dialogue, false);
        }
    }

    public override void Initialize()
    {
        _nameText.Text = Game.PlayerStats.TutorialComplete
            ? "Quit to Title Screen"
            : "Quit to Title Screen (skip tutorial)";

        base.Initialize();
    }

    public void GoBackToTitle()
    {
        IsActive = false;
        Program.Game.SaveAndReset();
        Game.ScreenManager.DisplayScreen(ScreenType.Title, true);
    }

    public void CancelCommand()
    {
        IsActive = false;
    }
}
