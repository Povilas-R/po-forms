using System.Drawing;
using System.Windows.Forms;

namespace Po.Forms
{
	/// <summary>
	/// <see cref="NumericUpDown"/> class with added cue text functionality and other features.
	/// </summary>
	public class NumericBox : NumericUpDown
	{
		/// <summary>
		/// Initializes a new instance of <see cref="NumericBox"/> class.
		/// </summary>
		public NumericBox() : base()
		{
            Controls.RemoveAt(0);
		}

		private string _cueText;
		/// <summary>
		/// Cue text.
		/// </summary>
		public string CueText
		{
			get => _cueText;
			set
			{
				_cueText = value;
				Invalidate();
			}
		}
		/// <summary>
		/// Processes Windows messages.
		/// </summary>
		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);
			if (m.Msg == 0xf)
			{
				if (!Focused && string.IsNullOrEmpty(Text)
					&& !string.IsNullOrEmpty(CueText))
				{
					using (var g = CreateGraphics())
					{
						TextRenderer.DrawText(g, CueText, Font,
							ClientRectangle, SystemColors.GrayText, BackColor,
							TextFormatFlags.Top | TextFormatFlags.Left);
					}
				}
			}
		}
	}
}
