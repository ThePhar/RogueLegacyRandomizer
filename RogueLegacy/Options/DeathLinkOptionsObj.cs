// RogueLegacyRandomizer - DeathLinkOptionsObj.cs
// Last Modified 2023-07-30 9:28 AM by 
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source - © 2011-2018, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Randomizer;
using RogueLegacy.Enums;
using RogueLegacy.Screens;

namespace RogueLegacy.Options;

public class DeathLinkOptionsObj : OptionsObj
{
    private TextObj _modeText;
    private string  _initalText = "Uninitialized";

    public DeathLinkOptionsObj(OptionsScreen parentScreen) : base(parentScreen, "Toggle DeathLink")
    {
        _modeText = m_nameText.Clone() as TextObj;
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
                _initalText = _modeText.Text;
                if (ArchipelagoManager.RandomizerData.CanToggleDeathLink)
                {
                    _modeText.TextureColor = Color.Yellow;
                    _initalText = _modeText.Text;
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
        _modeText.Text = ArchipelagoManager.RandomizerData.DeathLinkMode switch
        {
            DeathLinkMode.Disabled       => "Disabled",
            DeathLinkMode.Enabled        => "Enabled",
            DeathLinkMode.ForcedDisabled => "Forced Disabled",
            DeathLinkMode.ForcedEnabled  => "Forced Enabled",
            _                            => throw new ArgumentOutOfRangeException("Unknown DeathLink Mode"),
        };
    }

    public override void HandleInput()
    {
        if (InputTypeHelper.PressedCancel)
        {
            IsActive = false;
            _modeText.Text = _initalText;
            base.HandleInput();
            return;
        }

        if (InputTypeHelper.PressedConfirm)
        {
            SoundManager.PlaySound("Option_Menu_Select");
            IsActive = false;
            if (!ArchipelagoManager.RandomizerData.CanToggleDeathLink)
            {
                base.HandleInput();
                return;
            }

            if (_modeText.Text == "Enabled")
            {
                ArchipelagoManager.EnableDeathLink();
            }
            else
            {
                ArchipelagoManager.DisableDeathLink();
            }

            base.HandleInput();
            return;
        }

        if (!ArchipelagoManager.RandomizerData.CanToggleDeathLink)
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
        _initalText = null;
        base.Dispose();
    }
}