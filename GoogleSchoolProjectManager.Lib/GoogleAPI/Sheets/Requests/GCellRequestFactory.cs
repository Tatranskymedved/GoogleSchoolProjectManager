using Google.Apis.Sheets.v4.Data;
using GoogleSchoolProjectManager.Lib.GoogleAPI.Sheets.POCOs;
using System;
using System.Collections.Generic;

namespace GoogleSchoolProjectManager.Lib.GoogleAPI.Sheets.Requests
{
    public static class GCellRequestFactory
    {
        public static Request GenerateRepeatCellRequest(GRange range, GCell cell)
        {
            return new Request()
            {
                RepeatCell = new RepeatCellRequest()
                {
                    Cell = cell.GetCellData(),
                    Fields = cell.GetFields(),
                    Range = range.GetGridRange()
                }
            };
        }

        public static Request GenerateRepeatCellRequest(GCoordinate start, GRowList rows)
        {
            return new Request()
            {
                UpdateCells = new UpdateCellsRequest()
                {
                    Rows = rows.GetRowDataList(),
                    Fields = rows.GetFields().GetFieldsString(),
                    Start = start.GetGridCoordinate(),
                }
            };
        }

        public static Request GenerateUpdateCellsRequest(GRowList rows, GCoordinate gCoordinate)
        {
            return new Request()
            {
                UpdateCells = new UpdateCellsRequest()
                {
                    Start = gCoordinate.GetGridCoordinate(),
                    Fields = rows.GetFields().GetFieldsString(),
                    Rows = rows.GetRowDataList()
                    //"userEnteredValue.stringValue"
                    //                           + ",userEnteredFormat.textFormat.bold",
                    //Rows = requestBatchUpdate_AddData
                }
            };
        }
    }
}
