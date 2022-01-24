// 
//  Rogue Legacy Randomizer - InputType.cs
//  Last Modified 2022-01-24
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
    public enum InputType
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

    public static class InputTypeExtensions
    {
        public static int    Value(this InputType input) => (int) input;
        public static string Input(this InputType input) => $"[Input:{input.Value()}]";
    }

    public static class InputTypeHelper
    {
        public static bool PressedConfirm => PressedAny(InputType.MenuConfirm1, InputType.MenuConfirm2);
        public static bool PressedCancel  => PressedAny(InputType.MenuCancel1, InputType.MenuCancel2);
        public static bool PressedJump    => PressedAny(InputType.PlayerJump1, InputType.PlayerJump2);
        public static bool PressedUp      => PressedAny(InputType.PlayerUp1, InputType.PlayerUp2);
        public static bool PressedDown    => PressedAny(InputType.PlayerDown1, InputType.PlayerDown2);
        public static bool PressedLeft    => PressedAny(InputType.PlayerLeft1, InputType.PlayerLeft2);
        public static bool PressedRight   => PressedAny(InputType.PlayerRight1, InputType.PlayerRight2);

        public static bool PressedAny(params InputType[] inputs)
        {
            return inputs.Any(button => Game.GlobalInput.JustPressed((int) button));
        }
    }
}
