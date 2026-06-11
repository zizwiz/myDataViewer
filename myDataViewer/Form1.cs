using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using CenteredMessagebox;
using myDataViewer.help;

namespace myDataViewer
{
    public partial class Form1 : Form
    {
        private int counter;
        private Timer zoomTimer = null;
        private double targetXMin, targetXMax, targetYMin, targetYMax;
        private int zoomSteps = 10;
        private int zoomStep;
        private int loadedYear;
        private int loadedMonth;
        private ToolTip crosshairTooltip = new ToolTip();

        //used to pop and push zoom levels when using mouse scroll to zoom in and out
        private Stack<(double xMin, double xMax, double yMin, double yMax)> TemperaturezoomHistory
            = new Stack<(double, double, double, double)>();

        private Stack<(double xMin, double xMax, double yMin, double yMax)> HumidityzoomHistory
            = new Stack<(double, double, double, double)>();

        string dataRoot = "C:\\"; //Path.Combine(Application.StartupPath, "data");

        //used for moving crosshairs
        /*
         * Hovering = Crosshair follows the mouse position (but stays invisible).
           Left‑click = Crosshair becomes visible and locks to the mouse.
           Move mouse = Crosshair moves with the mouse.
           Right‑click = Crosshair disappears.
           Works with zoom = Crosshair stays aligned with the zoomed view.
           Works with compare mode = Crosshair overlays all series. 
           
         */

        private bool crosshairActive;
        bool movableCrosshairEnabled = true;
        private bool suppressNextCrosshairActivation = false;



        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Text += " : v" + Assembly.GetExecutingAssembly().GetName().Version; // put in the version number

            chartTemperature.Legends.Add(new Legend("TempLegend"));
            chartHumidity.Legends.Add(new Legend("HumLegend"));

            chartTemperature.Series.Clear();
            chartTemperature.ChartAreas[0].AxisX.ToolTip = "Day/Time";
            chartTemperature.ChartAreas[0].AxisY.ToolTip = "Value";

            chartHumidity.Series.Clear();
            chartHumidity.ChartAreas[0].AxisX.ToolTip = "Day/Time";
            chartHumidity.ChartAreas[0].AxisY.ToolTip = "Value";

            btnLoadData.Visible = false;
            btn_reset_chart.Visible = false;
            btn_save_chart_image.Visible = false;
            btn_reset_zoom.Visible = false;
            chkbx_zoom.Visible = false;
            chkCompareMode.Visible = false;
            chkbx_crosshairs.Visible = false;
            checkedListYears.Visible = false;
            checkedListMonths.Visible = false;
            checkedListSensors.Visible = false;

            //add this to allow mouse scroll to zoom
            chartTemperature.MouseWheel += chart_MouseWheel;
            chartHumidity.MouseWheel += chart_MouseWheel;

            // We create the crosshairs that follow the mouse,
            // left click = visible 
            // right click = invisible
            CreateCrosshair(chartTemperature);
            CreateCrosshair(chartHumidity);

            crosshairTooltip.InitialDelay = 0;
            crosshairTooltip.ReshowDelay = 0;
            crosshairTooltip.AutoPopDelay = 30000;
            crosshairTooltip.ShowAlways = true;
        }

        private void LoadYears()
        {
            checkedListYears.Items.Clear();

            string dataRoot = Path.Combine(Application.StartupPath, "Data");

            if (!Directory.Exists(dataRoot))
                return;

            var years = Directory.GetDirectories(dataRoot)
                .Select(Path.GetFileName)
                .OrderBy(y => y);

            foreach (var year in years)
                checkedListYears.Items.Add(year);
        }

