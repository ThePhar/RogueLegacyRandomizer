//
// RogueLegacyArchipelago - GameOverScreen.cs
// Last Modified 2021-12-28
//
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
//
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
//

using System;
using System.Collections.Generic;
using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueCastle.GameObjects;
using RogueCastle.Structs;
using Tweener;
using Tweener.Ease;

namespace RogueCastle
{
    public class GameOverScreen : Screen
    {
        private int m_bagsCollected;
        private int m_coinsCollected;
        private KeyIconTextObj m_continueText;
        private ObjContainer m_dialoguePlate;
        private int m_diamondsCollected;
        private bool m_droppingStats;
        private List<EnemyObj> m_enemyList;
        private List<Vector2> m_enemyStoredPositions;
        private bool m_lockControls;
        private GameObj m_objKilledPlayer;
        private PlayerObj m_player;
        private FrameSoundObj m_playerFallSound;
        private LineageObj m_playerFrame;
        private SpriteObj m_playerGhost;
        private FrameSoundObj m_playerSwordFallSound;
        private FrameSoundObj m_playerSwordSpinSound;
        private SpriteObj m_spotlight;

        public GameOverScreen()
        {
            m_enemyStoredPositions = new List<Vector2>();
        }

        public float BackBufferOpacity { get; set; }

        public override void PassInData(List<object> objList)
        {
            if (objList != null)
            {
                m_player = objList[0] as PlayerObj;
                if (m_playerFallSound == null)
                {
                    m_playerFallSound = new FrameSoundObj(m_player, 14, "Player_Death_BodyFall");
                    m_playerSwordSpinSound = new FrameSoundObj(m_player, 2, "Player_Death_SwordTwirl");
                    m_playerSwordFallSound = new FrameSoundObj(m_player, 9, "Player_Death_SwordLand");
                }

                m_enemyList = objList[1] as List<EnemyObj>;
                m_coinsCollected = (int) objList[2];
                m_bagsCollected = (int) objList[3];
                m_diamondsCollected = (int) objList[4];
                if (objList[5] != null)
                {
                    m_objKilledPlayer = objList[5] as GameObj;
                }

                var cause = SetObjectKilledPlayerText();
                m_enemyStoredPositions.Clear();

                // Do not send a death link if what is killing us is a DeathLink.
                if (!(m_objKilledPlayer is DeathLinkObj))
                {
                    Program.Game.ArchipelagoManager.SendDeathLink(cause);
                }

                base.PassInData(objList);
            }
        }

        public override void LoadContent()
        {
            m_continueText = new KeyIconTextObj(Game.JunicodeFont);
            m_continueText.FontSize = 14f;
            m_continueText.Align = Types.TextAlign.Right;
            m_continueText.Opacity = 0f;
            m_continueText.Position = new Vector2(1270f, 30f);
            m_continueText.ForceDraw = true;
            var dropShadow = new Vector2(2f, 2f);
            var textureColor = new Color(255, 254, 128);
            m_dialoguePlate = new ObjContainer("DialogBox_Character");
            m_dialoguePlate.Position = new Vector2(660f, 610f);
            m_dialoguePlate.ForceDraw = true;
            var textObj = new TextObj(Game.JunicodeFont);
            textObj.Align = Types.TextAlign.Centre;
            textObj.Text = "Your valor shown in battle shall never be forgotten.";
            textObj.FontSize = 17f;
            textObj.DropShadow = dropShadow;
            textObj.Position = new Vector2(0f, -(float) m_dialoguePlate.Height / 2 + 25);
            m_dialoguePlate.AddChild(textObj);
            var keyIconTextObj = new KeyIconTextObj(Game.JunicodeFont);
            keyIconTextObj.FontSize = 12f;
            keyIconTextObj.Align = Types.TextAlign.Centre;
            keyIconTextObj.Text = "\"Arrrrggghhhh\"";
            keyIconTextObj.DropShadow = dropShadow;
            keyIconTextObj.Y = 0f;
            keyIconTextObj.TextureColor = textureColor;
            m_dialoguePlate.AddChild(keyIconTextObj);
            var textObj2 = new TextObj(Game.JunicodeFont);
            textObj2.FontSize = 8f;
            textObj2.Text = "-Player X's parting words";
            textObj2.Y = keyIconTextObj.Y;
            textObj2.Y += 40f;
            textObj2.X += 20f;
            textObj2.DropShadow = dropShadow;
            m_dialoguePlate.AddChild(textObj2);
            m_playerGhost = new SpriteObj("PlayerGhost_Sprite");
            m_playerGhost.AnimationDelay = 0.1f;
            m_spotlight = new SpriteObj("GameOverSpotlight_Sprite");
            m_spotlight.Rotation = 90f;
            m_spotlight.ForceDraw = true;
            m_spotlight.Position = new Vector2(660f, 40 + m_spotlight.Height);
            m_playerFrame = new LineageObj(null, true);
            m_playerFrame.DisablePlaque = true;
            base.LoadContent();
        }

