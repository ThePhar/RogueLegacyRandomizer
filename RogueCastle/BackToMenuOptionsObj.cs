namespace RogueCastle
{
	public class BackToMenuOptionsObj : OptionsObj
	{
		public override bool IsActive
		{
			get
			{
				return base.IsActive;
			}
			set
			{
				base.IsActive = value;
				if (IsActive)
				{
					RCScreenManager rCScreenManager = m_parentScreen.ScreenManager as RCScreenManager;
					rCScreenManager.DialogueScreen.SetDialogue("Back to Menu");
					rCScreenManager.DialogueScreen.SetDialogueChoice("ConfirmTest1");
					rCScreenManager.DialogueScreen.SetConfirmEndHandler(this, "GoBackToTitle", new object[0]);
					rCScreenManager.DialogueScreen.SetCancelEndHandler(this, "CancelCommand", new object[0]);
					rCScreenManager.DisplayScreen(13, false, null);
				}
			}
		}
		public BackToMenuOptionsObj(OptionsScreen parentScreen) : base(parentScreen, "Quit to Title Screen")
		{
		}
		public override void Initialize()
		{
			if (Game.PlayerStats.TutorialComplete)
			{
				m_nameText.Text = "Quit to Title Screen";
			}
			else
			{
				m_nameText.Text = "Quit to Title Screen (skip tutorial)";
			}
			base.Initialize();
		}
		public void GoBackToTitle()
		{
			IsActive = false;
			ProceduralLevelScreen levelScreen = Game.ScreenManager.GetLevelScreen();
			if (levelScreen != null && (levelScreen.CurrentRoom is CarnivalShoot1BonusRoom || levelScreen.CurrentRoom is CarnivalShoot2BonusRoom))
			{
				if (levelScreen.CurrentRoom is CarnivalShoot1BonusRoom)
				{
					(levelScreen.CurrentRoom as CarnivalShoot1BonusRoom).UnequipPlayer();
				}
				if (levelScreen.CurrentRoom is CarnivalShoot2BonusRoom)
				{
					(levelScreen.CurrentRoom as CarnivalShoot2BonusRoom).UnequipPlayer();
				}
			}
			if (levelScreen != null)
			{
				ChallengeBossRoomObj challengeBossRoomObj = levelScreen.CurrentRoom as ChallengeBossRoomObj;
				if (challengeBossRoomObj != null)
				{
					challengeBossRoomObj.LoadPlayerData();
					(m_parentScreen.ScreenManager.Game as Game).SaveManager.LoadFiles(levelScreen, new SaveType[]
					{
						SaveType.UpgradeData
					});
					levelScreen.Player.CurrentHealth = challengeBossRoomObj.StoredHP;
					levelScreen.Player.CurrentMana = challengeBossRoomObj.StoredMP;
				}
			}
			(m_parentScreen.ScreenManager.Game as Game).SaveManager.SaveFiles(new SaveType[]
			{
				SaveType.PlayerData,
				SaveType.UpgradeData
			});
			if (Game.PlayerStats.TutorialComplete && levelScreen != null && levelScreen.CurrentRoom.Name != "Start" && levelScreen.CurrentRoom.Name != "Ending" && levelScreen.CurrentRoom.Name != "Tutorial")
			{
				(m_parentScreen.ScreenManager.Game as Game).SaveManager.SaveFiles(new SaveType[]
				{
					SaveType.MapData
				});
			}
			Game.ScreenManager.DisplayScreen(3, true, null);
		}
		public void CancelCommand()
		{
			IsActive = false;
		}
	}
}
