using DS2DEngine;
using System;
namespace RogueCastle
{
	public class DialogueObj : IDisposableObj
	{
		private bool m_isDisposed;
		public string[] Speakers
		{
			get;
			set;
		}
		public string[] Dialogue
		{
			get;
			set;
		}
		public bool IsDisposed
		{
			get
			{
				return this.m_isDisposed;
			}
		}
		public DialogueObj(string[] speakers, string[] dialogue)
		{
			if (speakers.Length != dialogue.Length)
			{
				throw new Exception("Cannot create dialogue obj with mismatching speakers and dialogue");
			}
			this.Speakers = speakers;
			this.Dialogue = dialogue;
		}
		public void Dispose()
		{
			if (!this.m_isDisposed)
			{
				Array.Clear(this.Dialogue, 0, this.Dialogue.Length);
				this.Dialogue = null;
				Array.Clear(this.Speakers, 0, this.Speakers.Length);
				this.Speakers = null;
				this.m_isDisposed = true;
			}
		}
	}
}
