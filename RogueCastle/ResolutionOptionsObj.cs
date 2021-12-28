// 
// RogueLegacyArchipelago - ResolutionOptionsObj.cs
// Last Modified 2021-12-27
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;

namespace RogueCastle
{
    public class ResolutionOptionsObj : OptionsObj
    {
        private List<Vector2> m_displayModeList;
        private float m_resetCounter;
        private int m_selectedResIndex;
        private Vector2 m_selectedResolution;
        private TextObj m_toggleText;

        public ResolutionOptionsObj(OptionsScreen parentScreen) : base(parentScreen, "Resolution")
        {
            m_toggleText = (m_nameText.Clone() as TextObj);
            m_toggleText.X = m_optionsTextOffset;
            m_toggleText.Text = "null";
            AddChild(m_toggleText);
        }

        public override bool IsActive
        {
            get { return base.IsActive; }
            set
            {
                base.IsActive = value;
                if (value)
                {
                    m_toggleText.TextureColor = Color.Yellow;
                    return;
                }
                m_toggleText.TextureColor = Color.White;
                m_toggleText.Text = m_selectedResolution.X + "x" + m_selectedResolution.Y;
            }
        }

        public override void Initialize()
        {
            m_resetCounter = 0f;
            m_selectedResolution = new Vector2(m_parentScreen.ScreenManager.Game.GraphicsDevice.Viewport.Width,
                m_parentScreen.ScreenManager.Game.GraphicsDevice.Viewport.Height);
            if (m_displayModeList != null)
            {
                m_displayModeList.Clear();
            }
            m_displayModeList = (m_parentScreen.ScreenManager.Game as Game).GetSupportedResolutions();
            m_toggleText.Text = m_selectedResolution.X + "x" + m_selectedResolution.Y;
            m_selectedResIndex = 0;
            for (var i = 0; i < m_displayModeList.Count; i++)
            {
                if (m_selectedResolution == m_displayModeList[i])
                {
                    m_selectedResIndex = i;
                    return;
                }
            }
        }

        public override void HandleInput()
        {
            var selectedResIndex = m_selectedResIndex;
            if (Game.GlobalInput.JustPressed(20) || Game.GlobalInput.JustPressed(21))
            {
                m_selectedResIndex--;
                SoundManager.PlaySound("frame_swap");
            }
            else if (Game.GlobalInput.JustPressed(22) || Game.GlobalInput.JustPressed(23))
            {
                m_selectedResIndex++;
                SoundManager.PlaySound("frame_swap");
            }
            if (m_selectedResIndex < 0)
            {
                m_selectedResIndex = 0;
            }
            if (m_selectedResIndex > m_displayModeList.Count - 1)
            {
                m_selectedResIndex = m_displayModeList.Count - 1;
            }
            if (m_selectedResIndex != selectedResIndex)
            {
                m_toggleText.Text = m_displayModeList[m_selectedResIndex].X + "x" +
                                    m_displayModeList[m_selectedResIndex].Y;
            }
            if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1))
            {
                SoundManager.PlaySound("Option_Menu_Select");
                var vector = m_displayModeList[m_selectedResIndex];
                if (m_selectedResolution != vector)
                {
                    (m_parentScreen.ScreenManager.Game as Game).GraphicsDeviceManager.PreferredBackBufferWidth = (int) vector.X;
                    (m_parentScreen.ScreenManager.Game as Game).GraphicsDeviceManager.PreferredBackBufferHeight = (int) vector.Y;
                    (m_parentScreen.ScreenManager.Game as Game).GraphicsDeviceManager.ApplyChanges();
                    (m_parentScreen.ScreenManager as RCScreenManager).ForceResolutionChangeCheck();
                    if ((m_parentScreen.ScreenManager.Game as Game).GraphicsDeviceManager.IsFullScreen)
                    {
                        var rCScreenManager = m_parentScreen.ScreenManager as RCScreenManager;
                        rCScreenManager.DialogueScreen.SetDialogue("Resolution Changed");
                        rCScreenManager.DialogueScreen.SetDialogueChoice("ConfirmTest1");
                        rCScreenManager.DialogueScreen.SetConfirmEndHandler(this, "SaveResolution", vector);
                        rCScreenManager.DialogueScreen.SetCancelEndHandler(this, "CancelResolution");
                        rCScreenManager.DisplayScreen(13, false);
                        m_resetCounter = 10f;
                    }
                    else
                    {
                        m_selectedResolution = vector;
                        SaveResolution(vector);
                    }
                }
            }
            if (Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3))
            {
                IsActive = false;
            }
            base.HandleInput();
        }

        public override void Update(GameTime gameTime)
        {
            if (m_resetCounter > 0f)
            {
                m_resetCounter -= (float) gameTime.ElapsedGameTime.TotalSeconds;
                if (m_resetCounter <= 0f)
                {
                    var rCScreenManager = m_parentScreen.ScreenManager as RCScreenManager;
                    rCScreenManager.HideCurrentScreen();
                    CancelResolution();
                }
            }
            base.Update(gameTime);
        }

        public void SaveResolution(Vector2 resolution)
        {
            Game.GameConfig.ScreenWidth = (int) resolution.X;
            Game.GameConfig.ScreenHeight = (int) resolution.Y;
            m_resetCounter = 0f;
            m_selectedResolution = resolution;
            IsActive = false;
        }

        public void CancelResolution()
        {
            m_resetCounter = 0f;
            (m_parentScreen.ScreenManager.Game as Game).GraphicsDeviceManager.PreferredBackBufferWidth = (int) m_selectedResolution.X;
            (m_parentScreen.ScreenManager.Game as Game).GraphicsDeviceManager.PreferredBackBufferHeight =
                (int) m_selectedResolution.Y;
            (m_parentScreen.ScreenManager.Game as Game).GraphicsDeviceManager.ApplyChanges();
            (m_parentScreen.ScreenManager as RCScreenManager).ForceResolutionChangeCheck();
            m_toggleText.Text = m_selectedResolution.X + "x" + m_selectedResolution.Y;
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                m_toggleText = null;
                m_displayModeList.Clear();
                m_displayModeList = null;
                base.Dispose();
            }
        }
    }
}
