// 
//  Rogue Legacy Randomizer - Button.cs
//  Last Modified 2022-01-23
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System.Linq;

namespace RogueCastle.Enums
{
    public enum Button
    {
        MenuConfirm1,
        MenuConfirm2,
        MenuCancel1,
        MenuCancel2,
        MenuOptions,
        MenuRogueMode,
        MenuCredits,
        MenuProfileCard,
        MenuPause,
        MenuMap,
        PlayerJump1,
        PlayerJump2,
        PlayerAttack,
        PlayerBlock,
        PlayerDashLeft,
        PlayerDashRight,
        PlayerUp1,
        PlayerUp2,
        PlayerDown1,
        PlayerDown2,
        PlayerLeft1,
        PlayerLeft2,
        PlayerRight1,
        PlayerRight2,
        PlayerSpell,
        MenuProfileSelect,
        MenuDeleteProfile
    }

    public static class ButtonExtensions
    {
        public static string ToString(this Button button)
        {
            return ((int) button).ToString();
        }

        public static int    GetValue(this Button inputButton) => (int) inputButton;
        public static string GetInput(this Button inputButton) => $"[Input:{inputButton.GetValue()}]";
    }

    public static class ButtonHelper
    {
        public static bool PressedConfirm => PressedAny(Button.MenuConfirm1, Button.MenuConfirm2);
        public static bool PressedCancel => PressedAny(Button.MenuCancel1, Button.MenuCancel2);
        public static bool PressedJump => PressedAny(Button.PlayerJump1, Button.PlayerJump2);
        public static bool PressedUp => PressedAny(Button.PlayerUp1, Button.PlayerUp2);
        public static bool PressedDown => PressedAny(Button.PlayerDown1, Button.PlayerDown2);
        public static bool PressedLeft => PressedAny(Button.PlayerLeft1, Button.PlayerLeft2);
        public static bool PressedRight => PressedAny(Button.PlayerRight1, Button.PlayerRight2);

        public static bool PressedAny(params Button[] input)
        {
            return input.Any(button => Game.GlobalInput.JustPressed((int) button));
        }
    }
}
