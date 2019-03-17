using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Po.Forms.Charting
{
	public partial class ZoomableChart
	{
		private bool _isSelectingZoomArea = false;
		private bool _isDraggingZoomArea = false;
		private int
			_zoomPixel_X1 = 0,
			_zoomPixel_X2 = 0,
			_zoomPixel_Y1 = 0,
			_zoomPixel_Y2 = 0;
		private int
			_dragPixel_X = 0,
			_dragPixel_Y = 0;
		private Rectangle _zoomPreview;

		/// <summary>
		/// xAxis / yAxis
		/// </summary>
		private double _zoomY2XRatio = 1;
		private const double ZoomSpeedDefault = 1.15;
		private double _zoomSpeed = 1.15;
		/// <summary>
		/// Defines mouse wheel zoom speed. Set to 1 to disable it.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double ZoomSpeed
		{
			get => _zoomSpeed;
			set => _zoomSpeed = value <= 0 ? ZoomSpeedDefault : value;
		}

		private void Induction_MouseDown(object sender, MouseEventArgs e)
		{
			if (!Focused)
			{
				return;
			}

			try
			{
				if (e.Button == MouseButtons.Left)
				{
					// Starts zoom area selection
					if (!IsZoomed())
					{
						_zoomPixel_X1 = e.X;
						_zoomPixel_Y1 = e.Y;
						_zoomPixel_X2 = _zoomPixel_X1;
						_zoomPixel_Y2 = _zoomPixel_Y1;
						_isSelectingZoomArea = true;
					}
					// Starts dragging zoom area
					else if (IsZoomed() && !_isDraggingZoomArea)
					{
						_dragPixel_X = e.X;
						_dragPixel_Y = e.Y;
						_isDraggingZoomArea = true;
					}
				}

				if (e.Button == MouseButtons.Right && !_isSelectingZoomArea)
				{
					// Resets zoom
					try
					{
						_isDraggingZoomArea = false;
						_isSelectingZoomArea = false;

						ResetZoom();

						UpdateChartsZoom(true);
					}
					catch { }
				}
			}
			catch { }
		}
		private void Induction_MouseUp(object sender, MouseEventArgs e)
		{
			if (!Focused)
			{
				return;
			}

			try
			{
				// Zooms the selected area
				if (_isSelectingZoomArea && !IsZoomed() && _zoomPixel_X1 != _zoomPixel_X2 && _zoomPixel_Y1 != _zoomPixel_Y2)
				{
					double zoomPixel_X_Start = Math.Min(_zoomPixel_X1, _zoomPixel_X2);
					double zoomPixel_X_End = Math.Max(_zoomPixel_X1, _zoomPixel_X2);
					double zoomPixel_Y_Start = Math.Min(_zoomPixel_Y1, _zoomPixel_Y2);
					double zoomPixel_Y_End = Math.Max(_zoomPixel_Y1, _zoomPixel_Y2);

					if (zoomPixel_X_End >= Width - 2)
					{
						zoomPixel_X_End = Width - 2;
					}

					if (zoomPixel_Y_End >= Height - 2)
					{
						zoomPixel_Y_End = Height - 2;
					}

					if (zoomPixel_Y_Start < 0)
					{
						zoomPixel_Y_Start = 0;
					}

					if (zoomPixel_X_Start < 0)
					{
						zoomPixel_X_Start = 0;
					}

					double xStart = Area.AxisX.PixelPositionToValue(zoomPixel_X_Start);
					double xEnd = Area.AxisX.PixelPositionToValue(zoomPixel_X_End);
					double yStart = Area.AxisY.PixelPositionToValue(zoomPixel_Y_End);
					double yEnd = Area.AxisY.PixelPositionToValue(zoomPixel_Y_Start);

					double xSize = xEnd - xStart;
					double ySize = yEnd - yStart;

					SetAxisInterval(AxisName.X, xSize / AxisXIntervalCount);
					Area.AxisX.ScaleView.Position = xStart;
					Area.AxisX.ScaleView.Size = xSize;

					SetAxisBounds(AxisName.X2, DateTimeIntervalType.Milliseconds, xStart, xStart + xSize);

					SetAxisInterval(AxisName.Y, ySize / AxisYIntervalCount);
					Area.AxisY.ScaleView.Position = yStart;
					Area.AxisY.ScaleView.Size = ySize;

					_zoomY2XRatio = Area.AxisX.ScaleView.Size / Area.AxisY.ScaleView.Size;
					UpdateChartsZoom();
				}

				// Resets zoom selection and zoom drag states
				_isDraggingZoomArea = false;
				_isSelectingZoomArea = false;
				_zoomPreview = new Rectangle(0, 0, 0, 0);
			}
			catch { }
		}

		private const double MinZoomPerc = 0.002;
		private void Induction_MouseWheel(object sender, MouseEventArgs e)
		{
			if (!Focused)
			{
				return;
			}

			try
			{
				if (IsZoomed())
				{
					double xSize = Area.AxisX.ScaleView.Size;
					double ySize = Area.AxisY.ScaleView.Size;
					if (double.IsNaN(xSize))
					{
						xSize = Area.AxisX.Maximum - Area.AxisX.Minimum;
					}

					if (double.IsNaN(ySize))
					{
						ySize = Area.AxisY.Maximum - Area.AxisY.Minimum;
					}

					_zoomY2XRatio = xSize / ySize;

					// Y axis
					double newYStart = Area.AxisY.ScaleView.ViewMinimum;
					double newYEnd = Area.AxisY.ScaleView.ViewMaximum;
					double yCenter = (newYStart + newYEnd) / 2.0;
					double yOut = (Area.AxisY.ScaleView.Size / 2) * ZoomSpeed;
					double yIn = (Area.AxisY.ScaleView.Size / 2) / ZoomSpeed;
					double newYSize = ySize;
					// X axis
					double newXStart = Area.AxisX.ScaleView.ViewMinimum;
					double newXEnd = Area.AxisX.ScaleView.ViewMaximum;
					double xCenter = (newXStart + newXEnd) / 2.0;
					double xOut = yOut * _zoomY2XRatio;
					double xIn = yIn * _zoomY2XRatio;
					double newXSize = xSize;

					// Zooms out (zoomSpeed > 1)
					if (e.Delta < 0)
					{
						// X axis
						newXStart = xCenter - xOut;
						newXEnd = xCenter + xOut;
						newXSize = newXEnd - newXStart;
						// Y axis
						newYStart = yCenter - yOut;
						newYEnd = yCenter + yOut;
						newYSize = newYEnd - newYStart;
					}
					// Zooms in (zoomSpeed > 1)
					if (e.Delta > 0)
					{
						// X axis
						newXStart = xCenter - xIn;
						newXEnd = xCenter + xIn;
						newXSize = newXEnd - newXStart;
						// Y axis
						newYStart = yCenter - yIn;
						newYEnd = yCenter + yIn;
						newYSize = newYEnd - newYStart;
					}

					if (newXSize > MinZoomPerc * (Area.AxisX.Maximum - Area.AxisX.Minimum))
					{
						// X axis
						SetAxisInterval(AxisName.X, (newXEnd - newXStart) / AxisXIntervalCount);
						Area.AxisX.ScaleView.Position = newXStart;
						Area.AxisX.ScaleView.Size = newXEnd - newXStart;
						// X2 (datetime) axis
						SetAxisBounds(AxisName.X2, DateTimeIntervalType.Milliseconds, Area.AxisX.ScaleView.ViewMinimum, Area.AxisX.ScaleView.ViewMaximum);
					}

					if (newYSize > MinZoomPerc * (Area.AxisY.Maximum - Area.AxisY.Minimum))
					{
						// Y axis
						SetAxisInterval(AxisName.Y, (newYEnd - newYStart) / AxisYIntervalCount);
						Area.AxisY.ScaleView.Position = newYStart;
						Area.AxisY.ScaleView.Size = newYEnd - newYStart;
					}

					Refresh();
					UpdateChartsZoom();
				}
			}
			catch { }
		}
		private void Induction_MouseMove(object sender, MouseEventArgs e)
		{
			if (!Focused)
			{
				return;
			}

			// Updates zoom area selection
			if (_isSelectingZoomArea && !IsZoomed())
			{
				_zoomPixel_X2 = e.X;
				_zoomPixel_Y2 = e.Y;

				int previewX = Math.Min(_zoomPixel_X1, _zoomPixel_X2);
				int previewY = Math.Min(_zoomPixel_Y1, _zoomPixel_Y2);
				int previewWidth = Math.Abs(_zoomPixel_X1 - _zoomPixel_X2);
				int previewHeight = Math.Abs(_zoomPixel_Y1 - _zoomPixel_Y2);

				_zoomPreview = new Rectangle(previewX, previewY, previewWidth, previewHeight);
				Refresh();
			}
			// Updates zoom area drag
			else if (_isDraggingZoomArea && IsZoomed())
			{
				int newX = e.X;
				int newY = e.Y;
				if (newX < 0)
				{
					newX = 0;
				}

				if (newY < 0)
				{
					newY = 0;
				}

				try
				{
					Area.AxisX.ScaleView.Position -= Area.AxisX.PixelPositionToValue(newX) - Area.AxisX.PixelPositionToValue(_dragPixel_X);
					SetAxisBounds(AxisName.X2, DateTimeIntervalType.Milliseconds, Area.AxisX.ScaleView.ViewMinimum, Area.AxisX.ScaleView.ViewMaximum);
					Area.AxisY.ScaleView.Position -= Area.AxisY.PixelPositionToValue(newY) - Area.AxisY.PixelPositionToValue(_dragPixel_Y);
					Refresh();

					UpdateChartsZoom();
				}
				catch { }

				_dragPixel_X = newX;
				_dragPixel_Y = newY;
			}
		}
		private void Induction_Paint(object sender, PaintEventArgs e)
		{
			if (_zoomPreview != null)
			{
				using (var pen = new Pen(Color.Red, 2))
				{
					e.Graphics.DrawRectangle(pen, _zoomPreview);
				}
			}
		}
		private void Induction_MouseEnter(object sender, EventArgs e) => Focus();
		private void Induction_MouseLeave(object sender, EventArgs e)
		{
			if (!_isSelectingZoomArea)
			{
				Parent.Focus();
			}
		}

		/// <summary>
		/// Grouped charts whose zoom is updated whenever this chart is zoomed
		/// </summary>
		public List<ZoomableChart> GroupedCharts = new List<ZoomableChart>();
		private void UpdateChartsZoom(bool resetZoom = false)
		{
			double x_start = Area.AxisX.ScaleView.Position;
			double x_size = Area.AxisX.ScaleView.Size;

			for (int i = 0; i < GroupedCharts.Count; i++)
			{
				if (GroupedCharts[i] == this)
				{
					continue;
				}

				if (resetZoom)
				{
					GroupedCharts[i].ResetZoom();
				}
				else
				{
					GroupedCharts[i].UpdateZoom(x_start, x_size);
				}
			}
		}

		/// <summary>
		/// Resets chart's zoom.
		/// </summary>
		public void ResetZoom()
		{
			// X axis
			SetAxisInterval(AxisName.X);
			Area.AxisX.ScaleView.ZoomReset();
			// X2 (datetime) axis
			SetAxisBounds(AxisName.X2, DateTimeIntervalType.Milliseconds, Area.AxisX.Minimum, Area.AxisX.Maximum);
			// Y axis
			SetAxisInterval(AxisName.Y);
			Area.AxisY.ScaleView.ZoomReset();

			Refresh();
		}

		/// <summary>
		/// Updates chart's zoom by the xAxis.
		/// </summary>
		/// <param name="x_start"></param>
		/// <param name="x_size"></param>
		public void UpdateZoom(double x_start, double x_size)
		{
			// X axis
			SetAxisInterval(AxisName.X, x_size / AxisXIntervalCount);
			Area.AxisX.ScaleView.Position = x_start;
			Area.AxisX.ScaleView.Size = x_size;
			// X2 (datetime) axis
			SetAxisBounds(AxisName.X2, DateTimeIntervalType.Milliseconds, Area.AxisX.ScaleView.ViewMinimum, Area.AxisX.ScaleView.ViewMaximum);
			// Y axis
			double y_start = Area.AxisY.Minimum;
			double y_size = Area.AxisY.Maximum - Area.AxisY.Minimum;
			SetAxisInterval(AxisName.Y, y_size / AxisYIntervalCount);
			Area.AxisY.ScaleView.Position = y_start;
			Area.AxisY.ScaleView.Size = y_size;

			Refresh();
		}

		/// <summary>
		/// Returns whether the chart is zoomed or not.
		/// </summary>
		public bool IsZoomed() => Area.AxisX.ScaleView.IsZoomed;
	}
}
