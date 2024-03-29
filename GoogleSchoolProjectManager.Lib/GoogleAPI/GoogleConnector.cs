﻿using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using GoogleSchoolProjectManager.Lib.Google.Drive;
using System;
using System.Threading;

namespace GoogleSchoolProjectManager.Lib.Google
{
    public class GoogleConnector : IDisposable
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/docs.googleapis.com-dotnet-quickstart.json
        private string[] _scopes = {
        SheetsService.Scope.Spreadsheets,
        DriveService.Scope.Drive
    };
        private string _applicationName = "SchoolProjectManager";

        public DriveService Drive { get; private set; }
        public SheetsService Sheets { get; private set; }

        public GDriveManager DriveManager => new GDriveManager(this);
        public GSheetsManager SheetsManager => new GSheetsManager(this);

        public GoogleConnector()
        {
            this.CreateConnection();
        }

        public void CreateConnection()
        {
            var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.FromFile("credentials.json").Secrets,
                _scopes,
                "user",
                CancellationToken.None).Result;

            // Creates Google Drive/Sheets API service against which the requests can be created/called.
            Drive = new DriveService(new BaseClientService.Initializer()
            {

                HttpClientInitializer = credential,
                ApplicationName = _applicationName
            });
            Sheets = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = _applicationName
            });
        }

        private void CloseConnection()
        {
            Drive.Dispose();
            Sheets.Dispose();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.CloseConnection();
                }

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~SpreadSheetConnector()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}