using DS2DEngine;

namespace RogueCastle
{
	public class DeleteSaveOptionsObj : OptionsObj
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
					rCScreenManager.DialogueScreen.SetDialogue("Delete Save");
					rCScreenManager.DialogueScreen.SetDialogueChoice("ConfirmTest1");
					rCScreenManager.DialogueScreen.SetConfirmEndHandler(this, "DeleteSaveAskAgain", new object[0]);
					rCScreenManager.DialogueScreen.SetCancelEndHandler(this, "CancelCommand", new object[0]);
					rCScreenManager.DisplayScreen(13, false, null);
				}
			}
		}
		public DeleteSaveOptionsObj(OptionsScreen parentScreen) : base(parentScreen, "Delete Save")
		{
		}
		public void CancelCommand()
		{
			IsActive = false;
		}
		public void DeleteSaveAskAgain()
		{
			RCScreenManager rCScreenManager = m_parentScreen.ScreenManager as RCScreenManager;
			rCScreenManager.DialogueScreen.SetDialogue("Delete Save2");
			rCScreenManager.DialogueScreen.SetDialogueChoice("ConfirmTest1");
			rCScreenManager.DialogueScreen.SetConfirmEndHandler(this, "DeleteSave", new object[0]);
			rCScreenManager.DialogueScreen.SetCancelEndHandler(this, "CancelCommand", new object[0]);
			rCScreenManager.DisplayScreen(13, false, null);
		}
		public void DeleteSave()
		{
			IsActive = false;
			Game.PlayerStats.Dispose();
			(m_parentScreen.ScreenManager.Game as Game).SaveManager.ClearAllFileTypes(false);
			(m_parentScreen.ScreenManager.Game as Game).SaveManager.ClearAllFileTypes(true);
			SkillSystem.ResetAllTraits();
			Game.PlayerStats = new PlayerStats();
			(m_parentScreen.ScreenManager as RCScreenManager).Player.Reset();
			SoundManager.StopMusic(1f);
			(m_parentScreen.ScreenManager as RCScreenManager).DisplayScreen(23, true, null);
		}
	}
}
