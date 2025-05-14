using DS2DEngine;
using Microsoft.Xna.Framework;
using RogueLegacy.Enums;
using RogueLegacy.Screens;

namespace RogueLegacy.Options
{
    public class RandoModeOption : RandomizerOption
    {
        private TextObj _toggleText;

        public RandoModeOption(RandomizerScreen parentScreen) : base(parentScreen, "Rando Mode")
        {
            _toggleText = _nameText.Clone() as TextObj;
            _toggleText.X = OPTIONS_TEXT_OFFSET;
            _toggleText.Text = "No";
            _toggleText.TextureColor = Color.Gray;
            AddChild(_toggleText);
        }

        public override bool IsActive
        {
            get => base.IsActive;
            set
            {
                return;
                base.IsActive = value;
                if (value)
                {
                    _toggleText.TextureColor = Color.Yellow;
                    return;
                }

                _toggleText.TextureColor = Color.White;
            }
        }

        public override void Initialize()
        {
            _toggleText.Text = true ? "Archipelago" : "Solo";
            base.Initialize();
        }

        public override void HandleInput()
        {
            if (InputTypeHelper.PressedConfirm)
            {
                SoundManager.PlaySound("Option_Menu_Select");
                // Game.RandomizerOptions.IsArchipelago = _toggleText.Text == "Archipelago";
                IsActive = false;
            }

            if (Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3))
            {
                _toggleText.Text = true ? "Archipelago" : "Solo";
                IsActive = false;
            }

            base.HandleInput();
        }

        public override void Dispose()
        {
            if (IsDisposed) return;

            _toggleText = null;
            base.Dispose();
        }
    }
}
