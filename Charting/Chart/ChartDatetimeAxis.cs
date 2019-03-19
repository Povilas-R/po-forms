using System;
using System.Windows.Forms.DataVisualization.Charting;

namespace Po.Forms.Charting
{
    public partial class ZoomableChart
    {
        private DateTime _startTime = new DateTime(2001, 01, 01);
        /// <summary>
        /// Start time for the secondary time axis.
        /// </summary>
        public DateTime StartTime
        {
            get => _startTime;
            set
            {
                _startTime = value;
                try
                {
                    SetAxisBounds(AxisName.X2, DateTimeIntervalType.Milliseconds, Area.AxisX.Minimum, Area.AxisX.Maximum);
                }
                catch { }
            }
        }

        private bool _datetimeAxisEnabled = false;
        /// <summary>
        /// Enables or disables the secondary time axis.
        /// </summary>
        public bool DatetimeAxisEnabled
        {
            get => _datetimeAxisEnabled;
            set
            {
                _datetimeAxisEnabled = value;
                if (value)
                {
                    Area.AxisX2.Enabled = AxisEnabled.True;
                }
                else
                {
                    Area.AxisX2.Enabled = AxisEnabled.False;
                }
            }
        }
    }
}
