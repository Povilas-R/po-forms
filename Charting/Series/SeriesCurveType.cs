using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace Po.Forms.Charting
{
	public partial class Series
	{
		/// <summary>
		/// Sets the curve type of this series.
		/// </summary>
		/// <param name="type">The type of series.</param>
		/// <param name="color">The color of series.</param>
		/// <param name="step">The marker step. 0 to disable markers.</param>
		public void SetCurveType(SeriesChartType type, Color color, int step = 0)
		{
			LabelFormat = "#.##";
			BorderWidth = 2;
			if (step == 0)
			{
				MarkerStyle = MarkerStyle.None;
				MarkerSize = 0;
				MarkerStep = 50;
			}
			else
			{
				MarkerStyle = MarkerStyle.Circle;
				MarkerSize = 7;
				MarkerStep = step;
			}
			BorderDashStyle = ChartDashStyle.Dash;
			MarkerBorderColor = Color.Black;
			BorderColor = Color.Transparent;
			ShadowColor = Color.Transparent;
			ChartType = type;
			Color = color;
		}
	}
}
