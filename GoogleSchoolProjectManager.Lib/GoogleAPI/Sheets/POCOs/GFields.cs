using System.Collections.Generic;

namespace GoogleSchoolProjectManager.Lib.GoogleAPI.Sheets.POCOs
{
    public class GFields : HashSet<string>
    {
        public string GetFieldsString()
        {
            return string.Join(",", this);
        }

        public override string ToString()
        {
            return GetFieldsString();
        }
    }
}