        public override void OnEnter()
        {
            m_playerFrame.Opacity = 0f;
            m_playerFrame.Position = m_player.Position;
            m_playerFrame.SetTraits(Game.PlayerStats.Traits);
            m_playerFrame.IsFemale = Game.PlayerStats.IsFemale;
            m_playerFrame.Class = Game.PlayerStats.Class;
            m_playerFrame.Y -= 120f;
            m_playerFrame.SetPortrait(Game.PlayerStats.HeadPiece, Game.PlayerStats.ShoulderPiece,
                Game.PlayerStats.ChestPiece);
            m_playerFrame.UpdateData();
            Tween.To(m_playerFrame, 1f, Tween.EaseNone, "delay", "4", "Opacity", "1");
            var item = new FamilyTreeNode
            {
                Name = Game.PlayerStats.PlayerName,
                Age = Game.PlayerStats.Age,
                ChildAge = Game.PlayerStats.ChildAge,
                Class = Game.PlayerStats.Class,
                HeadPiece = Game.PlayerStats.HeadPiece,
                ChestPiece = Game.PlayerStats.ChestPiece,
                ShoulderPiece = Game.PlayerStats.ShoulderPiece,
                NumEnemiesBeaten = Game.PlayerStats.NumEnemiesBeaten,
                BeatenABoss = Game.PlayerStats.NewBossBeaten,
                Traits = Game.PlayerStats.Traits,
                IsFemale = Game.PlayerStats.IsFemale
            };
            var traits = Game.PlayerStats.Traits;
            Game.PlayerStats.FamilyTreeArray.Add(item);
            if (Game.PlayerStats.CurrentBranches != null)
            {
                Game.PlayerStats.CurrentBranches.Clear();
            }

            Game.PlayerStats.IsDead = true;
            Game.PlayerStats.Traits = Vector2.Zero;
            Game.PlayerStats.NewBossBeaten = false;
            Game.PlayerStats.RerolledChildren = false;
            Game.PlayerStats.HasArchitectFee = false;
            Game.PlayerStats.NumEnemiesBeaten = 0;
            Game.PlayerStats.LichHealth = 0;
            Game.PlayerStats.LichMana = 0;
            Game.PlayerStats.LichHealthMod = 1f;
            Game.PlayerStats.TimesDead++;
            Game.PlayerStats.LoadStartingRoom = true;
            Game.PlayerStats.EnemiesKilledInRun.Clear();
            if (Game.PlayerStats.SpecialItem != 1 && Game.PlayerStats.SpecialItem != 9 &&
                Game.PlayerStats.SpecialItem != 10 && Game.PlayerStats.SpecialItem != 11 &&
                Game.PlayerStats.SpecialItem != 12 && Game.PlayerStats.SpecialItem != 13)
            {
                Game.PlayerStats.SpecialItem = 0;
            }

            (ScreenManager.Game as Game).SaveManager.SaveFiles(SaveType.PlayerData, SaveType.Lineage, SaveType.MapData);
            (ScreenManager.Game as Game).SaveManager.SaveAllFileTypes(true);
            Game.PlayerStats.Traits = traits;
            if (Game.PlayerStats.TimesDead >= 20)
            {
                GameUtil.UnlockAchievement("FEAR_OF_LIFE");
            }

            SoundManager.StopMusic(0.5f);
            m_droppingStats = false;
            m_lockControls = false;
            SoundManager.PlaySound("Player_Death_FadeToBlack");
            m_continueText.Text = "Press [Input:" + 0 + "] to move on";
            m_player.Visible = true;
            m_player.Opacity = 1f;
            m_continueText.Opacity = 0f;
            m_dialoguePlate.Opacity = 0f;
            m_playerGhost.Opacity = 0f;
            m_spotlight.Opacity = 0f;
            m_playerGhost.Position = new Vector2(m_player.X - m_playerGhost.Width / 2, m_player.Bounds.Top - 20);
            Tween.RunFunction(3f, typeof(SoundManager), "PlaySound", "Player_Ghost");
            Tween.To(m_playerGhost, 0.5f, Linear.EaseNone, "delay", "3", "Opacity", "0.4");
            Tween.By(m_playerGhost, 2f, Linear.EaseNone, "delay", "3", "Y", "-150");
            m_playerGhost.Opacity = 0.4f;
            Tween.To(m_playerGhost, 0.5f, Linear.EaseNone, "delay", "4", "Opacity", "0");
            m_playerGhost.Opacity = 0f;
            m_playerGhost.PlayAnimation();
            Tween.To(this, 0.5f, Linear.EaseNone, "BackBufferOpacity", "1");
            Tween.To(m_spotlight, 0.1f, Linear.EaseNone, "delay", "1", "Opacity", "1");
            Tween.AddEndHandlerToLastTween(typeof(SoundManager), "PlaySound", "Player_Death_Spotlight");
            Tween.RunFunction(1.2f, typeof(SoundManager), "PlayMusic", "GameOverStinger", false, 0.5f);
            Tween.To(Camera, 1f, Quad.EaseInOut, "X", m_player.AbsX.ToString(), "Y",
                (m_player.Bounds.Bottom - 10).ToString(), "Zoom", "1");
            Tween.RunFunction(2f, m_player, "RunDeathAnimation1");
            if (Game.PlayerStats.Traits.X == 13f || Game.PlayerStats.Traits.Y == 13f)
            {
                (m_dialoguePlate.GetChildAt(2) as TextObj).Text = "#)!(%*#@!%^";
                (m_dialoguePlate.GetChildAt(2) as TextObj).RandomizeSentence(true);
            }
            else
            {
                (m_dialoguePlate.GetChildAt(2) as TextObj).Text =
                    GameEV.GAME_HINTS[CDGMath.RandomInt(0, GameEV.GAME_HINTS.Length - 1)];
            }

            (m_dialoguePlate.GetChildAt(3) as TextObj).Text = "-" + Game.PlayerStats.PlayerName + "'s Parting Words";
            Tween.To(m_dialoguePlate, 0.5f, Tween.EaseNone, "delay", "2", "Opacity", "1");
            Tween.RunFunction(4f, this, "DropStats");
            Tween.To(m_continueText, 0.4f, Linear.EaseNone, "delay", "4", "Opacity", "1");
            base.OnEnter();
        }

