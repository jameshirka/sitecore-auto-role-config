# README #

This project has been created to simplify the process of renaming Sitecore configuration 

Switches:
 - -s Search provider to be used
 - -f CSV file to be read
 - -r The role header from the CSV
 - -w The website root folder
 - -v Validate the rename worked correclty

Sample usage: 
Rename configs for a Content Delivery site based on Azure search index with the CSV c:\temp\configsets\8.1.3.csv.

configrenamer.exe -s Azure -f c:\temp\configsets\8.1.3.csv -r "Content Delivery (CD)" -w c:\sites\website

Validate a Content Delivery site is setup correctly

configrenamer.exe -s Azure -f c:\temp\configsets\8.1.3.csv -r "Content Delivery (CD)" -w c:\sites\website -v


Some words of warning:

- 	It doesn’t handle changes in files, so you will still need to do the manual file changes – e.g. changes inside the web.config
- 	Always check the included CSV's against the latest version of the configuration spreadsheets on the doc site. We have seen a couple of instances where they have changed over time due to bug/support cases
- 	There is combined  CM/Rep/Proc config – this is best efforts only, it hasn’t been provided by the doc team and has been pulled together based on experience

Apart from that, feel free to pick it up and run with it. It is open source, so if you come across anything that doesn’t work or you think needs a change, please issue a pull request and we can merge it back in.