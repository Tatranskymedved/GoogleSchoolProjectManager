using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSchoolProjectManager.Lib.GoogleAPI.Sheets
{
    public class GCoordinate
    {
        public int SheetId { get; set; }
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }

        public GCoordinate() { }
        public GCoordinate(int sheetId, int rowIndex, int columnIndex)
        {
            SheetId = sheetId;
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
        }

        public GridCoordinate GetGridCoordinate()
        {
            return new GridCoordinate()
            {
                SheetId = SheetId,
                RowIndex = RowIndex,
                ColumnIndex = ColumnIndex
            };
        }
    }
}
