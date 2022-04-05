// 
//  Rogue Legacy Randomizer - InputType.cs
//  Last Modified 2022-04-03
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
        MenuConfirm1      = 0,
        MenuConfirm2      = 1,
        MenuCancel1       = 2,
        MenuCancel2       = 3,
        MenuOptions       = 4,
        MenuRogueMode     = 5,
        MenuCredits       = 6,
        MenuProfileCard   = 7,
        MenuPause         = 8,
        MenuMap           = 9,
        PlayerJump1       = 10,
        PlayerJump2       = 11,
        PlayerAttack      = 12,
        PlayerBlock       = 13,
        PlayerDashLeft    = 14,
        PlayerDashRight   = 15,
        PlayerUp1         = 16,
        PlayerUp2         = 17,
        PlayerDown1       = 18,
        PlayerDown2       = 19,
        PlayerLeft1       = 20,
        PlayerLeft2       = 21,
        PlayerRight1      = 22,
        PlayerRight2      = 23,
        PlayerSpell       = 24,
        MenuProfileSelect = 25,
        MenuDeleteProfile = 26
    }

    public static class InputTypeExtensions
    {
        public static int Value(this InputType input)
        {
            return (int) input;
        }

        public static string Input(this InputType input)
        {
            return $"[Input:{input.Value()}]";
        }
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
