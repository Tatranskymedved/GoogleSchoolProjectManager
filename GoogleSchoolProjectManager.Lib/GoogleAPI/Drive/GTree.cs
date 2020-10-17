using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Drive.v3.Data;

namespace GoogleSchoolProjectManager.Lib.Google.Drive
{
    public class GTree : GFolder
    {
        public GTree() : base() { }

        public GTree(IEnumerable<File> files)
            : base()
        {
            this.Init(files);
        }

        public void Init(IEnumerable<File> files)
        {
            var folderList = new List<GFolder>();
            var fileList = new List<GFile>();

            foreach (var file in files)
            {
                if (MimeTypes.IsGoogleFolder(file.MimeType))
                {
                    folderList.Add(new GFolder(file));
                }
                else
                {
                    fileList.Add(new GFile(file));
                }
            }

            TriageFolders(folderList);
            TriageFiles(fileList, folderList);

            this.Folders = new ObservableCollection<GFolder>(folderList.Where(a => a.GParent == null));
            this.Files = new ObservableCollection<GFile>(fileList.Where(a => a.GParent == null));

            OnPropertyChanged(nameof(Folders));
            OnPropertyChanged(nameof(Files));
            OnPropertyChanged(nameof(Items));
        }

        public void TriageFolders(List<GFolder> folderList)
        {
            foreach (GFolder folder in folderList)
            {
                if (folder.FileInfo.Parents?.Count == 1)
                {
                    var parent = folderList.FirstOrDefault(a => a.IsParentOf(folder));
                    if (parent != null)
                    {
                        folder.GParent = parent;
                        if (!parent.Folders.Contains(folder))
                        {
                            //parent.Items.Add(folder);
                            parent.Folders.Add(folder);
                        }
                    }
                }
                else if (folder.FileInfo.Parents?.Count == 1)
                {
                    throw new Exception("Has more parents, dont know how to handle.");
                }
                else //No parents, should be top folder
                {
                }
            }
        }

        public void TriageFiles(List<GFile> fileList, List<GFolder> folderList)
        {
            foreach (GFile file in fileList)
            {
                if (file.FileInfo.Parents?.Count == 1)
                {
                    var parent = folderList.FirstOrDefault(a => a.IsParentOf(file));
                    if (parent != null)
                    {
                        file.GParent = parent;
                        if (!parent.Files.Contains(file))
                        {
                            //parent.Items.Add(file);
                            parent.Files.Add(file);
                        }
                    }
                }
                else if (file.FileInfo.Parents?.Count == 1)
                {
                    throw new Exception("Has more parents, dont know how to handle.");
                }
                else //No parents, should be top folder
                {
                }
            }
        }

        public void UpdateAllFiles(Action<GFile> activity, bool applyToFolders = true)
        {
            UpdateAllFilesInFolder(this, activity, applyToFolders);
        }
        public void UpdateAllFilesInFolder(GFolder folder, Action<GFile> activity, bool applyToFolders = true)
        {
            if (applyToFolders) activity(this);

            folder.Folders.ToList().ForEach(f => UpdateAllFilesInFolder(f, activity, applyToFolders));
            folder.Files.ToList().ForEach(f => activity(f));
        }
    }
}