        public override void OnExit()
        {
            Tween.StopAll(false);
            if (m_enemyList != null)
            {
                m_enemyList.Clear();
                m_enemyList = null;
            }

            Game.PlayerStats.Traits = Vector2.Zero;
            BackBufferOpacity = 0f;
            base.OnExit();
        }

        public void DropStats()
        {
            m_droppingStats = true;
            var arg_0C_0 = Vector2.Zero;
            var num = 0f;
            var topLeftCorner = Camera.TopLeftCorner;
            topLeftCorner.X += 200f;
            topLeftCorner.Y += 450f;
            if (m_enemyList != null)
            {
                foreach (var current in m_enemyList)
                {
                    m_enemyStoredPositions.Add(current.Position);
                    current.Position = topLeftCorner;
                    current.ChangeSprite(current.ResetSpriteName);
                    if (current.SpriteName == "EnemyZombieRise_Character")
                    {
                        current.ChangeSprite("EnemyZombieWalk_Character");
                    }

                    current.Visible = true;
                    current.Flip = SpriteEffects.FlipHorizontally;
                    Tween.StopAllContaining(current, false);
                    current.Scale = current.InternalScale;
                    current.Scale /= 2f;
                    current.Opacity = 0f;
                    num += 0.05f;
                    var enemyObj_Eyeball = current as EnemyObj_Eyeball;
                    if (enemyObj_Eyeball != null && enemyObj_Eyeball.Difficulty == EnemyDifficulty.MiniBoss)
                    {
                        enemyObj_Eyeball.ChangeToBossPupil();
                    }

                    Tween.To(current, 0f, Tween.EaseNone, "delay", num.ToString(), "Opacity", "1");
                    Tween.RunFunction(num, this, "PlayEnemySound");
                    topLeftCorner.X += 25f;
                    if (current.X + current.Width > Camera.TopLeftCorner.X + 200f + 950f)
                    {
                        topLeftCorner.Y += 30f;
                        topLeftCorner.X = Camera.TopLeftCorner.X + 200f;
                    }
                }
            }
        }

