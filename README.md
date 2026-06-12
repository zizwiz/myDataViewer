# myDataViewer

<p align="center">
<img src=myDataViewer/images/icon.png alt="Icon"/>
</p>

This apps main task is to allow you to visualise temperature and humidity data that has been recorded into a comma separated value (csv) file. 

### How it works.
 
Open the app and you will have the following GUI.

<p align="center">
<img src=myDataViewer/images/initialise.png alt="Initial UI"/>
</p>

Buttons and other UI Controls will appear or disappear depending on the choices you make as you work with the app.

#### CSV Data

The folder structure where the data is stored and how it is named is important. The app starts to look under the folder Data into subfolders for the year, then under the year into subfolders for the months (Sentence case for months). Under the months are the csv files for that month. This is illustrated below:

![Filestructure](myDataViewer/images/filestructure.png)

The layout of the csv file is shown below:

![Csv File Structure](myDataViewer/images/csv_file_structure.png)

### Open Data Location

Click on the “Open Data Location” button and choose the folder where all your data is stored. The app will now search that folder and list with checkboxes all the year folders  

<p align="center">
<img src=myDataViewer/images/open_years.png alt="Open Years"/>
</p>

When you check the box next to the year you want the app searches the year folder and will list the months.

<p align="center">
<img src=myDataViewer/images/ope_months.png alt="Open Months"/>
</p>

When you check the box next to the month you want the app searches the month folder and will list the csv files it finds in that folder.

<p align="center">
<img src=myDataViewer/images/show_sensors.png alt="Show Sensors"/>
</p>

Now you can check the sensors that you want to graph. When you have made your selection click the button called “Load Data”.

<p align="center">
<img src=myDataViewer/images/load_data.png alt="Load Data"/>
</p>

This will now graph the Temperature and Humidity data. The graphs are in two separate tabs; click on the tab to view the data you want to view.

<p align="center">
<img src=myDataViewer/images/loaded_data.png alt="Loaded Data"/>
</p>

The graph will only allow 140 different sensor plots at the same time although with that many plots the data will be very dense and may just look like a black splodge.
 
### A note on the Horizontal X-Axis
This axis shows is used to plot the date and time. To allow for comparison of data from months of different lengths or part months with data from full months the date and time is converted into time from start of month. 

### Tooltips
If you put your mouse over a charted line and it finds a charted point then you will get a tooltip telling you the value and the date and time of that point.

<p align="center">
<img src=myDataViewer/images/tooltip.png alt="Loaded Data"/>
</p>











