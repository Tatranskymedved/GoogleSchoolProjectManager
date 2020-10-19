using Google.Apis.Sheets;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using GoogleSchoolProjectManager.Lib.GoogleAPI.Sheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

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

        /// <summary>
        /// Returns list of grouped rows by Week. E.g. there is week that 3 subjects:
        /// this should return List with single Tuple&lt;int,int&gt; with index of first and third row (List(){Tuple&lt;1,3&gt;}).
        /// 
        /// There will be always first group on single line matched as where the search started.
        /// </summary>
        /// <param name="spreadsheetId"></param>
        /// <returns></returns>
        public GSheetReply GetKHSWeekBlocks(string spreadsheetId, string sheetName)
        {
            var result = new GSheetReply();

            var request = Service.Spreadsheets.Get(spreadsheetId);
            request.Ranges = UpdateKHSRequest.A1_WeekColumn;
            request.IncludeGridData = true;
            request.Fields = "sheets.properties.title" +
                            ",sheets.properties.sheetId" +
                            ",sheets.data.rowData.values.effectiveFormat.wrapStrategy" +
                            ",sheets.data.rowData.values.formattedValue" +
                            ",sheets.data.startRow" +
                            ",sheets.data.startColumn";

            var res1 = request.Execute();

            var blocks = new List<Tuple<int, int>>();

            foreach (var sheet in res1?.Sheets)
            {
                var title = sheet?.Properties?.Title ?? "";
                var sheetId = sheet?.Properties?.SheetId ?? 0;

                //If sheetName is defined, check for exact sheet, otherwise skip
                if (!string.IsNullOrEmpty(sheetName)
                    && !string.IsNullOrEmpty(title)
                    && (!title.ToLowerInvariant().Equals(sheetName.ToLowerInvariant())))
                {
                    continue;
                }

                result.Title = title;
                result.SheetId = sheetId;

                foreach (var gridData in sheet?.Data)
                {
                    if (gridData == null) continue;

                    //var startColumn = gridData.StartColumn ?? 0;
                    var startRow = gridData.StartRow ?? 0;
                    blocks.Add(new Tuple<int, int>(startRow, startRow));

                    if (gridData.RowData == null) continue;

                    var blockStart = -1;
                    var blockEnd = -1;

                    for (int rowIndex = 0; rowIndex < gridData.RowData.Count; rowIndex++)
                    {
                        RowData rowData = gridData.RowData[rowIndex];
                        var list = rowData?.Values;

                        if (list?.Count > 0)
                        {
                            var dateCell = list[0];
                            var wrapStrategy = dateCell?.EffectiveFormat?.WrapStrategy;
                            var currentRow = startRow + rowIndex + 1;
                            //var value = dateCell.FormattedValue;

                            if (wrapStrategy != null && wrapStrategy.ToUpperInvariant().Equals("WRAP"))
                            {
                                if (blockStart != -1 && blockEnd != -1) blocks.Add(new Tuple<int, int>(blockStart, blockEnd));

                                blockStart = currentRow;
                                blockEnd = blockStart;
                            }
                            else if (wrapStrategy != null && wrapStrategy.ToUpperInvariant().Equals("OVERFLOW_CELL"))
                            {
                                blockEnd += 1;
                            }
                            else
                            {
                                blocks.Add(new Tuple<int, int>(blockStart, blockEnd));
                                blockStart = blockEnd = -1;
                            }
                        }
                    }

                    if (blockStart != -1 && blockEnd != -1) blocks.Add(new Tuple<int, int>(blockStart, blockEnd));
                }
            }

            result.Blocks = blocks;

            return result;
        }

        public void UpdateSheets(UpdateKHSRequest khsRequest, Action<ProgressInfo> updateProgress, CancellationToken cancelToken)
        {
            if (khsRequest == null) throw new ArgumentNullException(nameof(khsRequest));

            var weekRange = khsRequest.GetFormattedDateRange();
            var requestBatchUpdate_AddData = khsRequest.SubjectGoalList.ToList().Select((b, c) =>
                new RowData()
                {
                    Values = new List<CellData>()
                    {
                        new CellData() { UserEnteredValue = new ExtendedValue() { StringValue = c == 0 ? weekRange : "" } },//, UserEnteredFormat = new CellFormat() { WrapStrategy = c == 0 ? "WRAP" : "OVERFLOW_CELL" } },
                        new CellData() { UserEnteredValue = new ExtendedValue() { StringValue = b.Subject }, UserEnteredFormat = new CellFormat() { TextFormat = new TextFormat() { Bold = true } } },
                        new CellData() { UserEnteredValue = new ExtendedValue() { StringValue = b.Goal } }
                    }
                }).ToList();

            for (int i = 0; i < khsRequest.Files.Count; i++)
            {
                if (cancelToken.IsCancellationRequested) return;

                var gFile = khsRequest.Files[i];
                updateProgress(new ProgressInfo() { FileName = gFile.Name, FileIndex = i + 1, FilesCount = khsRequest.Files.Count, Progress = (i / (float)khsRequest.Files.Count) });

                try
                {

                    //var sheetId = Service.Spreadsheets.Get(a?.FileInfo?.Id).Execute();

                    // Define request parameters.
                    var spreadsheetId = gFile?.FileInfo?.Id;
                    //var sheetId = "1UP2JrDkAeitSo6jBzskaKNU6d6D9Hdv_XkdxQv-rw_Q";
                    var worksheetInfo = this.GetKHSWeekBlocks(spreadsheetId, khsRequest.SheetName);
                    var blocks = worksheetInfo.Blocks;

                    if (blocks == null) throw new Exception($"{nameof(UpdateSheets)}: Received null value from {nameof(GetKHSWeekBlocks)} method.");
                    var pair = blocks[blocks.Count - 1];
                    var lastWeekRow = Math.Max(pair.Item1, pair.Item2);

                    var firstNewWeekRow = lastWeekRow;
                    var lastNewWeekRow = firstNewWeekRow + khsRequest.SubjectGoalList.Count;
                    var range = UpdateKHSRequest.A1_GetListRange(khsRequest.SheetName, UpdateKHSRequest.A1_GetRange_WeekSubjectGoalColumns(firstNewWeekRow, lastNewWeekRow));

                    var batchRequest = new BatchUpdateSpreadsheetRequest()
                    {
                        Requests = new List<Request>()
                    {
                        new Request()
                        {
                            UpdateCells = new UpdateCellsRequest()
                            {
                                Start = new GridCoordinate()
                                {
                                    SheetId = worksheetInfo.SheetId,
                                    RowIndex = firstNewWeekRow,
                                    ColumnIndex = 1
                                },
                                Fields = "userEnteredValue.stringValue"
                                        +",userEnteredFormat.textFormat.bold"
                                        ,
                                //Fields = "effectiveFormat.wrapStrategy" +
                                //        ",userEnteredValue.stringValue",
                                Rows = requestBatchUpdate_AddData
                            }
                        },
                        new Request()
                        {
                            MergeCells = new MergeCellsRequest()
                            {
                                MergeType = "MERGE_ALL",
                                Range = new GridRange()
                                {
                                    SheetId = worksheetInfo.SheetId,
                                    StartRowIndex = firstNewWeekRow,
                                    EndRowIndex = lastNewWeekRow,
                                    StartColumnIndex = 1,
                                    EndColumnIndex = 2
                                }
                            }
                        },
                        new Request()
                        {
                            UpdateCells = new UpdateCellsRequest()
                            {
                                Start = new GridCoordinate()
                                {
                                    SheetId = worksheetInfo.SheetId,
                                    RowIndex = firstNewWeekRow,
                                    ColumnIndex = 1
                                },
                                Fields = "userEnteredFormat.wrapStrategy"
                                       +",userEnteredFormat.textRotation.vertical"
                                       //+",userEnteredFormat.textRotation.angle" //Must be sent in next request
                                       +",userEnteredFormat.verticalAlignment"
                                       +",userEnteredFormat.horizontalAlignment"
                                       +",userEnteredFormat.textFormat.bold"
                                       +",userEnteredFormat.textFormat.fontSize",
                                Rows = new List<RowData>() {
                                    new RowData()
                                    {
                                        Values = new List<CellData>()
                                        {
                                            new CellData() { UserEnteredFormat = new CellFormat()
                                            {
                                                WrapStrategy = "WRAP",
                                                TextRotation = new TextRotation()
                                                {
                                                    Vertical = true
                                                },
                                                VerticalAlignment = "MIDDLE",
                                                HorizontalAlignment = "CENTER",
                                                TextFormat = new TextFormat()
                                                {
                                                    Bold = true,
                                                    FontSize = 8,
                                                }
                                            } }
                                        }
                                    }
                                }
                            }
                        },
                        new Request()
                        {
                            UpdateCells = new UpdateCellsRequest()
                            {
                                Start = new GridCoordinate()
                                {
                                    SheetId = worksheetInfo.SheetId,
                                    RowIndex = firstNewWeekRow,
                                    ColumnIndex = 1
                                },
                                Fields = "userEnteredFormat.textRotation.angle",
                                Rows = new List<RowData>() {
                                    new RowData()
                                    {
                                        Values = new List<CellData>()
                                        {
                                            new CellData() { UserEnteredFormat = new CellFormat()
                                            {
                                                TextRotation = new TextRotation()
                                                {
                                                    Angle = 90
                                                }
                                            } }
                                        }
                                    }
                                }
                            }
                        },
                        GUpdateBorders.GenerateRequest(GUpdateBordersType.Outer, new GRange(worksheetInfo.SheetId, firstNewWeekRow, lastNewWeekRow, 1, 2)),
                        GUpdateBorders.GenerateRequest(GUpdateBordersType.Outer, new GRange(worksheetInfo.SheetId, firstNewWeekRow, lastNewWeekRow, 2, 3)),
                        GUpdateBorders.GenerateRequest(GUpdateBordersType.Outer, new GRange(worksheetInfo.SheetId, firstNewWeekRow, lastNewWeekRow, 3, 4)),
                        GUpdateBorders.GenerateRequest(GUpdateBordersType.Outer, new GRange(worksheetInfo.SheetId, firstNewWeekRow, lastNewWeekRow, 4, 5)),
                        GUpdateBorders.GenerateRequest(GUpdateBordersType.Outer, new GRange(worksheetInfo.SheetId, firstNewWeekRow, lastNewWeekRow, 5, 6)),
                        GUpdateBorders.GenerateRequest(GUpdateBordersType.Outer, new GRange(worksheetInfo.SheetId, firstNewWeekRow, lastNewWeekRow, 6, 7)),
                        GUpdateBorders.GenerateRequest(GUpdateBordersType.Outer, new GRange(worksheetInfo.SheetId, firstNewWeekRow, lastNewWeekRow, 7, 8)),
                        GUpdateBorders.GenerateRequest(GUpdateBordersType.Outer, new GRange(worksheetInfo.SheetId, firstNewWeekRow, lastNewWeekRow, 8, 9)),
                        GUpdateBorders.GenerateRequest(GUpdateBordersType.Outer, new GRange(worksheetInfo.SheetId, firstNewWeekRow, lastNewWeekRow, 9, 10)),
                        GUpdateBorders.GenerateRequest(GUpdateBordersType.Outer, new GRange(worksheetInfo.SheetId, firstNewWeekRow, lastNewWeekRow, 10, 11)),
                        GUpdateBorders.GenerateRequest(GUpdateBordersType.InnerRows, new GRange(worksheetInfo.SheetId, firstNewWeekRow, lastNewWeekRow, 1, 11)),
                        GCellRequestFactory.GenerateRepeactCellRequest(
                            new GRange(worksheetInfo.SheetId, firstNewWeekRow, lastNewWeekRow, 4, 9),
                            new GCell()
                                .AddFormat(GCellFormat.BackgroundColorRed, 1f)
                                .AddFormat(GCellFormat.BackgroundColorGreen, 242/255f)
                                .AddFormat(GCellFormat.BackgroundColorBlue, 204/255f)
                                ),
                        }
                    };
                    var update = Service.Spreadsheets.BatchUpdate(batchRequest, spreadsheetId);
                    update.Execute();

                }
                catch (Exception ex)
                {
                    ;
                }
            };
        }


        public void GetSomeDataSample()
        {
            throw new NotImplementedException();
            //SpreadsheetsResource.ValuesResource.GetRequest request = Service.Spreadsheets.Values.Get(sheetId, range);

            //ValueRange response = request.Execute();
            //var values = response.Values;
            //var res = new List<KHSWeekSubjectGoalReply>();
            //if (values != null && values.Count > 0)
            //{
            //    string week = "", subject = "", goal = "";

            //    foreach (var row in values)
            //    {
            //        if (row.Count > 0) week = row?[0]?.ToString(); else week = "";
            //        if (row.Count > 1) subject = row?[1]?.ToString(); else subject = "";
            //        if (row.Count > 2) goal = row?[2]?.ToString(); else goal = "";

            //        var reply = new KHSWeekSubjectGoalReply()
            //        {
            //            Week = week,
            //            Subject = subject,
            //            Goal = goal
            //        };
            //        res.Add(reply);
            //    }
            //}
            //;
            //return;
        }
    }

}