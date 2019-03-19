using System.Collections.Generic;
using System.Windows.Forms.DataVisualization.Charting;

namespace Po.Forms.Charting
{
    public partial class ChartCollection
    {
        /// <summary>
        /// All series of all charts. Must be added to the charts manually.
        /// </summary>
        public List<Series> Series = new List<Series>();

        /// <summary>
        /// Adds a point at the end of the series.
        /// </summary>
        /// <param name="seriesIndex">The series' index.</param>
        /// <param name="point">The point to add.</param>
        public void AddPointToSeries(int seriesIndex, DataPoint point)
        {
            if (double.IsInfinity(point.YValues[0]) || point.YValues[0] is double.NaN)
            {
                return;
            }

            Series[seriesIndex].Points.Add(point);

            if (!point.IsEmpty)
            {
                for (int i = 0; i < Series[seriesIndex].Charts.Count; i++)
                {
                    int chartIndex = IndexOf(Series[seriesIndex].Charts[i]);

                    double yValue = point.YValues[0];
                    double xValue = point.XValue;

                    if (yValue < MinYValues[chartIndex])
                    {
                        MinYValues[chartIndex] = yValue;
                    }

                    if (yValue > MaxYValues[chartIndex])
                    {
                        MaxYValues[chartIndex] = yValue;
                    }

                    if (xValue < MinXValues[chartIndex])
                    {
                        MinXValues[chartIndex] = xValue;
                    }

                    if (xValue > MaxXValues[chartIndex])
                    {
                        MaxXValues[chartIndex] = xValue;
                    }
                }
            }
        }
        /// <summary>
        /// Adds a point at the end of the series.
        /// </summary>
        /// <param name="seriesIndex">The series' index.</param>
        /// <param name="xValue">X value of the point.</param>
        /// <param name="yValue">Y value of the pointd.</param>
        public void AddPointToSeries(int seriesIndex, double xValue, double yValue) => AddPointToSeries(seriesIndex, new DataPoint(xValue, yValue));

        /// <summary>
        /// Clears all series.
        /// </summary>
        public void ClearSeries() => ClearSeriesRange(0, Series.Count - 1);
        /// <summary>
        /// Clears all series in the specified range.
        /// </summary>
        /// <param name="startIndex">First series to clear.</param>
        /// <param name="endIndex">Last series to clear.</param>
        public void ClearSeriesRange(int startIndex, int endIndex)
        {
            for (int i = startIndex; i <= endIndex; i++)
            {
                Series[i].Points.Clear();
            }
        }
        /// <summary>
        /// Clears the specified series.
        /// </summary>
        /// <param name="seriesIndexes">The series to clear.</param>
        public void ClearSeriesMultiple(params int[] seriesIndexes)
        {
            for (int i = 0; i < seriesIndexes.Length; i++)
            {
                Series[seriesIndexes[i]].Points.Clear();
            }
        }

        /// <summary>
        /// Adds series to chart.
        /// </summary>
        /// <param name="chartIndex">The target chart.</param>
        /// <param name="seriesIndex">The series to be added.</param>
        /// <param name="addChartReference">Adds chart reference to the series for automatic yAxis bounds if true.</param>
        public void AddSeriesToChart(int chartIndex, int seriesIndex, bool addChartReference = true)
        {
            this[chartIndex].Series.Add(Series[seriesIndex]);
            if (addChartReference)
            {
                Series[seriesIndex].Charts.Add(this[chartIndex]);
            }
        }

        /// <summary>
        /// Sets the series' visibility in charts.
        /// </summary>
        /// <param name="seriesIndex">The series' index.</param>
        /// <param name="visible">The series' visibility.</param>
        public void SetSeriesVisibility(int seriesIndex, bool visible)
        {
            if (Series.Count > seriesIndex)
            {
                Series[seriesIndex].Enabled = visible;
            }
        }
    }
}
