// 
//  RogueLegacyArchipelago - DiaryFlashbackScreen.cs
//  Last Modified 2021-12-29
// 
//  This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
//  original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
//  Original Source - © 2011-2015, Cellar Door Games Inc.
//  Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System;
using System.Collections.Generic;
using Archipelago;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tweener;
using Tweener.Ease;

namespace RogueCastle
{
    internal class DiaryFlashbackScreen : Screen
    {
        private BackgroundObj m_background;
        private SpriteObj m_filmGrain;
        private List<LineageObj> m_lineageArray;
        private RenderTarget2D m_sepiaRT;
        private Vector2 m_storedCameraPos;

        public DiaryFlashbackScreen()
        {
            m_lineageArray = new List<LineageObj>();
        }

        public float BackBufferOpacity { get; set; }

        public override void LoadContent()
        {
            m_filmGrain = new SpriteObj("FilmGrain_Sprite");
            m_filmGrain.ForceDraw = true;
            m_filmGrain.Scale = new Vector2(2.015f, 2.05f);
            m_filmGrain.X -= 5f;
            m_filmGrain.Y -= 5f;
            m_filmGrain.PlayAnimation();
            m_filmGrain.AnimationDelay = 0.0333333351f;
            base.LoadContent();
        }

        public override void ReinitializeRTs()
        {
            m_sepiaRT = new RenderTarget2D(Camera.GraphicsDevice, 1320, 720);
            if (m_background != null)
            {
                m_background.Dispose();
            }

            m_background = new BackgroundObj("LineageScreenBG_Sprite");
            m_background.SetRepeated(true, true, Camera);
            m_background.X -= 6600f;
            base.ReinitializeRTs();
        }

        public override void OnEnter()
        {
            GameUtil.UnlockAchievement("LOVE_OF_BOOKS");
            BackBufferOpacity = 0f;
            Tween.To(this, 0.05f, Tween.EaseNone, "BackBufferOpacity", "1");
            BackBufferOpacity = 1f;
            Tween.To(this, 1f, Tween.EaseNone, "delay", "0.1", "BackBufferOpacity", "0");
            BackBufferOpacity = 0f;
            m_storedCameraPos = Camera.Position;
            Camera.Position = Vector2.Zero;
            if (m_background == null)
            {
                m_sepiaRT = new RenderTarget2D(Camera.GraphicsDevice, 1320, 720);
                m_background = new BackgroundObj("LineageScreenBG_Sprite");
                m_background.SetRepeated(true, true, Camera);
                m_background.X -= 6600f;
            }

            CreateLineageObjDebug();
            Camera.X = m_lineageArray[m_lineageArray.Count - 1].X;
            SoundManager.PlaySound("Cutsc_Thunder");
            Tween.RunFunction(1f, this, "Cutscene1");
            base.OnEnter();
        }

        public void Cutscene1()
        {
            SoundManager.PlaySound("Cutsc_PictureMove");
            Tween.To(Camera, m_lineageArray.Count * 0.2f, Quad.EaseInOut, "X", m_lineageArray[0].X.ToString());
            Tween.AddEndHandlerToLastTween(this, "Cutscene2");
        }

        public void Cutscene2()
        {
            var lineageObj = m_lineageArray[0];
            lineageObj.ForceDraw = true;
            Tween.RunFunction(1f, lineageObj, "DropFrame");
            Tween.RunFunction(4.5f, this, "ExitTransition");
        }

        public void ExitTransition()
        {
            // Unlock all diary screens at once.
            for (var i = Game.PlayerStats.DiaryEntry; i < 25; i++)
            {
                var location = string.Format("Diary {0}", i + 1);
                Program.Game.ArchipelagoManager.CheckLocations(LocationManager.GetCodeByName(location));
                Game.PlayerStats.DiaryEntry += 1;
            }

            SoundManager.PlaySound("Cutsc_Picture_Break");
            Tween.To(this, 0.05f, Tween.EaseNone, "BackBufferOpacity", "1");
            Tween.RunFunction(0.1f, ScreenManager, "HideCurrentScreen");
        }

        public override void OnExit()
        {
            foreach (var current in m_lineageArray) current.Dispose();
            m_lineageArray.Clear();
            Camera.Position = m_storedCameraPos;
            base.OnExit();
        }

