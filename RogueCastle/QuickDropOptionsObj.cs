using DS2DEngine;
using Microsoft.Xna.Framework;

namespace RogueCastle
{
	public class QuickDropOptionsObj : OptionsObj
	{
		private TextObj m_toggleText;
		public override bool IsActive
		{
			get
			{
				return base.IsActive;
			}
			set
			{
				base.IsActive = value;
				if (value)
				{
					m_toggleText.TextureColor = Color.Yellow;
					return;
				}
				m_toggleText.TextureColor = Color.White;
			}
		}
		public QuickDropOptionsObj(OptionsScreen parentScreen) : base(parentScreen, "Enable Quick Drop")
		{
			m_toggleText = (m_nameText.Clone() as TextObj);
			m_toggleText.X = m_optionsTextOffset;
			m_toggleText.Text = "No";
			AddChild(m_toggleText);
		}
		public override void Initialize()
		{
			if (Game.GameConfig.QuickDrop)
			{
				m_toggleText.Text = "Yes";
			}
			else
			{
				m_toggleText.Text = "No";
			}
			base.Initialize();
		}
		public override void HandleInput()
		{
			if (Game.GlobalInput.JustPressed(20) || Game.GlobalInput.JustPressed(21) || Game.GlobalInput.JustPressed(22) || Game.GlobalInput.JustPressed(23))
			{
				SoundManager.PlaySound("frame_swap");
				if (m_toggleText.Text == "No")
				{
					m_toggleText.Text = "Yes";
				}
				else
				{
					m_toggleText.Text = "No";
				}
			}
			if (Game.GlobalInput.JustPressed(0) || Game.GlobalInput.JustPressed(1))
			{
				SoundManager.PlaySound("Option_Menu_Select");
				if (m_toggleText.Text == "Yes")
				{
					Game.GameConfig.QuickDrop = true;
				}
				else
				{
					Game.GameConfig.QuickDrop = false;
				}
				IsActive = false;
			}
			if (Game.GlobalInput.JustPressed(2) || Game.GlobalInput.JustPressed(3))
			{
				if (Game.GameConfig.QuickDrop)
				{
					m_toggleText.Text = "Yes";
				}
				else
				{
					m_toggleText.Text = "No";
				}
				IsActive = false;
			}
			base.HandleInput();
		}
		public override void Dispose()
		{
			if (!IsDisposed)
			{
				m_toggleText = null;
				base.Dispose();
			}
		}
	}
}
