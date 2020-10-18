using Google.Apis.Sheets;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using GoogleSchoolProjectManager.Lib.GoogleAPI.Sheets;
using System;
using System.Collections.Generic;

namespace GoogleSchoolProjectManager.Lib.Google
{
    public class GSheetsManager
    {
        public SheetsService Service { get; set; }

        public GSheetsManager(SheetsService service)
        {
            this.Service = service;
        }

        public GSheetsManager(GoogleConnector googleConnector)
        {
            this.Service = googleConnector.Sheets;
        }

        public void UpdateSheets(UpdateKHSRequest khsRequest)
        {
            if (khsRequest == null) throw new ArgumentNullException(nameof(khsRequest));

            khsRequest.Files.ForEach(a =>
            {
                //var sheetId = Service.Spreadsheets.Get(a?.FileInfo?.Id).Execute();

                // Define request parameters.
                var sheetId = a?.FileInfo?.Id;
                var range = string.Format("{0}!{1}", khsRequest.SheetName, khsRequest.WeekSubjectGoalColumns);
                SpreadsheetsResource.ValuesResource.GetRequest request = Service.Spreadsheets.Values.Get(sheetId, range);
                var b = Service.Spreadsheets.Get(sheetId).Execute();

                ValueRange response = request.Execute();
                var values = response.Values;
                var res = new List<KHSWeekSubjectGoalReply>();
                if (values != null && values.Count > 0)
                {
                    string week = "", subject = "", goal = "";

                    foreach (var row in values)
                    {
                        if (row.Count > 0) week = row?[0]?.ToString(); else week = "";
                        if (row.Count > 1) subject = row?[1]?.ToString(); else subject = "";
                        if (row.Count > 2) goal = row?[2]?.ToString(); else goal = "";

                        var reply = new KHSWeekSubjectGoalReply()
                        {
                            Week = week,
                            Subject = subject,
                            Goal = goal
                        };
                        res.Add(reply);
                    }
                }
                ;
            });
        }
    }
}