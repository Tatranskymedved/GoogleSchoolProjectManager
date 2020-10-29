# GoogleSchoolProjectManager
This project was created for purpose of primary school (in czech rep.) to allow school to simply copy files (templates) located at Google Drive + multi-file edit Google Sheets to invoke same changes over several documents. Automation of labour.

----
# Features supported

- Multi-language
- Downloading list of files/folders from drive
- Copying file into several files with different names & same content
- Updating existing Google Sheets document with data/format
- Metro style

----
# Build
1. Create/use Google APIs' account and add OAuth 2.0 authentication. Get `credentials.json` from it and save it to `GoogleSchoolProjectManager\GoogleSchoolProjectManager.Lib\` folder.
2. Build it via Visual Studio 2019 (or other applicable version)

----
# App screenshots

<div align="center">
Overview once list of folders/files received
<img alt="appScreenshot_Overview" src=".\docs\readme\img\MainWindow_unselectedFolder.PNG">

Prepared for copying a template into multiple files
<img alt="appScreenshot_copyTemplate" src=".\docs\readme\img\MainWindow_prepareForTemplateCopy.PNG">

Prepared for updating existing Google Sheets
<img alt="appScreenshot_copyTemplate" src=".\docs\readme\img\MainWindow_prepareForUpdateDocuments.PNG">
</div>

----
# License

Copyright © Jan Urbanec and contributors.

GoogleSchoolProjectManager is provided as-is under the MIT license. For more information see LICENSE.

----
# Libs used

- Google APIs (Drive,Sheets,Docs)
- Newtonsoft.JSON
- Ninject
- MahApps.Metro
- ControlzEx
- Microsoft.Xaml.Behaviors.Wpf
