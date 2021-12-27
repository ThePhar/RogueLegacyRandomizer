// 
// RogueLegacyArchipelago - EndingRoomObj.cs
// Last Modified 2021-12-27
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System.Collections.Generic;
using System.Globalization;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueCastle.Structs;
using Tweener;
using Tweener.Ease;

namespace RogueCastle
{
    internal class EndingRoomObj : RoomObj
    {
        private readonly float m_waypointSpeed = 5f;
        private BackgroundObj m_background;
        private EnemyObj_Blob m_blobBoss;
        private List<Vector2> m_cameraPosList;
        private KeyIconTextObj m_continueText;
        private bool m_displayingContinueText;
        private SpriteObj m_endingMask;
        private List<SpriteObj> m_frameList;
        private List<TextObj> m_nameList;
        private List<SpriteObj> m_plaqueList;
        private List<TextObj> m_slainCountText;
        private int m_waypointIndex;

        public EndingRoomObj()
        {
            m_plaqueList = new List<SpriteObj>();
        }

        public override void InitializeRenderTarget(RenderTarget2D bgRenderTarget)
        {
            if (m_background != null)
            {
                m_background.Dispose();
            }
            m_background = new BackgroundObj("LineageScreenBG_Sprite");
            m_background.SetRepeated(true, true, Game.ScreenManager.Camera);
            m_background.X -= 6600f;
            m_background.Opacity = 0.7f;
            base.InitializeRenderTarget(bgRenderTarget);
        }