        public void PlayEnemySound()
        {
            SoundManager.PlaySound("Enemy_Kill_Plant");
        }

        private string SetObjectKilledPlayerText()
        {
            var textObj = m_dialoguePlate.GetChildAt(1) as TextObj;
            if (m_objKilledPlayer != null)
            {
                var enemyObj = m_objKilledPlayer as EnemyObj;
                var projectileObj = m_objKilledPlayer as ProjectileObj;
                var deathLinkObj = m_objKilledPlayer as DeathLinkObj;
                if (enemyObj != null)
                {
                    if (enemyObj.Difficulty == EnemyDifficulty.MiniBoss || enemyObj is EnemyObj_LastBoss)
                    {
                        textObj.Text = Game.PlayerStats.PlayerName + " has been slain by " + enemyObj.Name;
                    }
                    else
                    {
                        textObj.Text = Game.PlayerStats.PlayerName + " has been slain by a " + enemyObj.Name;
                    }
                }
                else if (projectileObj != null)
                {
                    enemyObj = projectileObj.Source as EnemyObj;
                    if (enemyObj != null)
                    {
                        if (enemyObj.Difficulty == EnemyDifficulty.MiniBoss || enemyObj is EnemyObj_LastBoss)
                        {
                            textObj.Text = Game.PlayerStats.PlayerName + " has been slain by " + enemyObj.Name;
                        }
                        else
                        {
                            textObj.Text = Game.PlayerStats.PlayerName + " has been slain by a " + enemyObj.Name;
                        }
                    }
                    else
                    {
                        textObj.Text = Game.PlayerStats.PlayerName + " was done in by a projectile";
                    }
                }
                else if (deathLinkObj != null)
                {
                    textObj.Text = Game.PlayerStats.PlayerName + " was done in by " + deathLinkObj.Name +
                                   "'s carelessness";
                }

                var hazardObj = m_objKilledPlayer as HazardObj;
                if (hazardObj != null)
                {
                    textObj.Text = Game.PlayerStats.PlayerName + " slipped and was impaled by spikes";
                }
            }
            else
            {
                textObj.Text = Game.PlayerStats.PlayerName + " has been slain";
            }

            return textObj.Text;
        }