        private void CreateLineageObjs()
        {
            var num = 700;
            var num2 = 400;
            var num3 = 0;
            if (Game.PlayerStats.FamilyTreeArray.Count > 10)
            {
                for (var i = 0; i < 10; i++)
                {
                    var familyTreeNode = Game.PlayerStats.FamilyTreeArray[i];
                    var lineageObj = new LineageObj(null, true);
                    lineageObj.IsDead = true;
                    lineageObj.Age = familyTreeNode.Age;
                    lineageObj.ChildAge = familyTreeNode.ChildAge;
                    lineageObj.Class = familyTreeNode.Class;
                    lineageObj.PlayerName = familyTreeNode.Name;
                    lineageObj.IsFemale = familyTreeNode.IsFemale;
                    lineageObj.SetPortrait(familyTreeNode.HeadPiece, familyTreeNode.ShoulderPiece,
                        familyTreeNode.ChestPiece);
                    lineageObj.NumEnemiesKilled = familyTreeNode.NumEnemiesBeaten;
                    lineageObj.BeatenABoss = familyTreeNode.BeatenABoss;
                    lineageObj.SetTraits(familyTreeNode.Traits);
                    lineageObj.UpdateAge(num);
                    lineageObj.UpdateData();
                    lineageObj.UpdateClassRank();
                    num += lineageObj.Age;
                    lineageObj.X = num3;
                    num3 += num2;
                    m_lineageArray.Add(lineageObj);
                }

                return;
            }

            foreach (var current in Game.PlayerStats.FamilyTreeArray)
            {
                var lineageObj2 = new LineageObj(null, true);
                lineageObj2.IsDead = true;
                lineageObj2.Age = current.Age;
                lineageObj2.ChildAge = current.ChildAge;
                lineageObj2.Class = current.Class;
                lineageObj2.PlayerName = current.Name;
                lineageObj2.IsFemale = current.IsFemale;
                lineageObj2.SetPortrait(current.HeadPiece, current.ShoulderPiece, current.ChestPiece);
                lineageObj2.NumEnemiesKilled = current.NumEnemiesBeaten;
                lineageObj2.BeatenABoss = current.BeatenABoss;
                lineageObj2.SetTraits(current.Traits);
                lineageObj2.UpdateAge(num);
                lineageObj2.UpdateData();
                lineageObj2.UpdateClassRank();
                num += lineageObj2.Age;
                lineageObj2.X = num3;
                num3 += num2;
                m_lineageArray.Add(lineageObj2);
            }
        }

        private void CreateLineageObjDebug()
        {
            var num = 700;
            var num2 = 400;
            var num3 = 0;
            for (var i = 0; i < 10; i++)
            {
                FamilyTreeNode familyTreeNode;
                if (i > Game.PlayerStats.FamilyTreeArray.Count - 1)
                {
                    familyTreeNode = new FamilyTreeNode
                    {
                        Age = (byte) CDGMath.RandomInt(15, 30),
                        ChildAge = (byte) CDGMath.RandomInt(15, 30),
                        Name = Game.NameArray[CDGMath.RandomInt(0, Game.NameArray.Count - 1)],
                        HeadPiece = (byte) CDGMath.RandomInt(1, 5),
                        ShoulderPiece = (byte) CDGMath.RandomInt(1, 5),
                        ChestPiece = (byte) CDGMath.RandomInt(1, 5)
                    };
                }
                else
                {
                    familyTreeNode = Game.PlayerStats.FamilyTreeArray[i];
                }

                var lineageObj = new LineageObj(null, true);
                lineageObj.IsDead = true;
                lineageObj.Age = familyTreeNode.Age;
                lineageObj.ChildAge = familyTreeNode.ChildAge;
                lineageObj.Class = familyTreeNode.Class;
                lineageObj.PlayerName = familyTreeNode.Name;
                lineageObj.IsFemale = familyTreeNode.IsFemale;
                lineageObj.SetPortrait(familyTreeNode.HeadPiece, familyTreeNode.ShoulderPiece,
                    familyTreeNode.ChestPiece);
                lineageObj.NumEnemiesKilled = familyTreeNode.NumEnemiesBeaten;
                lineageObj.BeatenABoss = familyTreeNode.BeatenABoss;
                lineageObj.SetTraits(familyTreeNode.Traits);
                lineageObj.UpdateAge(num);
                lineageObj.UpdateData();
                lineageObj.UpdateClassRank();
                num += lineageObj.Age;
                lineageObj.X = num3;
                num3 += num2;
                m_lineageArray.Add(lineageObj);
            }
        }

        public override void Draw(GameTime gametime)
        {
            if (Camera.X > m_background.X + 6600f)
            {
                m_background.X = Camera.X;
            }

            if (Camera.X < m_background.X)
            {
                m_background.X = Camera.X - 1320f;
            }

            Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, null, null, null,
                Camera.GetTransformation());
            m_background.Draw(Camera);
            foreach (var current in m_lineageArray) current.Draw(Camera);
            Camera.End();
            Camera.GraphicsDevice.SetRenderTarget(m_sepiaRT);
            Game.HSVEffect.Parameters["Saturation"].SetValue(0.2f);
            Game.HSVEffect.Parameters["Brightness"].SetValue(0.1f);
            Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null,
                Game.HSVEffect);
            Camera.Draw((ScreenManager as RCScreenManager).RenderTarget, Vector2.Zero, Color.White);
            Camera.End();
            Camera.GraphicsDevice.SetRenderTarget((ScreenManager as RCScreenManager).RenderTarget);
            Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null);
            var color = new Color(180, 150, 80);
            Camera.Draw(m_sepiaRT, Vector2.Zero, color);
            m_filmGrain.Draw(Camera);
            Camera.End();
            Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null);
            Camera.Draw(Game.GenericTexture, new Rectangle(0, 0, 1320, 720), Color.White * BackBufferOpacity);
            Camera.End();
            base.Draw(gametime);
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                Console.WriteLine("Disposing Diary Flashback Screen");
                m_background.Dispose();
                m_background = null;
                foreach (var current in m_lineageArray) current.Dispose();
                m_lineageArray.Clear();
                m_lineageArray = null;
                m_filmGrain.Dispose();
                m_filmGrain = null;
                m_sepiaRT.Dispose();
                m_sepiaRT = null;
                base.Dispose();
            }
        }
    }
}