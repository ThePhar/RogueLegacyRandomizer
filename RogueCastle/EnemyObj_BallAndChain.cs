/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators..
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using System;
using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;

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
				return m_chainRadius;
			}
			set
			{
				m_chainRadius = value;
			}
		}
		public ProjectileObj BallAndChain
		{
			get
			{
				return m_ballAndChain;
			}
		}
		public ProjectileObj BallAndChain2
		{
			get
			{
				return m_ballAndChain2;
			}
		}
		protected override void InitializeEV()
		{
			ChainSpeed = 2.5f;
			ChainRadius = 260f;
			ChainSpeed2Modifier = 1.5f;
			Name = "Chaintor";
			MaxHealth = 40;
			Damage = 27;
			XPValue = 125;
			MinMoneyDropAmount = 1;
			MaxMoneyDropAmount = 2;
			MoneyDropChance = 0.4f;
			Speed = 100f;
			TurnSpeed = 10f;
			ProjectileSpeed = 1020f;
			JumpHeight = 600f;
			CooldownTime = 2f;
			AnimationDelay = 0.1f;
			AlwaysFaceTarget = true;
			CanFallOffLedges = false;
			CanBeKnockedBack = true;
			IsWeighted = true;
			Scale = EnemyEV.BallAndChain_Basic_Scale;
			ProjectileScale = EnemyEV.BallAndChain_Basic_ProjectileScale;
			TintablePart.TextureColor = EnemyEV.BallAndChain_Basic_Tint;
			MeleeRadius = 225;
			ProjectileRadius = 500;
			EngageRadius = 800;
			ProjectileDamage = Damage;
			KnockBack = EnemyEV.BallAndChain_Basic_KnockBack;
			switch (Difficulty)
			{
			case GameTypes.EnemyDifficulty.ADVANCED:
				ChainRadius = 275f;
				Name = "Chaintex";
				MaxHealth = 58;
				Damage = 32;
				XPValue = 150;
				MinMoneyDropAmount = 1;
				MaxMoneyDropAmount = 2;
				MoneyDropChance = 0.5f;
				Speed = 150f;
				TurnSpeed = 10f;
				ProjectileSpeed = 1020f;
				JumpHeight = 600f;
				CooldownTime = 2f;
				AnimationDelay = 0.1f;
				AlwaysFaceTarget = true;
				CanFallOffLedges = false;
				CanBeKnockedBack = true;
				IsWeighted = true;
				Scale = EnemyEV.BallAndChain_Advanced_Scale;
				ProjectileScale = EnemyEV.BallAndChain_Advanced_ProjectileScale;
				TintablePart.TextureColor = EnemyEV.BallAndChain_Advanced_Tint;
				MeleeRadius = 225;
				EngageRadius = 800;
				ProjectileRadius = 500;
				ProjectileDamage = Damage;
				KnockBack = EnemyEV.BallAndChain_Advanced_KnockBack;
				break;
			case GameTypes.EnemyDifficulty.EXPERT:
				ChainRadius = 350f;
				ChainSpeed2Modifier = 1.5f;
				Name = "Chaintus";
				MaxHealth = 79;
				Damage = 36;
				XPValue = 200;
				MinMoneyDropAmount = 2;
				MaxMoneyDropAmount = 4;
				MoneyDropChance = 1f;
				Speed = 175f;
				TurnSpeed = 10f;
				ProjectileSpeed = 1020f;
				JumpHeight = 600f;
				CooldownTime = 2f;
				AnimationDelay = 0.1f;
				AlwaysFaceTarget = true;
				CanFallOffLedges = false;
				CanBeKnockedBack = true;
				IsWeighted = true;
				Scale = EnemyEV.BallAndChain_Expert_Scale;
				ProjectileScale = EnemyEV.BallAndChain_Expert_ProjectileScale;
				TintablePart.TextureColor = EnemyEV.BallAndChain_Expert_Tint;
				MeleeRadius = 225;
				ProjectileRadius = 500;
				EngageRadius = 800;
				ProjectileDamage = Damage;
				KnockBack = EnemyEV.BallAndChain_Expert_KnockBack;
				break;
			case GameTypes.EnemyDifficulty.MINIBOSS:
				Name = "Pantheon";
				MaxHealth = 300;
				Damage = 60;
				XPValue = 1250;
				MinMoneyDropAmount = 1;
				MaxMoneyDropAmount = 4;
				MoneyDropChance = 1f;
				Speed = 100f;
				TurnSpeed = 10f;
				ProjectileSpeed = 1020f;
				JumpHeight = 600f;
				CooldownTime = 2f;
				AnimationDelay = 0.1f;
				AlwaysFaceTarget = false;
				CanFallOffLedges = false;
				CanBeKnockedBack = true;
				IsWeighted = true;
				Scale = EnemyEV.BallAndChain_Miniboss_Scale;
				ProjectileScale = EnemyEV.BallAndChain_Miniboss_ProjectileScale;
				TintablePart.TextureColor = EnemyEV.BallAndChain_Miniboss_Tint;
				MeleeRadius = 225;
				ProjectileRadius = 500;
				EngageRadius = 800;
				ProjectileDamage = Damage;
				KnockBack = EnemyEV.BallAndChain_Miniboss_KnockBack;
				break;
			}
			_objectList[1].TextureColor = TintablePart.TextureColor;
			m_ballAndChain.Damage = Damage;
			m_ballAndChain.Scale = ProjectileScale;
			m_ballAndChain2.Damage = Damage;
			m_ballAndChain2.Scale = ProjectileScale;
		}
		protected override void InitializeLogic()
		{
			LogicSet logicSet = new LogicSet(this);
			logicSet.AddAction(new MoveLogicAction(m_target, true, -1f), Types.Sequence.Serial);
			logicSet.AddAction(new DelayLogicAction(1.25f, 2.75f, false), Types.Sequence.Serial);
			LogicSet logicSet2 = new LogicSet(this);
			logicSet2.AddAction(new MoveLogicAction(m_target, false, -1f), Types.Sequence.Serial);
			logicSet2.AddAction(new DelayLogicAction(1.25f, 2.75f, false), Types.Sequence.Serial);
			LogicSet logicSet3 = new LogicSet(this);
			logicSet3.AddAction(new StopAnimationLogicAction(), Types.Sequence.Serial);
			logicSet3.AddAction(new MoveLogicAction(m_target, true, 0f), Types.Sequence.Serial);
			logicSet3.AddAction(new DelayLogicAction(1f, 1.5f, false), Types.Sequence.Serial);
			m_generalBasicLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2,
				logicSet3
			});
			m_generalCooldownLB.AddLogicSet(new LogicSet[]
			{
				logicSet,
				logicSet2,
				logicSet3
			});
			logicBlocksToDispose.Add(m_generalBasicLB);
			logicBlocksToDispose.Add(m_generalCooldownLB);
			SetCooldownLogicBlock(m_generalCooldownLB, new int[]
			{
				40,
				40,
				20
			});
			base.InitializeLogic();
		}
		protected override void RunBasicLogic()
		{
			switch (State)
			{
			case 0:
				RunLogicBlock(true, m_generalBasicLB, new int[]
				{
					0,
					0,
					100
				});
				return;
			case 1:
			case 2:
			case 3:
				RunLogicBlock(true, m_generalBasicLB, new int[]
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
			switch (State)
			{
			case 0:
				RunLogicBlock(true, m_generalBasicLB, new int[]
				{
					0,
					0,
					100
				});
				return;
			case 1:
			case 2:
			case 3:
				RunLogicBlock(true, m_generalBasicLB, new int[]
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
			switch (State)
			{
			case 0:
				RunLogicBlock(true, m_generalBasicLB, new int[]
				{
					0,
					0,
					100
				});
				return;
			case 1:
			case 2:
			case 3:
				RunLogicBlock(true, m_generalBasicLB, new int[]
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
			switch (State)
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
			m_ballAndChain = new ProjectileObj("EnemyFlailKnightBall_Sprite");
			m_ballAndChain.IsWeighted = false;
			m_ballAndChain.CollidesWithTerrain = false;
			m_ballAndChain.IgnoreBoundsCheck = true;
			m_ballAndChain.OutlineWidth = 2;
			m_ballAndChain2 = (m_ballAndChain.Clone() as ProjectileObj);
			m_chain = new SpriteObj("EnemyFlailKnightLink_Sprite");
			m_chainLinksList = new List<Vector2>();
			m_chainLinks2List = new List<Vector2>();
			for (int i = 0; i < m_numChainLinks; i++)
			{
				m_chainLinksList.Add(default(Vector2));
			}
			for (int j = 0; j < m_numChainLinks / 2; j++)
			{
				m_chainLinks2List.Add(default(Vector2));
			}
			Type = 1;
			TintablePart = _objectList[3];
			m_walkSound = new FrameSoundObj(this, m_target, 1, new string[]
			{
				"KnightWalk1",
				"KnightWalk2"
			});
			m_walkSound2 = new FrameSoundObj(this, m_target, 6, new string[]
			{
				"KnightWalk1",
				"KnightWalk2"
			});
		}
		public override void Update(GameTime gameTime)
		{
			if (!IsPaused)
			{
				if (!IsKilled && m_initialDelayCounter <= 0f)
				{
					float num = (float)gameTime.ElapsedGameTime.TotalSeconds;
					if (m_actualChainRadius < ChainRadius)
					{
						m_actualChainRadius += num * 200f;
						m_chainLinkDistance = m_actualChainRadius / m_numChainLinks;
					}
					float num2 = 0f;
					m_ballAndChain.Position = CDGMath.GetCirclePosition(m_ballAngle, m_actualChainRadius, new Vector2(X, Bounds.Top));
					for (int i = 0; i < m_chainLinksList.Count; i++)
					{
						m_chainLinksList[i] = CDGMath.GetCirclePosition(m_ballAngle, num2, new Vector2(X, Bounds.Top));
						num2 += m_chainLinkDistance;
					}
					num2 = 0f;
					if (Difficulty == GameTypes.EnemyDifficulty.ADVANCED)
					{
						m_ballAndChain2.Position = CDGMath.GetCirclePosition(m_ballAngle * ChainSpeed2Modifier, m_actualChainRadius / 2f, new Vector2(X, Bounds.Top));
					}
					else if (Difficulty == GameTypes.EnemyDifficulty.EXPERT)
					{
						m_ballAndChain2.Position = CDGMath.GetCirclePosition(-m_ballAngle * ChainSpeed2Modifier, -m_actualChainRadius / 2f, new Vector2(X, Bounds.Top));
					}
					for (int j = 0; j < m_chainLinks2List.Count; j++)
					{
						if (Difficulty == GameTypes.EnemyDifficulty.ADVANCED)
						{
							m_chainLinks2List[j] = CDGMath.GetCirclePosition(m_ballAngle * ChainSpeed2Modifier, num2, new Vector2(X, Bounds.Top));
						}
						else if (Difficulty == GameTypes.EnemyDifficulty.EXPERT)
						{
							m_chainLinks2List[j] = CDGMath.GetCirclePosition(-m_ballAngle * ChainSpeed2Modifier, -num2, new Vector2(X, Bounds.Top));
						}
						num2 += m_chainLinkDistance;
					}
					m_ballAngle += ChainSpeed * 60f * num;
					if (!IsAnimating && CurrentSpeed != 0f)
					{
						PlayAnimation(true);
					}
				}
				if (SpriteName == "EnemyFlailKnight_Character")
				{
					m_walkSound.Update();
					m_walkSound2.Update();
				}
			}
			base.Update(gameTime);
		}
		public override void Draw(Camera2D camera)
		{
			if (!IsKilled)
			{
				foreach (Vector2 current in m_chainLinksList)
				{
					m_chain.Position = current;
					m_chain.Draw(camera);
				}
				m_ballAndChain.Draw(camera);
				if (Difficulty > GameTypes.EnemyDifficulty.BASIC)
				{
					foreach (Vector2 current2 in m_chainLinks2List)
					{
						m_chain.Position = current2;
						m_chain.Draw(camera);
					}
					m_ballAndChain2.Draw(camera);
				}
			}
			base.Draw(camera);
		}
		public override void Kill(bool giveXP = true)
		{
			m_levelScreen.PhysicsManager.RemoveObject(m_ballAndChain);
			EnemyObj_BouncySpike enemyObj_BouncySpike = new EnemyObj_BouncySpike(m_target, null, m_levelScreen, Difficulty);
			enemyObj_BouncySpike.SavedStartingPos = Position;
			enemyObj_BouncySpike.Position = Position;
			m_levelScreen.AddEnemyToCurrentRoom(enemyObj_BouncySpike);
			enemyObj_BouncySpike.Position = m_ballAndChain.Position;
			enemyObj_BouncySpike.Speed = ChainSpeed * 200f / m_BallSpeedDivider;
			enemyObj_BouncySpike.HeadingX = (float)Math.Cos(MathHelper.WrapAngle(MathHelper.ToRadians(m_ballAngle + 90f)));
			enemyObj_BouncySpike.HeadingY = (float)Math.Sin(MathHelper.WrapAngle(MathHelper.ToRadians(m_ballAngle + 90f)));
			if (Difficulty > GameTypes.EnemyDifficulty.BASIC)
			{
				m_levelScreen.PhysicsManager.RemoveObject(m_ballAndChain2);
				EnemyObj_BouncySpike enemyObj_BouncySpike2 = new EnemyObj_BouncySpike(m_target, null, m_levelScreen, Difficulty);
				enemyObj_BouncySpike2.SavedStartingPos = Position;
				enemyObj_BouncySpike2.Position = Position;
				m_levelScreen.AddEnemyToCurrentRoom(enemyObj_BouncySpike2);
				enemyObj_BouncySpike2.Position = m_ballAndChain2.Position;
				enemyObj_BouncySpike2.Speed = ChainSpeed * 200f * ChainSpeed2Modifier / m_BallSpeedDivider;
				if (Difficulty == GameTypes.EnemyDifficulty.ADVANCED)
				{
					enemyObj_BouncySpike2.HeadingX = (float)Math.Cos(MathHelper.WrapAngle(MathHelper.ToRadians(m_ballAngle * ChainSpeed2Modifier + 90f)));
					enemyObj_BouncySpike2.HeadingY = (float)Math.Sin(MathHelper.WrapAngle(MathHelper.ToRadians(m_ballAngle * ChainSpeed2Modifier + 90f)));
				}
				else if (Difficulty == GameTypes.EnemyDifficulty.EXPERT)
				{
					enemyObj_BouncySpike2.HeadingX = (float)Math.Cos(MathHelper.WrapAngle(MathHelper.ToRadians(-m_ballAngle * ChainSpeed2Modifier + 90f)));
					enemyObj_BouncySpike2.HeadingY = (float)Math.Sin(MathHelper.WrapAngle(MathHelper.ToRadians(-m_ballAngle * ChainSpeed2Modifier + 90f)));
				}
				enemyObj_BouncySpike2.SpawnRoom = m_levelScreen.CurrentRoom;
				enemyObj_BouncySpike2.SaveToFile = false;
				if (IsPaused)
				{
					enemyObj_BouncySpike2.PauseEnemy(false);
				}
			}
			enemyObj_BouncySpike.SpawnRoom = m_levelScreen.CurrentRoom;
			enemyObj_BouncySpike.SaveToFile = false;
			if (IsPaused)
			{
				enemyObj_BouncySpike.PauseEnemy(false);
			}
			base.Kill(giveXP);
		}
		public override void ResetState()
		{
			base.ResetState();
			m_actualChainRadius = 0f;
			m_chainLinkDistance = m_actualChainRadius / m_numChainLinks;
			float num = 0f;
			m_ballAndChain.Position = CDGMath.GetCirclePosition(m_ballAngle, m_actualChainRadius, new Vector2(X, Bounds.Top));
			for (int i = 0; i < m_chainLinksList.Count; i++)
			{
				m_chainLinksList[i] = CDGMath.GetCirclePosition(m_ballAngle, num, new Vector2(X, Bounds.Top));
				num += m_chainLinkDistance;
			}
			num = 0f;
			if (Difficulty == GameTypes.EnemyDifficulty.ADVANCED)
			{
				m_ballAndChain2.Position = CDGMath.GetCirclePosition(m_ballAngle * ChainSpeed2Modifier, m_actualChainRadius / 2f, new Vector2(X, Bounds.Top));
			}
			else if (Difficulty == GameTypes.EnemyDifficulty.EXPERT)
			{
				m_ballAndChain2.Position = CDGMath.GetCirclePosition(-m_ballAngle * ChainSpeed2Modifier, -m_actualChainRadius / 2f, new Vector2(X, Bounds.Top));
			}
			for (int j = 0; j < m_chainLinks2List.Count; j++)
			{
				if (Difficulty == GameTypes.EnemyDifficulty.ADVANCED)
				{
					m_chainLinks2List[j] = CDGMath.GetCirclePosition(m_ballAngle * ChainSpeed2Modifier, num, new Vector2(X, Bounds.Top));
				}
				else if (Difficulty == GameTypes.EnemyDifficulty.EXPERT)
				{
					m_chainLinks2List[j] = CDGMath.GetCirclePosition(-m_ballAngle * ChainSpeed2Modifier, -num, new Vector2(X, Bounds.Top));
				}
				num += m_chainLinkDistance;
			}
		}
		public override void HitEnemy(int damage, Vector2 position, bool isPlayer)
		{
			SoundManager.Play3DSound(this, m_target, new string[]
			{
				"Knight_Hit01",
				"Knight_Hit02",
				"Knight_Hit03"
			});
			base.HitEnemy(damage, position, isPlayer);
		}
		public override void Dispose()
		{
			if (!IsDisposed)
			{
				m_chain.Dispose();
				m_chain = null;
				m_ballAndChain.Dispose();
				m_ballAndChain = null;
				m_ballAndChain2.Dispose();
				m_ballAndChain2 = null;
				m_chainLinksList.Clear();
				m_chainLinksList = null;
				m_chainLinks2List.Clear();
				m_chainLinks2List = null;
				m_walkSound.Dispose();
				m_walkSound = null;
				m_walkSound2.Dispose();
				m_walkSound2 = null;
				base.Dispose();
			}
		}
	}
}
