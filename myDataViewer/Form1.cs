using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

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
        }

        private void btnLoadData_Click(object sender, EventArgs e)
        {
            if (comboYear.SelectedItem == null || comboMonth.SelectedItem == null)
            {
                MessageBox.Show("Please select a year and month.");
                return;
            }

            string basePath = Path.Combine(Application.StartupPath, "Data", comboYear.Text, comboMonth.Text);

            chartTemperature.Series.Clear();
            chartHumidity.Series.Clear();

            foreach (string sensor in checkedListSensors.CheckedItems)
            {
                string csvPath = Path.Combine(basePath, sensor + ".csv");

                if (!File.Exists(csvPath))
                    continue;

                var tempSeries = new Series(sensor)
                {
                    ChartType = SeriesChartType.Line,
                    XValueType = ChartValueType.DateTime
                };

                var humSeries = new Series(sensor)
                {
                    ChartType = SeriesChartType.Line,
                    XValueType = ChartValueType.DateTime
                };

                LoadCsvIntoSeries(csvPath, tempSeries, humSeries);

                chartTemperature.Series.Add(tempSeries);
                chartHumidity.Series.Add(humSeries);

                //Add date time to graph. May get too busy so we comment out
                //chartTemperature.ChartAreas[0].AxisX.LabelStyle.Format = "dd/MM HH:mm";
                //chartHumidity.ChartAreas[0].AxisX.LabelStyle.Format = "dd/MM HH:mm";

                // As there are lots of days in a month angle the text on x-axis to show it all
                chartTemperature.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
                chartTemperature.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
                chartTemperature.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;

                chartHumidity.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
                chartHumidity.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
                chartHumidity.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;

            }
        }


        //private void LoadCsvIntoSeries(string filePath, Series tempSeries, Series humSeries)
        //{
        //    foreach (var line in File.ReadLines(filePath).Skip(1))
        //    {
        //        var parts = line.Split(',');
        //        if (parts.Length < 4)
        //            continue;

        //        string dateStr = parts[0];
        //        string timeStr = parts[1];


        //        if (!DateTime.TryParse($"{dateStr} {timeStr}", out DateTime timestamp))
        //            continue;

        //        if (double.TryParse(parts[2], NumberStyles.Any, CultureInfo.InvariantCulture, out double temp))
        //        {
        //            tempSeries.Points.AddXY(timestamp, temp);
        //        }

        //        if (double.TryParse(parts[3], NumberStyles.Any, CultureInfo.InvariantCulture, out double hum))
        //        {
        //            humSeries.Points.AddXY(timestamp, hum);
        //        }
        //    }
        //}

        private void LoadCsvIntoSeries(string filePath, Series tempSeries, Series humSeries)
        {
            foreach (var line in File.ReadLines(filePath).Skip(1))
            {
                var parts = line.Split(',');
                if (parts.Length < 4)
                    continue;

                string combined = parts[0] + " " + parts[1];

                //here we define how the date and time appears in the csv file
                if (!DateTime.TryParseExact(
                        combined,
                        "MM/dd/yyyy HH:mm:ss",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out DateTime timestamp))
                {
                    continue;
                }

                if (double.TryParse(parts[2], NumberStyles.Any, CultureInfo.InvariantCulture, out double temp))
                    tempSeries.Points.AddXY(timestamp, temp);

                if (double.TryParse(parts[3], NumberStyles.Any, CultureInfo.InvariantCulture, out double hum))
                    humSeries.Points.AddXY(timestamp, hum);
            }
        }



        private void btn_close_Click(object sender, EventArgs e)
        {
            Close();
        }


        private void comboYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboMonth.Items.Clear();

            string yearPath = Path.Combine(Application.StartupPath, "Data", comboYear.Text);

            if (Directory.Exists(yearPath))
            {
                var months = Directory.GetDirectories(yearPath)
                    .Select(Path.GetFileName)
                    .OrderBy(m => m);

                comboMonth.Items.AddRange(months.ToArray());

                comboMonth.SelectedIndex = 0;
            }
        }

    }
}
