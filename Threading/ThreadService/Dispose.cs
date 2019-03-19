using System;

namespace Po.Forms.Threading
{
    public partial class ThreadService
    {
        private bool _disposed = false;
        /// <summary>
        /// Releases all resources used by the <see cref="ThreadService"/>.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="ThreadService"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources. False to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _worker?.Dispose();
                // Release managed resources here
            }
            // Release unmanaged resources here
            _disposed = true;
        }
    }
}
