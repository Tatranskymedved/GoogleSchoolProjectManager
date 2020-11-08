using Google.Apis.Sheets.v4.Data;
using System.Collections.Generic;
using System.Linq;

namespace GoogleSchoolProjectManager.Lib.GoogleAPI.Sheets.POCOs
{
    public class GRowList : List<GRow>
    {
        public IList<RowData> GetRowDataList() => this.Select(a => a.GetRowData()).ToList();

        public GFields GetFields() => this.Aggregate(new GFields(), (result, item) =>
        {
            result.UnionWith(item.GetFields());
            return result;
        });
    }
}
