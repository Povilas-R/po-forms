using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using static Po.Forms.Threading.ThreadService;

namespace Po.Forms.Threading
{
    public interface IThreadService
	{
		string Name { get; set; }
		bool IsRunning { get; }
		long UpdateInterval { get; set; }

		void Start();
		void Stop();

		event EventHandler<MessageCallEventArgs> MessageCall;
		event EventHandler<ThreadService> ThreadStarted;
		event EventHandler<ThreadService> ThreadCompleted;
		event EventHandler<ThreadService> Update;
	}

	/// <summary>
	/// Provides easy usage of <see cref="AbortableBackgroundWorker"/>.
	/// </summary>
	public partial class ThreadService : IThreadService
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ThreadService"/> class.
		/// </summary>
		/// <param name="name"><see cref="Name"/> value.</param>
		public ThreadService(string name = "generic-thread")
		{
			Name = name;
		}

		/// <summary>
		/// Name of this listener's instance.
		/// </summary>
		public string Name { get; set; }

		protected AbortableBackgroundWorker _worker = null;
		/// <summary>
		/// Breaks thread loop when set to true.
		/// </summary>
		protected bool _doCancel = false;

		private Stopwatch _updateWatch = new Stopwatch();
		/// <summary>
		/// Update interval in milliseconds. Default: 100ms.
		/// </summary>
		public long UpdateInterval { get; set; } = 100;
        /// <summary>
        /// Start delay in milliseconds. Default: 0ms.
        /// </summary>
        public int StartDelay { get; set; } = 0;

		private void Run()
		{
			try
			{
                if (StartDelay > 0)
                {
                    Thread.Sleep(StartDelay);
                }
				ThreadStarted?.Invoke(this, this);
				Loop();
			}
			catch (Exception ex)
			{
				OnMessageCall($"Exception in {Name} thread Run(): {ex.Message}");
			}
		}

        /// <summary>
        /// True will disable thread sleep between updates. 
        /// WARNING: setting this without manually managing thread sleep will result in overloaded CPU.
        /// </summary>
        public virtual bool DisableThreadSleep { get; set; } = false;
        /// <summary>
        /// The thread loop.
        /// </summary>
		protected virtual void Loop()
		{
			while (!_worker.CancellationPending && !_doCancel)
			{
				try
				{
                    if (!DisableThreadSleep)
                    {
                        Thread.Sleep(1);
                    }

					if (_updateWatch.ElapsedMilliseconds > UpdateInterval || !_updateWatch.IsRunning)
					{
						_updateWatch.Restart();
						Update?.Invoke(this, this);
					}
				}
				catch (Exception ex)
				{
					string exceptionMessage = ex.Message;
					OnMessageCall($"Exception in {Name} thread Run() loop: {exceptionMessage}");
					_doCancel = true;
				}
			}
		}

		/// <summary>
		/// Returns whether the listener is running.
		/// </summary>
		public bool IsRunning { get => _worker?.IsBusy ?? false; private set => _ = value; }

		/// <summary>
		/// Stops the listener.
		/// </summary>
		public void Stop()
		{
			if (_worker != null && _worker.IsBusy)
			{
				_doCancel = true;
				_worker.CancelAsync();
			}
		}
		/// <summary>
		/// Triggers the listener.
		/// </summary>
		public void Start()
		{
            Stop();
			_worker = new AbortableBackgroundWorker() { WorkerSupportsCancellation = true };
			_worker.DoWork += new DoWorkEventHandler(DoWork);
			_worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(RunWorkerCompleted);
			_worker.RunWorkerAsync(this);
		}
		
		private void DoWork(object sender, DoWorkEventArgs e)
		{
			var listener = (ThreadService)e.Argument;
			listener.Run();
			e.Cancel = true;
		}
		private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			try
			{
				_worker = null;
				_doCancel = false;
				ThreadCompleted?.Invoke(this, this);
			}
			catch { }
		}
	}
}
