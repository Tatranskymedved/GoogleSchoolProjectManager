using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSchoolProjectManager.Lib.Google.Drive
{
    public class GDriveManager
    {
        private DriveService Service;

        public string DriveName { get; set; }
        private bool mDriveIdInitialized = false;
        private string mDriveId;
        public string DriveId
        {
            get
            {
                if (!mDriveIdInitialized)
                {
                    mDriveId = GetDriveId();
                    mDriveIdInitialized = true;
                }
                return mDriveId;
            }
        }

        public bool Trashed { get; set; } = false;

        public GDriveManager(GoogleConnector connector)
        {
            this.Service = connector.Drive;
        }

        public GDriveManager(DriveService service)
        {
            this.Service = service;
        }

        public IList<File> GetFiles()
        {
            var fileResult = new List<File>();

            if (DriveId != null)
            {
                var fileListRequest = this.Service.Files.List();

                if (!string.IsNullOrEmpty(DriveId))
                {
                    fileListRequest.Corpora = "drive";
                    fileListRequest.DriveId = DriveId;
                    fileListRequest.SupportsTeamDrives = true;
                    fileListRequest.SupportsAllDrives = false;
                    fileListRequest.IncludeItemsFromAllDrives = true;
                }
                else
                {
                    fileListRequest.SupportsTeamDrives = true;
                    fileListRequest.SupportsAllDrives = true;
                    fileListRequest.IncludeItemsFromAllDrives = true;
                }

                fileListRequest.Fields = "nextPageToken, files(teamDriveId, mimeType, name, id, parents)";
                if (!Trashed)
                {
                    fileListRequest.Q = "trashed: false";
                }
                fileListRequest.PageSize = 500;

                FileList fileListResult = default(FileList);
                do
                {
                    if (!string.IsNullOrEmpty(fileListResult?.NextPageToken))
                    {
                        fileListRequest.PageToken = fileListResult.NextPageToken;
                    }

                    fileListResult = fileListRequest.Execute();
                    fileResult.AddRange(fileListResult?.Files);
                }
                while (!string.IsNullOrEmpty(fileListResult.NextPageToken));
            }

            return fileResult;
        }

        public IEnumerable<string> GetFileNames()
        {
            return this.GetFiles().Select(a => a.Name);
        }

        public GTree GetTree()
        {
            return new GTree(GetFiles());
        }

        public void CreateFolder(GFolder destination, string newFolderName)
        {
            if (destination == null) throw new ArgumentNullException(nameof(destination));
            if (string.IsNullOrEmpty(newFolderName)) throw new ArgumentNullException(nameof(newFolderName));

            var folderCreateRequest = this.Service.Files.Create(new File()
            {
                Parents = new string [] { destination?.FileInfo?.Id },
                Name = newFolderName,
                MimeType = MimeTypes.GoogleFolder
            });

            if (!string.IsNullOrEmpty(DriveId))
            {
                folderCreateRequest.SupportsTeamDrives = true;
                folderCreateRequest.SupportsAllDrives = false;
            }
            else
            {
                folderCreateRequest.SupportsTeamDrives = true;
                folderCreateRequest.SupportsAllDrives = true;
            }

            folderCreateRequest.Execute();
        }

        public void CopyFile(GFile source, GFolder destination, string newFileName)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (destination == null) throw new ArgumentNullException(nameof(destination));

            var fileCopyRequest = this.Service.Files.Copy(new File()
            {
                Parents = new string[] { destination?.FileInfo?.Id },
                Name = newFileName
            }, source.FileInfo.Id);

            if (!string.IsNullOrEmpty(DriveId))
            {
                fileCopyRequest.SupportsTeamDrives = true;
                fileCopyRequest.SupportsAllDrives = false;
            }
            else
            {
                fileCopyRequest.SupportsTeamDrives = true;
                fileCopyRequest.SupportsAllDrives = true;
            }

            //var fileCopyRequest = this.Service.Files.Copy(source.FileInfo, source.FileInfo.Id);
            fileCopyRequest.Execute();
        }

        private string GetDriveId()
        {
            var teamDrivesList = this.Service.Teamdrives.List().Execute().TeamDrives;
            var teamDrive = teamDrivesList.FirstOrDefault(a => a.Name.ToLowerInvariant().Contains(DriveName?.ToLowerInvariant() ?? ""));
            var drivesList = this.Service.Drives.List().Execute().Drives;
            var drive = drivesList.FirstOrDefault(a => a.Name.ToLowerInvariant().Contains(DriveName?.ToLowerInvariant() ?? ""));

            return teamDrive?.Id ?? drive?.Id;
        }
    }
}