        public override void LoadContent(GraphicsDevice graphics)
        {
            m_continueText = new KeyIconTextObj(Game.JunicodeFont);
            m_continueText.FontSize = 14f;
            m_continueText.Align = Types.TextAlign.Right;
            m_continueText.Position = new Vector2(1270f, 650f);
            m_continueText.ForceDraw = true;
            m_continueText.Opacity = 0f;
            m_background = new BackgroundObj("LineageScreenBG_Sprite");
            m_background.SetRepeated(true, true, Game.ScreenManager.Camera);
            m_background.X -= 6600f;
            m_background.Opacity = 0.7f;
            m_endingMask = new SpriteObj("Blank_Sprite");
            m_endingMask.ForceDraw = true;
            m_endingMask.TextureColor = Color.Black;
            m_endingMask.Scale = new Vector2(1330f/m_endingMask.Width, 730f/m_endingMask.Height);
            m_cameraPosList = new List<Vector2>();
            m_frameList = new List<SpriteObj>();
            m_nameList = new List<TextObj>();
            m_slainCountText = new List<TextObj>();
            foreach (var current in GameObjList)
            {
                if (current is WaypointObj)
                {
                    m_cameraPosList.Add(default(Vector2));
                }
            }
            var cultureInfo = (CultureInfo) CultureInfo.CurrentCulture.Clone();
            cultureInfo.NumberFormat.CurrencyDecimalSeparator = ".";
            foreach (var current2 in GameObjList)
            {
                if (current2 is WaypointObj)
                {
                    var index = int.Parse(current2.Name, NumberStyles.Any, cultureInfo);
                    m_cameraPosList[index] = current2.Position;
                }
            }
            var num = 150f;
            foreach (var current3 in EnemyList)
            {
                current3.Initialize();
                current3.PauseEnemy(true);
                current3.IsWeighted = false;
                current3.PlayAnimation();
                current3.UpdateCollisionBoxes();
                var spriteObj = new SpriteObj("LineageScreenFrame_Sprite");
                spriteObj.DropShadow = new Vector2(4f, 6f);
                if (current3.Difficulty == EnemyDifficulty.MiniBoss)
                {
                    spriteObj.ChangeSprite("GiantPortrait_Sprite");
                    FixMiniboss(current3);
                }
                spriteObj.Scale = new Vector2((current3.Width + num)/spriteObj.Width,
                    (current3.Height + num)/spriteObj.Height);
                if (spriteObj.ScaleX < 1f)
                {
                    spriteObj.ScaleX = 1f;
                }
                if (spriteObj.ScaleY < 1f)
                {
                    spriteObj.ScaleY = 1f;
                }
                spriteObj.Position = new Vector2(current3.X, current3.Bounds.Top + current3.Height/2f);
                m_frameList.Add(spriteObj);
                var textObj = new TextObj(Game.JunicodeFont);
                textObj.FontSize = 12f;
                textObj.Align = Types.TextAlign.Centre;
                textObj.Text = current3.Name;
                textObj.OutlineColour = new Color(181, 142, 39);
                textObj.OutlineWidth = 2;
                textObj.Position = new Vector2(spriteObj.X, spriteObj.Bounds.Bottom + 40);
                m_nameList.Add(textObj);
                var textObj2 = new TextObj(Game.JunicodeFont);
                textObj2.FontSize = 10f;
                textObj2.Align = Types.TextAlign.Centre;
                textObj2.OutlineColour = new Color(181, 142, 39);
                textObj2.Text = "Slain: 0";
                textObj2.OutlineWidth = 2;
                textObj2.HeadingX = current3.Type;
                textObj2.HeadingY = (float) current3.Difficulty;
                textObj2.Position = new Vector2(spriteObj.X, spriteObj.Bounds.Bottom + 80);
                m_slainCountText.Add(textObj2);
                var type = current3.Type;
                if (type <= 15)
                {
                    if (type != 1)
                    {
                        if (type != 7)
                        {
                            if (type == 15)
                            {
                                if (current3.Difficulty == EnemyDifficulty.MiniBoss)
                                {
                                    if (current3.Flip == SpriteEffects.None)
                                    {
                                        current3.X -= 25f;
                                    }
                                    else
                                    {
                                        current3.X += 25f;
                                    }
                                }
                            }
                        }
                        else if (current3.Difficulty == EnemyDifficulty.MiniBoss)
                        {
                            current3.X += 30f;
                            current3.Y -= 20f;
                        }
                    }
                    else
                    {
                        (current3 as EnemyObj_BallAndChain).BallAndChain.Visible = false;
                        (current3 as EnemyObj_BallAndChain).BallAndChain2.Visible = false;
                    }
                }
                else if (type != 20)
                {
                    if (type != 29)
                    {
                        if (type == 32)
                        {
                            if (current3.Difficulty == EnemyDifficulty.MiniBoss)
                            {
                                spriteObj.Visible = false;
                            }
                        }
                    }
                    else
                    {
                        if (current3.Difficulty == EnemyDifficulty.Advanced)
                        {
                            (current3 as EnemyObj_LastBoss).ForceSecondForm(true);
                            current3.ChangeSprite("EnemyLastBossIdle_Character");
                            current3.PlayAnimation();
                        }
                        spriteObj.ChangeSprite("GiantPortrait_Sprite");
                        spriteObj.Scale = Vector2.One;
                        spriteObj.Scale = new Vector2((current3.Width + num)/spriteObj.Width,
                            (current3.Height + num)/spriteObj.Height);
                        textObj.Position = new Vector2(spriteObj.X, spriteObj.Bounds.Bottom + 40);
                        textObj2.Position = new Vector2(spriteObj.X, spriteObj.Bounds.Bottom + 80);
                    }
                }
                else
                {
                    current3.ChangeSprite("EnemyZombieWalk_Character");
                    current3.PlayAnimation();
                }
                var spriteObj2 = new SpriteObj("LineageScreenPlaque1Long_Sprite");
                spriteObj2.Scale = new Vector2(1.8f, 1.8f);
                spriteObj2.Position = new Vector2(spriteObj.X, spriteObj.Bounds.Bottom + 80);
                m_plaqueList.Add(spriteObj2);
            }
            base.LoadContent(graphics);
        }

