using System;
using System.Collections.Generic;
using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueLegacy.Enums;
using RogueLegacy.GameObjects;
using Tweener;
using Tweener.Ease;

namespace RogueLegacy.Screens
{
    public class GameOverScreen : Screen
    {
        private int            _bagsCollected;
        private int            _coinsCollected;
        private KeyIconTextObj _continueText;
        private ObjContainer   _dialoguePlate;
        private int            _diamondsCollected;
        private bool           _droppingStats;
        private List<EnemyObj> _enemyList;
        private List<Vector2>  _enemyStoredPositions;
        private bool           _lockControls;
        private GameObj        _objKilledPlayer;
        private PlayerObj      _player;
        private FrameSoundObj  _playerFallSound;
        private LineageObj     _playerFrame;
        private SpriteObj      _playerGhost;
        private FrameSoundObj  _playerSwordFallSound;
        private FrameSoundObj  _playerSwordSpinSound;
        private SpriteObj      _spotlight;

        public GameOverScreen()
        {
            _enemyStoredPositions = new List<Vector2>();
        }

        public float BackBufferOpacity { get; set; }

        public override void PassInData(List<object> objList)
        {
            if (objList != null)
            {
                _player = objList[0] as PlayerObj;
                if (_playerFallSound == null)
                {
                    _playerFallSound = new FrameSoundObj(_player, 14, "Player_Death_BodyFall");
                    _playerSwordSpinSound = new FrameSoundObj(_player, 2, "Player_Death_SwordTwirl");
                    _playerSwordFallSound = new FrameSoundObj(_player, 9, "Player_Death_SwordLand");
                }

                _enemyList = objList[1] as List<EnemyObj>;
                _coinsCollected = (int) objList[2];
                _bagsCollected = (int) objList[3];
                _diamondsCollected = (int) objList[4];
                if (objList[5] != null)
                {
                    _objKilledPlayer = objList[5] as GameObj;
                }

                var cause = SetObjectKilledPlayerText();
                _enemyStoredPositions.Clear();

                // Do not send a death link if what is killing us is a DeathLink.
                if (!(_objKilledPlayer is DeathLinkObj))
                {
                    Program.Game.ArchipelagoManager.SendDeathLinkIfEnabled(cause);
                }

                base.PassInData(objList);
            }
        }

        public override void LoadContent()
        {
            _continueText = new KeyIconTextObj(Game.JunicodeFont);
            _continueText.FontSize = 14f;
            _continueText.Align = Types.TextAlign.Right;
            _continueText.Opacity = 0f;
            _continueText.Position = new Vector2(1270f, 30f);
            _continueText.ForceDraw = true;
            var dropShadow = new Vector2(2f, 2f);
            var textureColor = new Color(255, 254, 128);
            _dialoguePlate = new ObjContainer("DialogBox_Character");
            _dialoguePlate.Position = new Vector2(660f, 610f);
            _dialoguePlate.ForceDraw = true;
            var textObj = new TextObj(Game.JunicodeFont);
            textObj.Align = Types.TextAlign.Centre;
            textObj.Text = "Your valor shown in battle shall never be forgotten.";
            textObj.FontSize = 17f;
            textObj.DropShadow = dropShadow;
            textObj.Position = new Vector2(0f, -(float) _dialoguePlate.Height / 2 + 25);
            _dialoguePlate.AddChild(textObj);
            var keyIconTextObj = new KeyIconTextObj(Game.JunicodeFont);
            keyIconTextObj.FontSize = 12f;
            keyIconTextObj.Align = Types.TextAlign.Centre;
            keyIconTextObj.Text = "\"Arrrrggghhhh\"";
            keyIconTextObj.DropShadow = dropShadow;
            keyIconTextObj.Y = 0f;
            keyIconTextObj.TextureColor = textureColor;
            _dialoguePlate.AddChild(keyIconTextObj);
            var textObj2 = new TextObj(Game.JunicodeFont);
            textObj2.FontSize = 8f;
            textObj2.Text = "-Player X's parting words";
            textObj2.Y = keyIconTextObj.Y;
            textObj2.Y += 40f;
            textObj2.X += 20f;
            textObj2.DropShadow = dropShadow;
            _dialoguePlate.AddChild(textObj2);
            _playerGhost = new SpriteObj("PlayerGhost_Sprite");
            _playerGhost.AnimationDelay = 0.1f;
            _spotlight = new SpriteObj("GameOverSpotlight_Sprite");
            _spotlight.Rotation = 90f;
            _spotlight.ForceDraw = true;
            _spotlight.Position = new Vector2(660f, 40 + _spotlight.Height);
            _playerFrame = new LineageObj(null, true);
            _playerFrame.DisablePlaque = true;
            base.LoadContent();
        }

