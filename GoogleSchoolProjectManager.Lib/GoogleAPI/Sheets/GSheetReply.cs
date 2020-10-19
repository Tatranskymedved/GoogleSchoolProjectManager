using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSchoolProjectManager.Lib.GoogleAPI.Sheets
{
    public class GSheetReply
    {
        public List<Tuple<int, int>> Blocks { get; set; } = new List<Tuple<int, int>>();
        public string Title { get; set; }
        public int SheetId { get; set; }

    }
}
