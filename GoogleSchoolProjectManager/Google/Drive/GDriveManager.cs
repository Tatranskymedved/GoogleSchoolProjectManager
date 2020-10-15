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
            var request = this._service.Files.List();

            request.PageSize = 50;
            request.Fields = "files(teamDriveId, mimeType, name, id, parents)";
            var fileList = request.Execute();

            return fileList.Files;
        }

        public IEnumerable<string> GetFileNames()
        {
            return this.GetFiles().Select(a => a.Name);
        }

        public void Get()
        {
            ;
        }
    }
}