        public override void OnEnter()
        {
            if (_objKilledPlayer is not RetireObj)
            {
                _playerFrame.Opacity = 0f;
                _playerFrame.Position = _player.Position;
                _playerFrame.SetTraits(Game.PlayerStats.Traits);
                _playerFrame.IsFemale = Game.PlayerStats.IsFemale;
                _playerFrame.Class = Game.PlayerStats.Class;
                _playerFrame.Y -= 120f;
                _playerFrame.SetPortrait(Game.PlayerStats.HeadPiece, Game.PlayerStats.ShoulderPiece,
                    Game.PlayerStats.ChestPiece);
                _playerFrame.UpdateData();
                Tween.To(_playerFrame, 1f, Tween.EaseNone, "delay", "4", "Opacity", "1");
            }

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

            SoundManager.StopMusic(0.5f);
            _droppingStats = false;
            _lockControls = false;
            SoundManager.PlaySound("Player_Death_FadeToBlack");
            _continueText.Text = "Press [Input:" + 0 + "] to move on";
            _player.Visible = true;
            _player.Opacity = 1f;
            _continueText.Opacity = 0f;
            _dialoguePlate.Opacity = 0f;
            _playerGhost.Opacity = 0f;
            _spotlight.Opacity = 0f;
            if (_objKilledPlayer is not RetireObj)
            {
                _playerGhost.Position = new Vector2(_player.X - _playerGhost.Width / 2, _player.Bounds.Top - 20);
                Tween.RunFunction(3f, typeof(SoundManager), "PlaySound", "Player_Ghost");
                Tween.To(_playerGhost, 0.5f, Linear.EaseNone, "delay", "3", "Opacity", "0.4");
                Tween.By(_playerGhost, 2f, Linear.EaseNone, "delay", "3", "Y", "-150");
                _playerGhost.Opacity = 0.4f;
                Tween.To(_playerGhost, 0.5f, Linear.EaseNone, "delay", "4", "Opacity", "0");
                _playerGhost.Opacity = 0f;
                _playerGhost.PlayAnimation();
            }

            Tween.To(this, 0.5f, Linear.EaseNone, "BackBufferOpacity", "1");
            Tween.To(_spotlight, 0.1f, Linear.EaseNone, "delay", "1", "Opacity", "1");
            Tween.AddEndHandlerToLastTween(typeof(SoundManager), "PlaySound", "Player_Death_Spotlight");
            Tween.RunFunction(1.2f, typeof(SoundManager), "PlayMusic", "GameOverStinger", false, 0.5f);
            Tween.To(Camera, 1f, Quad.EaseInOut, "X", _player.AbsX.ToString(), "Y",
                (_player.Bounds.Bottom - 10).ToString(), "Zoom", "1");

            if (_objKilledPlayer is not RetireObj)
            {
                Tween.RunFunction(2f, _player, "RunDeathAnimation1");
            }
            else
            {
                _player.Flip = SpriteEffects.FlipHorizontally;
                Tween.RunFunction(2f, _player, "RunDeathAnimation2");
                Tween.To(_player, 2f, Linear.EaseNone, "delay", "2", "Opacity", "0");
                Tween.By(_player, 2f, Linear.EaseNone, "delay", "2", "X", "-400");
            }

            if (Game.PlayerStats.Traits.X == 13f || Game.PlayerStats.Traits.Y == 13f)
            {
                (_dialoguePlate.GetChildAt(2) as TextObj).Text = "#)!(%*#@!%^";
                (_dialoguePlate.GetChildAt(2) as TextObj).RandomizeSentence(true);
            }
            else
            {
                (_dialoguePlate.GetChildAt(2) as TextObj).Text =
                    GameEV.GameHints[CDGMath.RandomInt(0, GameEV.GameHints.Length - 1)];
            }

            (_dialoguePlate.GetChildAt(3) as TextObj).Text = "-" + Game.PlayerStats.PlayerName + "'s Parting Words";
            Tween.To(_dialoguePlate, 0.5f, Tween.EaseNone, "delay", "2", "Opacity", "1");
            Tween.RunFunction(4.5f, this, "DropStats");
            Tween.To(_continueText, 0.4f, Linear.EaseNone, "delay", "4", "Opacity", "1");
            base.OnEnter();
        }

