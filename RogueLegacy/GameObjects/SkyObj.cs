// Rogue Legacy Randomizer - SkyObj.cs
// Last Modified 2022-12-01
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueLegacy.Screens;
using Tweener;

namespace RogueLegacy.GameObjects;

public class SkyObj : GameObj
{
    private BackgroundObj         _differenceCloud;
    private BackgroundObj         _differenceCloud2;
    private BackgroundObj         _differenceCloud3;
    private ProceduralLevelScreen _levelScreen;
    private SpriteObj             _moon;
    private Vector2               _moonPos;
    private SpriteObj             _silhouette;
    private bool                  _silhouetteFlying;
    private float                 _silhouetteTimer;

    public SkyObj(ProceduralLevelScreen levelScreen)
    {
        _levelScreen = levelScreen;
    }

    public float MorningOpacity { get; set; }

    public void LoadContent(Camera2D camera)
    {
        var one = new Vector2(2f, 2f);
        _moon = new SpriteObj("ParallaxMoon_Sprite") { Position = new Vector2(900f, 200f) };
        if (LevelENV.SaveFrames)
        {
            _moon.Position /= 2f;
            one = Vector2.One;
        }

        _moon.Scale = one;
        _moon.ForceDraw = true;
        _moonPos = _moon.Position;
        _differenceCloud = new BackgroundObj("ParallaxDifferenceClouds_Sprite");
        _differenceCloud.SetRepeated(true, true, camera, SamplerState.LinearWrap);
        _differenceCloud.Scale = one;
        _differenceCloud.TextureColor = new Color(10, 10, 80);
        _differenceCloud.ParallaxSpeed = new Vector2(0.2f, 0f);
        _differenceCloud2 = new BackgroundObj("ParallaxDifferenceClouds_Sprite");
        _differenceCloud2.SetRepeated(true, true, camera, SamplerState.LinearWrap);
        _differenceCloud2.Scale = one;
        _differenceCloud2.Flip = SpriteEffects.FlipHorizontally;
        _differenceCloud2.TextureColor = new Color(80, 80, 160);
        _differenceCloud2.X -= 500f;
        _differenceCloud2.ParallaxSpeed = new Vector2(0.4f, 0f);
        _differenceCloud3 = new BackgroundObj("ParallaxDifferenceClouds_Sprite");
        _differenceCloud3.SetRepeated(true, true, camera, SamplerState.LinearWrap);
        _differenceCloud3.Scale = one;
        _differenceCloud3.Flip = SpriteEffects.FlipHorizontally;
        _differenceCloud3.TextureColor = Color.White;
        _differenceCloud3.X -= 500f;
        _differenceCloud3.ParallaxSpeed = new Vector2(0.4f, 0f);
        _silhouette = new SpriteObj("GardenBat_Sprite")
        {
            ForceDraw = true,
            AnimationDelay = 0.05f,
            Scale = one
        };
    }

    public void ReinitializeRT(Camera2D camera)
    {
        var one = new Vector2(2f, 2f);
        if (LevelENV.SaveFrames)
        {
            _moon.Position /= 2f;
            one = Vector2.One;
        }

        _differenceCloud?.Dispose();
        _differenceCloud = new BackgroundObj("ParallaxDifferenceClouds_Sprite");
        _differenceCloud.SetRepeated(true, true, camera, SamplerState.LinearWrap);
        _differenceCloud.Scale = one;
        _differenceCloud.TextureColor = new Color(10, 10, 80);
        _differenceCloud.ParallaxSpeed = new Vector2(0.2f, 0f);

        _differenceCloud2?.Dispose();
        _differenceCloud2 = new BackgroundObj("ParallaxDifferenceClouds_Sprite");
        _differenceCloud2.SetRepeated(true, true, camera, SamplerState.LinearWrap);
        _differenceCloud2.Scale = one;
        _differenceCloud2.Flip = SpriteEffects.FlipHorizontally;
        _differenceCloud2.TextureColor = new Color(80, 80, 160);
        _differenceCloud2.X -= 500f;
        _differenceCloud2.ParallaxSpeed = new Vector2(0.4f, 0f);

        _differenceCloud3?.Dispose();
        _differenceCloud3 = new BackgroundObj("ParallaxDifferenceClouds_Sprite");
        _differenceCloud3.SetRepeated(true, true, camera, SamplerState.LinearWrap);
        _differenceCloud3.Scale = one;
        _differenceCloud3.Flip = SpriteEffects.FlipHorizontally;
        _differenceCloud3.TextureColor = new Color(80, 80, 160);
        _differenceCloud3.X -= 500f;
        _differenceCloud3.ParallaxSpeed = new Vector2(0.4f, 0f);
    }

