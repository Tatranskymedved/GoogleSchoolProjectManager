using Google.Apis.Drive.v3.Data;
using GoogleSchoolProjectManager.Lib.Google.Drive;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSchoolProjectManager.Lib.GoogleAPI.Test
{
    public class FakeGTree : GTree
    {
        private List<File> TestFiles = new List<File>()
        {
            new File() { Name = "File1", Id = "File1", DriveId = "", Parents = new List<string>(){"Folder2"}, MimeType = MimeTypes.GoogleSpreadSheet },
            new File() { Name = "File2", Id = "File2", DriveId = "", Parents = new List<string>(){"Folder1"}, MimeType = MimeTypes.GoogleSpreadSheet },
            new File() { Name = "File3", Id = "File3", DriveId = "", Parents = new List<string>(){"Folder2"}, MimeType = MimeTypes.GoogleSpreadSheet },
            new File() { Name = "File4", Id = "File4", DriveId = "", Parents = new List<string>(){""}, MimeType = MimeTypes.GoogleSpreadSheet },
            new File() { Name = "File5", Id = "File5", DriveId = "", Parents = new List<string>(){""}, MimeType = MimeTypes.GoogleSpreadSheet },
            new File() { Name = "File6", Id = "File6", DriveId = "", Parents = new List<string>(){"Folder4"}, MimeType = MimeTypes.GoogleSpreadSheet },
            new File() { Name = "File7", Id = "File7", DriveId = "", Parents = new List<string>(){"Folder4"}, MimeType = MimeTypes.GoogleSpreadSheet },

            new File() { Name = "Folder1", Id = "Folder1", DriveId = "", Parents = new List<string>(){"Folder2"}, MimeType = MimeTypes.GoogleFolder },
            new File() { Name = "Folder2", Id = "Folder2", DriveId = "", Parents = new List<string>(){""}, MimeType = MimeTypes.GoogleFolder },
            new File() { Name = "Folder3", Id = "Folder3", DriveId = "", Parents = new List<string>(){""}, MimeType = MimeTypes.GoogleFolder },
            new File() { Name = "Folder4", Id = "Folder4", DriveId = "", Parents = new List<string>(){""}, MimeType = MimeTypes.GoogleFolder },


        };

        public FakeGTree()
        {
            this.Init(TestFiles);
        }
    }
}