        public override void OnExit()
        {
            Tween.StopAll(false);
            if (_enemyList != null)
            {
                _enemyList.Clear();
                _enemyList = null;
            }

            Game.PlayerStats.Traits = Vector2.Zero;
            BackBufferOpacity = 0f;
            base.OnExit();
        }

        public void DropStats()
        {
            _droppingStats = true;
            var arg_0C_0 = Vector2.Zero;
            var num = 0f;
            var topLeftCorner = Camera.TopLeftCorner;
            topLeftCorner.X += 200f;
            topLeftCorner.Y += 450f;
            if (_enemyList != null)
            {
                foreach (var current in _enemyList)
                {
                    _enemyStoredPositions.Add(current.Position);
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
            var textObj = _dialoguePlate.GetChildAt(1) as TextObj;
            if (_objKilledPlayer != null)
            {
                var enemyObj = _objKilledPlayer as EnemyObj;
                var projectileObj = _objKilledPlayer as ProjectileObj;
                var deathLinkObj = _objKilledPlayer as DeathLinkObj;
                var retireObj = _objKilledPlayer as RetireObj;
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
                else if (retireObj != null)
                {
                    textObj.Text = Game.PlayerStats.PlayerName + " had enough and decided to call it quits";
                }

                var hazardObj = _objKilledPlayer as HazardObj;
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
            if (!_lockControls && _droppingStats &&
                (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1) ||
                 Game.GlobalInput.JustPressed(2) ||
                 Game.GlobalInput.JustPressed(3)))
            {
                if (_enemyList.Count > 0 && _enemyList[_enemyList.Count - 1].Opacity != 1f)
                {
                    foreach (var current in _enemyList)
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
                    var received = Game.PlayerStats.ReceivedItems;
                    var pieces = Game.PlayerStats.FountainPieces;
                    Game.PlayerStats.Dispose();
                    Game.PlayerStats = new PlayerStats();
                    (ScreenManager as RCScreenManager).Player.Reset();
                    (ScreenManager.Game as Game).SaveManager.LoadFiles(null, SaveType.PlayerData, SaveType.Lineage,
                        SaveType.UpgradeData);
                    Game.ScreenManager.Player.CurrentHealth = Game.PlayerStats.CurrentHealth;
                    Game.ScreenManager.Player.CurrentMana = Game.PlayerStats.CurrentMana;
                    Game.PlayerStats.ReceivedItems = received;
                    Game.PlayerStats.FountainPieces = pieces;
                    (ScreenManager as RCScreenManager).DisplayScreen((int) ScreenType.Lineage, true);
                    _lockControls = true;
                }
            }

            base.HandleInput();
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.JustPressed(Keys.Space, PlayerIndex.One))
            {
                (_dialoguePlate.GetChildAt(2) as TextObj).Text =
                    GameEV.GameHints[CDGMath.RandomInt(0, GameEV.GameHints.Length - 1)];
            }

            if (_player.SpriteName == "PlayerDeath_Character")
            {
                _playerFallSound.Update();
                _playerSwordFallSound.Update();
                _playerSwordSpinSound.Update();
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
            foreach (var current in _enemyList)
            {
                current.Draw(Camera);
            }

            _playerFrame.Draw(Camera);
            _player.Draw(Camera);
            if (_playerGhost.Opacity > 0f)
            {
                _playerGhost.X += (float) Math.Sin(Game.TotalGameTimeSeconds * 5f) * 60f *
                                   (float) gameTime.ElapsedGameTime.TotalSeconds;
            }

            _playerGhost.Draw(Camera);
            Camera.End();
            Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null);
            _spotlight.Draw(Camera);
            _dialoguePlate.Draw(Camera);
            _continueText.Draw(Camera);
            Camera.End();
            base.Draw(gameTime);
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            Console.WriteLine("Disposing Game Over Screen");
            _player = null;
            _dialoguePlate.Dispose();
            _dialoguePlate = null;
            _continueText.Dispose();
            _continueText = null;
            _playerGhost.Dispose();
            _playerGhost = null;
            _spotlight.Dispose();
            _spotlight = null;
            _playerFallSound.Dispose();
            _playerFallSound = null;
            _playerSwordFallSound.Dispose();
            _playerSwordFallSound = null;
            _playerSwordSpinSound.Dispose();
            _playerSwordSpinSound = null;
            _objKilledPlayer = null;
            if (_enemyList != null)
            {
                _enemyList.Clear();
            }

            _enemyList = null;
            if (_enemyStoredPositions != null)
            {
                _enemyStoredPositions.Clear();
            }

            _enemyStoredPositions = null;
            _playerFrame.Dispose();
            _playerFrame = null;
            base.Dispose();
        }
    }
}