    public void Update(GameTime gameTime)
    {
        var seconds = (float) gameTime.ElapsedGameTime.TotalSeconds;
        if (!_silhouetteFlying && _silhouetteTimer > 0f)
        {
            _silhouetteTimer -= (float) gameTime.ElapsedGameTime.TotalSeconds;
            if (_silhouetteTimer <= 0f)
            {
                var chance = CDGMath.RandomInt(1, 100);
                switch (chance)
                {
                    case > 95:
                        ShowSilhouette(true);
                        break;
                    case > 65:
                        ShowSilhouette(false);
                        break;
                    default:
                        _silhouetteTimer = 5f;
                        break;
                }
            }
        }

        if (!_silhouetteFlying && _silhouetteTimer <= 0f)
        {
            _silhouetteTimer = 5f;
        }

        if (_silhouette.SpriteName == "GardenPerson_Sprite")
        {
            _silhouette.Rotation += 120f * seconds;
        }
    }

    private void ShowSilhouette(bool showSanta)
    {
        if (_levelScreen == null)
        {
            return;
        }

        _silhouetteFlying = true;
        _silhouette.Rotation = 0f;
        _silhouette.Flip = SpriteEffects.None;

        var reverse = CDGMath.RandomInt(0, 1) > 0;
        string[] array =
        {
            "GardenBat_Sprite",
            "GardenCrow_Sprite",
            "GardenBat_Sprite",
            "GardenCrow_Sprite",
            "GardenPerson_Sprite"
        };

        _silhouette.ChangeSprite(!showSanta ? array[CDGMath.RandomInt(0, 4)] : "GardenSanta_Sprite");

        _silhouette.PlayAnimation();
        if (reverse)
        {
            _silhouette.X = -(float) _silhouette.Width;
        }
        else
        {
            _silhouette.Flip = SpriteEffects.FlipHorizontally;
            _silhouette.X = _levelScreen.CurrentRoom.Width + _silhouette.Width;
        }

        _silhouette.Y = CDGMath.RandomFloat(100f, 500f);
        var width = _levelScreen.CurrentRoom.Bounds.Width + _silhouette.Width * 2;
        if (!reverse)
        {
            width = -width;
        }

        Tween.By(_silhouette, CDGMath.RandomFloat(10f, 15f), Tween.EaseNone, "X", width.ToString(), "Y",
            CDGMath.RandomInt(-200, 200).ToString());
        Tween.AddEndHandlerToLastTween(this, "SilhouetteComplete");
    }

    public void SilhouetteComplete()
    {
        _silhouetteFlying = false;
    }

    public override void Draw(Camera2D camera)
    {
        _moon.X = _moonPos.X - camera.TopLeftCorner.X * 0.01f;
        _moon.Y = _moonPos.Y - camera.TopLeftCorner.Y * 0.01f;
        camera.GraphicsDevice.Clear(new Color(4, 29, 86));
        camera.Draw(Game.GenericTexture, new Rectangle(-10, -10, 1400, 800), Color.SkyBlue * MorningOpacity);
        _moon.Opacity = 1f - MorningOpacity;
        _silhouette.Opacity = 1f - MorningOpacity;
        _differenceCloud.Opacity = 1f - MorningOpacity;
        _differenceCloud2.Opacity = 1f - MorningOpacity;
        _differenceCloud3.Opacity = MorningOpacity;
        _moon.Draw(camera);
        _differenceCloud.Draw(camera);
        _differenceCloud2.Draw(camera);
        _differenceCloud3.Draw(camera);
        _silhouette.Draw(camera);
        base.Draw(camera);
    }

    protected override GameObj CreateCloneInstance()
    {
        return new SkyObj(_levelScreen);
    }

    public override void Dispose()
    {
        if (IsDisposed)
        {
            return;
        }

        _differenceCloud.Dispose();
        _differenceCloud = null;
        _differenceCloud2.Dispose();
        _differenceCloud2 = null;
        _differenceCloud3.Dispose();
        _differenceCloud3 = null;
        _moon.Dispose();
        _moon = null;
        _silhouette.Dispose();
        _silhouette = null;
        _levelScreen = null;
        base.Dispose();
    }
}