        private void LoadMonthsForSelectedYears()
        {
            checkedListMonths.Items.Clear();

            string dataRoot = Path.Combine(Application.StartupPath, "Data");

            HashSet<string> months = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (string year in checkedListYears.CheckedItems)
            {
                string yearPath = Path.Combine(dataRoot, year);

                if (!Directory.Exists(yearPath))
                    continue;

                var monthFolders = Directory.GetDirectories(yearPath)
                    .Select(Path.GetFileName);

                foreach (var month in monthFolders)
                    months.Add(month);
            }

            foreach (var month in months.OrderBy(m => m))
                checkedListMonths.Items.Add(month);
        }

        private void btnLoadData_Click(object sender, EventArgs e)
        {
            if (checkedListYears.CheckedItems.Count == 0 ||
                checkedListMonths.CheckedItems.Count == 0 ||
                checkedListSensors.CheckedItems.Count == 0)
            {
                MessageBox.Show("Please select a year, month, and at least one sensor.");
                return;
            }

            string year = checkedListYears.CheckedItems[0].ToString();
            string month = checkedListMonths.CheckedItems[0].ToString();
            string dataRoot = Path.Combine(Application.StartupPath, "Data", year, month);

            // NORMAL MODE: clear charts
            if (!chkCompareMode.Checked)
            {
                chartTemperature.Series.Clear();
                chartHumidity.Series.Clear();
                counter = 0;
            }

            foreach (string sensor in checkedListSensors.CheckedItems)
            {
                counter++;

                if (counter > 140)
                {
                    MsgBox.Show("I can only draw 140 sensors", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string csvPath = Path.Combine(dataRoot, sensor + ".csv");
                if (!File.Exists(csvPath))
                    continue;

                // Unique series name for compare mode
                string seriesName = $"{sensor} - {month} {year}";

                // Avoid duplicate series in compare mode
                if (chartTemperature.Series.FindByName(seriesName) != null)
                    continue;

                var tempSeries = new Series(seriesName)
                {
                    ChartType = SeriesChartType.FastLine,
                    XValueType = ChartValueType.Double,
                    Color = Color.FromName(ColourList.SelectColour(counter)),
                    // BorderWidth = 1
                };

                var humSeries = new Series(seriesName)
                {
                    ChartType = SeriesChartType.FastLine,
                    XValueType = ChartValueType.Double,
                    Color = Color.FromName(ColourList.SelectColour(counter)),
                    //BorderWidth = 2
                };

                LoadCsvIntoSeries(csvPath, tempSeries, humSeries);

                chartTemperature.Series.Add(tempSeries);
                chartHumidity.Series.Add(humSeries);
            }

            //Allow checkboxes
            chkbx_zoom.Visible = true;
            chkbx_crosshairs.Visible = true;

            FormatOverlayXAxis(chartTemperature);
            FormatOverlayXAxis(chartHumidity);
        }


        private void FormatOverlayXAxis(Chart chart)
        {
            var axis = chart.ChartAreas[0].AxisX;

            axis.CustomLabels.Clear();
            axis.Interval = 24; // 1 tick per day
            axis.LabelStyle.Angle = -45;
            axis.MajorGrid.LineColor = Color.LightGray;

            // Add labels for days 1–31
            for (int day = 1; day <= 31; day++)
            {
                double start = (day - 1) * 24;
                double end = day * 24;
                axis.CustomLabels.Add(start, end, $"{day}");
            }
        }


        private void LoadCsvIntoSeries(string filePath, Series tempSeries, Series humSeries)
        {
            foreach (var line in File.ReadLines(filePath).Skip(1))
            {
                var parts = line.Split(',');
                if (parts.Length < 4)
                    continue;

                string combined = parts[0] + " " + parts[1];

                if (!DateTime.TryParseExact(
                        combined,
                        "MM/dd/yyyy HH:mm:ss",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out DateTime timestamp))
                {
                    continue;
                }

                double x = HoursSinceStartOfMonth(timestamp);

                if (double.TryParse(parts[2], NumberStyles.Any, CultureInfo.InvariantCulture, out double temp))

                {
                    //tempSeries.Points.AddXY(x, temp);
                    int p1 = tempSeries.Points.AddXY(x, temp);
                    tempSeries.Points[p1].ToolTip =
                        $"{tempSeries.Name}\nDay {timestamp.Day} {timestamp:HH:mm}\nTemp: {temp}°C";
                }

                if (double.TryParse(parts[3], NumberStyles.Any, CultureInfo.InvariantCulture, out double hum))
                {
                    //humSeries.Points.AddXY(x, hum);
                    int p2 = humSeries.Points.AddXY(x, hum);
                    humSeries.Points[p2].ToolTip =
                        $"{humSeries.Name}\nDay {timestamp.Day} {timestamp:HH:mm}\nHumidity: {hum}%";
                }

                btn_reset_chart.Visible = true;
                btn_save_chart_image.Visible = true;

                if (loadedYear == 0)
                {
                    loadedYear = timestamp.Year;
                    loadedMonth = timestamp.Month;
                }

            }
        }


        private void btn_close_Click(object sender, EventArgs e)
        {
            Close();
        }


        private double HoursSinceStartOfMonth(DateTime timestamp)
        {
            var start = new DateTime(timestamp.Year, timestamp.Month, 1, 0, 0, 0);
            return (timestamp - start).TotalHours;
        }

        private void chkbx_crosshairs_CheckedChanged(object sender, EventArgs e)
        {
            SetCrosshairEnabled(chartTemperature, chkbx_crosshairs.Checked);
            SetCrosshairEnabled(chartHumidity, chkbx_crosshairs.Checked);

            UpdateMovableCrosshairState();
        }

        private void chkbx_zoom_CheckedChanged(object sender, EventArgs e)
        {
            SetZoomEnabled(chartTemperature, chkbx_zoom.Checked);
            SetZoomEnabled(chartHumidity, chkbx_zoom.Checked);

            UpdateMovableCrosshairState();

        }


        private void SetZoomEnabled(Chart chart, bool enabled)
        {
            btn_reset_zoom.Visible = true;
            chart.ChartAreas[0].CursorX.IsUserSelectionEnabled = enabled;
            chart.ChartAreas[0].CursorY.IsUserSelectionEnabled = enabled;

            if (!enabled)
            {
                btn_reset_zoom.Visible = false;
                chart.ChartAreas[0].AxisX.ScaleView.ZoomReset();
                chart.ChartAreas[0].AxisY.ScaleView.ZoomReset();
            }
        }

        //private void SetZoomEnabled(Chart chart, bool enabled)
        //{
        //    btn_reset_zoom.Visible = enabled;

        //    var area = chart.ChartAreas[0];

        //    area.CursorX.IsUserEnabled = enabled;
        //    area.CursorY.IsUserEnabled = enabled;
        //    area.CursorX.IsUserSelectionEnabled = enabled;
        //    area.CursorY.IsUserSelectionEnabled = enabled;

        //    // NEW: disable mouse wheel zoom when zoom checkbox is OFF
        //    area.AxisX.ScaleView.Zoomable = enabled;
        //    area.AxisY.ScaleView.Zoomable = enabled;
        //}


        private void SetCrosshairEnabled(Chart chart, bool enabled)
        {
            chart.ChartAreas[0].CursorX.IsUserEnabled = enabled;
            chart.ChartAreas[0].CursorY.IsUserEnabled = enabled;

            // Remove any existing crosshair
            chart.ChartAreas[0].CursorX.SetCursorPosition(double.NaN);
            chart.ChartAreas[0].CursorY.SetCursorPosition(double.NaN);

            chart.Invalidate();
        }



        private void btn_reset_zoom_Click(object sender, EventArgs e)
        {
            // zoom reset by 1 level at a time. Add 1 as parameter of ZoomReset
            // No paramater will only store one level. If you zoomed twice you can only
            // unzoom by one so add the paramater as 1.
            chartTemperature.ChartAreas[0].AxisX.ScaleView.ZoomReset(1);
            chartTemperature.ChartAreas[0].AxisY.ScaleView.ZoomReset(1);
            chartHumidity.ChartAreas[0].AxisX.ScaleView.ZoomReset(1);
            chartHumidity.ChartAreas[0].AxisY.ScaleView.ZoomReset(1);
            //btn_reset_zoom.Visible = false;
        }

        private void checkedListYears_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // Delay execution until after the check state updates
            this.BeginInvoke((MethodInvoker)delegate
           {
               for (int i = 0; i < checkedListYears.Items.Count; i++)
               {
                   if (i != e.Index)
                       checkedListYears.SetItemChecked(i, false);
               }

               // Now reload months and sensors for the selected year
               LoadMonthsForSelectedYears();
               LoadSensorsForSelectedMonths();
           });
        }

        private void checkedListMonths_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate
           {
               for (int i = 0; i < checkedListMonths.Items.Count; i++)
               {
                   if (i != e.Index)
                       checkedListMonths.SetItemChecked(i, false);
               }

               // Reload sensors for the selected month
               LoadSensorsForSelectedMonths();
           });
        }

        private void LoadSensorsForSelectedMonths()
        {
            checkedListSensors.Items.Clear();

            string dataRoot = Path.Combine(Application.StartupPath, "Data");

            HashSet<string> sensors = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (string year in checkedListYears.CheckedItems)
            {
                foreach (string month in checkedListMonths.CheckedItems)
                {
                    string monthPath = Path.Combine(dataRoot, year, month);

                    if (!Directory.Exists(monthPath))
                        continue;

                    // Find all CSV files in this month folder
                    var csvFiles = Directory.GetFiles(monthPath, "*.csv");

                    foreach (var file in csvFiles)
                    {
                        string sensorName = Path.GetFileNameWithoutExtension(file);

                        // Add only if not already present
                        sensors.Add(sensorName);
                    }
                }
            }

            // Populate the checklist
            foreach (var sensor in sensors.OrderBy(s => s))
                checkedListSensors.Items.Add(sensor);
        }


        private void chartTemperature_DoubleClick(object sender, EventArgs e)
        {
            TemperaturezoomHistory.Clear();
            chartTemperature.ChartAreas[0].AxisX.ScaleView.ZoomReset();
            chartTemperature.ChartAreas[0].AxisY.ScaleView.ZoomReset();

            //Hide the crosshairs
            suppressNextCrosshairActivation = true;
            HideMovableCrosshair(chartHumidity, true);
            
        }

        private void chartHumidity_DoubleClick(object sender, EventArgs e)
        {
            HumidityzoomHistory.Clear();
            chartHumidity.ChartAreas[0].AxisX.ScaleView.ZoomReset();
            chartHumidity.ChartAreas[0].AxisY.ScaleView.ZoomReset();

            //Hide the crosshairs
            suppressNextCrosshairActivation = true;
            HideMovableCrosshair(chartHumidity, true);
        }

        private void btn_save_chart_image_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog dialog = new SaveFileDialog())
                {
                    dialog.Title = "Save Chart As Image";
                    dialog.Filter =
                        "JPEG Image (*.jpg)|*.jpg|PNG Image (*.png)|*.png|Bitmap Image (*.bmp)|*.bmp";
                    dialog.DefaultExt = "png";
                    dialog.AddExtension = true;

                    // Show dialog and validate selection
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        // Determine chosen format
                        ChartImageFormat format;
                        string ext = Path.GetExtension(dialog.FileName).ToLower();

                        switch (ext)
                        {
                            case ".jpg":
                            case ".jpeg":
                                format = ChartImageFormat.Jpeg;
                                break;
                            case ".png":
                                format = ChartImageFormat.Png;
                                break;
                            case ".bmp":
                                format = ChartImageFormat.Bmp;
                                break;
                            default:
                                MessageBox.Show("Unsupported file format.", "Error");
                                return;
                        }

                        // Save the chart safely
                        if (tabControl1.SelectedIndex.Equals(0))
                        {
                            chartTemperature.SaveImage(dialog.FileName, format);
                        }
                        else
                        {
                            chartHumidity.SaveImage(dialog.FileName, format);
                        }

                        MsgBox.Show("Chart saved successfully", "Success", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.Show($"An error occurred while saving the chart:\n{ex.Message}",
                    "Save Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }




        private void btn_reset_chart_Click(object sender, EventArgs e)
        {
            ResetChart();
        }


        private void ResetChart()
        {
        TemperaturezoomHistory.Clear();
            chartTemperature.Series.Clear();
            HumidityzoomHistory.Clear();
            chartHumidity.Series.Clear();
            counter = 0;
            btn_reset_chart.Visible = false;
            btn_save_chart_image.Visible = false;
        }

        // Wheel scroll zooms both X-Axis and Y-Axis
        // Control + wheel scroll zooms X-Axis
        // Shift + wheel scroll Zooms Y-Axis
        // Control + Shift + wheel scroll = ultrafine scrolling
        private void chart_MouseWheel(object sender, MouseEventArgs e)
        {
            var chart = sender as Chart;
            var area = chart.ChartAreas[0];

            try
            {
                double xMin = area.AxisX.ScaleView.ViewMinimum;
                double xMax = area.AxisX.ScaleView.ViewMaximum;

                double yMin = area.AxisY.ScaleView.ViewMinimum;
                double yMax = area.AxisY.ScaleView.ViewMaximum;

                double posX = area.AxisX.PixelPositionToValue(e.X);
                double posY = area.AxisY.PixelPositionToValue(e.Y);

                bool ctrl = (ModifierKeys & Keys.Control) == Keys.Control;
                bool shift = (ModifierKeys & Keys.Shift) == Keys.Shift;

                double zoomFactor = 0.20;     // normal zoom
                double fineFactor = 0.05;     // ultra‑fine zoom

                //used to push and pop zoom levels in and out.
                if (tabControl1.SelectedIndex.Equals(0))
                {
                    TemperaturezoomHistory.Push((
                        area.AxisX.ScaleView.ViewMinimum,
                        area.AxisX.ScaleView.ViewMaximum,
                        area.AxisY.ScaleView.ViewMinimum,
                        area.AxisY.ScaleView.ViewMaximum
                    ));
                }
                else
                {
                    HumidityzoomHistory.Push((
                        area.AxisX.ScaleView.ViewMinimum,
                        area.AxisX.ScaleView.ViewMaximum,
                        area.AxisY.ScaleView.ViewMinimum,
                        area.AxisY.ScaleView.ViewMaximum
                    ));
                }

                // Ctrl + Shift = ultra‑fine zoom
                if (ctrl && shift)
                    zoomFactor = fineFactor;

                // -----------------------------
                // SCROLL DOWN = ZOOM OUT
                // -----------------------------

                if (e.Delta < 0) // scroll down = zoom OUT
                {
                    if (tabControl1.SelectedIndex.Equals(0))
                    {
                        if (TemperaturezoomHistory.Count > 0)
                        {
                            var prev = TemperaturezoomHistory.Pop();

                            StartSmoothZoom(chartTemperature, prev.xMin, prev.xMax, prev.yMin, prev.yMax);
                        }
                    }
                    else
                    {
                        if (HumidityzoomHistory.Count > 0)
                        {
                            var prev = HumidityzoomHistory.Pop();

                            StartSmoothZoom(chartHumidity, prev.xMin, prev.xMax, prev.yMin, prev.yMax);
                        }
                    }

                    return;
                }


                // -----------------------------
                // SCROLL UP = ZOOM IN
                // -----------------------------
                if (e.Delta > 0)
                {
                    // X‑axis only
                    if (ctrl && !shift)
                    {
                        double newXMin = posX - (posX - xMin) * (1 - zoomFactor);
                        double newXMax = posX + (xMax - posX) * (1 - zoomFactor);
                        area.AxisX.ScaleView.Zoom(newXMin, newXMax);
                        return;
                    }

                    // Y‑axis only
                    if (shift && !ctrl)
                    {
                        double newYMin = posY - (posY - yMin) * (1 - zoomFactor);
                        double newYMax = posY + (yMax - posY) * (1 - zoomFactor);
                        area.AxisY.ScaleView.Zoom(newYMin, newYMax);
                        return;
                    }

                    // Both axes
                    double newXMinBoth = posX - (posX - xMin) * (1 - zoomFactor);
                    double newXMaxBoth = posX + (xMax - posX) * (1 - zoomFactor);

                    double newYMinBoth = posY - (posY - yMin) * (1 - zoomFactor);
                    double newYMaxBoth = posY + (yMax - posY) * (1 - zoomFactor);

                    StartSmoothZoom(chart, newXMinBoth, newXMaxBoth, newYMinBoth, newYMaxBoth);
                }
            }
            catch
            {
                // Ignore zoom errors
            }
        }

        private void StartSmoothZoom(Chart chart, double newXMin, double newXMax, double newYMin, double newYMax)
        {
            var area = chart.ChartAreas[0];

            targetXMin = newXMin;
            targetXMax = newXMax;
            targetYMin = newYMin;
            targetYMax = newYMax;

            double startXMin = area.AxisX.ScaleView.ViewMinimum;
            double startXMax = area.AxisX.ScaleView.ViewMaximum;
            double startYMin = area.AxisY.ScaleView.ViewMinimum;
            double startYMax = area.AxisY.ScaleView.ViewMaximum;

            zoomStep = 0;

            if (zoomTimer != null)
                zoomTimer.Stop();

            zoomTimer = new Timer();
            zoomTimer.Interval = 15; // smooth animation
            zoomTimer.Tick += (s, e) =>
            {
                zoomStep++;
                double t = zoomStep / (double)zoomSteps;

                area.AxisX.ScaleView.Zoom(
                    startXMin + (targetXMin - startXMin) * t,
                    startXMax + (targetXMax - startXMax) * t
                );

                area.AxisY.ScaleView.Zoom(
                    startYMin + (targetYMin - startYMin) * t,
                    startYMax + (targetYMax - startYMax) * t
                );

                if (zoomStep >= zoomSteps)
                    zoomTimer.Stop();
            };

            zoomTimer.Start();
        }

        private void btn_open_data_location_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                checkedListMonths.Items.Clear();
                checkedListSensors.Items.Clear();
                ResetChart();

                dataRoot = fbd.SelectedPath;
                if (Directory.Exists(dataRoot))
                {
                    LoadYears();
                }

                btnLoadData.Visible = true;
                chkCompareMode.Visible = true;
                checkedListYears.Visible = true;
                checkedListMonths.Visible = true;
                checkedListSensors.Visible = true;
            }
        }

        private void btn_help_Click(object sender, EventArgs e)
        {
            help_form help = new help_form();
            help.Show();
        }

        private void CreateCrosshair(Chart chart)
        {
            var area = chart.ChartAreas[0];

            var crossX = new VerticalLineAnnotation
            {
                AxisX = area.AxisX,
                ClipToChartArea = area.Name,
                LineColor = Color.Red,
                LineWidth = 1,
                Visible = false,
                IsInfinitive = true,
                Name = chart.Name + "_CrossX"
            };

            var crossY = new HorizontalLineAnnotation
            {
                AxisY = area.AxisY,
                ClipToChartArea = area.Name,
                LineColor = Color.Red,
                LineWidth = 1,
                Visible = false,
                IsInfinitive = true,
                Name = chart.Name + "_CrossY"
            };

            chart.Annotations.Add(crossX);
            chart.Annotations.Add(crossY);
        }

        private void chart_MouseMove(object sender, MouseEventArgs e)
        {
            var chart = sender as Chart;
            var area = chart.ChartAreas[0];
            
            if (!movableCrosshairEnabled)
            {
                crosshairTooltip.Hide(chart);
                return;
            }
            
            if (!crosshairActive)
                return;

            // Get plot area boundaries in pixels
            double xMinPix = area.AxisX.ValueToPixelPosition(area.AxisX.Minimum);
            double xMaxPix = area.AxisX.ValueToPixelPosition(area.AxisX.Maximum);
            double yMinPix = area.AxisY.ValueToPixelPosition(area.AxisY.Minimum);
            double yMaxPix = area.AxisY.ValueToPixelPosition(area.AxisY.Maximum);

            // If mouse is outside plot area → do nothing
            if (e.X < xMinPix || e.X > xMaxPix || e.Y < yMaxPix || e.Y > yMinPix)
                return;

            // Convert pixel → axis values (safe now)
            double xVal = area.AxisX.PixelPositionToValue(e.X);
            double yVal = area.AxisY.PixelPositionToValue(e.Y);

            var crossX = chart.Annotations[chart.Name + "_CrossX"] as VerticalLineAnnotation;
            var crossY = chart.Annotations[chart.Name + "_CrossY"] as HorizontalLineAnnotation;

            crossX.X = xVal;
            crossY.Y = yVal;

            chart.Invalidate();

            // Show tooltip with X/Y values
            // Convert X-axis value (hours since start of month) back to real timestamp
            DateTime startOfMonth = new DateTime(loadedYear, loadedMonth, 1);
            DateTime realTime = startOfMonth.AddHours(xVal);

            // Format X-axis text
            string xText = realTime.ToString("dd MMM yyyy HH:mm:ss");

            // Format Y-axis based on chart
            bool isTemperatureChart = chart == chartTemperature;
            string yText = isTemperatureChart
                ? $"{yVal:0.0} °C"
                : $"{yVal:0.0} %";

            // Build tooltip text
            string text = $"Time: {xText}\nValue: {yText}";

            // Show tooltip
            crosshairTooltip.Show(text, chart, e.X + 15, e.Y + 15);
        }


        private void chart_MouseDown(object sender, MouseEventArgs e)
        {
            var chart = sender as Chart;

            if (!movableCrosshairEnabled)
                return; // ignore clicks

            // Prevent crosshair activation immediately after double-click
            if (suppressNextCrosshairActivation)
            {
                suppressNextCrosshairActivation = false;
                crosshairActive = false;
                HideMovableCrosshair(chart, true);
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                crosshairActive = true;

                var crossX = chart.Annotations[chart.Name + "_CrossX"];
                var crossY = chart.Annotations[chart.Name + "_CrossY"];

                crossX.Visible = true;
                crossY.Visible = true;

                chart_MouseMove(sender, e);
            }
            else if (e.Button == MouseButtons.Right)
            {
                crosshairActive = false;
                HideMovableCrosshair(chart, true);
            }
        }


        private void UpdateMovableCrosshairState()
        {
            // Movable crosshair is only allowed when BOTH checkboxes are off
            movableCrosshairEnabled = !chkbx_crosshairs.Checked && !chkbx_zoom.Checked;

            if (!movableCrosshairEnabled)
            {
                // Turn off movable crosshair immediately
                crosshairActive = false;

                HideMovableCrosshair(chartTemperature, false);
                HideMovableCrosshair(chartHumidity, false);
            }
        }

        private void HideMovableCrosshair(Chart chart, bool flag)
        {
            //if (flag) crosshairActive = false; //flag true for double clicks.

            if (flag)
            {
                // Turn off movable crosshair immediately
                crosshairActive = false;

                HideMovableCrosshair(chartTemperature, false);
                HideMovableCrosshair(chartHumidity, false);
            }

            var crossX = chart.Annotations[chart.Name + "_CrossX"];
            var crossY = chart.Annotations[chart.Name + "_CrossY"];

            crossX.Visible = false;
            crossY.Visible = false;

            crosshairTooltip.Hide(chart);
            chart.Invalidate();
        }
    }
}
