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





