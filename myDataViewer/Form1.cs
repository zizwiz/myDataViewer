using System;
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

        private readonly string[] Sensors =
        {
            "Outside", "Sanctuary", "Thermostat", "Centre", "Gallery"
        };

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
                var years = Directory.GetDirectories(dataRoot)
                    .Select(Path.GetFileName)
                    .OrderBy(y => y);

                comboYear.Items.AddRange(years.ToArray());
            }

            checkedListSensors.Items.AddRange(Sensors);
            checkedListSensors.CheckOnClick = true;

            chartTemperature.ChartAreas.Add(new ChartArea("TempArea"));
            chartHumidity.ChartAreas.Add(new ChartArea("HumArea"));

            chartTemperature.Legends.Add(new Legend("TempLegend"));
            chartHumidity.Legends.Add(new Legend("HumLegend"));

            comboYear.SelectedIndex = 0;

            chartTemperature.Series.Clear();
            chartTemperature.ChartAreas[0].CursorX.IsUserEnabled = true;
            chartTemperature.ChartAreas[0].CursorY.IsUserEnabled = true;
            chartTemperature.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chartTemperature.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            chartTemperature.ChartAreas[0].AxisX.ToolTip = "Day/Time";
            chartTemperature.ChartAreas[0].AxisY.ToolTip = "Value";

            chartHumidity.Series.Clear();
            chartHumidity.ChartAreas[0].CursorX.IsUserEnabled = true;
            chartHumidity.ChartAreas[0].CursorY.IsUserEnabled = true;
            chartHumidity.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chartHumidity.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            chartHumidity.ChartAreas[0].AxisX.ToolTip = "Day/Time";
            chartHumidity.ChartAreas[0].AxisY.ToolTip = "Value";

        }

        private void btnLoadData_Click(object sender, EventArgs e)
        {
            int counter = 0;
            // Validate selections
            if (comboYear.SelectedItem == null)
            {
                MsgBox.Show("Please select a year.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (checkedListMonths.CheckedItems.Count == 0)
            {
                MsgBox.Show("Please select at least one month.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (checkedListSensors.CheckedItems.Count == 0)
            {
                MsgBox.Show("Please select at least one sensor.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string year = comboYear.SelectedItem.ToString();
            string dataRoot = Path.Combine(Application.StartupPath, "Data", year);

            // Clear charts
            chartTemperature.Series.Clear();
            chartHumidity.Series.Clear();

            // Loop through each selected sensor
            foreach (string sensor in checkedListSensors.CheckedItems)
            {
                // Loop through each selected month
                foreach (string month in checkedListMonths.CheckedItems)
                {
                    counter++;

                    if (counter > 140)
                    {
                        MsgBox.Show("I can only draw 140 sensors", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string csvPath = Path.Combine(dataRoot, month, sensor + ".csv");

                    if (!File.Exists(csvPath))
                        continue;

                    // Create unique series name e.g. "Outside - April - 2026"
                    string seriesNameTemp = $"{sensor} - {month} - {year}";
                    string seriesNameHum = $"{sensor} - {month} - {year}";


                    var tempSeries = new Series(seriesNameTemp)
                    {
                        ChartType = SeriesChartType.Line,
                        XValueType = ChartValueType.Double,
                        Color = Color.FromName(ColourList.SelectColour(counter))
                    };

                    var humSeries = new Series(seriesNameHum)
                    {
                        ChartType = SeriesChartType.Line,
                        XValueType = ChartValueType.Double,
                        Color = Color.FromName(ColourList.SelectColour(counter))
                    };


                    // Load CSV into the series
                    LoadCsvIntoSeries(csvPath, tempSeries, humSeries);

                    // Add to charts
                    chartTemperature.Series.Add(tempSeries);
                    chartHumidity.Series.Add(humSeries);
                }
            }

            // Format X-axis for overlay mode (hours since start of month)
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


        private void comboYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkedListMonths.Items.Clear();

            string yearPath = Path.Combine(Application.StartupPath, "Data", comboYear.Text);

            if (Directory.Exists(yearPath))
            {
                var months = Directory.GetDirectories(yearPath)
                    .Select(Path.GetFileName)
                    .OrderBy(m => m);

                checkedListMonths.Items.AddRange(months.ToArray());
            }
        }

        private double HoursSinceStartOfMonth(DateTime timestamp)
        {
            var start = new DateTime(timestamp.Year, timestamp.Month, 1, 0, 0, 0);
            return (timestamp - start).TotalHours;
        }

    }
}
