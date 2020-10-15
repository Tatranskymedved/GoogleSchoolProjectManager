using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSchoolProjectManager.Google.Drive
{
    public class GDriveManager
    {
        private DriveService _service;
        public string DriveName { get; set; }
        public GDriveManager(GoogleConnector connector)
        {
            this._service = connector.Drive;
        }

        public GDriveManager(DriveService service)
        {
            this._service = service;
        }

        public IList<File> GetFiles()
        {
            var fileResult = new List<File>();

            var teamDrivesList = this._service.Teamdrives.List().Execute().TeamDrives;
            var teamDrive = teamDrivesList.FirstOrDefault(a => a.Name.ToLowerInvariant().Contains(DriveName.ToLowerInvariant()));
            var drivesList = this._service.Drives.List().Execute().Drives;
            var drive = drivesList.FirstOrDefault(a => a.Name.ToLowerInvariant().Contains(DriveName.ToLowerInvariant()));

            var driveId = teamDrive?.Id ?? drive?.Id;

            if (teamDrive != null)
            {
                var fileListRequest = this._service.Files.List();

                if (!string.IsNullOrEmpty(driveId))
                {
                    fileListRequest.Corpora = "drive";
                    fileListRequest.DriveId = driveId;
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

                fileListRequest.Fields = "files(teamDriveId, mimeType, name, id, parents)";
                fileListRequest.PageSize = 1000;

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
    }
}
