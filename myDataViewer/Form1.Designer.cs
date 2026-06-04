
namespace myDataViewer
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabTemperature = new System.Windows.Forms.TabPage();
            this.chartTemperature = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabHumidity = new System.Windows.Forms.TabPage();
            this.chartHumidity = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panel2 = new System.Windows.Forms.Panel();
            this.checkedListMonths = new System.Windows.Forms.CheckedListBox();
            this.comboYear = new System.Windows.Forms.ComboBox();
            this.btn_close = new System.Windows.Forms.Button();
            this.btnLoadData = new System.Windows.Forms.Button();
            this.checkedListSensors = new System.Windows.Forms.CheckedListBox();
            this.chkbx_crosshairs = new System.Windows.Forms.CheckBox();
            this.chkbx_zoom = new System.Windows.Forms.CheckBox();
            this.btn_reset_zoom = new System.Windows.Forms.Button();
            this.checkedListYears = new System.Windows.Forms.CheckedListBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabTemperature.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartTemperature)).BeginInit();
            this.tabHumidity.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartHumidity)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1235, 661);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tabControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1229, 455);
            this.panel1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabTemperature);
            this.tabControl1.Controls.Add(this.tabHumidity);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1229, 455);
            this.tabControl1.TabIndex = 0;
            // 
            // tabTemperature
            // 
            this.tabTemperature.Controls.Add(this.chartTemperature);
            this.tabTemperature.Location = new System.Drawing.Point(4, 29);
            this.tabTemperature.Name = "tabTemperature";
            this.tabTemperature.Padding = new System.Windows.Forms.Padding(3);
            this.tabTemperature.Size = new System.Drawing.Size(1221, 422);
            this.tabTemperature.TabIndex = 0;
            this.tabTemperature.Text = "Temperature";
            this.tabTemperature.UseVisualStyleBackColor = true;
            // 
            // chartTemperature
            // 
            chartArea1.Name = "ChartArea1";
            this.chartTemperature.ChartAreas.Add(chartArea1);
            this.chartTemperature.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chartTemperature.Legends.Add(legend1);
            this.chartTemperature.Location = new System.Drawing.Point(3, 3);
            this.chartTemperature.Name = "chartTemperature";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartTemperature.Series.Add(series1);
            this.chartTemperature.Size = new System.Drawing.Size(1215, 416);
            this.chartTemperature.TabIndex = 0;
            this.chartTemperature.Text = "chart1";
            // 
            // tabHumidity
            // 
            this.tabHumidity.Controls.Add(this.chartHumidity);
            this.tabHumidity.Location = new System.Drawing.Point(4, 29);
            this.tabHumidity.Name = "tabHumidity";
            this.tabHumidity.Padding = new System.Windows.Forms.Padding(3);
            this.tabHumidity.Size = new System.Drawing.Size(1221, 422);
            this.tabHumidity.TabIndex = 1;
            this.tabHumidity.Text = "Humidity";
            this.tabHumidity.UseVisualStyleBackColor = true;
            // 
            // chartHumidity
            // 
            this.chartHumidity.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea2.Name = "ChartArea1";
            this.chartHumidity.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chartHumidity.Legends.Add(legend2);
            this.chartHumidity.Location = new System.Drawing.Point(6, 6);
            this.chartHumidity.Name = "chartHumidity";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chartHumidity.Series.Add(series2);
            this.chartHumidity.Size = new System.Drawing.Size(1209, 410);
            this.chartHumidity.TabIndex = 0;
            this.chartHumidity.Text = "chart1";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.checkedListYears);
            this.panel2.Controls.Add(this.btn_reset_zoom);
            this.panel2.Controls.Add(this.chkbx_zoom);
            this.panel2.Controls.Add(this.chkbx_crosshairs);
            this.panel2.Controls.Add(this.checkedListMonths);
            this.panel2.Controls.Add(this.comboYear);
            this.panel2.Controls.Add(this.btn_close);
            this.panel2.Controls.Add(this.btnLoadData);
            this.panel2.Controls.Add(this.checkedListSensors);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 464);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1229, 194);
            this.panel2.TabIndex = 1;
            // 
            // checkedListMonths
            // 
            this.checkedListMonths.CheckOnClick = true;
            this.checkedListMonths.FormattingEnabled = true;
            this.checkedListMonths.Location = new System.Drawing.Point(207, 27);
            this.checkedListMonths.Name = "checkedListMonths";
            this.checkedListMonths.Size = new System.Drawing.Size(192, 142);
            this.checkedListMonths.TabIndex = 5;
            // 
            // comboYear
            // 
            this.comboYear.FormattingEnabled = true;
            this.comboYear.Location = new System.Drawing.Point(864, 122);
            this.comboYear.Name = "comboYear";
            this.comboYear.Size = new System.Drawing.Size(121, 28);
            this.comboYear.TabIndex = 3;
            this.comboYear.SelectedIndexChanged += new System.EventHandler(this.comboYear_SelectedIndexChanged);
            // 
            // btn_close
            // 
            this.btn_close.Location = new System.Drawing.Point(1077, 133);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(142, 52);
            this.btn_close.TabIndex = 2;
            this.btn_close.Text = "Close";
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // btnLoadData
            // 
            this.btnLoadData.Location = new System.Drawing.Point(1077, 75);
            this.btnLoadData.Name = "btnLoadData";
            this.btnLoadData.Size = new System.Drawing.Size(142, 52);
            this.btnLoadData.TabIndex = 1;
            this.btnLoadData.Text = "Load Data";
            this.btnLoadData.UseVisualStyleBackColor = true;
            this.btnLoadData.Click += new System.EventHandler(this.btnLoadData_Click);
            // 
            // checkedListSensors
            // 
            this.checkedListSensors.CheckOnClick = true;
            this.checkedListSensors.FormattingEnabled = true;
            this.checkedListSensors.Location = new System.Drawing.Point(405, 27);
            this.checkedListSensors.Name = "checkedListSensors";
            this.checkedListSensors.Size = new System.Drawing.Size(386, 142);
            this.checkedListSensors.TabIndex = 0;
            // 
            // chkbx_crosshairs
            // 
            this.chkbx_crosshairs.AutoSize = true;
            this.chkbx_crosshairs.Location = new System.Drawing.Point(864, 29);
            this.chkbx_crosshairs.Name = "chkbx_crosshairs";
            this.chkbx_crosshairs.Size = new System.Drawing.Size(154, 24);
            this.chkbx_crosshairs.TabIndex = 6;
            this.chkbx_crosshairs.Text = "Show Crosshairs";
            this.chkbx_crosshairs.UseVisualStyleBackColor = true;
            this.chkbx_crosshairs.CheckedChanged += new System.EventHandler(this.chkbx_crosshairs_CheckedChanged);
            // 
            // chkbx_zoom
            // 
            this.chkbx_zoom.AutoSize = true;
            this.chkbx_zoom.Location = new System.Drawing.Point(864, 59);
            this.chkbx_zoom.Name = "chkbx_zoom";
            this.chkbx_zoom.Size = new System.Drawing.Size(138, 24);
            this.chkbx_zoom.TabIndex = 7;
            this.chkbx_zoom.Text = "Allow Zooming";
            this.chkbx_zoom.UseVisualStyleBackColor = true;
            this.chkbx_zoom.CheckedChanged += new System.EventHandler(this.chkbx_zoom_CheckedChanged);
            // 
            // btn_reset_zoom
            // 
            this.btn_reset_zoom.Location = new System.Drawing.Point(1077, 14);
            this.btn_reset_zoom.Name = "btn_reset_zoom";
            this.btn_reset_zoom.Size = new System.Drawing.Size(142, 52);
            this.btn_reset_zoom.TabIndex = 8;
            this.btn_reset_zoom.Text = "Reset Zoom";
            this.btn_reset_zoom.UseVisualStyleBackColor = true;
            this.btn_reset_zoom.Click += new System.EventHandler(this.btn_reset_zoom_Click);
            // 
            // checkedListYears
            // 
            this.checkedListYears.CheckOnClick = true;
            this.checkedListYears.FormattingEnabled = true;
            this.checkedListYears.Location = new System.Drawing.Point(9, 27);
            this.checkedListYears.Name = "checkedListYears";
            this.checkedListYears.Size = new System.Drawing.Size(192, 142);
            this.checkedListYears.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1235, 661);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "myDataViewer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabTemperature.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartTemperature)).EndInit();
            this.tabHumidity.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartHumidity)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabTemperature;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartTemperature;
        private System.Windows.Forms.TabPage tabHumidity;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartHumidity;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.Button btnLoadData;
        private System.Windows.Forms.CheckedListBox checkedListSensors;
        private System.Windows.Forms.ComboBox comboYear;
        private System.Windows.Forms.CheckedListBox checkedListMonths;
        private System.Windows.Forms.CheckBox chkbx_crosshairs;
        private System.Windows.Forms.CheckBox chkbx_zoom;
        private System.Windows.Forms.Button btn_reset_zoom;
        private System.Windows.Forms.CheckedListBox checkedListYears;
    }
}