        private void FixMiniboss(EnemyObj enemy)
        {
            var type = enemy.Type;
            switch (type)
            {
                case 2:
                    m_blobBoss = (enemy as EnemyObj_Blob);
                    enemy.ChangeSprite("EnemyBlobBossIdle_Character");
                    enemy.GetChildAt(0).TextureColor = Color.White;
                    enemy.GetChildAt(2).TextureColor = Color.LightSkyBlue;
                    enemy.GetChildAt(2).Opacity = 0.8f;
                    (enemy.GetChildAt(1) as SpriteObj).OutlineColour = Color.Black;
                    enemy.GetChildAt(1).TextureColor = Color.Black;
                    break;
                case 3:
                case 4:
                    break;
                case 5:
                    if (enemy.Flip == SpriteEffects.None)
                    {
                        enemy.Name = "Amon";
                    }
                    else
                    {
                        enemy.Name = "Barbatos";
                    }
                    break;
                case 6:
                    enemy.ChangeSprite("EnemyEyeballBossEye_Character");
                    (enemy as EnemyObj_Eyeball).ChangeToBossPupil();
                    break;
                case 7:
                    enemy.ChangeSprite("EnemyFairyGhostBossIdle_Character");
                    break;
                case 8:
                    enemy.ChangeSprite("EnemyGhostBossIdle_Character");
                    break;
                default:
                    if (type != 15)
                    {
                        if (type == 22)
                        {
                            if (enemy.Flip == SpriteEffects.None)
                            {
                                enemy.Name = "Stolas";
                            }
                            else
                            {
                                enemy.Name = "Focalor";
                            }
                        }
                    }
                    else if (enemy.Flip == SpriteEffects.None)
                    {
                        enemy.Name = "Berith";
                    }
                    else
                    {
                        enemy.Name = "Halphas";
                    }
                    break;
            }
            enemy.PlayAnimation();
        }

        public override void OnEnter()
        {
            m_blobBoss.PlayAnimation();
            foreach (var current in EnemyList)
            {
                if (current.Type == 5)
                {
                    (current as EnemyObj_EarthWizard).EarthProjectile.Visible = false;
                }
            }
            m_displayingContinueText = false;
            UpdateEnemiesSlainText();
            m_continueText.Text = "Press [Input:" + 0 + "] to exit";
            m_continueText.Opacity = 0f;
            Player.AttachedLevel.Camera.Position = new Vector2(0f, 360f);
            Player.Position = new Vector2(100f, 100f);
            m_waypointIndex = 1;
            Player.ForceInvincible = true;
            Player.AttachedLevel.SetMapDisplayVisibility(false);
            Player.AttachedLevel.SetPlayerHUDVisibility(false);
            SoundManager.PlayMusic("EndSongDrums", true, 1f);
            Game.PlayerStats.TutorialComplete = true;
            Player.LockControls();
            Player.Visible = false;
            Player.Opacity = 0f;
            Player.AttachedLevel.CameraLockedToPlayer = false;
            base.OnEnter();
            ChangeWaypoints();
        }

        private void UpdateEnemiesSlainText()
        {
            foreach (var current in m_slainCountText)
            {
                int index = (byte) current.HeadingX;
                var num = (int) current.HeadingY;
                var num2 = 0;
                switch (num)
                {
                    case 0:
                        num2 = (int) Game.PlayerStats.EnemiesKilledList[index].X;
                        break;
                    case 1:
                        num2 = (int) Game.PlayerStats.EnemiesKilledList[index].Y;
                        break;
                    case 2:
                        num2 = (int) Game.PlayerStats.EnemiesKilledList[index].Z;
                        break;
                    case 3:
                        num2 = (int) Game.PlayerStats.EnemiesKilledList[index].W;
                        break;
                }
                current.Text = "Slain: " + num2;
            }
        }

        public void ChangeWaypoints()
        {
            if (m_waypointIndex < m_cameraPosList.Count)
            {
                object arg_91_0 = Player.AttachedLevel.Camera;
                var arg_91_1 = 1.5f;
                Easing arg_91_2 = Quad.EaseInOut;
                var array = new string[4];
                array[0] = "X";
                var arg_66_0 = array;
                var arg_66_1 = 1;
                var x = m_cameraPosList[m_waypointIndex].X;
                arg_66_0[arg_66_1] = x.ToString();
                array[2] = "Y";
                var arg_8F_0 = array;
                var arg_8F_1 = 3;
                var y = m_cameraPosList[m_waypointIndex].Y;
                arg_8F_0[arg_8F_1] = y.ToString();
                Tween.To(arg_91_0, arg_91_1, arg_91_2, array);
                object arg_10A_0 = Player;
                var arg_10A_1 = 1.5f;
                Easing arg_10A_2 = Quad.EaseInOut;
                var array2 = new string[4];
                array2[0] = "X";
                var arg_DE_0 = array2;
                var arg_DE_1 = 1;
                var x2 = m_cameraPosList[m_waypointIndex].X;
                arg_DE_0[arg_DE_1] = x2.ToString();
                array2[2] = "Y";
                var arg_108_0 = array2;
                var arg_108_1 = 3;
                var y2 = m_cameraPosList[m_waypointIndex].Y;
                arg_108_0[arg_108_1] = y2.ToString();
                Tween.To(arg_10A_0, arg_10A_1, arg_10A_2, array2);
                m_waypointIndex++;
                if (m_waypointIndex > m_cameraPosList.Count - 1)
                {
                    m_waypointIndex = 0;
                    Tween.RunFunction(0f, Player.AttachedLevel.ScreenManager, "DisplayScreen", 18, true,
                        typeof (List<object>));
                    return;
                }
                Tween.RunFunction(m_waypointSpeed, this, "ChangeWaypoints");
            }
        }

