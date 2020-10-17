using Google.Apis.Drive.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSchoolProjectManager.Lib.Google.Drive
{
    public class GFile
    {
        public File FileInfo { get; set; }

        public GFolder GParent { get; set; }

        public GFile() { }
        public GFile(File file) { FileInfo = file; }

        public bool IsParentOf(GFile possibleChild)
        {
            var parents = possibleChild?.FileInfo?.Parents;

            if (parents == null || parents.Count > 1 || parents.Count == 0)
            {
                return false;
            }

            return this.FileInfo.Id == parents.First();
        }

        public override string ToString()
        {
            return this.FileInfo.Name + ": " + MimeTypes.GetMimeTypeDescription(this);
        }
    }
}
