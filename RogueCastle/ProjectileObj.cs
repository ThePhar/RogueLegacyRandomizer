using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Tweener;
using Tweener.Ease;
namespace RogueCastle
{
	public class ProjectileObj : PhysicsObj, IDealsDamageObj
	{
		public float LifeSpan;
		private float m_elapsedLifeSpan;
		private Color m_blinkColour = Color.White;
		private float m_blinkTimer;
		public int Damage
		{
			get;
			set;
		}
		public float RotationSpeed
		{
			get;
			set;
		}
		public bool IsAlive
		{
			get;
			internal set;
		}
		public bool CollidesWithTerrain
		{
			get;
			set;
		}
		public bool DestroysWithTerrain
		{
			get;
			set;
		}
		public bool DestroysWithEnemy
		{
			get;
			set;
		}
		public GameObj Target
		{
			get;
			set;
		}
		public bool ChaseTarget
		{
			get;
			set;
		}
		public bool FollowArc
		{
			get;
			set;
		}
		public ProjectileIconObj AttachedIcon
		{
			get;
			set;
		}
		public bool ShowIcon
		{
			get;
			set;
		}
		public bool IgnoreBoundsCheck
		{
			get;
			set;
		}
		public bool CollidesWith1Ways
		{
			get;
			set;
		}
		public bool GamePaused
		{
			get;
			set;
		}
		public bool DestroyOnRoomTransition
		{
			get;
			set;
		}
		public bool CanBeFusRohDahed
		{
			get;
			set;
		}
		public bool IgnoreInvincibleCounter
		{
			get;
			set;
		}
		public bool IsDying
		{
			get;
			internal set;
		}
		public int Spell
		{
			get;
			set;
		}
		public float AltX
		{
			get;
			set;
		}
		public float AltY
		{
			get;
			set;
		}
		public float BlinkTime
		{
			get;
			set;
		}
		public GameObj Source
		{
			get;
			set;
		}
		public bool WrapProjectile
		{
			get;
			set;
		}
		public bool IsDemented
		{
			get
			{
				EnemyObj enemyObj = this.Source as EnemyObj;
				return enemyObj != null && enemyObj.IsDemented;
			}
		}
		public ProjectileObj(string spriteName) : base(spriteName, null)
		{
			base.CollisionTypeTag = 3;
			this.CollidesWithTerrain = true;
			this.ChaseTarget = false;
			this.IsDying = false;
			this.DestroysWithEnemy = true;
			this.DestroyOnRoomTransition = true;
		}
		public void Reset()
		{
			this.Source = null;
			this.CollidesWithTerrain = true;
			this.DestroysWithTerrain = true;
			this.DestroysWithEnemy = true;
			base.IsCollidable = true;
			base.IsWeighted = true;
			this.IsDying = false;
			this.IsAlive = true;
			this.m_elapsedLifeSpan = 0f;
			base.Rotation = 0f;
			base.TextureColor = Color.White;
			this.Spell = 0;
			this.AltY = 0f;
			this.AltX = 0f;
			this.BlinkTime = 0f;
			this.IgnoreBoundsCheck = false;
			this.Scale = Vector2.One;
			base.DisableHitboxUpdating = false;
			this.m_blinkColour = Color.White;
			this.m_blinkTimer = 0f;
			base.AccelerationYEnabled = true;
			base.AccelerationXEnabled = true;
			this.GamePaused = false;
			this.DestroyOnRoomTransition = true;
			this.CanBeFusRohDahed = true;
			this.Flip = SpriteEffects.None;
			this.IgnoreInvincibleCounter = false;
			this.WrapProjectile = false;
			base.DisableCollisionBoxRotations = true;
			Tween.StopAllContaining(this, false);
		}
		public void UpdateHeading()
		{
			if (this.ChaseTarget && this.Target != null)
			{
				Vector2 position = this.Target.Position;
				this.TurnToFace(position, this.TurnSpeed, 0.0166666675f);
				base.HeadingX = (float)Math.Cos((double)base.Orientation);
				base.HeadingY = (float)Math.Sin((double)base.Orientation);
			}
		}
		public void Update(GameTime gameTime)
		{
			if (!base.IsPaused)
			{
				float num = (float)gameTime.ElapsedGameTime.TotalSeconds;
				int spell = this.Spell;
				switch (spell)
				{
				case 3:
					if (this.BlinkTime >= this.AltX && this.AltX != 0f)
					{
						this.Blink(Color.Red, 0.1f);
						this.BlinkTime = this.AltX / 1.5f;
					}
					if (this.AltX > 0f)
					{
						this.AltX -= num;
						if (this.AltX <= 0f)
						{
							this.ActivateEffect();
						}
					}
					break;
				case 4:
					break;
				case 5:
					if (this.AltY > 0f)
					{
						this.AltY -= num;
						if (this.AltY <= 0f)
						{
							ProceduralLevelScreen proceduralLevelScreen = Game.ScreenManager.CurrentScreen as ProceduralLevelScreen;
							if (proceduralLevelScreen != null)
							{
								proceduralLevelScreen.ImpactEffectPool.CrowSmokeEffect(base.Position);
								this.AltY = 0.05f;
							}
						}
					}
					if (this.AltX <= 0f)
					{
						Vector2 position = this.Target.Position;
						this.TurnToFace(position, this.TurnSpeed, num);
					}
					else
					{
						this.AltX -= num;
						base.Orientation = MathHelper.WrapAngle(base.Orientation);
					}
					base.HeadingX = (float)Math.Cos((double)base.Orientation);
					base.HeadingY = (float)Math.Sin((double)base.Orientation);
					base.AccelerationX = 0f;
					base.AccelerationY = 0f;
					base.Position += base.Heading * (base.CurrentSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
					if (base.HeadingX > 0f)
					{
						this.Flip = SpriteEffects.None;
						base.Rotation = MathHelper.ToDegrees(base.Orientation);
					}
					else
					{
						this.Flip = SpriteEffects.FlipHorizontally;
						float num2 = MathHelper.ToDegrees(base.Orientation);
						if (num2 < 0f)
						{
							base.Rotation = (180f + num2) * 60f * num;
						}
						else
						{
							base.Rotation = (-180f + num2) * 60f * num;
						}
					}
					base.Rotation = MathHelper.Clamp(base.Rotation, -90f, 90f);
					if (this.Target != null)
					{
						EnemyObj enemyObj = this.Target as EnemyObj;
						if (enemyObj != null && enemyObj.IsKilled)
						{
							this.RunDestroyAnimation(false);
						}
					}
					else
					{
						this.RunDestroyAnimation(false);
					}
					break;
				default:
					switch (spell)
					{
					case 8:
						base.AccelerationX -= this.AltX * 60f * num;
						if (this.AltY > 0f)
						{
							this.AltY -= num;
							if (this.AltY <= 0f)
							{
								this.ActivateEffect();
							}
						}
						break;
					case 9:
					case 10:
						break;
					case 11:
					{
						PlayerObj player = Game.ScreenManager.Player;
						if (player.CastingDamageShield || this.Source is EnemyObj)
						{
							this.AltX += base.CurrentSpeed * 60f * num;
							base.Position = CDGMath.GetCirclePosition(this.AltX, this.AltY, this.Target.Position);
						}
						else
						{
							this.KillProjectile();
						}
						break;
					}
					case 12:
						base.AccelerationX = 0f;
						base.AccelerationY = 0f;
						base.HeadingX = (float)Math.Cos((double)base.Orientation);
						base.HeadingY = (float)Math.Sin((double)base.Orientation);
						base.Position += base.Heading * (base.CurrentSpeed * num);
						if (this.AltY > 0f)
						{
							this.AltY -= num;
							if (this.AltY <= 0f)
							{
								this.ActivateEffect();
							}
						}
						break;
					default:
						if (spell == 100)
						{
							if (this.AltX > 0f)
							{
								this.AltX -= num;
								base.Opacity = 0.9f - this.AltX;
								this.ScaleY = 1f - this.AltX;
								if (this.AltX <= 0f)
								{
									this.ActivateEffect();
								}
							}
						}
						break;
					}
					break;
				}
				if (this.ChaseTarget && this.Target != null)
				{
					Vector2 position2 = this.Target.Position;
					this.TurnToFace(position2, this.TurnSpeed, num);
					base.HeadingX = (float)Math.Cos((double)base.Orientation);
					base.HeadingY = (float)Math.Sin((double)base.Orientation);
					base.AccelerationX = 0f;
					base.AccelerationY = 0f;
					base.Position += base.Heading * (base.CurrentSpeed * num);
					base.Rotation = MathHelper.ToDegrees(base.Orientation);
				}
				if (this.FollowArc && !this.ChaseTarget && !this.IsDying)
				{
					float radians = (float)Math.Atan2((double)base.AccelerationY, (double)base.AccelerationX);
					base.Rotation = MathHelper.ToDegrees(radians);
				}
				else if (!this.ChaseTarget)
				{
					base.Rotation += this.RotationSpeed * 60f * num;
				}
				this.m_elapsedLifeSpan += num;
				if (this.m_elapsedLifeSpan >= this.LifeSpan)
				{
					this.IsAlive = false;
				}
				if (this.m_blinkTimer > 0f)
				{
					this.m_blinkTimer -= num;
					base.TextureColor = this.m_blinkColour;
					return;
				}
				if (base.TextureColor == this.m_blinkColour)
				{
					base.TextureColor = Color.White;
				}
			}
		}
		public void Blink(Color blinkColour, float duration)
		{
			this.m_blinkColour = blinkColour;
			this.m_blinkTimer = duration;
		}
		private void TurnToFace(Vector2 facePosition, float turnSpeed, float elapsedSeconds)
		{
			float num = facePosition.X - base.Position.X;
			float num2 = facePosition.Y - base.Position.Y;
			float num3 = (float)Math.Atan2((double)num2, (double)num);
			float num4 = MathHelper.WrapAngle(num3 - base.Orientation);
			float num5 = turnSpeed * 60f * elapsedSeconds;
			num4 = MathHelper.Clamp(num4, -num5, num5);
			base.Orientation = MathHelper.WrapAngle(base.Orientation + num4);
		}
		public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
		{
			if (this.Spell == 20)
			{
				ProjectileObj projectileObj = otherBox.AbsParent as ProjectileObj;
				if (projectileObj != null && projectileObj.CollisionTypeTag != 2 && projectileObj.CanBeFusRohDahed)
				{
					projectileObj.RunDestroyAnimation(false);
				}
			}
			TerrainObj terrainObj = otherBox.Parent as TerrainObj;
			if (this.CollidesWithTerrain && !(otherBox.Parent is DoorObj) && terrainObj != null && ((terrainObj.CollidesTop && terrainObj.CollidesBottom && terrainObj.CollidesLeft && terrainObj.CollidesRight) || this.CollidesWith1Ways))
			{
				int spell = this.Spell;
				if (spell != 3)
				{
					if (spell == 7)
					{
						base.CollisionResponse(thisBox, otherBox, collisionResponseType);
						base.IsWeighted = false;
						this.ActivateEffect();
						return;
					}
					if (spell != 12)
					{
						if (this.DestroysWithTerrain)
						{
							this.RunDestroyAnimation(false);
							return;
						}
						base.AccelerationY = 0f;
						base.AccelerationX = 0f;
						base.IsWeighted = false;
						return;
					}
					else
					{
						Vector2 value = CollisionMath.RotatedRectIntersectsMTD(thisBox.AbsRect, thisBox.AbsRotation, Vector2.Zero, otherBox.AbsRect, otherBox.AbsRotation, Vector2.Zero);
						if (value != Vector2.Zero)
						{
							SoundManager.Play3DSound(this, Game.ScreenManager.Player, new string[]
							{
								"Spike_Bounce_01",
								"Spike_Bounce_02",
								"Spike_Bounce_03"
							});
							Vector2 heading = base.Heading;
							Vector2 vector = new Vector2(value.Y, value.X * -1f);
							Vector2 pt = 2f * (CDGMath.DotProduct(heading, vector) / CDGMath.DotProduct(vector, vector)) * vector - heading;
							base.X += value.X;
							base.Y += value.Y;
							base.Orientation = MathHelper.ToRadians(CDGMath.VectorToAngle(pt));
							return;
						}
					}
				}
				else if (terrainObj.CollidesBottom && terrainObj.CollidesTop && terrainObj.CollidesLeft && terrainObj.CollidesRight)
				{
					Vector2 vector2 = CollisionMath.RotatedRectIntersectsMTD(thisBox.AbsRect, thisBox.AbsRotation, Vector2.Zero, otherBox.AbsRect, otherBox.AbsRotation, Vector2.Zero);
					base.CollisionResponse(thisBox, otherBox, collisionResponseType);
					if ((vector2.Y <= 0f && vector2.X == 0f) || otherBox.AbsRotation != 0f)
					{
						base.AccelerationY = 0f;
						base.AccelerationX = 0f;
						base.IsWeighted = false;
						return;
					}
				}
				else if (!terrainObj.CollidesBottom && terrainObj.CollidesTop && !terrainObj.CollidesLeft && !terrainObj.CollidesRight)
				{
					Vector2 vector3 = CollisionMath.RotatedRectIntersectsMTD(thisBox.AbsRect, thisBox.AbsRotation, Vector2.Zero, otherBox.AbsRect, otherBox.AbsRotation, Vector2.Zero);
					if (vector3.Y <= 0f && base.AccelerationY > 0f)
					{
						base.CollisionResponse(thisBox, otherBox, collisionResponseType);
						if ((vector3.Y <= 0f && vector3.X == 0f) || otherBox.AbsRotation != 0f)
						{
							base.AccelerationY = 0f;
							base.AccelerationX = 0f;
							base.IsWeighted = false;
							return;
						}
					}
				}
			}
			else if (otherBox.Type != 0)
			{
				int spell2 = this.Spell;
				if (spell2 == 5)
				{
					if (otherBox.AbsParent == this.Target)
					{
						base.CollisionTypeTag = 2;
						return;
					}
				}
				else
				{
					base.CollisionResponse(thisBox, otherBox, collisionResponseType);
				}
			}
		}
		public void RunDisplacerEffect(RoomObj room, PlayerObj player)
		{
			int num = 2147483647;
			TerrainObj terrainObj = null;
			Vector2 value = Vector2.Zero;
			foreach (TerrainObj current in room.TerrainObjList)
			{
				value = Vector2.Zero;
				float num2 = 3.40282347E+38f;
				if (player.Flip == SpriteEffects.None)
				{
					if (current.X > base.X && current.Bounds.Top < this.Bounds.Bottom && current.Bounds.Bottom > this.Bounds.Top)
					{
						if (current.Rotation < 0f)
						{
							value = CollisionMath.LineToLineIntersect(base.Position, new Vector2(base.X + 6600f, base.Y), CollisionMath.UpperLeftCorner(current.NonRotatedBounds, current.Rotation, Vector2.Zero), CollisionMath.UpperRightCorner(current.NonRotatedBounds, current.Rotation, Vector2.Zero));
						}
						else if (current.Rotation > 0f)
						{
							value = CollisionMath.LineToLineIntersect(base.Position, new Vector2(base.X + 6600f, base.Y), CollisionMath.LowerLeftCorner(current.NonRotatedBounds, current.Rotation, Vector2.Zero), CollisionMath.UpperLeftCorner(current.NonRotatedBounds, current.Rotation, Vector2.Zero));
						}
						if (value != Vector2.Zero)
						{
							num2 = value.X - base.X;
						}
						else
						{
							num2 = (float)(current.Bounds.Left - this.Bounds.Right);
						}
					}
				}
				else if (current.X < base.X && current.Bounds.Top < this.Bounds.Bottom && current.Bounds.Bottom > this.Bounds.Top)
				{
					if (current.Rotation < 0f)
					{
						value = CollisionMath.LineToLineIntersect(new Vector2(base.X - 6600f, base.Y), base.Position, CollisionMath.UpperRightCorner(current.NonRotatedBounds, current.Rotation, Vector2.Zero), CollisionMath.LowerRightCorner(current.NonRotatedBounds, current.Rotation, Vector2.Zero));
					}
					else if (current.Rotation > 0f)
					{
						value = CollisionMath.LineToLineIntersect(new Vector2(base.X - 6600f, base.Y), base.Position, CollisionMath.UpperLeftCorner(current.NonRotatedBounds, current.Rotation, Vector2.Zero), CollisionMath.UpperRightCorner(current.NonRotatedBounds, current.Rotation, Vector2.Zero));
					}
					if (value != Vector2.Zero)
					{
						num2 = base.X - value.X;
					}
					else
					{
						num2 = (float)(this.Bounds.Left - current.Bounds.Right);
					}
				}
				if (num2 < (float)num)
				{
					num = (int)num2;
					terrainObj = current;
				}
			}
			if (terrainObj != null)
			{
				if (player.Flip == SpriteEffects.None)
				{
					if (terrainObj.Rotation == 0f)
					{
						player.X += (float)num - (float)player.TerrainBounds.Width / 2f;
						return;
					}
					player.X += (float)num - (float)player.Width / 2f;
					return;
				}
				else
				{
					if (terrainObj.Rotation == 0f)
					{
						player.X -= (float)num - (float)player.TerrainBounds.Width / 2f;
						return;
					}
					player.X -= (float)num - (float)player.Width / 2f;
				}
			}
		}
		public void RunDestroyAnimation(bool hitPlayer)
		{
			if (!this.IsDying && !this.IsDemented)
			{
				base.CurrentSpeed = 0f;
				base.AccelerationX = 0f;
				base.AccelerationY = 0f;
				this.IsDying = true;
				string spriteName;
				switch (spriteName = base.SpriteName)
				{
				case "ArrowProjectile_Sprite":
				case "SpellClose_Sprite":
				case "SpellDagger_Sprite":
					if (hitPlayer)
					{
						Tween.By(this, 0.3f, new Easing(Linear.EaseNone), new string[]
						{
							"Rotation",
							"270"
						});
						int num2 = CDGMath.RandomInt(-50, 50);
						int num3 = CDGMath.RandomInt(-100, -50);
						Tween.By(this, 0.3f, new Easing(Linear.EaseNone), new string[]
						{
							"X",
							num2.ToString(),
							"Y",
							num3.ToString()
						});
						Tween.To(this, 0.3f, new Easing(Linear.EaseNone), new string[]
						{
							"Opacity",
							"0"
						});
						Tween.AddEndHandlerToLastTween(this, "KillProjectile", new object[0]);
						return;
					}
					base.IsWeighted = false;
					base.IsCollidable = false;
					Tween.To(this, 0.3f, new Easing(Linear.EaseNone), new string[]
					{
						"delay",
						"0.3",
						"Opacity",
						"0"
					});
					Tween.AddEndHandlerToLastTween(this, "KillProjectile", new object[0]);
					return;
				case "ShurikenProjectile1_Sprite":
				case "BoneProjectile_Sprite":
				case "SpellBounce_Sprite":
				case "LastBossSwordVerticalProjectile_Sprite":
				{
					Tween.StopAllContaining(this, false);
					base.IsCollidable = false;
					int num4 = CDGMath.RandomInt(-50, 50);
					int num5 = CDGMath.RandomInt(-100, 100);
					Tween.By(this, 0.3f, new Easing(Linear.EaseNone), new string[]
					{
						"X",
						num4.ToString(),
						"Y",
						num5.ToString()
					});
					Tween.To(this, 0.3f, new Easing(Linear.EaseNone), new string[]
					{
						"Opacity",
						"0"
					});
					Tween.AddEndHandlerToLastTween(this, "KillProjectile", new object[0]);
					return;
				}
				case "HomingProjectile_Sprite":
				{
					ProceduralLevelScreen proceduralLevelScreen = Game.ScreenManager.CurrentScreen as ProceduralLevelScreen;
					if (proceduralLevelScreen != null)
					{
						proceduralLevelScreen.ImpactEffectPool.DisplayExplosionEffect(base.Position);
					}
					SoundManager.Play3DSound(this, Game.ScreenManager.Player, new string[]
					{
						"MissileExplosion_01",
						"MissileExplosion_02"
					});
					this.KillProjectile();
					return;
				}
				case "SpellAxe_Sprite":
				case "SpellDualBlades_Sprite":
					base.IsCollidable = false;
					base.AccelerationX = 0f;
					base.AccelerationY = 0f;
					Tween.To(this, 0.3f, new Easing(Tween.EaseNone), new string[]
					{
						"Opacity",
						"0"
					});
					Tween.AddEndHandlerToLastTween(this, "KillProjectile", new object[0]);
					return;
				case "SpellDamageShield_Sprite":
				case "SpellDisplacer_Sprite":
					Tween.To(this, 0.2f, new Easing(Tween.EaseNone), new string[]
					{
						"Opacity",
						"0"
					});
					Tween.AddEndHandlerToLastTween(this, "KillProjectile", new object[0]);
					return;
				case "LastBossSwordProjectile_Sprite":
				{
					base.IsCollidable = false;
					Tween.StopAllContaining(this, false);
					Tween.By(this, 0.3f, new Easing(Linear.EaseNone), new string[]
					{
						"Rotation",
						"270"
					});
					int num6 = CDGMath.RandomInt(-100, -50);
					Tween.By(this, 0.3f, new Easing(Linear.EaseNone), new string[]
					{
						"Y",
						num6.ToString()
					});
					Tween.To(this, 0.3f, new Easing(Linear.EaseNone), new string[]
					{
						"Opacity",
						"0"
					});
					Tween.AddEndHandlerToLastTween(this, "KillProjectile", new object[0]);
					return;
				}
				case "SpellNuke_Sprite":
				{
					ProceduralLevelScreen proceduralLevelScreen2 = Game.ScreenManager.CurrentScreen as ProceduralLevelScreen;
					if (proceduralLevelScreen2 != null)
					{
						proceduralLevelScreen2.ImpactEffectPool.CrowDestructionEffect(base.Position);
					}
					this.KillProjectile();
					return;
				}
				case "EnemyFlailKnightBall_Sprite":
				case "WizardIceSpell_Sprite":
				case "WizardEarthSpell_Sprite":
				case "SpellTimeBomb_Sprite":
				case "SpellLaser_Sprite":
				case "SpellBoomerang_Sprite":
					this.KillProjectile();
					return;
				}
				if (base.SpriteName == "WizardIceProjectile_Sprite")
				{
					SoundManager.Play3DSound(this, Game.ScreenManager.Player, new string[]
					{
						"Ice_Wizard_Break_01",
						"Ice_Wizard_Break_02",
						"Ice_Wizard_Break_03"
					});
				}
				string text = base.SpriteName.Replace("_", "Explosion_");
				this.ChangeSprite(text);
				base.AnimationDelay = 0.0333333351f;
				base.PlayAnimation(false);
				base.IsWeighted = false;
				base.IsCollidable = false;
				if (text != "EnemySpearKnightWaveExplosion_Sprite" && text != "WizardIceProjectileExplosion_Sprite")
				{
					base.Rotation = 0f;
				}
				Tween.RunFunction(0.5f, this, "KillProjectile", new object[0]);
			}
		}
		public void ActivateEffect()
		{
			int spell = this.Spell;
			switch (spell)
			{
			case 3:
				base.IsWeighted = false;
				this.ChangeSprite("SpellTimeBombExplosion_Sprite");
				base.PlayAnimation(false);
				this.IsDying = true;
				base.CollisionTypeTag = 10;
				base.AnimationDelay = 0.0333333351f;
				this.Scale = new Vector2(4f, 4f);
				Tween.RunFunction(0.5f, this, "KillProjectile", new object[0]);
				(Game.ScreenManager.CurrentScreen as ProceduralLevelScreen).ImpactEffectPool.DisplayExplosionEffect(base.Position);
				return;
			case 4:
			case 6:
				return;
			case 5:
				this.RunDestroyAnimation(false);
				(Game.ScreenManager.CurrentScreen as ProceduralLevelScreen).DamageAllEnemies(this.Damage);
				(Game.ScreenManager.CurrentScreen as ProceduralLevelScreen).ImpactEffectPool.DisplayDeathEffect(base.Position);
				return;
			case 7:
			{
				this.RunDestroyAnimation(false);
				PlayerObj player = (Game.ScreenManager.CurrentScreen as ProceduralLevelScreen).Player;
				player.Translocate(base.Position);
				return;
			}
			case 8:
				break;
			default:
				if (spell != 12)
				{
					if (spell != 100)
					{
						return;
					}
					base.CollisionTypeTag = 10;
					this.LifeSpan = this.AltY;
					this.m_elapsedLifeSpan = 0f;
					base.IsCollidable = true;
					base.Opacity = 1f;
					return;
				}
				break;
			}
			base.CollisionTypeTag = 10;
		}
		public void KillProjectile()
		{
			this.IsAlive = false;
			this.IsDying = false;
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.Target = null;
				this.AttachedIcon = null;
				this.Source = null;
				base.Dispose();
			}
		}
		protected override GameObj CreateCloneInstance()
		{
			return new ProjectileObj(this._spriteName);
		}
		protected override void FillCloneInstance(object obj)
		{
			base.FillCloneInstance(obj);
			ProjectileObj projectileObj = obj as ProjectileObj;
			projectileObj.Target = this.Target;
			projectileObj.CollidesWithTerrain = this.CollidesWithTerrain;
			projectileObj.ChaseTarget = this.ChaseTarget;
			projectileObj.FollowArc = this.FollowArc;
			projectileObj.ShowIcon = this.ShowIcon;
			projectileObj.DestroysWithTerrain = this.DestroysWithTerrain;
			projectileObj.AltX = this.AltX;
			projectileObj.AltY = this.AltY;
			projectileObj.Spell = this.Spell;
			projectileObj.IgnoreBoundsCheck = this.IgnoreBoundsCheck;
			projectileObj.DestroysWithEnemy = this.DestroysWithEnemy;
			projectileObj.DestroyOnRoomTransition = this.DestroyOnRoomTransition;
			projectileObj.Source = this.Source;
			projectileObj.CanBeFusRohDahed = this.CanBeFusRohDahed;
			projectileObj.WrapProjectile = this.WrapProjectile;
			projectileObj.IgnoreInvincibleCounter = this.IgnoreInvincibleCounter;
		}
	}
}
