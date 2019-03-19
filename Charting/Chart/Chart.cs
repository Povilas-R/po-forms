using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace Po.Forms.Charting
{
    /// <summary>
    /// <see cref="Chart"/> class with added zoom functionality.
    /// </summary>
    public partial class ZoomableChart : Chart
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ZoomableChart"/> class.
        /// </summary>
        public ZoomableChart() : base()
        {
            // Adds the main chart area
            ChartAreas.Clear();
            ChartAreas.Add(Area);
            // Adds the legend
            Legend.Alignment = StringAlignment.Near;
            Legend.Docking = Docking.Right;
            Legends.Clear();
            Legends.Add(Legend);
            // Sets the style
            DataManipulator.IsEmptyPointIgnored = true;
            DataManipulator.FilterSetEmptyPoints = true;
            // X axis
            Area.AxisX.TitleFont = new Font("Arial", 11, FontStyle.Bold);
            Area.AxisX.LabelStyle.Format = "#.###";
            Area.AxisX.LabelStyle.Angle = -45;
            Area.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            Area.AxisX.ScaleBreakStyle.Enabled = false;
            Area.AxisX.ScaleBreakStyle.CollapsibleSpaceThreshold = 90;
            Area.AxisX.ScaleBreakStyle.StartFromZero = StartFromZero.Yes;
            Area.AxisX.ScaleBreakStyle.Spacing = 1d;
            Area.AxisX.IsMarginVisible = false;
            Area.AxisX.ScrollBar.Enabled = false;
            // X2 (datetime) axis
            Area.AxisX2.LabelStyle.Format = "HH:mm (dd)";
            Area.AxisX2.LabelStyle.Angle = -45;
            Area.AxisX2.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            Area.AxisX2.ScaleBreakStyle.Enabled = false;
            Area.AxisX2.ScaleBreakStyle.CollapsibleSpaceThreshold = 90;
            Area.AxisX2.ScaleBreakStyle.StartFromZero = StartFromZero.Yes;
            Area.AxisX2.ScaleBreakStyle.Spacing = 1d;
            Area.AxisX2.IsMarginVisible = false;
            Area.AxisX2.IntervalType = DateTimeIntervalType.Milliseconds;
            Area.AxisX2.Interval = 5 * 60 * 1000;
            Area.AxisX2.ScrollBar.Enabled = false;
            // Y axis
            Area.AxisY.TitleFont = new Font("Arial", 11, FontStyle.Bold);
            Area.AxisY.LabelStyle.Format = "#.###";
            Area.AxisY.ScaleBreakStyle.Enabled = false;
            Area.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            Area.AxisY.ScaleBreakStyle.StartFromZero = StartFromZero.Yes;
            Area.AxisY.IsMarginVisible = false;
            Area.AxisY.ScrollBar.Enabled = false;
            // Adds datetime display series for X2 axis
            Series.Add(new Series(DatetimeDisplaySeriesName));
            Series[0].XValueType = ChartValueType.DateTime;
            Series[0].XAxisType = AxisType.Secondary;
            Series[0].ChartType = SeriesChartType.Line;
            Series[0].Points.Add(new DataPoint(new DateTime(2001, 01, 01).ToOADate(), 0));
            Series[0].Points.Add(new DataPoint(new DateTime(2001, 01, 02).ToOADate(), 10));
            Series[0].IsVisibleInLegend = false;
            // Adds zoom functionality
            MouseDown += Induction_MouseDown;
            MouseUp += Induction_MouseUp;
            MouseWheel += Induction_MouseWheel;
            MouseMove += Induction_MouseMove;
            Paint += Induction_Paint;
            MouseEnter += Induction_MouseEnter;
            MouseLeave += Induction_MouseLeave;

            if (DatetimeAxisEnabled)
            {
                Area.AxisX2.Enabled = AxisEnabled.True;
            }
            else
            {
                Area.AxisX2.Enabled = AxisEnabled.False;
            }
        }

        /// <summary>
        /// Gets a read-only <see cref="ChartAreaCollection"/> object that is used to store <see cref="ChartArea"/> objects.
        /// </summary>
        // Required to prevent DesignerSerialization
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new ChartAreaCollection ChartAreas => base.ChartAreas;
        /// <summary>
        /// Gets or sets a <see cref="LegendCollection"/> that stores all <see cref="Legend"/> objects used by the <see cref="Chart"/> control.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new LegendCollection Legends => base.Legends;
        /// <summary>
        /// Gets a <see cref="SeriesCollection"/> object, which contains <see cref="Series"/> objects.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new SeriesCollection Series => base.Series;

        /// <summary>
        /// Date time interval type for X axis.
        /// </summary>
        [Category("Chart"), Description("Specifies the interval type for X axis.")]
        public DateTimeIntervalType AxisXIntervalType { get; set; } = DateTimeIntervalType.Hours;

        private const string DatetimeDisplaySeriesName = "Series required for datetime display";

        /// <summary>
        /// The one and only <see cref="ChartArea"/> the chart works in.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ChartArea Area = new ChartArea("MainArea");
        /// <summary>
        /// The legend of the chart.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Legend Legend = new Legend("Legend");
    }
}
