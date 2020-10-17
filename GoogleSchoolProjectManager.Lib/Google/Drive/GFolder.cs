using Google.Apis.Drive.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSchoolProjectManager.Lib.Google.Drive
{
    public class GFolder : GFile
    {
        public List<GFile> Files { get; set; } = new List<GFile>();
        public List<GFolder> Folders { get; set; } = new List<GFolder>();

        public GFolder() : base() { }
        public GFolder(File file) : base(file) { }

        public override string ToString()
        {
            return this.FileInfo.Name;// + ": " + string.Join(", ", Folders.Select(a => a.FileInfo.Name)) + " | " + string.Join(", ", Files.Select(a => a.FileInfo.Name));
        }

        public GFile findFile(Func<GFile, bool> predicate)
        {
            return findFileInFolder(this, predicate);
        }

        /// <summary>
        /// Recursive function going through all the folders & trying to find the file by the selector
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public GFile findFileInFolder(GFolder folder, Func<GFile, bool> predicate)
        {
            GFile result;
            result = folder.Files.FirstOrDefault(predicate);
            if (result != null) return result;

            foreach (GFolder item in folder.Folders)
            {
                GFile r = findFileInFolder(item, predicate);
                if (r != null) return r;
            }

            return null;
        }
        public List<GFile> findAllFilesDistinct(Func<GFile, bool> predicate)
        {
            var result = new List<GFile>();
            GFile lastResult;
            do
            {
                lastResult = findFile(a => predicate(a) && !result.Contains(a));
                if (lastResult != null) result.Add(lastResult);
            } while (lastResult != null);

            return result;
        }

        public GFile findFileByName(string fileName) => findFile(a => a.FileInfo.Name.Equals(fileName));
        public GFile findFileByNameContains(string lookupPhrase) => findFile(a => a.FileInfo.Name.Contains(lookupPhrase));
        public GFile findFileById(string id) => findFile(a => a.FileInfo.Id.Equals(id));

        public List<GFile> findFilesByName(string fileName) => findAllFilesDistinct(a => a.FileInfo.Name.Equals(fileName));
        public List<GFile> findFilesByNameContains(string lookupPhrase) => findAllFilesDistinct(a => a.FileInfo.Name.Contains(lookupPhrase));

        public List<GFile> findAllSpreadSheets() => findAllFilesDistinct(a => MimeTypes.IsGoogleSpreadSheet(a.FileInfo.MimeType));
        public List<GFile> findAllDocuments() => findAllFilesDistinct(a => MimeTypes.IsGoogleDocument(a.FileInfo.MimeType));
    }
}
