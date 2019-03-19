using System.Collections.Generic;

namespace Po.Forms.Charting
{
    public partial class ChartCollection
    {
        /// <summary>
        /// Minimum values of yAxes. Call <see cref="UpdateCharts"/> to take effect.
        /// </summary>
        public List<double> MinYValues = new List<double>();
        /// <summary>
        /// Maximum values of yAxes. Must call <see cref="UpdateCharts"/> to take effect.
        /// </summary>
        public List<double> MaxYValues = new List<double>();
        /// <summary>
        /// Minimum values of xAxis. Must call <see cref="UpdateCharts"/> to take effect.
        /// </summary>
        public List<double> MinXValues = new List<double>();
        /// <summary>
        /// Maximum values of xAxis. Must call <see cref="UpdateCharts"/> to take effect.
        /// </summary>
        public List<double> MaxXValues = new List<double>();

        /// <summary>
        /// Sets the specified X values for all charts in <see cref="ChartCollection"/>. Call <see cref="UpdateCharts"/> to take effect.
        /// </summary>
        /// <param name="minXValue">Default to keep min bounds.</param>
        /// <param name="maxXValue">Default to keep max bounds.</param>
        public void SetXBoundsGlobal(double minXValue = double.NaN, double maxXValue = double.NaN)
        {
            for (int i = 0; i < Count; i++)
            {
                if (!double.IsNaN(minXValue))
                {
                    MinXValues[i] = minXValue;
                }
                if (!double.IsNaN(maxXValue))
                {
                    MaxXValues[i] = maxXValue;
                }
            }
        }

        /// <summary>
        /// Resets charts' limits.
        /// </summary>
        public void ResetBounds()
        {
            MinYValues.Clear();
            MinXValues.Clear();
            MaxYValues.Clear();
            MaxXValues.Clear();

            for (int i = 0; i <= Count; i++)
            {
                MinYValues.Add(double.MaxValue);
                MinXValues.Add(double.MaxValue);
                MaxYValues.Add(double.MinValue);
                MaxXValues.Add(double.MinValue);
            }
        }
    }
}
