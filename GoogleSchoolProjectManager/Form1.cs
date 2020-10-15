using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Docs.v1;
using Google.Apis.Docs.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using GoogleSchoolProjectManager.Google;
using GoogleSchoolProjectManager.Google.Drive;

namespace GoogleSchoolProjectManager
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            const string diskName = "OSTODISK";
            string result = "";
            using (var con = new GoogleConnector())
            {
                var teamDrivesList = con.Drive.Teamdrives.List().Execute();
                var drivesList = con.Drive.Drives.List().Execute().Drives;
                var teamDrive = drivesList.FirstOrDefault(a => a.Name.ToUpperInvariant().Contains(diskName.ToUpperInvariant()));
                if(teamDrive != null)
                {

                    var fileRequest = con.Drive.Files.List();
                    fileRequest.IncludeItemsFromAllDrives = true;
                    fileRequest.SupportsTeamDrives = true;
                    fileRequest.SupportsAllDrives = false;
                    //fileRequest.Q = ";
                    var fileResult = fileRequest.Execute();

                    var tree = new GTree(fileResult.Files);
                    //con.Drive.Drives.Get(teamDrive.Id).Execute();
                    


                }
                    //List().Execute();
                var filesList = con.Drive.Files.List().Execute();

                //filesList.Files.First(a => a.MimeType == )

                //var repliesList = con.Drive.;

                ;
                //var drive = new GDriveManager(con);
                //result = string.Join(", ", drive.GetFileNames());
            };

            MessageBox.Show(result);

        }


        //// Pass in your data as a list of a list (2-D lists are equivalent to the 2-D spreadsheet structure)
        //public string UpdateData(List<IList<object>> data)
        //{
        //    String range = "My Tab Name!A1:Y";
        //    string valueInputOption = "USER_ENTERED";

        //    // The new values to apply to the spreadsheet.
        //    var updateData = new List<Google.Apis.Sheets.v4.Data.ValueRange>();
        //    var dataValueRange = new Google.Apis.Sheets.v4.Data.ValueRange();
        //    dataValueRange.Range = range;
        //    dataValueRange.Values = data;
        //    updateData.Add(dataValueRange);

        //    var requestBody = new Google.Apis.Sheets.v4.Data.BatchUpdateValuesRequest();
        //    requestBody.ValueInputOption = valueInputOption;
        //    requestBody.Data = updateData;

        //    var request = SheetsService.Spreadsheets.Values.BatchUpdate(requestBody, _spreadsheetId);

        //    var response = request.Execute();
        //    // Data.BatchUpdateValuesResponse response = await request.ExecuteAsync(); // For async 

        //    return JsonConvert.SerializeObject(response);
        //}

        //UserCredential credential;

        //using (var stream =
        //    new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
        //{
        //    // The file token.json stores the user's access and refresh tokens, and is created
        //    // automatically when the authorization flow completes for the first time.
        //    string credPath = "token.json";
        //    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
        //        GoogleClientSecrets.Load(stream).Secrets,
        //        Scopes,
        //        "user",
        //        CancellationToken.None,
        //        new FileDataStore(credPath, true)).Result;
        //    Console.WriteLine("Credential file saved to: " + credPath);
        //}

        //// Create Google Sheets API service.
        //var service = new SheetsService(new BaseClientService.Initializer()
        //{
        //    HttpClientInitializer = credential,
        //    ApplicationName = ApplicationName,
        //});

        //// Define request parameters.
        //String spreadsheetId = "1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms";
        //String range = "Class Data!A2:E";
        //SpreadsheetsResource.ValuesResource.GetRequest request =
        //        service.Spreadsheets.Values.Get(spreadsheetId, range);

        //// Prints the names and majors of students in a sample spreadsheet:
        //// https://docs.google.com/spreadsheets/d/1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms/edit
        //ValueRange response = request.Execute();
        //IList<IList<Object>> values = response.Values;
        //if (values != null && values.Count > 0)
        //{
        //    Console.WriteLine("Name, Major");
        //    foreach (var row in values)
        //    {
        //        // Print columns A and E, which correspond to indices 0 and 4.
        //        Console.WriteLine("{0}, {1}", row[0], row[4]);
        //    }
        //}
        //else
        //{
        //    Console.WriteLine("No data found.");
        //}
        //Console.Read();


    }
}
