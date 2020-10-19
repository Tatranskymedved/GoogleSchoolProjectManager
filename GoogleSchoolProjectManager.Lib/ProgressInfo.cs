using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSchoolProjectManager.Lib
{
    public class ProgressInfo
    {
        public float Progress { get; set; } = 0.0f;
        public int FileIndex { get; set; } = 0;
        public int FilesCount { get; set; } = 0;
        public string FileName { get; set; } = "";
    }
}
