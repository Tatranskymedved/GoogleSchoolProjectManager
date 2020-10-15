using Google.Apis.Drive.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSchoolProjectManager.Google.Drive
{
    public class GFolder : GFile
    {
        public List<GFile> Files { get; set; } = new List<GFile>();
        public List<GFolder> Folders { get; set; } = new List<GFolder>();

        public GFolder() : base() { }
        public GFolder(File file) : base(file) { }

        public override string ToString()
        {
            return this.FileInfo.Name + ": " + string.Join(", ", Folders.Select(a => a.FileInfo.Name)) + " | " + string.Join(", ", Files.Select(a => a.FileInfo.Name));
        }
    }
}
