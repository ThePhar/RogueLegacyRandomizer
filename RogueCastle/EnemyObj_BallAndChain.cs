using DS2DEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
namespace RogueCastle
{
	public class EnemyObj_BallAndChain : EnemyObj
	{
		private LogicBlock m_generalBasicLB = new LogicBlock();
		private LogicBlock m_generalCooldownLB = new LogicBlock();
		private ProjectileObj m_ballAndChain;
		private ProjectileObj m_ballAndChain2;
		private SpriteObj m_chain;
		private int m_numChainLinks = 10;
		private List<Vector2> m_chainLinksList;
		private List<Vector2> m_chainLinks2List;
		private float m_chainRadius;
		private float m_actualChainRadius;
		private float m_ballAngle;
		private float m_chainLinkDistance;
		private float m_BallSpeedDivider = 1.5f;
		private FrameSoundObj m_walkSound;
		private FrameSoundObj m_walkSound2;
		public float ChainSpeed
		{
			get;
			set;
		}
		public float ChainSpeed2Modifier
		{
			get;
			set;
		}
		private float ChainRadius
		{
			get
			{
				return this.m_chainRadius;
			}
			set
			{
				this.m_chainRadius = value;
			}
		}
		public ProjectileObj BallAndChain
		{
			get
			{
				return this.m_ballAndChain;
			}
		}
		public ProjectileObj BallAndChain2
		{
			get
			{
				return this.m_ballAndChain2;
			}
		}
		protected override void InitializeEV()
		{
			this.ChainSpeed = 2.5f;
			this.ChainRadius = 260f;
			this.ChainSpeed2Modifier = 1.5f;
			base.Name = "Chaintor";
			this.MaxHealth = 40;
			base.Damage = 27;
			base.XPValue = 125;
			this.MinMoneyDropAmount = 1;
			this.MaxMoneyDropAmount = 2;
			this.MoneyDropChance = 0.4f;
			base.Speed = 100f;
			this.TurnSpeed = 10f;
			this.ProjectileSpeed = 1020f;
			base.JumpHeight = 600f;
			this.CooldownTime = 2f;
			base.AnimationDelay = 0.1f;
			this.AlwaysFaceTarget = true;
			this.CanFallOffLedges = false;
			base.CanBeKnockedBack = true;
			base.IsWeighted = true;
			this.Scale = EnemyEV.BallAndChain_Basic_Scale;
			base.ProjectileScale = EnemyEV.BallAndChain_Basic_ProjectileScale;
			this.TintablePart.TextureColor = EnemyEV.BallAndChain_Basic_Tint;
			this.MeleeRadius = 225;
			this.ProjectileRadius = 500;
			this.EngageRadius = 800;
			this.ProjectileDamage = base.Damage;
			base.KnockBack = EnemyEV.BallAndChain_Basic_KnockBack;
			switch (base.Difficulty)
			{
			case GameTypes.EnemyDifficulty.ADVANCED:
				this.ChainRadius = 275f;
				base.Name = "Chaintex";
				this.MaxHealth = 58;
				base.Damage = 32;
				base.XPValue = 150;
				this.MinMoneyDropAmount = 1;
				this.MaxMoneyDropAmount = 2;
				this.MoneyDropChance = 0.5f;
				base.Speed = 150f;
				this.TurnSpeed = 10f;
				this.ProjectileSpeed = 1020f;
				base.JumpHeight = 600f;
				this.CooldownTime = 2f;
				base.AnimationDelay = 0.1f;
				this.AlwaysFaceTarget = true;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = true;
				base.IsWeighted = true;
				this.Scale = EnemyEV.BallAndChain_Advanced_Scale;
				base.ProjectileScale = EnemyEV.BallAndChain_Advanced_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.BallAndChain_Advanced_Tint;
				this.MeleeRadius = 225;
				this.EngageRadius = 800;
				this.ProjectileRadius = 500;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.BallAndChain_Advanced_KnockBack;
				break;
			case GameTypes.EnemyDifficulty.EXPERT:
				this.ChainRadius = 350f;
				this.ChainSpeed2Modifier = 1.5f;
				base.Name = "Chaintus";
				this.MaxHealth = 79;
				base.Damage = 36;
				base.XPValue = 200;
				this.MinMoneyDropAmount = 2;
				this.MaxMoneyDropAmount = 4;
				this.MoneyDropChance = 1f;
				base.Speed = 175f;
				this.TurnSpeed = 10f;
				this.ProjectileSpeed = 1020f;
				base.JumpHeight = 600f;
				this.CooldownTime = 2f;
				base.AnimationDelay = 0.1f;
				this.AlwaysFaceTarget = true;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = true;
				base.IsWeighted = true;
				this.Scale = EnemyEV.BallAndChain_Expert_Scale;
				base.ProjectileScale = EnemyEV.BallAndChain_Expert_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.BallAndChain_Expert_Tint;
				this.MeleeRadius = 225;
				this.ProjectileRadius = 500;
				this.EngageRadius = 800;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.BallAndChain_Expert_KnockBack;
				break;
			case GameTypes.EnemyDifficulty.MINIBOSS:
				base.Name = "Pantheon";
				this.MaxHealth = 300;
				base.Damage = 60;
				base.XPValue = 1250;
				this.MinMoneyDropAmount = 1;
				this.MaxMoneyDropAmount = 4;
				this.MoneyDropChance = 1f;
				base.Speed = 100f;
				this.TurnSpeed = 10f;
				this.ProjectileSpeed = 1020f;
				base.JumpHeight = 600f;
				this.CooldownTime = 2f;
				base.AnimationDelay = 0.1f;
				this.AlwaysFaceTarget = false;
				this.CanFallOffLedges = false;
				base.CanBeKnockedBack = true;
				base.IsWeighted = true;
				this.Scale = EnemyEV.BallAndChain_Miniboss_Scale;
				base.ProjectileScale = EnemyEV.BallAndChain_Miniboss_ProjectileScale;
				this.TintablePart.TextureColor = EnemyEV.BallAndChain_Miniboss_Tint;
				this.MeleeRadius = 225;
				this.ProjectileRadius = 500;
				this.EngageRadius = 800;
				this.ProjectileDamage = base.Damage;
				base.KnockBack = EnemyEV.BallAndChain_Miniboss_KnockBack;
				break;
			}
			this._objectList[1].TextureColor = this.TintablePart.TextureColor;
			this.m_ballAndChain.Damage = base.Damage;
			this.m_ballAndChain.Scale = base.ProjectileScale;
			this.m_ballAndChain2.Damage = base.Damage;
			this.m_ballAndChain2.Scale = base.ProjectileScale;
		}
		protected override void InitializeLogic()
		{
			LogicSet logicSet = new LogicSet(this);
			logicSet.AddAction(new MoveLogicAction(this.m_target, true, -1f), Types.Sequence.Serial);
			logicSet.AddAction(new DelayLogicAction(1.25f, 2.75f, false), Types.Sequence.Serial);
			LogicSet logicSet2 = new LogicSet(this);
			logicSet2.AddAction(new MoveLogicAction(this.m_target, false, -1f), Types.Sequence.Serial);
			logicSet2.AddAction(new DelayLogicAction(1.25f, 2.75f, false), Types.Sequence.Serial);
			LogicSet logicSet3 = new LogicSet(this);
			logicSet3.AddAction(new StopAnimationLogicAction(), Types.Sequence.Serial);
			logicSet3.AddAction(new MoveLogicAction(this.m_target, true, 0f), Types.Sequence.Serial);
			logicSet3.AddAction(new DelayLogicAction(1f, 1.5f, false), Types.Sequence.Serial);
			this.m_generalBasicLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2,
				logicSet3
			});
			this.m_generalCooldownLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2,
				logicSet3
			});
			this.logicBlocksToDispose.Add(this.m_generalBasicLB);
			this.logicBlocksToDispose.Add(this.m_generalCooldownLB);
			base.SetCooldownLogicBlock(this.m_generalCooldownLB, new int[]
			{
				40,
				40,
				20
			});
			base.InitializeLogic();
		}
		protected override void RunBasicLogic()
		{
			switch (base.State)
			{
			case 0:
				base.RunLogicBlock(true, this.m_generalBasicLB, new int[]
				{
					0,
					0,
					100
				});
				return;
			case 1:
			case 2:
			case 3:
				base.RunLogicBlock(true, this.m_generalBasicLB, new int[]
				{
					60,
					20,
					20
				});
				return;
			default:
				return;
			}
		}
		protected override void RunAdvancedLogic()
		{
			switch (base.State)
			{
			case 0:
				base.RunLogicBlock(true, this.m_generalBasicLB, new int[]
				{
					0,
					0,
					100
				});
				return;
			case 1:
			case 2:
			case 3:
				base.RunLogicBlock(true, this.m_generalBasicLB, new int[]
				{
					60,
					20,
					20
				});
				return;
			default:
				return;
			}
		}
		protected override void RunExpertLogic()
		{
			switch (base.State)
			{
			case 0:
				base.RunLogicBlock(true, this.m_generalBasicLB, new int[]
				{
					0,
					0,
					100
				});
				return;
			case 1:
			case 2:
			case 3:
				base.RunLogicBlock(true, this.m_generalBasicLB, new int[]
				{
					60,
					20,
					20
				});
				return;
			default:
				return;
			}
		}
		protected override void RunMinibossLogic()
		{
			switch (base.State)
			{
			case 0:
			case 1:
			case 2:
			case 3:
				return;
			}
		}
		public EnemyObj_BallAndChain(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty) : base("EnemyFlailKnight_Character", target, physicsManager, levelToAttachTo, difficulty)
		{
			this.m_ballAndChain = new ProjectileObj("EnemyFlailKnightBall_Sprite");
			this.m_ballAndChain.IsWeighted = false;
			this.m_ballAndChain.CollidesWithTerrain = false;
			this.m_ballAndChain.IgnoreBoundsCheck = true;
			this.m_ballAndChain.OutlineWidth = 2;
			this.m_ballAndChain2 = (this.m_ballAndChain.Clone() as ProjectileObj);
			this.m_chain = new SpriteObj("EnemyFlailKnightLink_Sprite");
			this.m_chainLinksList = new List<Vector2>();
			this.m_chainLinks2List = new List<Vector2>();
			for (int i = 0; i < this.m_numChainLinks; i++)
			{
				this.m_chainLinksList.Add(default(Vector2));
			}
			for (int j = 0; j < this.m_numChainLinks / 2; j++)
			{
				this.m_chainLinks2List.Add(default(Vector2));
			}
			this.Type = 1;
			this.TintablePart = this._objectList[3];
			this.m_walkSound = new FrameSoundObj(this, this.m_target, 1, new string[]
			{
				"KnightWalk1",
				"KnightWalk2"
			});
			this.m_walkSound2 = new FrameSoundObj(this, this.m_target, 6, new string[]
			{
				"KnightWalk1",
				"KnightWalk2"
			});
		}
		public override void Update(GameTime gameTime)
		{
			if (!base.IsPaused)
			{
				if (!base.IsKilled && this.m_initialDelayCounter <= 0f)
				{
					float num = (float)gameTime.ElapsedGameTime.TotalSeconds;
					if (this.m_actualChainRadius < this.ChainRadius)
					{
						this.m_actualChainRadius += num * 200f;
						this.m_chainLinkDistance = this.m_actualChainRadius / (float)this.m_numChainLinks;
					}
					float num2 = 0f;
					this.m_ballAndChain.Position = CDGMath.GetCirclePosition(this.m_ballAngle, this.m_actualChainRadius, new Vector2(base.X, (float)this.Bounds.Top));
					for (int i = 0; i < this.m_chainLinksList.Count; i++)
					{
						this.m_chainLinksList[i] = CDGMath.GetCirclePosition(this.m_ballAngle, num2, new Vector2(base.X, (float)this.Bounds.Top));
						num2 += this.m_chainLinkDistance;
					}
					num2 = 0f;
					if (base.Difficulty == GameTypes.EnemyDifficulty.ADVANCED)
					{
						this.m_ballAndChain2.Position = CDGMath.GetCirclePosition(this.m_ballAngle * this.ChainSpeed2Modifier, this.m_actualChainRadius / 2f, new Vector2(base.X, (float)this.Bounds.Top));
					}
					else if (base.Difficulty == GameTypes.EnemyDifficulty.EXPERT)
					{
						this.m_ballAndChain2.Position = CDGMath.GetCirclePosition(-this.m_ballAngle * this.ChainSpeed2Modifier, -this.m_actualChainRadius / 2f, new Vector2(base.X, (float)this.Bounds.Top));
					}
					for (int j = 0; j < this.m_chainLinks2List.Count; j++)
					{
						if (base.Difficulty == GameTypes.EnemyDifficulty.ADVANCED)
						{
							this.m_chainLinks2List[j] = CDGMath.GetCirclePosition(this.m_ballAngle * this.ChainSpeed2Modifier, num2, new Vector2(base.X, (float)this.Bounds.Top));
						}
						else if (base.Difficulty == GameTypes.EnemyDifficulty.EXPERT)
						{
							this.m_chainLinks2List[j] = CDGMath.GetCirclePosition(-this.m_ballAngle * this.ChainSpeed2Modifier, -num2, new Vector2(base.X, (float)this.Bounds.Top));
						}
						num2 += this.m_chainLinkDistance;
					}
					this.m_ballAngle += this.ChainSpeed * 60f * num;
					if (!base.IsAnimating && base.CurrentSpeed != 0f)
					{
						base.PlayAnimation(true);
					}
				}
				if (base.SpriteName == "EnemyFlailKnight_Character")
				{
					this.m_walkSound.Update();
					this.m_walkSound2.Update();
				}
			}
			base.Update(gameTime);
		}
		public override void Draw(Camera2D camera)
		{
			if (!base.IsKilled)
			{
				foreach (Vector2 current in this.m_chainLinksList)
				{
					this.m_chain.Position = current;
					this.m_chain.Draw(camera);
				}
				this.m_ballAndChain.Draw(camera);
				if (base.Difficulty > GameTypes.EnemyDifficulty.BASIC)
				{
					foreach (Vector2 current2 in this.m_chainLinks2List)
					{
						this.m_chain.Position = current2;
						this.m_chain.Draw(camera);
					}
					this.m_ballAndChain2.Draw(camera);
				}
			}
			base.Draw(camera);
		}
		public override void Kill(bool giveXP = true)
		{
			this.m_levelScreen.PhysicsManager.RemoveObject(this.m_ballAndChain);
			EnemyObj_BouncySpike enemyObj_BouncySpike = new EnemyObj_BouncySpike(this.m_target, null, this.m_levelScreen, base.Difficulty);
			enemyObj_BouncySpike.SavedStartingPos = base.Position;
			enemyObj_BouncySpike.Position = base.Position;
			this.m_levelScreen.AddEnemyToCurrentRoom(enemyObj_BouncySpike);
			enemyObj_BouncySpike.Position = this.m_ballAndChain.Position;
			enemyObj_BouncySpike.Speed = this.ChainSpeed * 200f / this.m_BallSpeedDivider;
			enemyObj_BouncySpike.HeadingX = (float)Math.Cos((double)MathHelper.WrapAngle(MathHelper.ToRadians(this.m_ballAngle + 90f)));
			enemyObj_BouncySpike.HeadingY = (float)Math.Sin((double)MathHelper.WrapAngle(MathHelper.ToRadians(this.m_ballAngle + 90f)));
			if (base.Difficulty > GameTypes.EnemyDifficulty.BASIC)
			{
				this.m_levelScreen.PhysicsManager.RemoveObject(this.m_ballAndChain2);
				EnemyObj_BouncySpike enemyObj_BouncySpike2 = new EnemyObj_BouncySpike(this.m_target, null, this.m_levelScreen, base.Difficulty);
				enemyObj_BouncySpike2.SavedStartingPos = base.Position;
				enemyObj_BouncySpike2.Position = base.Position;
				this.m_levelScreen.AddEnemyToCurrentRoom(enemyObj_BouncySpike2);
				enemyObj_BouncySpike2.Position = this.m_ballAndChain2.Position;
				enemyObj_BouncySpike2.Speed = this.ChainSpeed * 200f * this.ChainSpeed2Modifier / this.m_BallSpeedDivider;
				if (base.Difficulty == GameTypes.EnemyDifficulty.ADVANCED)
				{
					enemyObj_BouncySpike2.HeadingX = (float)Math.Cos((double)MathHelper.WrapAngle(MathHelper.ToRadians(this.m_ballAngle * this.ChainSpeed2Modifier + 90f)));
					enemyObj_BouncySpike2.HeadingY = (float)Math.Sin((double)MathHelper.WrapAngle(MathHelper.ToRadians(this.m_ballAngle * this.ChainSpeed2Modifier + 90f)));
				}
				else if (base.Difficulty == GameTypes.EnemyDifficulty.EXPERT)
				{
					enemyObj_BouncySpike2.HeadingX = (float)Math.Cos((double)MathHelper.WrapAngle(MathHelper.ToRadians(-this.m_ballAngle * this.ChainSpeed2Modifier + 90f)));
					enemyObj_BouncySpike2.HeadingY = (float)Math.Sin((double)MathHelper.WrapAngle(MathHelper.ToRadians(-this.m_ballAngle * this.ChainSpeed2Modifier + 90f)));
				}
				enemyObj_BouncySpike2.SpawnRoom = this.m_levelScreen.CurrentRoom;
				enemyObj_BouncySpike2.SaveToFile = false;
				if (base.IsPaused)
				{
					enemyObj_BouncySpike2.PauseEnemy(false);
				}
			}
			enemyObj_BouncySpike.SpawnRoom = this.m_levelScreen.CurrentRoom;
			enemyObj_BouncySpike.SaveToFile = false;
			if (base.IsPaused)
			{
				enemyObj_BouncySpike.PauseEnemy(false);
			}
			base.Kill(giveXP);
		}
		public override void ResetState()
		{
			base.ResetState();
			this.m_actualChainRadius = 0f;
			this.m_chainLinkDistance = this.m_actualChainRadius / (float)this.m_numChainLinks;
			float num = 0f;
			this.m_ballAndChain.Position = CDGMath.GetCirclePosition(this.m_ballAngle, this.m_actualChainRadius, new Vector2(base.X, (float)this.Bounds.Top));
			for (int i = 0; i < this.m_chainLinksList.Count; i++)
			{
				this.m_chainLinksList[i] = CDGMath.GetCirclePosition(this.m_ballAngle, num, new Vector2(base.X, (float)this.Bounds.Top));
				num += this.m_chainLinkDistance;
			}
			num = 0f;
			if (base.Difficulty == GameTypes.EnemyDifficulty.ADVANCED)
			{
				this.m_ballAndChain2.Position = CDGMath.GetCirclePosition(this.m_ballAngle * this.ChainSpeed2Modifier, this.m_actualChainRadius / 2f, new Vector2(base.X, (float)this.Bounds.Top));
			}
			else if (base.Difficulty == GameTypes.EnemyDifficulty.EXPERT)
			{
				this.m_ballAndChain2.Position = CDGMath.GetCirclePosition(-this.m_ballAngle * this.ChainSpeed2Modifier, -this.m_actualChainRadius / 2f, new Vector2(base.X, (float)this.Bounds.Top));
			}
			for (int j = 0; j < this.m_chainLinks2List.Count; j++)
			{
				if (base.Difficulty == GameTypes.EnemyDifficulty.ADVANCED)
				{
					this.m_chainLinks2List[j] = CDGMath.GetCirclePosition(this.m_ballAngle * this.ChainSpeed2Modifier, num, new Vector2(base.X, (float)this.Bounds.Top));
				}
				else if (base.Difficulty == GameTypes.EnemyDifficulty.EXPERT)
				{
					this.m_chainLinks2List[j] = CDGMath.GetCirclePosition(-this.m_ballAngle * this.ChainSpeed2Modifier, -num, new Vector2(base.X, (float)this.Bounds.Top));
				}
				num += this.m_chainLinkDistance;
			}
		}
		public override void HitEnemy(int damage, Vector2 position, bool isPlayer)
		{
			SoundManager.Play3DSound(this, this.m_target, new string[]
			{
				"Knight_Hit01",
				"Knight_Hit02",
				"Knight_Hit03"
			});
			base.HitEnemy(damage, position, isPlayer);
		}
		public override void Dispose()
		{
			if (!base.IsDisposed)
			{
				this.m_chain.Dispose();
				this.m_chain = null;
				this.m_ballAndChain.Dispose();
				this.m_ballAndChain = null;
				this.m_ballAndChain2.Dispose();
				this.m_ballAndChain2 = null;
				this.m_chainLinksList.Clear();
				this.m_chainLinksList = null;
				this.m_chainLinks2List.Clear();
				this.m_chainLinks2List = null;
				this.m_walkSound.Dispose();
				this.m_walkSound = null;
				this.m_walkSound2.Dispose();
				this.m_walkSound2 = null;
				base.Dispose();
			}
		}
	}
}
