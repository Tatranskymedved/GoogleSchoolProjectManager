using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSchoolProjectManager.Lib.GoogleAPI.Sheets
{
    public class GRange
    {
        public int SheetId { get; set; }
        public int StartRow { get; set; }
        public int EndRow { get; set; }
        public int StartCol { get; set; }
        public int EndCol { get; set; }

        public GRange() { }
        public GRange(int sheetId, int startRow, int endRow, int startCol, int endCol)
        {
            SheetId = sheetId;
            StartRow = startRow;
            EndRow = endRow;
            StartCol = startCol;
            EndCol = endCol;
        }

        public GridRange GetGridRange()
        {
            return new GridRange()
            {
                SheetId = SheetId,
                StartRowIndex = StartRow,
                EndRowIndex = EndRow,
                StartColumnIndex = StartCol,
                EndColumnIndex = EndCol
            };
        }
    }
}
