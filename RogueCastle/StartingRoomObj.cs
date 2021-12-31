// 
// RogueLegacyArchipelago - StartingRoomObj.cs
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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using RogueCastle.Structs;
using Tweener;
using Tweener.Ease;

namespace RogueCastle
{
    public class StartingRoomObj : RoomObj
    {
        private const int ENCHANTRESS_HEAD_LAYER = 4;
        private const byte ARCHITECT_HEAD_LAYER = 1;
        private ObjContainer m_architect;
        private TerrainObj m_architectBlock;
        private SpriteObj m_architectIcon;
        private readonly Vector2 m_architectIconPosition;
        private bool m_architectRenovating;
        private BlacksmithObj m_blacksmith;
        private FrameSoundObj m_blacksmithAnvilSound;
        private TerrainObj m_blacksmithBlock;
        private SpriteObj m_blacksmithBoard;
        private SpriteObj m_blacksmithIcon;
        private readonly Vector2 m_blacksmithIconPosition;
        private SpriteObj m_blacksmithNewIcon;
        private bool m_controlsLocked;
        private ObjContainer m_enchantress;
        private TerrainObj m_enchantressBlock;
        private SpriteObj m_enchantressIcon;
        private readonly Vector2 m_enchantressIconPosition;
        private SpriteObj m_enchantressNewIcon;
        private GameObj m_fern1;
        private GameObj m_fern2;
        private GameObj m_fern3;
        private bool m_horizontalShake;
        private bool m_isRaining;
        private bool m_isSnowing;
        private float m_lightningTimer;
        private SpriteObj m_mountain1;
        private SpriteObj m_mountain2;
        private bool m_playerWalkedOut;
        private List<RaindropObj> m_rainFG;
        private Cue m_rainSFX;
        private float m_screenShakeCounter;
        private float m_screenShakeMagnitude;
        private SpriteObj m_screw;
        private bool m_shakeScreen;
        private Vector2 m_shakeStartingPos;
        private SpriteObj m_tent;
        private PhysicsObjContainer m_tollCollector;
        private SpriteObj m_tollCollectorIcon;
        private GameObj m_tree1;
        private GameObj m_tree2;
        private GameObj m_tree3;
        private bool m_verticalShake;

        public StartingRoomObj()
        {
            m_blacksmith = new BlacksmithObj();
            m_blacksmith.Flip = SpriteEffects.FlipHorizontally;
            m_blacksmith.Scale = new Vector2(2.5f, 2.5f);
            m_blacksmith.Position = new Vector2(700f, 660f - (m_blacksmith.Bounds.Bottom - m_blacksmith.Y) - 1f);
            m_blacksmith.OutlineWidth = 2;
            m_blacksmithBoard = new SpriteObj("StartRoomBlacksmithBoard_Sprite");
            m_blacksmithBoard.Scale = new Vector2(2f, 2f);
            m_blacksmithBoard.OutlineWidth = 2;
            m_blacksmithBoard.Position = new Vector2(m_blacksmith.X - m_blacksmithBoard.Width / 2 - 35f,
                m_blacksmith.Bounds.Bottom - m_blacksmithBoard.Height - 1);
            m_blacksmithIcon = new SpriteObj("UpArrowBubble_Sprite");
            m_blacksmithIcon.Scale = new Vector2(2f, 2f);
            m_blacksmithIcon.Visible = false;
            m_blacksmithIconPosition = new Vector2(m_blacksmith.X - 60f, m_blacksmith.Y - 10f);
            m_blacksmithIcon.Flip = m_blacksmith.Flip;
            m_blacksmithIcon.OutlineWidth = 2;
            m_blacksmithNewIcon = new SpriteObj("ExclamationSquare_Sprite");
            m_blacksmithNewIcon.Visible = false;
            m_blacksmithNewIcon.OutlineWidth = 2;
            m_enchantressNewIcon = m_blacksmithNewIcon.Clone() as SpriteObj;
            m_enchantress = new ObjContainer("Enchantress_Character");
            m_enchantress.Scale = new Vector2(2f, 2f);
            m_enchantress.Flip = SpriteEffects.FlipHorizontally;
            m_enchantress.Position = new Vector2(1150f,
                660f - (m_enchantress.Bounds.Bottom - m_enchantress.AnchorY) - 2f);
            m_enchantress.PlayAnimation();
            m_enchantress.AnimationDelay = 0.1f;
            (m_enchantress.GetChildAt(4) as IAnimateableObj).StopAnimation();
            m_enchantress.OutlineWidth = 2;
            m_tent = new SpriteObj("StartRoomGypsyTent_Sprite");
            m_tent.Scale = new Vector2(1.5f, 1.5f);
            m_tent.OutlineWidth = 2;
            m_tent.Position = new Vector2(m_enchantress.X - m_tent.Width / 2 + 5f,
                m_enchantress.Bounds.Bottom - m_tent.Height);
            m_enchantressIcon = new SpriteObj("UpArrowBubble_Sprite");
            m_enchantressIcon.Scale = new Vector2(2f, 2f);
            m_enchantressIcon.Visible = false;
            m_enchantressIconPosition = new Vector2(m_enchantress.X - 60f, m_enchantress.Y - 100f);
            m_enchantressIcon.Flip = m_enchantress.Flip;
            m_enchantressIcon.OutlineWidth = 2;
            m_architect = new ObjContainer("ArchitectIdle_Character");
            m_architect.Flip = SpriteEffects.FlipHorizontally;
            m_architect.Scale = new Vector2(2f, 2f);
            m_architect.Position = new Vector2(1550f, 660f - (m_architect.Bounds.Bottom - m_architect.AnchorY) - 2f);
            m_architect.PlayAnimation();
            m_architect.AnimationDelay = 0.1f;
            m_architect.OutlineWidth = 2;
            (m_architect.GetChildAt(1) as IAnimateableObj).StopAnimation();
            m_architectIcon = new SpriteObj("UpArrowBubble_Sprite");
            m_architectIcon.Scale = new Vector2(2f, 2f);
            m_architectIcon.Visible = false;
            m_architectIconPosition = new Vector2(m_architect.X - 60f, m_architect.Y - 100f);
            m_architectIcon.Flip = m_architect.Flip;
            m_architectIcon.OutlineWidth = 2;
            m_architectRenovating = false;
            m_screw = new SpriteObj("ArchitectGear_Sprite");
            m_screw.Scale = new Vector2(2f, 2f);
            m_screw.OutlineWidth = 2;
            m_screw.Position = new Vector2(m_architect.X + 30f, m_architect.Bounds.Bottom - 1);
            m_screw.AnimationDelay = 0.1f;
            m_tollCollector = new PhysicsObjContainer("NPCTollCollectorIdle_Character");
            m_tollCollector.Flip = SpriteEffects.FlipHorizontally;
            m_tollCollector.Scale = new Vector2(2.5f, 2.5f);
            m_tollCollector.IsWeighted = false;
            m_tollCollector.IsCollidable = true;
            m_tollCollector.Position = new Vector2(2565f,
                420f - (m_tollCollector.Bounds.Bottom - m_tollCollector.AnchorY));
            m_tollCollector.PlayAnimation();
            m_tollCollector.AnimationDelay = 0.1f;
            m_tollCollector.OutlineWidth = 2;
            m_tollCollector.CollisionTypeTag = 1;
            m_tollCollectorIcon = new SpriteObj("UpArrowBubble_Sprite");
            m_tollCollectorIcon.Scale = new Vector2(2f, 2f);
            m_tollCollectorIcon.Visible = false;
            m_tollCollectorIcon.Flip = m_tollCollector.Flip;
            m_tollCollectorIcon.OutlineWidth = 2;
            m_rainFG = new List<RaindropObj>();
            var num = 400;
            if (LevelENV.SaveFrames)
            {
                num /= 2;
            }

            for (var i = 0; i < num; i++)
            {
                var item =
                    new RaindropObj(new Vector2(CDGMath.RandomInt(-100, 2540), CDGMath.RandomInt(-400, 720)));
                m_rainFG.Add(item);
            }
        }

