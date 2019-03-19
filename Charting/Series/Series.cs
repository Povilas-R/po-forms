using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace Po.Forms.Charting
{
    /// <summary>
    /// <see cref="System.Windows.Forms.DataVisualization.Charting.Series"/> class with added curve types.
    /// </summary>
    public partial class Series : System.Windows.Forms.DataVisualization.Charting.Series
    {
        /// <summary>
        /// Initializes a new instance of the Forms.Series class.
        /// </summary>
        public Series() : base() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="Series"/> class with the specified series name.
        /// </summary>
        public Series(string name) : base(name) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="Series"/> class with the specified curve type, marker step and yAxisType.
        /// </summary>
        public Series(string name, SeriesChartType curveType, Color color, int step = 0, AxisType yAxisType = AxisType.Primary) : base(name)
        {
            YAxisType = yAxisType;
            SetCurveType(curveType, color, step);
            MarkerStyle = MarkerStyle.Circle;
        }

        /// <summary>
        /// The charts the series appears in.
        /// </summary>
        public List<ZoomableChart> Charts = new List<ZoomableChart>();
    }
}
