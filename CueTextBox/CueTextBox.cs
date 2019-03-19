using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Po.Forms
{
    /// <summary>
    /// <see cref="TextBox"/> class with added cue text functionality and other features.
    /// </summary>
    public class CueTextBox : TextBox
    {
        /// <summary>
        /// Initializes a new instance of <see cref="CueTextBox"/> class.
        /// </summary>
        public CueTextBox() : base()
        {
            KeyPress += FilterKeys;
        }

        private bool _onlyAllowNumbers = false;
        /// <summary>
        /// Text box only allows numbers, can be with decimal point.
        /// </summary>
        public bool OnlyAllowNumbers
        {
            get => _onlyAllowNumbers;
            set
            {
                _onlyAllowNumbers = value;
                if (value)
                {
                    _onlyAllowDigits = false;
                }
            }
        }

        private bool _onlyAllowDigits = false;
        /// <summary>
        /// Text box only allows digits, without decimal point.
        /// </summary>
        public bool OnlyAllowDigits
        {
            get => _onlyAllowDigits;
            set
            {
                _onlyAllowDigits = value;
                if (value)
                {
                    _onlyAllowNumbers = false;
                }
            }
        }

        private void FilterKeys(object obj, KeyPressEventArgs e)
        {
            if (OnlyAllowNumbers || OnlyAllowDigits)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    if (OnlyAllowNumbers)
                    {
                        e.Handled = true;
                    }
                    else if
                        (OnlyAllowDigits
                        && (Text.Contains('.') || e.KeyChar != '.' || Text.Length == 0))
                    {
                        e.Handled = true;
                    }
                }
            }
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
        /// <param name="m"></param>
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
