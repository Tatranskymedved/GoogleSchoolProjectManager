using Google.Apis.Sheets.v4.Data;
using GoogleSchoolProjectManager.Lib.GoogleAPI.Sheets.POCOs;
using System.Collections.Generic;

namespace GoogleSchoolProjectManager.Lib.GoogleAPI.Sheets.Requests
{
    public enum GMergeType
    {
        MergeAll = 0,
        MergeCols = 1,
        MergeRows = 2
    }

    public static class GMergeCellsFactory
    {
        public static Request GenerateRequest(GMergeType type, GRange range)
        {
            return new Request()
            {
                MergeCells = new MergeCellsRequest()
                {
                    MergeType = GetMergeType(type),
                    Range = range.GetGridRange()
                }
            };
        }

        public static string GetMergeType(GMergeType type)
        {
            string result;
            if (MergeDictionary.TryGetValue(type, out result))
                return result;

            return "";
        }

        //Merge types:
        //
        //MERGE_ALL                 Create a single merge from the range.
        //MERGE_COLUMNS             Create a merge for each column in the range.
        //MERGE_ROWS                Create a merge for each row in the range.
        private static Dictionary<GMergeType, string> MergeDictionary = new Dictionary<GMergeType, string>
        {
            { GMergeType.MergeAll, "MERGE_ALL" },
            { GMergeType.MergeCols, "MERGE_COLUMNS" },
            { GMergeType.MergeRows, "MERGE_ROWS" },
        };
    }
}
