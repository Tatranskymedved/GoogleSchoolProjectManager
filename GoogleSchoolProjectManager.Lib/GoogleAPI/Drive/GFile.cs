using Google.Apis.Drive.v3.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSchoolProjectManager.Lib.Google.Drive
{
    public class GFile : INotifyPropertyChanged
    {
        #region [Model properties]
        private File mFileInfo = null;
        ///<summary>
        /// FileInfo
        ///</summary>
        public File FileInfo
        {
            get { return this.mFileInfo; }
            set
            {
                if (value == this.mFileInfo) return;

                this.mFileInfo = value;
                OnPropertyChanged(nameof(FileInfo));
            }
        }

        private GFolder mGParent = null;
        ///<summary>
        /// GParent
        ///</summary>
        public GFolder GParent
        {
            get { return this.mGParent; }
            set
            {
                if (value == this.mGParent) return;

                this.mGParent = value;
                OnPropertyChanged(nameof(GParent));
            }
        }

        public string Name => this?.FileInfo?.Name;
        #endregion

        #region [ViewModel properties]

        private bool mIsSelectedForUpdate = false;
        ///<summary>
        /// IsSelectedForUpdate
        ///</summary>
        public bool IsSelectedForUpdate
        {
            get { return this.mIsSelectedForUpdate; }
            set
            {
                if (value == this.mIsSelectedForUpdate) return;

                this.mIsSelectedForUpdate = value;
                OnPropertyChanged(nameof(IsSelectedForUpdate));
            }
        }

        #endregion

        public GFile() { }
        public GFile(File file) { FileInfo = file; }

        public bool IsParentOf(GFile possibleChild)
        {
            var parents = possibleChild?.FileInfo?.Parents;

            if (parents == null || parents.Count > 1 || parents.Count == 0)
            {
                return false;
            }

            return this.FileInfo.Id == parents.First();
        }

        public string PathWithParents()
        {
            return getNameWithParentPath(this);
        }

        private string getNameWithParentPath(GFile file)
        {
            if (file == null) return "";
            return getNameWithParentPath(file.GParent) + @"/" + file.Name;
        }

        public override string ToString()
        {
            return this.FileInfo.Name + ": " + MimeTypes.GetMimeTypeDescription(this);
        }

        public bool IsNotGFolder => (this as GFolder) == null ? true : false;
        public bool IsGFolder => (this as GFolder) != null ? true : false;


        #region [Implementation of INotifyPropertyChanged]
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string aPropertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(aPropertyName));
        #endregion
    }
}
