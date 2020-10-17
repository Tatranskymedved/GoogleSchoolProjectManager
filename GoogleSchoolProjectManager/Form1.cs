using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using GoogleSchoolProjectManager.Lib.Google;
using GoogleSchoolProjectManager.Lib.Google.Drive;

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


            using (var con = new GoogleConnector())
            {
                Do(con);

                var man = new GDriveManager(con);
                man.DriveName = diskName;
                var tree = man.GetTree();
                Add(new GFolder()
                {
                    Files = tree.findAllSpreadSheets()
                });
                
                //Add(tree);
            };
        }

        private void Add(GFolder collection, string prefix = "")
        {
            var sortedFolders = collection.Folders.OrderBy(a => a.FileInfo.Name);
            foreach (GFolder item in sortedFolders)
            {
                a(prefix + item.ToString());
                Add(item, prefix + "------------");
            }

            var sortedFiles = collection.Files.OrderBy(a => a.FileInfo.Name);
            foreach (GFile item in sortedFiles)
            {
                a(prefix + item.ToString());
            }
        }

        private void a(string child)
        {
            treeView1.Nodes.Add(child.ToString());
        }

        public void Do(GoogleConnector con)
        {
            //con.Sheets.Spreadsheets.Get();

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
