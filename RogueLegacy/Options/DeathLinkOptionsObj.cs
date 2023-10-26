//  RogueLegacyRandomizer - DeathLinkOptionsObj.cs
//  Last Modified 2023-10-25 8:36 PM
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2018, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using DS2DEngine;
using Microsoft.Xna.Framework;
using Randomizer;
using RogueLegacy.Enums;
using RogueLegacy.Screens;

namespace RogueLegacy.Options;

public class DeathLinkOptionsObj : OptionsObj
{
    private TextObj _modeText;
    private string  _initialText = "Uninitialized";

    public DeathLinkOptionsObj(OptionsScreen parentScreen) : base(parentScreen, "Toggle DeathLink")
    {
        _modeText = _nameText.Clone() as TextObj;
        _modeText.X = m_optionsTextOffset;
        _modeText.Text = "Uninitialized";
        AddChild(_modeText);
    }

    public override bool IsActive
    {
        get => base.IsActive;
        set
        {
            base.IsActive = value;
            if (value)
            {
                _initialText = _modeText.Text;
                if (RandomizerData.CanToggleDeathLink)
                {
                    _modeText.TextureColor = Color.Yellow;
                    _initialText = _modeText.Text;
                }
                else
                {
                    _modeText.TextureColor = Color.Red;
                }

                return;
            }

            _modeText.TextureColor = Color.White;
        }
    }

    public override void Initialize()
    {
        _modeText.Text = RandomizerData.DeathLinkMode switch
        {
            DeathLinkMode.ForcedDisabled => "Forced Disabled",
            DeathLinkMode.ForcedEnabled  => "Forced Enabled",
            _                            => Program.Game.ArchipelagoManager.DeathLink ? "Enabled" : "Disabled",
        };
    }

    public override void HandleInput()
    {
        var manager = Program.Game.ArchipelagoManager;
        if (InputTypeHelper.PressedCancel)
        {
            IsActive = false;
            _modeText.Text = _initialText;
            base.HandleInput();
            return;
        }

        if (InputTypeHelper.PressedConfirm)
        {
            SoundManager.PlaySound("Option_Menu_Select");
            IsActive = false;
            if (!RandomizerData.CanToggleDeathLink)
            {
                base.HandleInput();
                return;
            }

            if (_modeText.Text == "Enabled")
            {
                manager.EnableDeathLink();
            }
            else
            {
                manager.DisableDeathLink();
            }

            base.HandleInput();
            return;
        }

        if (!RandomizerData.CanToggleDeathLink)
        {
            base.HandleInput();
            return;
        }

        if (InputTypeHelper.PressedLeft || InputTypeHelper.PressedRight)
        {
            SoundManager.PlaySound("frame_swap");
            _modeText.Text = _modeText.Text == "Disabled" ? "Enabled" : "Disabled";
        }

        base.HandleInput();
    }

    public override void Dispose()
    {
        if (IsDisposed)
        {
            return;
        }

        _modeText = null;
        _initialText = null;
        base.Dispose();
    }
}
