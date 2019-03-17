using System.Windows.Forms;

namespace Po.Forms
{
    public static class UtilityExtensions
    {
        /// <summary>
        /// Focuses the given <see cref="Form"/> window.
        /// </summary>
        public static void FocusWindow(this Form form)
        {
            form.Invoke((MethodInvoker)delegate
            {
                if (!form.TopMost)
                {
                    form.TopMost = true;
                    form.TopMost = false;
                }
                form.Activate();
                if (form.TopMost)
                {
                    form.TopMost = false;
                    form.TopMost = true;
                }
            });
        }
    }
}
