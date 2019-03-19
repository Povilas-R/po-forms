using System;
using System.IO;
using System.Windows.Forms;

namespace Po.Forms.Logging
{
    /// <summary>
    /// Class for logging information throughout the entire application.
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Date pattern for logging to text box. Default: "[HH:mm:ss] "
        /// </summary>
        public static string LogBoxDatePattern = "[HH:mm:ss] ";
        /// <summary>
        /// Date pattern for logging to message box. Default: "[HH:mm:ss] "
        /// </summary>
        public static string MessageDatePattern = "[HH:mm:ss] ";
        /// <summary>
        /// Date pattern for logging to file. Default: "[HH:mm:ss] "
        /// </summary>
        public static string RecordDatePattern = "[HH:mm:ss] ";
        /// <summary>
        /// Date pattern for log file name. Default: "[yyyy-MM-dd] "
        /// </summary>
        public static string LogNameDatePattern = "[yyyy-MM-dd] ";
        /// <summary>
        /// Log file name. Default: "Log.txt"
        /// </summary>
        public static string LogFileName = "Log.txt";

        /// <summary>
        /// Default <see cref="TextBox"/> for logging.
        /// </summary>
        public static TextBox LogBox = null;
        /// <summary>
        /// Default <see cref="System.Windows.Forms.Form"/> for logging.
        /// </summary>
        public static Form Form = null;

        /// <summary>
        /// Logs the given info to <see cref="LogBox"/> and file.
        /// When calling from a different thread, use <see cref="InvokeLog(string, TextBox, Form)"/> or its overloads.
        /// </summary>
        public static void Log(string info)
        {
            if (LogBox != null)
            {
                Log(info, LogBox);
            }
        }
        /// <summary>
        /// Logs the given info to the given <see cref="TextBox"/> and file.
        /// </summary>
        public static void Log(string info, TextBox log)
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
        /// Invokes the appending of the given info to <see cref="LogBox"/> in <see cref="Form"/> and file.
        /// </summary>
        public static void InvokeLog(string info)
        {
            if (LogBox != null && Form != null)
            {
                if (info == null || info.Trim().Length == 0)
                {
                    return;
                }

                if (Form.IsHandleCreated)
                {
                    Form.Invoke((MethodInvoker)delegate
                    {
                        LogBox.AppendText(GetInfoLine(LogBoxDatePattern, info));
                        if (!LogToFile(info, out string ex))
                        {
                            LogBox.AppendText(GetInfoLine(LogBoxDatePattern, ex));
                        }
                    });
                }
            }
        }
        /// <summary>
        /// Invokes the appending of the given info to the given <see cref="TextBox"/> in <see cref="Form"/> and file.
        /// </summary>
        public static void InvokeLog(string info, TextBox log)
        {
            if (LogBox != null && Form != null)
            {
                if (info == null || info.Trim().Length == 0)
                {
                    return;
                }

                if (Form.IsHandleCreated)
                {
                    Form.Invoke((MethodInvoker)delegate
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
        public static void InvokeLog(string info, TextBox log, Form form)
        {
            if (LogBox != null && Form != null)
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
        public static void ShowMessage(string info)
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
        public static bool LogToFile(string info) => LogToFile(info, out _);
        /// <summary>
        /// Writes the given info to file.
        /// </summary>
        public static bool LogToFile(string info, out string exceptionMessage)
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

        public static string GetInfoLine(string datePattern, string text)
        {
            return
                DateTime.Now.ToString(datePattern) + text + Environment.NewLine;
        }
    }
}
