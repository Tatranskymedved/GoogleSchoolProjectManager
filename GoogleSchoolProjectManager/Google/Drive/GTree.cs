using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Drive.v3.Data;

namespace GoogleSchoolProjectManager.Google.Drive
{
    public class GTree : GFolder
    {
        public GTree(IEnumerable<File> files)
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

            this.Folders = folderList.Where(a => a.Parent == null).ToList();
            this.Files = fileList.Where(a => a.Parent == null).ToList();
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
                        folder.Parent = parent;
                        if (!parent.Folders.Contains(folder))
                        {
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
                        file.Parent = parent;
                        if (!parent.Files.Contains(file))
                        {
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
    }


}
