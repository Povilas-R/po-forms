using System;
using System.Windows.Forms;

namespace Po.Forms
{
    /// <summary>
    /// <see cref="Button"/> class with better functionality.
    /// </summary>
    public class BetterButton : Button
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BetterButton"/> class
        /// </summary>
        public BetterButton() : base()
        {
            Click += new EventHandler((o, e) =>
            {
                object form = Parent;
                while (((Control)form).Parent != null)
                {
                    form = ((Control)form).Parent;
                }
                ((Form)form).ActiveControl = null;
            });
        }
    }
}
