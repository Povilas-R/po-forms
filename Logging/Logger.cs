using System;
using System.IO;
using System.Windows.Forms;

namespace Po.Forms.Logging
{
    /// <summary>
    /// Class for logging information throughout the entire application.
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// Initializes a new <see cref="Logger"/> instance.
        /// </summary>
        /// <param name="form">The default <see cref="Form"/> for this logger.</param>
        /// <param name="logBox">The default <see cref="TextBox"/> for this logger.</param>
        public Logger(Form form = null, TextBox logBox = null)
        {
            _form = form;
            _logBox = logBox;
        }

        /// <summary>
        /// Date pattern for logging to text box. Default: "[HH:mm:ss] "
        /// </summary>
        public string LogBoxDatePattern = "[HH:mm:ss] ";
        /// <summary>
        /// Date pattern for logging to message box. Default: "[HH:mm:ss] "
        /// </summary>
        public string MessageDatePattern = "[HH:mm:ss] ";
        /// <summary>
        /// Date pattern for logging to file. Default: "[HH:mm:ss] "
        /// </summary>
        public string RecordDatePattern = "[HH:mm:ss] ";
        /// <summary>
        /// Date pattern for log file name. Default: "[yyyy-MM-dd] "
        /// </summary>
        public string LogNameDatePattern = "[yyyy-MM-dd] ";
        /// <summary>
        /// Log file name. Default: "Log.txt"
        /// </summary>
        public string LogFileName = "Log.txt";

        private TextBox _logBox = null;
        private Form _form = null;

        /// <summary>
        /// Sets the default <see cref="Form"/> and <see cref="TextBox"/> for logging.
        /// </summary>
        public void SetDefaults(Form form, TextBox logBox)
        {
            _form = form;
            _logBox = logBox;
        }
        /// <summary>
        /// Sets the default <see cref="Form"/> for logging.
        /// </summary>
        public void SetDefaultForm(Form form)
        {
            _form = form;
        }
        /// <summary>
        /// Sets the default <see cref="TextBox"/> for logging.
        /// </summary>
        public void SetDefaultLogBox(TextBox logBox)
        {
            _logBox = logBox;
        }

        /// <summary>
        /// Logs the given info to default <see cref="TextBox"/> and file.
        /// When calling from a different thread, use <see cref="InvokeLog(string, TextBox, Form)"/> or its overloads.
        /// </summary>
        public void Log(string info)
        {
            if (_logBox != null)
            {
                Log(info, _logBox);
            }
        }
        /// <summary>
        /// Logs the given info to the given <see cref="TextBox"/> and file.
        /// </summary>
        public void Log(string info, TextBox log)
        {
            if (info == null || info.Trim().Length == 0)
            {
                return;
            }

            log.AppendText(GetInfoLine(LogBoxDatePattern, info));
            if (!LogToFile(info, out string ex))
            {
                log.AppendText(GetInfoLine(LogBoxDatePattern, ex));
            }
        }

        /// <summary>
        /// Invokes the appending of the given info to default <see cref="TextBox"/> and file.
        /// </summary>
        public void InvokeLog(string info)
        {
            if (_logBox != null && _form != null)
            {
                if (info == null || info.Trim().Length == 0)
                {
                    return;
                }

                if (_form.IsHandleCreated)
                {
                    _form.Invoke((MethodInvoker)delegate
                    {
                        _logBox.AppendText(GetInfoLine(LogBoxDatePattern, info));
                        if (!LogToFile(info, out string ex))
                        {
                            _logBox.AppendText(GetInfoLine(LogBoxDatePattern, ex));
                        }
                    });
                }
            }
        }
        /// <summary>
        /// Invokes the appending of the given info to the given <see cref="TextBox"/> in the default <see cref="Form"/> and file.
        /// </summary>
        public void InvokeLog(string info, TextBox log)
        {
            if (_logBox != null && _form != null)
            {
                if (info == null || info.Trim().Length == 0)
                {
                    return;
                }

                if (_form.IsHandleCreated)
                {
                    _form.Invoke((MethodInvoker)delegate
                    {
                        log.AppendText(GetInfoLine(LogBoxDatePattern, info));
                        if (!LogToFile(info, out string ex))
                        {
                            log.AppendText(GetInfoLine(LogBoxDatePattern, ex));
                        }
                    });
                }
            }
        }
        /// <summary>
        /// Invokes the appending of the given info to the given <see cref="TextBox"/> in the given <see cref="System.Windows.Forms.Form"/> and file.
        /// </summary>
        public void InvokeLog(string info, TextBox log, Form form)
        {
            if (_logBox != null && _form != null)
            {
                if (info == null || info.Trim().Length == 0)
                {
                    return;
                }

                if (form.IsHandleCreated)
                {
                    form.Invoke((MethodInvoker)delegate
                    {
                        log.AppendText(GetInfoLine(LogBoxDatePattern, info));
                        if (!LogToFile(info, out string ex))
                        {
                            log.AppendText(GetInfoLine(LogBoxDatePattern, ex));
                        }
                    });
                }
            }
        }

        /// <summary>
        /// Pops a <see cref="MessageBox"/> with the given info and writes it to file.
        /// </summary>
        public void ShowMessage(string info)
        {
            if (info == null || info.Trim().Length == 0)
            {
                return;
            }

            LogToFile(GetInfoLine(RecordDatePattern, info));
            MessageBox.Show(GetInfoLine(MessageDatePattern, info));
        }

        /// <summary>
        /// Writes the given info to file.
        /// </summary>
        public bool LogToFile(string info) => LogToFile(info, out _);
        /// <summary>
        /// Writes the given info to file.
        /// </summary>
        public bool LogToFile(string info, out string exceptionMessage)
        {
            exceptionMessage = null;
            if (info == null || info.Trim().Length == 0)
            {
                return true;
            }

            try
            {
                File.AppendAllText(
                    (!string.IsNullOrEmpty(LogNameDatePattern)
                    ? DateTime.Now.ToString(LogNameDatePattern)
                    : "") + LogFileName,
                    GetInfoLine(RecordDatePattern, info));
                return true;
            }
            catch (Exception ex)
            {
                exceptionMessage = $"Error logging to file: {ex.Message}";
                return false;
            }
        }

        public string GetInfoLine(string datePattern, string text)
        {
            return
                DateTime.Now.ToString(datePattern) + text + Environment.NewLine;
        }
    }
}
