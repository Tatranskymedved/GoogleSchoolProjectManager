using Google.Apis.Drive.v3.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSchoolProjectManager.Lib.Google.Drive
{
    public class GFolder : GFile
    {
        //public IEnumerable<GFile> Files => Items.Where(a => !(a is GFolder)); //{ get; set; } = new ObservableCollection<GFile>();
        public ObservableCollection<GFile> Files { get; set; } = new ObservableCollection<GFile>();
        //public IEnumerable<GFolder> Folders => Items.Where(a => a is GFolder).Cast<GFolder>();//{ get; set; } = new ObservableCollection<GFolder>();
        public ObservableCollection<GFolder> Folders { get; set; } = new ObservableCollection<GFolder>();
        //public ObservableCollection<GFile> Items { get; set; } = new ObservableCollection<GFile>();
        public ObservableCollection<GFile> Items => new ObservableCollection<GFile>(Folders.Cast<GFile>().Concat(Files));

        public GFolder() : base() { }
        public GFolder(File file) : base(file) { }

        public override string ToString()
        {
            return this.FileInfo.Name;// + ": " + string.Join(", ", Folders.Select(a => a.FileInfo.Name)) + " | " + string.Join(", ", Files.Select(a => a.FileInfo.Name));
        }

        public GFile findFile(Func<GFile, bool> predicate)
        {
            return FindFileInFolder(this, predicate);
        }

        /// <summary>
        /// Recursive function going through all the folders & trying to find the file by the selector
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public GFile FindFileInFolder(GFolder folder, Func<GFile, bool> predicate)
        {
            GFile result;
            result = folder.Files.FirstOrDefault(predicate);
            if (result != null) return result;

            foreach (GFolder item in folder.Folders)
            {
                GFile r = FindFileInFolder(item, predicate);
                if (r != null) return r;
            }

            return null;
        }
        public List<GFile> FindAllFilesDistinct(Func<GFile, bool> predicate)
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

        public GFile FindFileByName(string fileName) => findFile(a => a.FileInfo.Name.Equals(fileName));
        public GFile FindFileByNameContains(string lookupPhrase) => findFile(a => a.FileInfo.Name.Contains(lookupPhrase));
        public GFile FindFileById(string id) => findFile(a => a.FileInfo.Id.Equals(id));

        public List<GFile> FindFilesByName(string fileName) => FindAllFilesDistinct(a => a.FileInfo.Name.Equals(fileName));
        public List<GFile> FindFilesByNameContains(string lookupPhrase) => FindAllFilesDistinct(a => a.FileInfo.Name.Contains(lookupPhrase));

        public List<GFile> FindAllSpreadSheets() => FindAllFilesDistinct(a => MimeTypes.IsGoogleSpreadSheet(a.FileInfo.MimeType));
        public List<GFile> FindAllDocuments() => FindAllFilesDistinct(a => MimeTypes.IsGoogleDocument(a.FileInfo.MimeType));
        public List<GFile> FindAllFilesSelectedForUpdate() => FindAllFilesDistinct(a => a.IsSelectedForUpdate);
    }
}
