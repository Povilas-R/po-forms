using System;
using System.Collections.Generic;
using System.Windows.Forms.DataVisualization.Charting;

namespace Po.Forms.Charting
{
    /// <summary>
    /// <see cref="ZoomableChart"/> collection.
    /// </summary>
    public partial class ChartCollection : List<ZoomableChart>
    {
        /// <summary>
        /// Creates <see cref="ChartCollection"/> instance and adds all given charts to the list.
        /// </summary>
        /// <param name="charts">The charts to be added.</param>
        public ChartCollection(params ZoomableChart[] charts) : base()
        {
            AddMultiple(charts);
        }

        /// <summary>
        /// Adds a chart to the end of the list.
        /// </summary>
        /// <param name="chart">The chart to be added.</param>
        public new void Add(ZoomableChart chart)
        {
            base.Add(chart);
            for (int i = 0; i < Count - 1; i++)
            {
                this[Count - 1].GroupedCharts.Add(this[i]);
                this[i].GroupedCharts.Add(this[Count - 1]);
            }
            MinYValues.Add(double.MaxValue);
            MinXValues.Add(double.MaxValue);
            MaxYValues.Add(double.MinValue);
            MaxXValues.Add(double.MinValue);
        }
        /// <summary>
        /// Adds multiple charts to the end of the list.
        /// </summary>
        /// <param name="args">The charts to be added.</param>
        public void AddMultiple(params ZoomableChart[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                Add(args[i]);
            }
        }

        /// <summary>
        /// Resets charts' zoom.
        /// </summary>
        public void ResetZoom()
        {
            for (int i = 0; i < Count; i++)
            {
                this[i].ResetZoom();
            }
        }

        /// <summary>
        /// Resets charts' limits, zoom and clears all series.
        /// </summary>
        public void Reset()
        {
            ResetBounds();
            ResetZoom();
            ClearSeries();
        }

        /// <summary>
        /// Updates charts' bounds (limits).
        /// </summary>
        public void UpdateCharts()
        {
            for (int i = 0; i < Count; i++)
            {
                this[i].SetAxisBounds(AxisName.X, this[i].AxisXIntervalType, MinXValues[i], MaxXValues[i]);
                this[i].SetAxisBounds(AxisName.Y, DateTimeIntervalType.Auto, MinYValues[i], MaxYValues[i]);
            }
        }

        /// <summary>
        /// Sets the start time for all charts.
        /// </summary>
        /// <param name="startTime"></param>
        public void SetStartTime(DateTime startTime)
        {
            for (int i = 0; i < Count; i++)
            {
                this[i].StartTime = startTime;
            }
        }

        /// <summary>
        /// Sets mouse wheel zoom speed for all charts. 1 to disable.
        /// </summary>
        /// <param name="zoomSpeed">Mouse wheel zoom speed. 1.15 default. 1 to disable.</param>
        public void SetZoomSpeedGlobal(double zoomSpeed)
        {
            for (int i = 0; i < Count; i++)
            {
                this[i].ZoomSpeed = zoomSpeed;
            }
        }
    }
}
