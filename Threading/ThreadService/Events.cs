using System;

namespace Po.Forms.Threading
{
    public partial class ThreadService
    {
        /// <summary>
        /// Calls all MessageCall events.
        /// </summary>
        /// <param name="message">Message call event args.</param>
        protected void OnMessageCall(string message) => MessageCall?.Invoke(this, new MessageCallEventArgs(message));

        /// <summary>
        /// Events to be called on message calls.
        /// </summary>
        public event EventHandler<MessageCallEventArgs> MessageCall;
        /// <summary>
        /// Events to be called when the thread is started.
        /// </summary>
        public event EventHandler<ThreadService> ThreadStarted;
        /// <summary>
        /// Events to be called when the thread is completed
        /// </summary>
        public event EventHandler<ThreadService> ThreadCompleted;
        /// <summary>
        /// Events to be called when thread update is called.
        /// </summary>
        public event EventHandler<ThreadService> Update;

        /// <summary>
        /// Event args for the <see cref="MessageCall"/> event.
        /// </summary>
        public class MessageCallEventArgs : EventArgs
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="MessageCallEventArgs"/> class.
            /// </summary>
            /// <param name="message"><see cref="Message"/> value.</param>
            public MessageCallEventArgs(string message) : base()
            {
                Message = message;
            }

            /// <summary>
            /// Message to be called.
            /// </summary>
            public string Message;
        }
    }
}
