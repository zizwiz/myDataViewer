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

namespace myDataViewer
{
    public partial class Form1 : Form
    {

        //private readonly string[] Sensors =
        //{
        //    "Outside", "Sanctuary", "Thermostat", "Centre", "Gallery"
        //};

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Text += " : v" + Assembly.GetExecutingAssembly().GetName().Version; // put in the version number

            string dataRoot = Path.Combine(Application.StartupPath, "data");

            if (Directory.Exists(dataRoot))
            {
                LoadYears();
            }

            //checkedListSensors.Items.AddRange(Sensors);

            chartTemperature.ChartAreas.Add(new ChartArea("TempArea"));
            chartHumidity.ChartAreas.Add(new ChartArea("HumArea"));

            chartTemperature.Legends.Add(new Legend("TempLegend"));
            chartHumidity.Legends.Add(new Legend("HumLegend"));
            
            chartTemperature.Series.Clear();
            chartTemperature.ChartAreas[0].AxisX.ToolTip = "Day/Time";
            chartTemperature.ChartAreas[0].AxisY.ToolTip = "Value";

            chartHumidity.Series.Clear();
            chartHumidity.ChartAreas[0].AxisX.ToolTip = "Day/Time";
            chartHumidity.ChartAreas[0].AxisY.ToolTip = "Value";

            btn_reset_zoom.Visible = false;
            chkbx_zoom.Visible = false;
            chkbx_crosshairs.Visible = false;
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

        //private void LoadMonthsForSelectedYears()
        //{
        //    checkedListMonths.Items.Clear();

        //    string dataRoot = Path.Combine(Application.StartupPath, "Data");

        //    foreach (string year in checkedListYears.CheckedItems)
        //    {
        //        string yearPath = Path.Combine(dataRoot, year);

        //        if (!Directory.Exists(yearPath))
        //            continue;

        //        var months = Directory.GetDirectories(yearPath)
        //            .Select(Path.GetFileName)
        //            .OrderBy(m => m);

        //        foreach (var month in months)
        //        {
        //            if (!checkedListMonths.Items.Contains(month))
        //                checkedListMonths.Items.Add(month);
        //        }
        //    }
        //}

        //private void btnLoadData_Click(object sender, EventArgs e)
        //{
        //    int counter = 0;

        //    if (checkedListYears.CheckedItems.Count == 0)
        //    {
        //        MessageBox.Show("Please select at least one year.");
        //        return;
        //    }

        //    if (checkedListMonths.CheckedItems.Count == 0)
        //    {
        //        MessageBox.Show("Please select at least one month.");
        //        return;
        //    }

        //    if (checkedListSensors.CheckedItems.Count == 0)
        //    {
        //        MessageBox.Show("Please select at least one sensor.");
        //        return;
        //    }

        //    chartTemperature.Series.Clear();
        //    chartHumidity.Series.Clear();

        //    string dataRoot = Path.Combine(Application.StartupPath, "Data");

        //    foreach (string year in checkedListYears.CheckedItems)
        //    {
        //        foreach (string month in checkedListMonths.CheckedItems)
        //        {
        //            foreach (string sensor in checkedListSensors.CheckedItems)
        //            {
        //                counter++;

        //                if (counter > 140)
        //                {
        //                    MsgBox.Show("I can only draw 140 sensors", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                    return;
        //                }

        //                string csvPath = Path.Combine(dataRoot, year, month, sensor + ".csv");

        //                if (!File.Exists(csvPath))
        //                    continue;

        //                string seriesName = $"{sensor} - {month} {year}";

        //                var tempSeries = new Series(seriesName)
        //                {
        //                    ChartType = SeriesChartType.Line,
        //                    XValueType = ChartValueType.Double,
        //                    Color = Color.FromName(ColourList.SelectColour(counter))
        //                };

        //                var humSeries = new Series(seriesName)
        //                {
        //                    ChartType = SeriesChartType.Line,
        //                    XValueType = ChartValueType.Double,
        //                    Color = Color.FromName(ColourList.SelectColour(counter))
        //                };

        //                LoadCsvIntoSeries(csvPath, tempSeries, humSeries);

        //                chartTemperature.Series.Add(tempSeries);
        //                chartHumidity.Series.Add(humSeries);
        //            }
        //        }
        //    }

        //    //Allow checkboxes
        //    chkbx_zoom.Visible = true;
        //    chkbx_crosshairs.Visible = true;

        //    FormatOverlayXAxis(chartTemperature);
        //    FormatOverlayXAxis(chartHumidity);
        //}

        private void btnLoadData_Click(object sender, EventArgs e)
        {
            int counter = 0;

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

            chartTemperature.Series.Clear();
            chartHumidity.Series.Clear();

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

                string seriesName = $"{sensor} - {month} {year}";

                var tempSeries = new Series(seriesName)
                {
                    ChartType = SeriesChartType.Line,
                    XValueType = ChartValueType.Double,
                    Color = Color.FromName(ColourList.SelectColour(counter))
                };

                var humSeries = new Series(seriesName)
                {
                    ChartType = SeriesChartType.Line,
                    XValueType = ChartValueType.Double,
                    Color = Color.FromName(ColourList.SelectColour(counter))
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
                    humSeries.Points[p2].ToolTip = $"{humSeries.Name}\nDay {timestamp.Day} {timestamp:HH:mm}\nHumidity: {hum}%";
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
        }

        private void chkbx_zoom_CheckedChanged(object sender, EventArgs e)
        {
            SetZoomEnabled(chartTemperature, chkbx_zoom.Checked);
            SetZoomEnabled(chartHumidity, chkbx_zoom.Checked);
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
            chartTemperature.ChartAreas[0].AxisX.ScaleView.ZoomReset();
            chartTemperature.ChartAreas[0].AxisY.ScaleView.ZoomReset();
            chartHumidity.ChartAreas[0].AxisX.ScaleView.ZoomReset();
            chartHumidity.ChartAreas[0].AxisY.ScaleView.ZoomReset();
            btn_reset_zoom.Visible = false;
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

       
    }
}
