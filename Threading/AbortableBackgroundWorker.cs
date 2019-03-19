using System.ComponentModel;
using System.Threading;

namespace Po.Forms.Threading
{
    /// <summary>
    /// An abortable background worker.
    /// </summary>
    public class AbortableBackgroundWorker : BackgroundWorker
    {
        private Thread _workerThread;

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            _workerThread = Thread.CurrentThread;
            try
            {
                base.OnDoWork(e);
            }
            catch (ThreadAbortException)
            {
                e.Cancel = true;
                Thread.ResetAbort();
            }
        }

        public void Abort()
        {
            _workerThread?.Abort();
            _workerThread = null;
        }
    }
}