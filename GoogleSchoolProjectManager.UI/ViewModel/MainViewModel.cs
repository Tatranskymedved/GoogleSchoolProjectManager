using GoogleSchoolProjectManager.Lib.Google;
using GoogleSchoolProjectManager.Lib.Google.Drive;
using GoogleSchoolProjectManager.Lib.GoogleAPI.Sheets;
using GoogleSchoolProjectManager.UI.View;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GoogleSchoolProjectManager.UI.ViewModel
{
    public class MainViewModel : IViewModel
    {
        private IDialogCoordinator DialogCoordinator { get; set; }

        #region [Properties]
        private GTree mTree = new GTree();
        ///<summary>
        /// Tree
        ///</summary>
        public GTree Tree
        {
            get { return this.mTree; }
            set
            {
                if (value == this.mTree) return;

                this.mTree = value;
                OnPropertyChanged(nameof(Tree));
            }
        }

        private GFile mMainSelectedItem = null;
        public bool IsFolderSelected => mMainSelectedItem != null && (mMainSelectedItem is GFolder);
        public bool IsFileSelected => mMainSelectedItem != null && !IsFolderSelected;
        ///<summary>
        /// SelectedItem
        ///</summary>
        public GFile MainSelectedItem
        {
            get { return this.mMainSelectedItem; }
            set
            {
                if (value == this.mMainSelectedItem) return;

                this.mMainSelectedItem = value;
                OnPropertyChanged(nameof(MainSelectedItem));
                OnPropertyChanged(nameof(IsFileSelected));
                OnPropertyChanged(nameof(IsFolderSelected));
            }
        }

        private bool mIsFolderLocked = false;
        ///<summary>
        /// IsFolderLocked
        ///</summary>
        public bool IsFolderLocked
        {
            get { return this.mIsFolderLocked; }
            set
            {
                if (value == this.mIsFolderLocked) return;

                this.mIsFolderLocked = value;
                OnPropertyChanged(nameof(IsFolderLocked));
            }
        }

        private GFile mGFileTemplateSource = null;
        public string GFileTemplateSourcePath => GFileTemplateSource?.PathWithParents();
        ///<summary>
        /// GFileTemplateSource
        ///</summary>
        public GFile GFileTemplateSource
        {
            get { return this.mGFileTemplateSource; }
            set
            {
                if (value == this.mGFileTemplateSource) return;

                this.mGFileTemplateSource = value;
                OnPropertyChanged(nameof(GFileTemplateSource));
                OnPropertyChanged(nameof(GFileTemplateSourcePath));
            }
        }

        private string mGFolderSingleFolderOutput = null;
        ///<summary>
        /// GFolderSingleFolderOutput
        ///</summary>
        public string GFolderSingleFolderOutput
        {
            get { return this.mGFolderSingleFolderOutput; }
            set
            {
                if (value == this.mGFolderSingleFolderOutput) return;

                this.mGFolderSingleFolderOutput = value;
                OnPropertyChanged(nameof(GFolderSingleFolderOutput));
            }
        }

        private string mGFileCreationPrefix = null;
        ///<summary>
        /// GFileCreationPrefix
        ///</summary>
        public string GFileCreationPrefix
        {
            get { return this.mGFileCreationPrefix; }
            set
            {
                if (value == this.mGFileCreationPrefix) return;

                this.mGFileCreationPrefix = value;
                OnPropertyChanged(nameof(GFileCreationPrefix));
            }
        }

        private string mGFileCreationPostfix = null;
        ///<summary>
        /// GFileCreationPostfix
        ///</summary>
        public string GFileCreationPostfix
        {
            get { return this.mGFileCreationPostfix; }
            set
            {
                if (value == this.mGFileCreationPostfix) return;

                this.mGFileCreationPostfix = value;
                OnPropertyChanged(nameof(GFileCreationPostfix));
            }
        }


        private string mKHSRequestEditSubject = null;
        ///<summary>
        /// KHSRequestEditSubject
        ///</summary>
        public string KHSRequestEditSubject
        {
            get { return this.mKHSRequestEditSubject; }
            set
            {
                if (value == this.mKHSRequestEditSubject) return;

                this.mKHSRequestEditSubject = value;
                OnPropertyChanged(nameof(KHSRequestEditSubject));
            }
        }

        private string mKHSRequestEditGoal = null;
        ///<summary>
        /// KHSRequestEditGoal
        ///</summary>
        public string KHSRequestEditGoal
        {
            get { return this.mKHSRequestEditGoal; }
            set
            {
                if (value == this.mKHSRequestEditGoal) return;

                this.mKHSRequestEditGoal = value;
                OnPropertyChanged(nameof(KHSRequestEditGoal));
            }
        }

        private DateTime? mDatePicker_KHSUpdate_DateFrom = null;
        ///<summary>
        /// DatePicker_KHSUpdate_DateFrom
        ///</summary>
        public DateTime? DatePicker_KHSUpdate_DateFrom
        {
            get { return this.mDatePicker_KHSUpdate_DateFrom; }
            set
            {
                if (value == this.mDatePicker_KHSUpdate_DateFrom) return;

                this.mDatePicker_KHSUpdate_DateFrom = value;
                OnPropertyChanged(nameof(DatePicker_KHSUpdate_DateFrom));
            }
        }

        private DateTime? mDatePicker_KHSUpdate_DateTo = null;
        ///<summary>
        /// DatePicker_KHSUpdate_DateTo
        ///</summary>
        public DateTime? DatePicker_KHSUpdate_DateTo
        {
            get { return this.mDatePicker_KHSUpdate_DateTo; }
            set
            {
                if (value == this.mDatePicker_KHSUpdate_DateTo) return;

                this.mDatePicker_KHSUpdate_DateTo = value;
                OnPropertyChanged(nameof(DatePicker_KHSUpdate_DateTo));
            }
        }

        private string mGenerateFilesFromTemplatePrefix = "";
        ///<summary>
        /// GenerateFilesFromTemplatePrefix
        ///</summary>
        public string GenerateFilesFromTemplatePrefix
        {
            get { return this.mGenerateFilesFromTemplatePrefix; }
            set
            {
                if (value == this.mGenerateFilesFromTemplatePrefix) return;

                this.mGenerateFilesFromTemplatePrefix = value;
                OnPropertyChanged(nameof(GenerateFilesFromTemplatePrefix));
                OnPropertyChanged(nameof(FileNamesList));
            }
        }

        private string mGenerateFilesFromTemplatePostfix = "";
        ///<summary>
        /// GenerateFilesFromTemplatePostfix
        ///</summary>
        public string GenerateFilesFromTemplatePostfix
        {
            get { return this.mGenerateFilesFromTemplatePostfix; }
            set
            {
                if (value == this.mGenerateFilesFromTemplatePostfix) return;

                this.mGenerateFilesFromTemplatePostfix = value;
                OnPropertyChanged(nameof(GenerateFilesFromTemplatePostfix));
                OnPropertyChanged(nameof(FileNamesList));
            }
        }

        private string mFileNamesSource = "";
        ///<summary>
        /// FileNamesSource
        ///</summary>
        public string FileNamesSource
        {
            get { return this.mFileNamesSource; }
            set
            {
                if (value == this.mFileNamesSource) return;

                this.mFileNamesSource = value;
                OnPropertyChanged(nameof(FileNamesSource));
                OnPropertyChanged(nameof(FileNamesList));
            }
        }
        public ObservableCollection<string> FileNamesList => new ObservableCollection<string>(
            FileNamesSource?.Split(new char[] { '\n', ',', ';', '|' }, StringSplitOptions.RemoveEmptyEntries)
                            ?.Where(a => !string.IsNullOrWhiteSpace(a))
                            ?.Select(a => GenerateFilesFromTemplatePrefix + a.Trim() + GenerateFilesFromTemplatePostfix)
                            ?.ToList()
            );

        private UpdateKHSRequest mUpdateKHSRequest = new UpdateKHSRequest();
        ///<summary>
        /// UpdateKHSRequest
        ///</summary>
        public UpdateKHSRequest UpdateKHSRequest
        {
            get { return this.mUpdateKHSRequest; }
            set
            {
                if (value == this.mUpdateKHSRequest) return;

                this.mUpdateKHSRequest = value;
                OnPropertyChanged(nameof(UpdateKHSRequest));
            }
        }


        #endregion

        public MainViewModel(IDialogCoordinator dialogCoordinator)
        {
            DialogCoordinator = dialogCoordinator;
        }

        #region [Commands]

        private ICommand mCMD_GetFolderTree = null;
        private bool mIsExecutingCMD_GetFolderTree = false;
        ///<summary>
        /// CMD_GetFolderTree
        ///</summary>
        public ICommand CMD_GetFolderTree
        {
            get
            {
                if (mCMD_GetFolderTree != null) return mCMD_GetFolderTree;

                mCMD_GetFolderTree = new RelayAsyncCommand(async () =>
                {
                    mIsExecutingCMD_GetFolderTree = true;
                    var dialog = await DialogCoordinator.ShowProgressAsync(this,
                        Properties.Resources.DIALOG_CMDGetFolderTree_PROGRESS_Title,
                        Properties.Resources.DIALOG_CMDGetFolderTree_PROGRESS_Message,
                        false);
                    try
                    {
                        using (var con = new GoogleConnector())
                        {
                            string diskName = Properties.Settings.Default["GoogleDiskDrive"].ToString();
                            //const string diskName = "OSTODISK";
                            Tree = new GDriveManager(con) { DriveName = diskName }.GetTree();
                        }
                    }
                    finally
                    {
                        await dialog.CloseAsync();
                        mIsExecutingCMD_GetFolderTree = false;
                    }
                },
                () =>
                {
                    return !mIsExecutingCMD_GetFolderTree;
                }, null);

                return this.mCMD_GetFolderTree;
            }
        }

        private ICommand mCMD_SelectFileTemplateSource = null;
        ///<summary>
        /// CMD_SelectFileTemplateSource
        ///</summary>
        public ICommand CMD_SelectFileTemplateSource
        {
            get
            {
                if (mCMD_SelectFileTemplateSource != null) return mCMD_SelectFileTemplateSource;

                mCMD_SelectFileTemplateSource = new RelayCommand(() =>
                {
                    var result = SelectItemFromTreeDialog();
                    if (result != null)
                    {
                        GFileTemplateSource = result;
                    }
                },
                () =>
                {
                    return true;
                });

                return this.mCMD_SelectFileTemplateSource;
            }
        }

        private ICommand mCMD_SelectFileNamesSource = null;
        ///<summary>
        /// CMD_SelectFileNamesSource
        ///</summary>
        public ICommand CMD_SelectFileNamesSource
        {
            get
            {
                if (mCMD_SelectFileNamesSource != null) return mCMD_SelectFileNamesSource;

                mCMD_SelectFileNamesSource = new RelayCommand(() =>
                {

                });

                return this.mCMD_SelectFileNamesSource;
            }
        }


        private ICommand mCMD_GenerateFilesFromTemplate = null;
        private bool mIsExecutingCMD_GenerateFilesFromTemplate = false;
        ///<summary>
        /// CMD_GenerateFilesFromTemplate
        ///</summary>
        public ICommand CMD_GenerateFilesFromTemplate
        {
            get
            {
                if (mCMD_GenerateFilesFromTemplate != null) return mCMD_GenerateFilesFromTemplate;

                mCMD_GenerateFilesFromTemplate = new RelayAsyncCommand(async () =>
                {
                    mIsExecutingCMD_GenerateFilesFromTemplate = true;
                    var dialog = await DialogCoordinator.ShowProgressAsync(this,
                        Properties.Resources.DIALOG_CMD_GenerateFilesFromTemplate_PROGRESS_Title,
                        Properties.Resources.DIALOG_CMD_GenerateFilesFromTemplate_PROGRESS_Message_Before,
                        false);

                    try
                    {
                        using (var con = new GoogleConnector())
                        {
                            var manager = new GDriveManager(con);
                            var nameList = FileNamesList.ToList();
                            for (int i = 0; i < nameList.Count; i++)
                            {
                                var fileName = nameList[i];

                                double progress = i / Convert.ToDouble(nameList.Count);
                                dialog.SetProgress(progress);
                                dialog.SetMessage(string.Format(Properties.Resources.DIALOG_CMD_GenerateFilesFromTemplate_PROGRESS_Message,
                                    i,
                                    nameList.Count,
                                    fileName));

                                manager.CopyFile(GFileTemplateSource, (MainSelectedItem as GFolder), fileName);
                            }
                        }
                    }
                    finally
                    {
                        await dialog.CloseAsync();
                        mIsExecutingCMD_GenerateFilesFromTemplate = false;
                    }
                },
                () =>
                {
                    return !mIsExecutingCMD_GenerateFilesFromTemplate
                        && (MainSelectedItem?.IsGFolder ?? false)
                        && (GFileTemplateSource?.IsNotGFolder ?? false)
                        && (FileNamesList.Count > 0);
                }, null);

                return this.mCMD_GenerateFilesFromTemplate;
            }
        }

        private ICommand mCMD_KHSRequest_UpdateSelectedKHSes = null;
        private bool mIsExecutingCMD_KHSRequest_UpdateSelectedKHSes = false;
        ///<summary>
        /// CMD_UpdateSelectedKHSes
        ///</summary>
        public ICommand CMD_KHSRequest_UpdateSelectedKHSes
        {
            get
            {
                if (mCMD_KHSRequest_UpdateSelectedKHSes != null) return mCMD_KHSRequest_UpdateSelectedKHSes;

                mCMD_KHSRequest_UpdateSelectedKHSes = new RelayAsyncCommand(() =>
                {
                    mIsExecutingCMD_KHSRequest_UpdateSelectedKHSes = true;
                    try
                    {
                        UpdateKHSRequest.Files = Tree.FindAllFilesSelectedForUpdate();
                        using (var con = new GoogleConnector())
                        {
                            var manager = new GSheetsManager(con);
                            manager.UpdateSheets(UpdateKHSRequest);
                        }
                    }
                    finally
                    {
                        mIsExecutingCMD_KHSRequest_UpdateSelectedKHSes = false;
                    }
                },
                () =>
                {
                    return !mIsExecutingCMD_KHSRequest_UpdateSelectedKHSes;
                }, null);

                return this.mCMD_KHSRequest_UpdateSelectedKHSes;
            }
        }

        private ICommand mCMD_CheckAllFiles_ForUpdate = null;
        ///<summary>
        /// CMD_CheckAllFiles_ForUpdate
        ///</summary>
        public ICommand CMD_CheckAllFiles_ForUpdate
        {
            get
            {
                if (mCMD_CheckAllFiles_ForUpdate != null) return mCMD_CheckAllFiles_ForUpdate;

                mCMD_CheckAllFiles_ForUpdate = new RelayCommand<bool>((checkIt) =>
                {
                    Tree.UpdateAllFiles(a => a.IsSelectedForUpdate = checkIt);
                });

                return this.mCMD_CheckAllFiles_ForUpdate;
            }
        }

        private ICommand mCMD_KHSRequest_AddSubjectGoal = null;
        ///<summary>
        /// CMD_KHSRequest_AddSubjectGoal
        ///</summary>
        public ICommand CMD_KHSRequest_AddSubjectGoal
        {
            get
            {
                if (mCMD_KHSRequest_AddSubjectGoal != null) return mCMD_KHSRequest_AddSubjectGoal;

                mCMD_KHSRequest_AddSubjectGoal = new RelayCommand(() =>
                {
                    UpdateKHSRequest.SubjectGoalList.Add(new SubjectGoalPair(KHSRequestEditSubject, KHSRequestEditGoal));
                    KHSRequestEditSubject = null;
                    KHSRequestEditGoal = null;
                });

                return this.mCMD_KHSRequest_AddSubjectGoal;
            }
        }

        private ICommand mCMD_KHSRequest_RemoveSubjectGoal = null;
        ///<summary>
        /// CMD_KHSRequest_RemoveSubjectGoal
        ///</summary>
        public ICommand CMD_KHSRequest_RemoveSubjectGoal
        {
            get
            {
                if (mCMD_KHSRequest_RemoveSubjectGoal != null) return mCMD_KHSRequest_RemoveSubjectGoal;

                mCMD_KHSRequest_RemoveSubjectGoal = new RelayCommand(() =>
                {
                    UpdateKHSRequest.SubjectGoalList.Remove(UpdateKHSRequest.SelectedSubjectGoal);
                },
                () =>
                {
                    return UpdateKHSRequest?.SelectedSubjectGoal != null;
                });

                return this.mCMD_KHSRequest_RemoveSubjectGoal;
            }
        }

        #endregion

        #region [Methods]
        /// <summary>
        /// Creates new Dialog window with selection tree, where user can select File/Folder
        /// </summary>
        /// <returns></returns>
        private GFile SelectItemFromTreeDialog()
        {
            var dialog = new SelectItemFromTreeWindow(new SelectItemFromTreeViewModel(Tree));
            var result = dialog?.ShowDialog();
            if (result.HasValue && result.Value)
            {
                return (dialog.DataContext as SelectItemFromTreeViewModel).SelectedItem;
            }

            return null;
        }
        #endregion

        #region [Implementation of INotifyPropertyChanged]
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string aPropertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(aPropertyName));
        #endregion
    }
}
