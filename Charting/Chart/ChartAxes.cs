using System;
using System.ComponentModel;
using System.Windows.Forms.DataVisualization.Charting;

namespace Po.Forms.Charting
{
    public partial class ZoomableChart
    {
        private const double AxisErrorOffset = 0.005;
        /// <summary>
        /// Sets the specified bounds for the axis.
        /// </summary>
        public void SetAxisBounds(AxisName axis, DateTimeIntervalType intervalType, double min, double max)
        {
            int index = GetAxisIndex(axis);

            bool minWasInvalid = false;
            if (min == double.MaxValue || min is double.NaN)
            {
                min = 0;
                minWasInvalid = true;
            }
            if (max == double.MinValue || max is double.NaN)
            {
                max = 0;
            }
            if (min == max && min == 0)
            {
                min -= 0.05;
                max += 0.05;
            }
            if (max < min)
            {
                if (minWasInvalid)
                {
                    min = max * (1 - AxisErrorOffset);
                    max *= 1 + AxisErrorOffset;
                }
                else
                {
                    min *= 1 - AxisErrorOffset;
                    max = min * (1 + AxisErrorOffset);
                }
            }
            min = (min == double.MaxValue) ? 0 : min;
            max = (max < min) ? min + 0.01 : max;

            if (axis != AxisName.X2)
            {
                Area.Axes[index].Minimum = min;
                Area.Axes[index].Maximum = max;
            }

            if (axis == AxisName.Y)
            {
                Area.AxisY.LabelStyle.Format = "#.#";
                if (Math.Log10(max - min) < -1)
                {
                    Area.AxisY.LabelStyle.Format += "###";
                }
                else if (Math.Log10(max - min) < 1)
                {
                    Area.AxisY.LabelStyle.Format += "##";
                }
                else if (Math.Log10(max - min) < 2)
                {
                    Area.AxisY.LabelStyle.Format += "#";
                }
            }

            if (!Area.Axes[index].ScaleView.IsZoomed && (axis != AxisName.X2 || !DatetimeAxisEnabled))
            {
                SetAxisInterval(axis, (max - min) / GetAxisIntervalCount(axis));
            }
            else if (axis != AxisName.X2 || !DatetimeAxisEnabled)
            {
                SetAxisInterval(axis, Area.Axes[index].ScaleView.Size / GetAxisIntervalCount(axis));
            }

            if ((axis == AxisName.X || axis == AxisName.X2) && !(min is double.NaN || max is double.NaN) && DatetimeAxisEnabled)
            {
                try
                {
                    switch (AxisXIntervalType)
                    {
                        case DateTimeIntervalType.Hours:
                            min *= 60 * 60 * 1000;
                            max *= 60 * 60 * 1000;
                            break;
                        case DateTimeIntervalType.Minutes:
                            min *= 60 * 1000;
                            max *= 60 * 1000;
                            break;
                        case DateTimeIntervalType.Seconds:
                            min *= 1000;
                            max *= 1000;
                            break;
                    }

                    double size = max - min;
                    min = StartTime.AddMilliseconds(min).ToOADate();
                    max = StartTime.AddMilliseconds(max).ToOADate();
                    Area.AxisX2.IntervalType = DateTimeIntervalType.Milliseconds;
                    Area.AxisX2.Minimum = min;
                    Area.AxisX2.Maximum = max;
                    Area.AxisX2.Interval = size / GetAxisIntervalCount(AxisName.X2);
                    Area.AxisX2.MajorGrid.Enabled = false;
                }
                catch { }
            }
        }

        /// <summary>
        /// Interval count for xAxis.
        /// </summary>
        [Category("Chart"), Description("Specifies the interval count for X axis.")]
        public double AxisXIntervalCount
        {
            get => _axisXIntervalCount;
            set => _axisXIntervalCount = value >= 3 ? value : 3;
        }
        private double _axisXIntervalCount = 20;
        /// <summary>
        /// Interval count for yAxis.
        /// </summary>
        [Category("Chart"), Description("Specifies the interval count for Y axis.")]
        public double AxisYIntervalCount
        {
            get => _axisYIntervalCount;
            set => _axisYIntervalCount = value >= 3 ? value : 3;
        }
        private double _axisYIntervalCount = 20;
        /// <summary>
        /// Sets the specified interval for the axis.
        /// </summary>
        /// <param name="axis">The axis for which the interval is set.</param>
        /// <param name="interval">0 to set to default.</param>
        public void SetAxisInterval(AxisName axis, double interval = 0)
        {
            string name = Name;
            int index = GetAxisIndex(axis);

            if (interval <= 0)
            {
                interval = (Area.Axes[index].Maximum - Area.Axes[index].Minimum) / GetAxisIntervalCount(axis);
            }

            if (double.IsNaN(interval))
            {
                return;
            }

            interval = interval <= 0 ? 0.5 : interval;

            Area.Axes[index].Interval = interval;
        }

        private int GetAxisIndex(AxisName axis)
        {
            switch (axis)
            {
                case AxisName.X:
                    return 0;
                case AxisName.X2:
                    return 2;
                case AxisName.Y:
                    return 1;
                case AxisName.Y2:
                    return 3;
                default:
                    return -1;
            }
        }

        private double GetAxisIntervalCount(AxisName axis)
        {
            if (axis == AxisName.X || axis == AxisName.X2)
            {
                return AxisXIntervalCount;
            }
            else
            {
                return AxisYIntervalCount;
            }
        }
    }
}
