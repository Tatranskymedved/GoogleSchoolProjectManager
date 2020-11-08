using Google.Apis.Sheets.v4.Data;
using System.Collections.Generic;
using System.Linq;

namespace GoogleSchoolProjectManager.Lib.GoogleAPI.Sheets.POCOs
{
    public class GRow : List<GCell>
    {
        public RowData GetRowData()
        {
            return new RowData()
            {
                Values = this.Select(a => a.GetCellData()).ToList()
            };
        }

        public GFields GetFields() => this.Aggregate(new GFields(), (result, item) =>
        {
            result.UnionWith(item.Fields);
            return result;
        });
    }
}
