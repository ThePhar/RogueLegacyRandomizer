using DS2DEngine;
using System;
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
				if (this.IsActive)
				{
					RCScreenManager rCScreenManager = this.m_parentScreen.ScreenManager as RCScreenManager;
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
			this.IsActive = false;
		}
		public void DeleteSaveAskAgain()
		{
			RCScreenManager rCScreenManager = this.m_parentScreen.ScreenManager as RCScreenManager;
			rCScreenManager.DialogueScreen.SetDialogue("Delete Save2");
			rCScreenManager.DialogueScreen.SetDialogueChoice("ConfirmTest1");
			rCScreenManager.DialogueScreen.SetConfirmEndHandler(this, "DeleteSave", new object[0]);
			rCScreenManager.DialogueScreen.SetCancelEndHandler(this, "CancelCommand", new object[0]);
			rCScreenManager.DisplayScreen(13, false, null);
		}
		public void DeleteSave()
		{
			this.IsActive = false;
			Game.PlayerStats.Dispose();
			(this.m_parentScreen.ScreenManager.Game as Game).SaveManager.ClearAllFileTypes(false);
			(this.m_parentScreen.ScreenManager.Game as Game).SaveManager.ClearAllFileTypes(true);
			SkillSystem.ResetAllTraits();
			Game.PlayerStats = new PlayerStats();
			(this.m_parentScreen.ScreenManager as RCScreenManager).Player.Reset();
			SoundManager.StopMusic(1f);
			(this.m_parentScreen.ScreenManager as RCScreenManager).DisplayScreen(23, true, null);
		}
	}
}