        private bool BlacksmithNewIconVisible
        {
            get
            {
                foreach (var current in Game.PlayerStats.GetBlueprintArray)
                {
                    var array = current;
                    for (var i = 0; i < array.Length; i++)
                    {
                        var b = array[i];
                        if (b == 1)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        private bool EnchantressNewIconVisible
        {
            get
            {
                foreach (var current in Game.PlayerStats.GetRuneArray)
                {
                    var array = current;
                    for (var i = 0; i < array.Length; i++)
                    {
                        var b = array[i];
                        if (b == 1)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        private bool SmithyAvailable
        {
            get { return SkillSystem.GetSkill(SkillType.Smithy).ModifierAmount > 0f; }
        }

        private bool EnchantressAvailable
        {
            get { return SkillSystem.GetSkill(SkillType.Enchanter).ModifierAmount > 0f; }
        }

        private bool ArchitectAvailable
        {
            get { return SkillSystem.GetSkill(SkillType.Architect).ModifierAmount > 0f; }
        }

        private bool TollCollectorAvailable
        {
            get { return Game.PlayerStats.TimesDead > 0 && m_tollCollector.Visible; }
        }

        public override void Initialize()
        {
            foreach (var current in TerrainObjList)
            {
                if (current.Name == "BlacksmithBlock")
                {
                    m_blacksmithBlock = current;
                }

                if (current.Name == "EnchantressBlock")
                {
                    m_enchantressBlock = current;
                }

                if (current.Name == "ArchitectBlock")
                {
                    m_architectBlock = current;
                }

                if (current.Name == "bridge")
                {
                    current.ShowTerrain = false;
                }
            }

            for (var i = 0; i < GameObjList.Count; i++)
            {
                if (GameObjList[i].Name == "Mountains 1")
                {
                    m_mountain1 = GameObjList[i] as SpriteObj;
                }

                if (GameObjList[i].Name == "Mountains 2")
                {
                    m_mountain2 = GameObjList[i] as SpriteObj;
                }
            }

            base.Initialize();
        }

        public override void LoadContent(GraphicsDevice graphics)
        {
            if (m_tree1 == null)
            {
                foreach (var current in GameObjList)
                    if (current.Name == "Tree1")
                    {
                        m_tree1 = current;
                    }
                    else if (current.Name == "Tree2")
                    {
                        m_tree2 = current;
                    }
                    else if (current.Name == "Tree3")
                    {
                        m_tree3 = current;
                    }
                    else if (current.Name == "Fern1")
                    {
                        m_fern1 = current;
                    }
                    else if (current.Name == "Fern2")
                    {
                        m_fern2 = current;
                    }
                    else if (current.Name == "Fern3")
                    {
                        m_fern3 = current;
                    }
            }

            base.LoadContent(graphics);
        }

        public override void OnEnter()
        {
            if (Game.PlayerStats.SpecialItem == 9 && Game.PlayerStats.ChallengeEyeballBeaten)
            {
                Game.PlayerStats.SpecialItem = 0;
            }

            if (Game.PlayerStats.SpecialItem == 10 && Game.PlayerStats.ChallengeSkullBeaten)
            {
                Game.PlayerStats.SpecialItem = 0;
            }

            if (Game.PlayerStats.SpecialItem == 11 && Game.PlayerStats.ChallengeFireballBeaten)
            {
                Game.PlayerStats.SpecialItem = 0;
            }

            if (Game.PlayerStats.SpecialItem == 12 && Game.PlayerStats.ChallengeBlobBeaten)
            {
                Game.PlayerStats.SpecialItem = 0;
            }

            if (Game.PlayerStats.SpecialItem == 13 && Game.PlayerStats.ChallengeLastBossBeaten)
            {
                Game.PlayerStats.SpecialItem = 0;
            }

            Player.AttachedLevel.UpdatePlayerHUDSpecialItem();
            m_isSnowing = DateTime.Now.Month == 12 || DateTime.Now.Month == 1;
            if (m_isSnowing)
            {
                foreach (var current in m_rainFG) current.ChangeToSnowflake();
            }

            if (!(Game.ScreenManager.Game as Game).SaveManager.FileExists(SaveType.Map) &&
                Game.PlayerStats.HasArchitectFee)
            {
                Game.PlayerStats.HasArchitectFee = false;
            }

            Game.PlayerStats.TutorialComplete = true;
            Game.PlayerStats.IsDead = false;
            m_lightningTimer = 5f;
            Player.CurrentHealth = Player.MaxHealth;
            Player.CurrentMana = Player.MaxMana;
            Player.ForceInvincible = false;
            (Player.AttachedLevel.ScreenManager.Game as Game).SaveManager.SaveFiles(SaveType.PlayerData);
            if (TollCollectorAvailable && !Player.AttachedLevel.PhysicsManager.ObjectList.Contains(m_tollCollector))
            {
                Player.AttachedLevel.PhysicsManager.AddObject(m_tollCollector);
            }

            if (m_blacksmithAnvilSound == null)
            {
                m_blacksmithAnvilSound = new FrameSoundObj(m_blacksmith.GetChildAt(5) as IAnimateableObj, Player, 7,
                    "Anvil1", "Anvil2", "Anvil3");
            }

            if (Game.PlayerStats.Traits.X == 35f || Game.PlayerStats.Traits.Y == 35f)
            {
                Game.ShadowEffect.Parameters["ShadowIntensity"].SetValue(0.7f);
            }
            else
            {
                Game.ShadowEffect.Parameters["ShadowIntensity"].SetValue(0);
            }

            m_playerWalkedOut = false;
            Player.UpdateCollisionBoxes();
            Player.Position = new Vector2(0f, 660f - (Player.Bounds.Bottom - Player.Y));
            Player.State = 1;
            Player.IsWeighted = false;
            Player.IsCollidable = false;
            var logicSet = new LogicSet(Player);
            logicSet.AddAction(new RunFunctionLogicAction(Player, "LockControls"));
            logicSet.AddAction(new MoveDirectionLogicAction(new Vector2(1f, 0f)));
            logicSet.AddAction(new ChangeSpriteLogicAction("PlayerWalking_Character"));
            logicSet.AddAction(new PlayAnimationLogicAction());
            logicSet.AddAction(new DelayLogicAction(0.5f));
            logicSet.AddAction(new ChangePropertyLogicAction(Player, "CurrentSpeed", 0));
            logicSet.AddAction(new ChangePropertyLogicAction(Player, "IsWeighted", true));
            logicSet.AddAction(new ChangePropertyLogicAction(Player, "IsCollidable", true));
            Player.RunExternalLogicSet(logicSet);
            Tween.By(this, 1f, Linear.EaseNone);
            Tween.AddEndHandlerToLastTween(Player, "UnlockControls");
            SoundManager.StopMusic(1f);
            m_isRaining = CDGMath.RandomPlusMinus() > 0;
            m_isRaining = true;
            if (m_isRaining)
            {
                if (m_rainSFX != null)
                {
                    m_rainSFX.Dispose();
                }

                if (!m_isSnowing)
                {
                    m_rainSFX = SoundManager.PlaySound("Rain1");
                }
                else
                {
                    m_rainSFX = SoundManager.PlaySound("snowloop_filtered");
                }
            }

            m_tent.TextureColor = new Color(200, 200, 200);
            m_blacksmithBoard.TextureColor = new Color(200, 200, 200);
            m_screw.TextureColor = new Color(200, 200, 200);
            if (Game.PlayerStats.LockCastle)
            {
                m_screw.GoToFrame(m_screw.TotalFrames);
                m_architectBlock.Position = new Vector2(1492f, 579f);
            }
            else
            {
                m_screw.GoToFrame(1);
                m_architectBlock.Position = new Vector2(1492f, 439f);
            }

            Player.UpdateEquipmentColours();
            base.OnEnter();
        }

        public override void OnExit()
        {
            if (m_rainSFX != null && !m_rainSFX.IsDisposed)
            {
                m_rainSFX.Stop(AudioStopOptions.Immediate);
            }
        }

        public override void Update(GameTime gameTime)
        {
            Player.CurrentMana = Player.MaxMana;
            Player.CurrentHealth = Player.MaxHealth;
            m_enchantressBlock.Visible = EnchantressAvailable;
            m_blacksmithBlock.Visible = SmithyAvailable;
            m_architectBlock.Visible = ArchitectAvailable;
            var totalGameTime = Game.TotalGameTimeSeconds;
            if (!m_playerWalkedOut)
            {
                if (!Player.ControlsLocked && Player.X < Bounds.Left)
                {
                    m_playerWalkedOut = true;
                    (Player.AttachedLevel.ScreenManager as RCScreenManager).StartWipeTransition();
                    Tween.RunFunction(0.2f, Player.AttachedLevel.ScreenManager, "DisplayScreen", 6, true,
                        typeof(List<object>));
                }
                else if (!Player.ControlsLocked && Player.X > Bounds.Right && !TollCollectorAvailable)
                {
                    m_playerWalkedOut = true;
                    LoadLevel();
                }
            }

            if (m_isRaining)
            {
                foreach (var current in TerrainObjList) current.UseCachedValues = true;
                foreach (var current2 in m_rainFG) current2.Update(TerrainObjList, gameTime);
            }

            m_tree1.Rotation = -(float) Math.Sin(totalGameTime) * 2f;
            m_tree2.Rotation = (float) Math.Sin(totalGameTime * 2f);
            m_tree3.Rotation = (float) Math.Sin(totalGameTime * 2f) * 2f;
            m_fern1.Rotation = (float) Math.Sin(totalGameTime * 3f) / 2f;
            m_fern2.Rotation = -(float) Math.Sin(totalGameTime * 4f);
            m_fern3.Rotation = (float) Math.Sin(totalGameTime * 4f) / 2f;
            if (!m_architectRenovating)
            {
                HandleInput();
            }

            if (SmithyAvailable)
            {
                if (m_blacksmithAnvilSound != null)
                {
                    m_blacksmithAnvilSound.Update();
                }

                m_blacksmith.Update(gameTime);
            }

            m_blacksmithIcon.Visible = false;
            if (Player != null && CollisionMath.Intersects(Player.TerrainBounds, m_blacksmith.Bounds) &&
                Player.IsTouchingGround && SmithyAvailable)
            {
                m_blacksmithIcon.Visible = true;
            }

            m_blacksmithIcon.Position = new Vector2(m_blacksmithIconPosition.X,
                m_blacksmithIconPosition.Y - 70f + (float) Math.Sin(totalGameTime * 20f) * 2f);
            m_enchantressIcon.Visible = false;
            var b = new Rectangle((int) (m_enchantress.X - 100f), (int) m_enchantress.Y,
                m_enchantress.Bounds.Width + 100, m_enchantress.Bounds.Height);
            if (Player != null && CollisionMath.Intersects(Player.TerrainBounds, b) && Player.IsTouchingGround &&
                EnchantressAvailable)
            {
                m_enchantressIcon.Visible = true;
            }

            m_enchantressIcon.Position = new Vector2(m_enchantressIconPosition.X + 20f,
                m_enchantressIconPosition.Y + (float) Math.Sin(totalGameTime * 20f) * 2f);
            if (Player != null &&
                CollisionMath.Intersects(Player.TerrainBounds,
                    new Rectangle((int) m_architect.X - 100, (int) m_architect.Y, m_architect.Width + 200,
                        m_architect.Height)) && Player.X < m_architect.X && Player.Flip == SpriteEffects.None &&
                ArchitectAvailable)
            {
                m_architectIcon.Visible = true;
            }
            else
            {
                m_architectIcon.Visible = false;
            }

            m_architectIcon.Position = new Vector2(m_architectIconPosition.X,
                m_architectIconPosition.Y + (float) Math.Sin(totalGameTime * 20f) * 2f);
            if (Player != null &&
                CollisionMath.Intersects(Player.TerrainBounds,
                    new Rectangle((int) m_tollCollector.X - 100, (int) m_tollCollector.Y, m_tollCollector.Width + 200,
                        m_tollCollector.Height)) && Player.X < m_tollCollector.X && Player.Flip == SpriteEffects.None &&
                TollCollectorAvailable && m_tollCollector.SpriteName == "NPCTollCollectorIdle_Character")
            {
                m_tollCollectorIcon.Visible = true;
            }
            else
            {
                m_tollCollectorIcon.Visible = false;
            }

            m_tollCollectorIcon.Position = new Vector2(m_tollCollector.X - m_tollCollector.Width / 2 - 10f,
                m_tollCollector.Y - m_tollCollectorIcon.Height - m_tollCollector.Height / 2 +
                (float) Math.Sin(totalGameTime * 20f) * 2f);
            m_blacksmithNewIcon.Visible = false;
            if (SmithyAvailable)
            {
                if (m_blacksmithIcon.Visible && m_blacksmithNewIcon.Visible)
                {
                    m_blacksmithNewIcon.Visible = false;
                }
                else if (!m_blacksmithIcon.Visible && BlacksmithNewIconVisible)
                {
                    m_blacksmithNewIcon.Visible = true;
                }

                m_blacksmithNewIcon.Position = new Vector2(m_blacksmithIcon.X + 50f, m_blacksmithIcon.Y - 30f);
            }

            m_enchantressNewIcon.Visible = false;
            if (EnchantressAvailable)
            {
                if (m_enchantressIcon.Visible && m_enchantressNewIcon.Visible)
                {
                    m_enchantressNewIcon.Visible = false;
                }
                else if (!m_enchantressIcon.Visible && EnchantressNewIconVisible)
                {
                    m_enchantressNewIcon.Visible = true;
                }

                m_enchantressNewIcon.Position = new Vector2(m_enchantressIcon.X + 40f, m_enchantressIcon.Y - 0f);
            }

            if (m_isRaining && !m_isSnowing && m_lightningTimer > 0f)
            {
                m_lightningTimer -= (float) gameTime.ElapsedGameTime.TotalSeconds;
                if (m_lightningTimer <= 0f)
                {
                    if (CDGMath.RandomInt(0, 100) > 70)
                    {
                        if (CDGMath.RandomInt(0, 1) > 0)
                        {
                            Player.AttachedLevel.LightningEffectTwice();
                        }
                        else
                        {
                            Player.AttachedLevel.LightningEffectOnce();
                        }
                    }

                    m_lightningTimer = 5f;
                }
            }

            if (m_shakeScreen)
            {
                UpdateShake();
            }

            if (Player.Bounds.Right > m_tollCollector.Bounds.Left && TollCollectorAvailable)
            {
                Player.X = m_tollCollector.Bounds.Left - (Player.Bounds.Right - Player.X);
                Player.AttachedLevel.UpdateCamera();
            }

            base.Update(gameTime);
        }

        private void LoadLevel()
        {
            Game.ScreenManager.DisplayScreen(5, true);
        }

        private void HandleInput()
        {
            if (!m_controlsLocked)
            {
                if (Player.State != 4)
                {
                    if (m_blacksmithIcon.Visible &&
                        (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17)))
                    {
                        MovePlayerTo(m_blacksmith);
                    }

                    if (m_enchantressIcon.Visible &&
                        (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17)))
                    {
                        MovePlayerTo(m_enchantress);
                    }

                    if (m_architectIcon.Visible &&
                        (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17)))
                    {
                        var rCScreenManager = Player.AttachedLevel.ScreenManager as RCScreenManager;
                        if ((Game.ScreenManager.Game as Game).SaveManager.FileExists(SaveType.Map))
                        {
                            if (!Game.PlayerStats.LockCastle)
                            {
                                if (!Game.PlayerStats.SpokeToArchitect)
                                {
                                    Game.PlayerStats.SpokeToArchitect = true;
                                    rCScreenManager.DialogueScreen.SetDialogue("Meet Architect");
                                }
                                else
                                {
                                    rCScreenManager.DialogueScreen.SetDialogue("Meet Architect 2");
                                }

                                rCScreenManager.DialogueScreen.SetDialogueChoice("ConfirmTest1");
                                rCScreenManager.DialogueScreen.SetConfirmEndHandler(this, "ActivateArchitect");
                                rCScreenManager.DialogueScreen.SetCancelEndHandler(typeof(Console), "WriteLine",
                                    "Canceling Selection");
                            }
                            else
                            {
                                rCScreenManager.DialogueScreen.SetDialogue("Castle Already Locked Architect");
                            }
                        }
                        else
                        {
                            rCScreenManager.DialogueScreen.SetDialogue("No Castle Architect");
                        }

                        rCScreenManager.DisplayScreen(13, true);
                    }
                }

                if (m_tollCollectorIcon.Visible &&
                    (Game.GlobalInput.JustPressed(16) || Game.GlobalInput.JustPressed(17)))
                {
                    var rCScreenManager2 = Player.AttachedLevel.ScreenManager as RCScreenManager;
                    if (Game.PlayerStats.SpecialItem == 1)
                    {
                        Tween.RunFunction(0.1f, this, "TollPaid", false);
                        rCScreenManager2.DialogueScreen.SetDialogue("Toll Collector Obol");
                        rCScreenManager2.DisplayScreen(13, true);
                        return;
                    }

                    if (Game.PlayerStats.SpecialItem == 9)
                    {
                        rCScreenManager2.DialogueScreen.SetDialogue("Challenge Icon Eyeball");
                        RunTollPaidSelection(rCScreenManager2);
                        return;
                    }

                    if (Game.PlayerStats.SpecialItem == 10)
                    {
                        rCScreenManager2.DialogueScreen.SetDialogue("Challenge Icon Skull");
                        RunTollPaidSelection(rCScreenManager2);
                        return;
                    }

                    if (Game.PlayerStats.SpecialItem == 11)
                    {
                        rCScreenManager2.DialogueScreen.SetDialogue("Challenge Icon Fireball");
                        RunTollPaidSelection(rCScreenManager2);
                        return;
                    }

                    if (Game.PlayerStats.SpecialItem == 12)
                    {
                        rCScreenManager2.DialogueScreen.SetDialogue("Challenge Icon Blob");
                        RunTollPaidSelection(rCScreenManager2);
                        return;
                    }

                    if (Game.PlayerStats.SpecialItem == 13)
                    {
                        rCScreenManager2.DialogueScreen.SetDialogue("Challenge Icon Last Boss");
                        RunTollPaidSelection(rCScreenManager2);
                        return;
                    }

                    if (!Game.PlayerStats.SpokeToTollCollector)
                    {
                        rCScreenManager2.DialogueScreen.SetDialogue("Meet Toll Collector 1");
                    }
                    else
                    {
                        var num = SkillSystem.GetSkill(SkillType.PricesDown).ModifierAmount * 100f;
                        rCScreenManager2.DialogueScreen.SetDialogue("Meet Toll Collector Skip" +
                                                                    (int) Math.Round(num,
                                                                        MidpointRounding.AwayFromZero));
                    }

                    RunTollPaidSelection(rCScreenManager2);
                }
            }
        }

        private void RunTollPaidSelection(RCScreenManager manager)
        {
            manager.DialogueScreen.SetDialogueChoice("ConfirmTest1");
            manager.DialogueScreen.SetConfirmEndHandler(this, "TollPaid", true);
            manager.DialogueScreen.SetCancelEndHandler(typeof(Console), "WriteLine", "Canceling Selection");
            manager.DisplayScreen(13, true);
        }

        public void MovePlayerTo(GameObj target)
        {
            m_controlsLocked = true;
            if (Player.X != target.X - 150f)
            {
                if (Player.X > target.Position.X - 150f)
                {
                    Player.Flip = SpriteEffects.FlipHorizontally;
                }

                var num = CDGMath.DistanceBetweenPts(Player.Position, new Vector2(target.X - 150f, target.Y)) /
                          Player.Speed;
                Player.UpdateCollisionBoxes();
                Player.State = 1;
                Player.IsWeighted = false;
                Player.AccelerationY = 0f;
                Player.AccelerationX = 0f;
                Player.IsCollidable = false;
                Player.CurrentSpeed = 0f;
                Player.LockControls();
                Player.ChangeSprite("PlayerWalking_Character");
                Player.PlayAnimation();
                var logicSet = new LogicSet(Player);
                logicSet.AddAction(new DelayLogicAction(num));
                Player.RunExternalLogicSet(logicSet);
                Tween.To(Player, num, Tween.EaseNone, "X", (target.Position.X - 150f).ToString());
                Tween.AddEndHandlerToLastTween(this, "MovePlayerComplete", target);
                return;
            }

            MovePlayerComplete(target);
        }

        public void MovePlayerComplete(GameObj target)
        {
            m_controlsLocked = false;
            Player.IsWeighted = true;
            Player.IsCollidable = true;
            Player.UnlockControls();
            Player.Flip = SpriteEffects.None;
            if (target != m_blacksmith)
            {
                if (target == m_enchantress)
                {
                    if (!Game.PlayerStats.SpokeToEnchantress)
                    {
                        Game.PlayerStats.SpokeToEnchantress = true;
                        (Player.AttachedLevel.ScreenManager as RCScreenManager).DialogueScreen.SetDialogue(
                            "Meet Enchantress");
                        var arg_1A1_0 =
                            (Player.AttachedLevel.ScreenManager as RCScreenManager).DialogueScreen;
                        object arg_1A1_1 = Player.AttachedLevel.ScreenManager;
                        var arg_1A1_2 = "DisplayScreen";
                        var array = new object[3];
                        array[0] = 11;
                        array[1] = true;
                        arg_1A1_0.SetConfirmEndHandler(arg_1A1_1, arg_1A1_2, array);
                        (Player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(13, true);
                        return;
                    }

                    (Player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(11, true);
                }

                return;
            }

            if (!Game.PlayerStats.SpokeToBlacksmith)
            {
                Game.PlayerStats.SpokeToBlacksmith = true;
                (Player.AttachedLevel.ScreenManager as RCScreenManager).DialogueScreen.SetDialogue("Meet Blacksmith");
                var arg_CA_0 = (Player.AttachedLevel.ScreenManager as RCScreenManager).DialogueScreen;
                object arg_CA_1 = Player.AttachedLevel.ScreenManager;
                var arg_CA_2 = "DisplayScreen";
                var array2 = new object[3];
                array2[0] = 10;
                array2[1] = true;
                arg_CA_0.SetConfirmEndHandler(arg_CA_1, arg_CA_2, array2);
                (Player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(13, true);
                return;
            }

            (Player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(10, true);
        }

        public void TollPaid(bool chargeFee)
        {
            if (chargeFee)
            {
                var num = Game.PlayerStats.Gold * (1f - SkillSystem.GetSkill(SkillType.PricesDown).ModifierAmount);
                Game.PlayerStats.Gold -= (int) num;
                if (num > 0f)
                {
                    Player.AttachedLevel.TextManager.DisplayNumberStringText(-(int) num, "gold", Color.Yellow,
                        new Vector2(Player.X, Player.Bounds.Top));
                }
            }

            if (Game.PlayerStats.SpokeToTollCollector && Game.PlayerStats.SpecialItem != 1 &&
                Game.PlayerStats.SpecialItem != 12 && Game.PlayerStats.SpecialItem != 13 &&
                Game.PlayerStats.SpecialItem != 11 && Game.PlayerStats.SpecialItem != 9 &&
                Game.PlayerStats.SpecialItem != 10)
            {
                Player.AttachedLevel.ImpactEffectPool.DisplayDeathEffect(m_tollCollector.Position);
                SoundManager.PlaySound("Charon_Laugh");
                HideTollCollector();
            }
            else
            {
                Game.PlayerStats.SpokeToTollCollector = true;
                SoundManager.PlaySound("Charon_Laugh");
                m_tollCollector.ChangeSprite("NPCTollCollectorLaugh_Character");
                m_tollCollector.AnimationDelay = 0.05f;
                m_tollCollector.PlayAnimation();
                Tween.RunFunction(1f, Player.AttachedLevel.ImpactEffectPool, "DisplayDeathEffect",
                    m_tollCollector.Position);
                Tween.RunFunction(1f, this, "HideTollCollector");
            }

            if (Game.PlayerStats.SpecialItem == 1 || Game.PlayerStats.SpecialItem == 10 ||
                Game.PlayerStats.SpecialItem == 9 || Game.PlayerStats.SpecialItem == 13 ||
                Game.PlayerStats.SpecialItem == 11 || Game.PlayerStats.SpecialItem == 12)
            {
                if (Game.PlayerStats.SpecialItem == 9)
                {
                    Game.PlayerStats.ChallengeEyeballUnlocked = true;
                }
                else if (Game.PlayerStats.SpecialItem == 10)
                {
                    Game.PlayerStats.ChallengeSkullUnlocked = true;
                }
                else if (Game.PlayerStats.SpecialItem == 11)
                {
                    Game.PlayerStats.ChallengeFireballUnlocked = true;
                }
                else if (Game.PlayerStats.SpecialItem == 12)
                {
                    Game.PlayerStats.ChallengeBlobUnlocked = true;
                }
                else if (Game.PlayerStats.SpecialItem == 13)
                {
                    Game.PlayerStats.ChallengeLastBossUnlocked = true;
                }

                Game.PlayerStats.SpecialItem = 0;
                Player.AttachedLevel.UpdatePlayerHUDSpecialItem();
            }
        }

        public void HideTollCollector()
        {
            SoundManager.Play3DSound(this, Player, "Charon_Poof");
            m_tollCollector.Visible = false;
            Player.AttachedLevel.PhysicsManager.RemoveObject(m_tollCollector);
        }

        public void ActivateArchitect()
        {
            Player.LockControls();
            Player.CurrentSpeed = 0f;
            m_architectIcon.Visible = false;
            m_architectRenovating = true;
            m_architect.ChangeSprite("ArchitectPull_Character");
            (m_architect.GetChildAt(1) as SpriteObj).PlayAnimation(false);
            m_screw.AnimationDelay = 0.0333333351f;
            Tween.RunFunction(0.5f, m_architect.GetChildAt(0), "PlayAnimation", true);
            Tween.RunFunction(0.5f, typeof(SoundManager), "PlaySound", "Architect_Lever");
            Tween.RunFunction(1f, typeof(SoundManager), "PlaySound", "Architect_Screw");
            Tween.RunFunction(1f, m_screw, "PlayAnimation", false);
            Tween.By(m_architectBlock, 0.8f, Tween.EaseNone, "delay", "1.1", "Y", "135");
            Tween.RunFunction(1f, this, "ShakeScreen", 2, true, false);
            Tween.RunFunction(1.5f, this, "StopScreenShake");
            Tween.RunFunction(1.5f, Player.AttachedLevel.ImpactEffectPool, "SkillTreeDustEffect",
                new Vector2(m_screw.X - m_screw.Width / 2f, m_screw.Y - 40f), true, m_screw.Width);
            Tween.RunFunction(3f, this, "StopArchitectActivation");
        }

        public void StopArchitectActivation()
        {
            m_architectRenovating = false;
            m_architectIcon.Visible = true;
            Player.UnlockControls();
            Game.PlayerStats.LockCastle = true;
            Game.PlayerStats.HasArchitectFee = true;
            foreach (var current in Player.AttachedLevel.ChestList)
            {
                var fairyChestObj = current as FairyChestObj;
                if (fairyChestObj != null && fairyChestObj.State == 2)
                {
                    fairyChestObj.ResetChest();
                }
            }

            foreach (var current2 in Player.AttachedLevel.RoomList)
            foreach (var current3 in current2.GameObjList)
            {
                var breakableObj = current3 as BreakableObj;
                if (breakableObj != null)
                {
                    breakableObj.Reset();
                }
            }

            var rCScreenManager = Player.AttachedLevel.ScreenManager as RCScreenManager;
            rCScreenManager.DialogueScreen.SetDialogue("Castle Lock Complete Architect");
            rCScreenManager.DisplayScreen(13, true);
        }

        public override void Draw(Camera2D camera)
        {
            m_mountain1.X = camera.TopLeftCorner.X * 0.5f;
            m_mountain2.X = m_mountain1.X + 2640f;
            base.Draw(camera);
            if (m_isRaining)
            {
                camera.Draw(Game.GenericTexture, new Rectangle(0, 0, 2640, 720), Color.Black * 0.3f);
            }

            if (m_screenShakeCounter > 0f)
            {
                camera.X += CDGMath.RandomPlusMinus();
                camera.Y += CDGMath.RandomPlusMinus();
                m_screenShakeCounter -= (float) camera.GameTime.ElapsedGameTime.TotalSeconds;
            }

            if (SmithyAvailable)
            {
                m_blacksmithBoard.Draw(camera);
                m_blacksmith.Draw(camera);
                m_blacksmithIcon.Draw(camera);
            }

            if (EnchantressAvailable)
            {
                m_tent.Draw(camera);
                m_enchantress.Draw(camera);
                m_enchantressIcon.Draw(camera);
            }

            if (ArchitectAvailable)
            {
                m_screw.Draw(camera);
                m_architect.Draw(camera);
                m_architectIcon.Draw(camera);
            }

            if (TollCollectorAvailable)
            {
                m_tollCollector.Draw(camera);
                m_tollCollectorIcon.Draw(camera);
            }

            m_blacksmithNewIcon.Draw(camera);
            m_enchantressNewIcon.Draw(camera);
            if (m_isRaining)
            {
                foreach (var current in m_rainFG) current.Draw(camera);
            }
        }

        public override void PauseRoom()
        {
            foreach (var current in m_rainFG) current.PauseAnimation();
            if (m_rainSFX != null)
            {
                m_rainSFX.Pause();
            }

            m_enchantress.PauseAnimation();
            m_blacksmith.PauseAnimation();
            m_architect.PauseAnimation();
            m_tollCollector.PauseAnimation();
            base.PauseRoom();
        }

        public override void UnpauseRoom()
        {
            foreach (var current in m_rainFG) current.ResumeAnimation();
            if (m_rainSFX != null && m_rainSFX.IsPaused)
            {
                m_rainSFX.Resume();
            }

            m_enchantress.ResumeAnimation();
            m_blacksmith.ResumeAnimation();
            m_architect.ResumeAnimation();
            m_tollCollector.ResumeAnimation();
            base.UnpauseRoom();
        }

        public void ShakeScreen(float magnitude, bool horizontalShake = true, bool verticalShake = true)
        {
            m_shakeStartingPos = Player.AttachedLevel.Camera.Position;
            Player.AttachedLevel.CameraLockedToPlayer = false;
            m_screenShakeMagnitude = magnitude;
            m_horizontalShake = horizontalShake;
            m_verticalShake = verticalShake;
            m_shakeScreen = true;
        }

        public void UpdateShake()
        {
            if (m_horizontalShake)
            {
                Player.AttachedLevel.Camera.X = m_shakeStartingPos.X +
                                                CDGMath.RandomPlusMinus() *
                                                (CDGMath.RandomFloat(0f, 1f) * m_screenShakeMagnitude);
            }

            if (m_verticalShake)
            {
                Player.AttachedLevel.Camera.Y = m_shakeStartingPos.Y +
                                                CDGMath.RandomPlusMinus() *
                                                (CDGMath.RandomFloat(0f, 1f) * m_screenShakeMagnitude);
            }
        }

        public void StopScreenShake()
        {
            Player.AttachedLevel.CameraLockedToPlayer = true;
            m_shakeScreen = false;
        }

        protected override GameObj CreateCloneInstance()
        {
            return new StartingRoomObj();
        }

        protected override void FillCloneInstance(object obj)
        {
            base.FillCloneInstance(obj);
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                m_blacksmith.Dispose();
                m_blacksmith = null;
                m_blacksmithIcon.Dispose();
                m_blacksmithIcon = null;
                m_blacksmithNewIcon.Dispose();
                m_blacksmithNewIcon = null;
                m_blacksmithBoard.Dispose();
                m_blacksmithBoard = null;
                m_enchantress.Dispose();
                m_enchantress = null;
                m_enchantressIcon.Dispose();
                m_enchantressIcon = null;
                m_enchantressNewIcon.Dispose();
                m_enchantressNewIcon = null;
                m_tent.Dispose();
                m_tent = null;
                m_architect.Dispose();
                m_architect = null;
                m_architectIcon.Dispose();
                m_architectIcon = null;
                m_screw.Dispose();
                m_screw = null;
                if (m_blacksmithAnvilSound != null)
                {
                    m_blacksmithAnvilSound.Dispose();
                }

                m_blacksmithAnvilSound = null;
                m_tree1 = null;
                m_tree2 = null;
                m_tree3 = null;
                m_fern1 = null;
                m_fern2 = null;
                m_fern3 = null;
                foreach (var current in m_rainFG) current.Dispose();
                m_rainFG.Clear();
                m_rainFG = null;
                m_mountain1 = null;
                m_mountain2 = null;
                m_tollCollector.Dispose();
                m_tollCollector = null;
                m_tollCollectorIcon.Dispose();
                m_tollCollectorIcon = null;
                m_blacksmithBlock = null;
                m_enchantressBlock = null;
                m_architectBlock = null;
                if (m_rainSFX != null)
                {
                    m_rainSFX.Dispose();
                }

                m_rainSFX = null;
                base.Dispose();
            }
        }
    }
}