        public override void HandleInput()
        {
            if (!m_lockControls && m_droppingStats &&
                (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1) ||
                 Game.GlobalInput.JustPressed(2) ||
                 Game.GlobalInput.JustPressed(3)))
            {
                if (m_enemyList.Count > 0 && m_enemyList[m_enemyList.Count - 1].Opacity != 1f)
                {
                    foreach (var current in m_enemyList)
                    {
                        Tween.StopAllContaining(current, false);
                        current.Opacity = 1f;
                    }

                    Tween.StopAllContaining(this, false);
                    PlayEnemySound();
                }
                else
                {
                    SkillSystem.ResetAllTraits();
                    var chests = Game.PlayerStats.OpenedChests;
                    var received = Game.PlayerStats.ReceivedItems;
                    Game.PlayerStats.Dispose();
                    Game.PlayerStats = new PlayerStats();
                    (ScreenManager as RCScreenManager).Player.Reset();
                    (ScreenManager.Game as Game).SaveManager.LoadFiles(null, SaveType.PlayerData, SaveType.Lineage,
                        SaveType.UpgradeData);
                    Game.ScreenManager.Player.CurrentHealth = Game.PlayerStats.CurrentHealth;
                    Game.ScreenManager.Player.CurrentMana = Game.PlayerStats.CurrentMana;
                    Game.PlayerStats.OpenedChests = chests;
                    Game.PlayerStats.ReceivedItems = received;
                    (ScreenManager as RCScreenManager).DisplayScreen(ScreenType.Lineage, true);
                    m_lockControls = true;
                }
            }

            base.HandleInput();
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.JustPressed(Keys.Space, PlayerIndex.One))
            {
                (m_dialoguePlate.GetChildAt(2) as TextObj).Text =
                    GameEV.GAME_HINTS[CDGMath.RandomInt(0, GameEV.GAME_HINTS.Length - 1)];
            }

            if (m_player.SpriteName == "PlayerDeath_Character")
            {
                m_playerFallSound.Update();
                m_playerSwordFallSound.Update();
                m_playerSwordSpinSound.Update();
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null,
                Camera.GetTransformation());
            Camera.Draw(Game.GenericTexture,
                new Rectangle((int) Camera.TopLeftCorner.X - 10, (int) Camera.TopLeftCorner.Y - 10, 1420, 820),
                Color.Black * BackBufferOpacity);
            foreach (var current in m_enemyList) current.Draw(Camera);

            m_playerFrame.Draw(Camera);
            m_player.Draw(Camera);
            if (m_playerGhost.Opacity > 0f)
            {
                m_playerGhost.X += (float) Math.Sin(Game.TotalGameTimeSeconds * 5f) * 60f *
                                   (float) gameTime.ElapsedGameTime.TotalSeconds;
            }

            m_playerGhost.Draw(Camera);
            Camera.End();
            Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null);
            m_spotlight.Draw(Camera);
            m_dialoguePlate.Draw(Camera);
            m_continueText.Draw(Camera);
            Camera.End();
            base.Draw(gameTime);
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                Console.WriteLine("Disposing Game Over Screen");
                m_player = null;
                m_dialoguePlate.Dispose();
                m_dialoguePlate = null;
                m_continueText.Dispose();
                m_continueText = null;
                m_playerGhost.Dispose();
                m_playerGhost = null;
                m_spotlight.Dispose();
                m_spotlight = null;
                m_playerFallSound.Dispose();
                m_playerFallSound = null;
                m_playerSwordFallSound.Dispose();
                m_playerSwordFallSound = null;
                m_playerSwordSpinSound.Dispose();
                m_playerSwordSpinSound = null;
                m_objKilledPlayer = null;
                if (m_enemyList != null)
                {
                    m_enemyList.Clear();
                }

                m_enemyList = null;
                if (m_enemyStoredPositions != null)
                {
                    m_enemyStoredPositions.Clear();
                }

                m_enemyStoredPositions = null;
                m_playerFrame.Dispose();
                m_playerFrame = null;
                base.Dispose();
            }
        }
    }
}