        public void ChangeLevelType()
        {
            LevelType = LevelType.Dungeon;
            Player.AttachedLevel.UpdateLevel(LevelType);
        }

        public override void Update(GameTime gameTime)
        {
            if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1) || Game.GlobalInput.JustPressed(2) ||
                Game.GlobalInput.JustPressed(3))
            {
                if (m_displayingContinueText)
                {
                    Tween.StopAll(false);
                    Game.ScreenManager.DisplayScreen(18, true);
                }
                else
                {
                    m_displayingContinueText = true;
                    Tween.StopAllContaining(m_continueText, false);
                    Tween.To(m_continueText, 0.5f, Tween.EaseNone, "Opacity", "1");
                    Tween.RunFunction(4f, this, "HideContinueText");
                }
            }
            base.Update(gameTime);
        }

        public void HideContinueText()
        {
            m_displayingContinueText = false;
            Tween.To(m_continueText, 0.5f, Tween.EaseNone, "delay", "0", "Opacity", "0");
        }

        public override void Draw(Camera2D camera)
        {
            m_continueText.Position = new Vector2(camera.Bounds.Right - 50, camera.Bounds.Bottom - 70);
            m_endingMask.Position = camera.Position - new Vector2(660f, 360f);
            m_endingMask.Draw(camera);
            if (camera.X > m_background.X + 6600f)
            {
                m_background.X = camera.X;
            }
            if (camera.X < m_background.X)
            {
                m_background.X = camera.X - 1320f;
            }
            m_background.Draw(camera);
            foreach (var current in m_frameList)
            {
                current.Draw(camera);
            }
            foreach (var current2 in m_plaqueList)
            {
                current2.Draw(camera);
            }
            base.Draw(camera);
            camera.End();
            if (!LevelENV.ShowEnemyRadii)
            {
                camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null,
                    camera.GetTransformation());
            }
            else
            {
                camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null,
                    camera.GetTransformation());
            }
            foreach (var current3 in m_nameList)
            {
                current3.Draw(camera);
            }
            foreach (var current4 in m_slainCountText)
            {
                current4.Draw(camera);
            }
            m_continueText.Draw(camera);
            camera.End();
            if (!LevelENV.ShowEnemyRadii)
            {
                camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null,
                    camera.GetTransformation());
                return;
            }
            camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null,
                camera.GetTransformation());
        }

        protected override GameObj CreateCloneInstance()
        {
            return new EndingRoomObj();
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                foreach (var current in m_frameList)
                {
                    current.Dispose();
                }
                m_frameList.Clear();
                m_frameList = null;
                foreach (var current2 in m_plaqueList)
                {
                    current2.Dispose();
                }
                m_plaqueList.Clear();
                m_plaqueList = null;
                m_cameraPosList.Clear();
                m_cameraPosList = null;
                foreach (var current3 in m_nameList)
                {
                    current3.Dispose();
                }
                m_nameList.Clear();
                m_nameList = null;
                foreach (var current4 in m_slainCountText)
                {
                    current4.Dispose();
                }
                m_slainCountText.Clear();
                m_slainCountText = null;
                m_endingMask.Dispose();
                m_endingMask = null;
                m_continueText.Dispose();
                m_continueText = null;
                m_background.Dispose();
                m_background = null;
                base.Dispose();
            }
        }
    }
}
