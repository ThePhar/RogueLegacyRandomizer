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