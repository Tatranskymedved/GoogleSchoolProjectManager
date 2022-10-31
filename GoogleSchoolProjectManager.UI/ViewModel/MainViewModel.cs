using GoogleSchoolProjectManager.Lib.Google;
using GoogleSchoolProjectManager.Lib.Google.Drive;
using GoogleSchoolProjectManager.Lib.GoogleAPI.Sheets;
using GoogleSchoolProjectManager.Lib.KHS;
using GoogleSchoolProjectManager.UI.View;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading;
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

        private string mNewFolderName = string.Format("TÝDEN {0:d. M.} - {1:d. M.}", DateRange.GetNextWeekday(DateTime.Now, DayOfWeek.Monday), DateRange.GetNextWeekday(DateTime.Now, DayOfWeek.Monday).AddDays(4));
        ///<summary>
        /// NewFolderName
        ///</summary>
        public string NewFolderName
        {
            get { return this.mNewFolderName; }
            set
            {
                if (value == this.mNewFolderName) return;

                this.mNewFolderName = value;
                OnPropertyChanged(nameof(NewFolderName));
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

        private string mTitle = null;
        ///<summary>
        /// Title
        ///</summary>
        public string Title
        {
            get { return this.mTitle; }
            set
            {
                if (value == this.mTitle) return;

                this.mTitle = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        private string mGoogleDriveName = null;
        ///<summary>
        /// GoogleDriveName - name of the drive, that should be accessed
        ///</summary>
        public string GoogleDriveName
        {
            get { return this.mGoogleDriveName; }
            set
            {
                if (value == this.mGoogleDriveName) return;
                if (string.IsNullOrWhiteSpace(value)) return;

                this.mGoogleDriveName = value;
                OnPropertyChanged(nameof(GoogleDriveName));
                if (preventConfigFileSave == false)
                {
                    Properties.Settings.Default.GoogleDiskDrive = value;
                    SaveConfigFile();
                }
            }
        }

        private bool preventConfigFileSave = false;

        /// <summary>
        /// List of Drives that are available to select from.
        /// </summary>
        public ObservableCollection<string> GoogleDrivesNames { get; set; } = new ObservableCollection<string>();

        #endregion

        public MainViewModel(IDialogCoordinator dialogCoordinator)
        {
            DialogCoordinator = dialogCoordinator;

            var sheetName = Properties.Settings.Default.KHSSheetName;
            var a1_weekColumn = Properties.Settings.Default.KHS_A1_WeekColumn;
            if (!string.IsNullOrEmpty(sheetName)) UpdateKHSRequest.SheetName = sheetName;
            if (!string.IsNullOrEmpty(a1_weekColumn)) UpdateKHSRequest.A1_WeekColumn = a1_weekColumn;

            var version = Assembly.GetExecutingAssembly()?.GetName()?.Version;
            Title = Properties.Resources.APP_Title + $" - {version.Major}.{version.Minor}";

            var drives = Properties.Settings.Default.GoogleDiskDrives?.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var drive in drives)
            {
                GoogleDrivesNames.Add(drive);
            }
            var previouslySetGoogleDiskName = Properties.Settings.Default.GoogleDiskDrive;
            GoogleDriveName = drives.Contains(previouslySetGoogleDiskName) ? previouslySetGoogleDiskName : GoogleDrivesNames.FirstOrDefault();
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
                            string diskName = GoogleDriveName;
                            Tree = new GDriveManager(con) { DriveName = diskName }.GetTree();
                        }
                    }
                    catch (Exception ex)
                    {
                        await DialogCoordinator.ShowMessageAsync(this,
                            Properties.Resources.Dialog_ExceptionOccurs_Title,
                            ex.Message + Environment.NewLine + Environment.NewLine + ex.ToString() + Environment.NewLine + ex.StackTrace,
                            MessageDialogStyle.Affirmative);
                    }
                    finally
                    {
                        await dialog.CloseAsync();
                        mIsExecutingCMD_GetFolderTree = false;
                    }
                },
                () =>
                {
                    return mIsExecutingCMD_GetFolderTree == false
                    && string.IsNullOrWhiteSpace(GoogleDriveName) == false;
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

                    var dialogResult = await DialogCoordinator.ShowMessageAsync(this,
                        Properties.Resources.DIALOG_CMD_GenerateFilesFromTemplate_BEFORE_Title,
                        Properties.Resources.DIALOG_CMD_GenerateFilesFromTemplate_BEFORE_Message,
                        MessageDialogStyle.AffirmativeAndNegative,
                        new MetroDialogSettings()
                        {
                            AffirmativeButtonText = Properties.Resources.Button_Yes,
                            NegativeButtonText = Properties.Resources.Button_No
                        });

                    if (dialogResult == MessageDialogResult.Canceled || dialogResult == MessageDialogResult.Negative)
                    {
                        mIsExecutingCMD_GenerateFilesFromTemplate = false;
                        return;
                    }

                    var cancelToken = new CancellationToken();

                    var dialog = await DialogCoordinator.ShowProgressAsync(this,
                        Properties.Resources.DIALOG_CMD_GenerateFilesFromTemplate_PROGRESS_Title,
                        Properties.Resources.DIALOG_CMD_GenerateFilesFromTemplate_PROGRESS_Message_Before,
                        true,
                        new MetroDialogSettings()
                        {
                            CancellationToken = cancelToken,
                            DialogResultOnCancel = MessageDialogResult.Canceled
                        });

                    List<Exception> errorList = new List<Exception>();

                    try
                    {
                        using (var con = new GoogleConnector())
                        {
                            var manager = new GDriveManager(con);
                            var nameList = FileNamesList.ToList();
                            for (int i = 0; i < nameList.Count; i++)
                            {
                                if (cancelToken.IsCancellationRequested || dialog.IsCanceled) break;

                                var fileName = nameList[i];

                                double progress = i / Convert.ToDouble(nameList.Count);
                                dialog.SetProgress(progress);
                                dialog.SetMessage(string.Format(Properties.Resources.DIALOG_CMD_GenerateFilesFromTemplate_PROGRESS_Message,
                                    i,
                                    nameList.Count,
                                    fileName));

                                try
                                {
                                    manager.CopyFile(GFileTemplateSource, (MainSelectedItem as GFolder), fileName);

                                }
                                catch (Exception ex)
                                {
                                    errorList.Add(ex);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        await DialogCoordinator.ShowMessageAsync(this,
                            Properties.Resources.Dialog_ExceptionOccurs_Title,
                            ex.Message + Environment.NewLine + Environment.NewLine + ex.ToString() + Environment.NewLine + ex.StackTrace,
                            MessageDialogStyle.Affirmative);
                    }
                    finally
                    {
                        if (errorList?.Count > 0)
                            await DialogCoordinator.ShowMessageAsync(this,
                                Properties.Resources.Dialog_ExceptionOccurs_Title,
                                string.Join(Environment.NewLine, errorList.Select(a => a.Message)),
                                MessageDialogStyle.Affirmative);

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

                mCMD_KHSRequest_UpdateSelectedKHSes = new RelayAsyncCommand(async () =>
                {
                    mIsExecutingCMD_KHSRequest_UpdateSelectedKHSes = true;

                    var dialogResult = await DialogCoordinator.ShowMessageAsync(this,
                        Properties.Resources.DIALOG_CMD_KHSRequest_UpdateSelectedKHSes_BEFORE_Title,
                        Properties.Resources.DIALOG_CMD_KHSRequest_UpdateSelectedKHSes_BEFORE_Message,
                        MessageDialogStyle.AffirmativeAndNegative,
                        new MetroDialogSettings()
                        {
                            AffirmativeButtonText = Properties.Resources.Button_Yes,
                            NegativeButtonText = Properties.Resources.Button_No
                        });

                    if (dialogResult == MessageDialogResult.Canceled || dialogResult == MessageDialogResult.Negative)
                    {
                        mIsExecutingCMD_KHSRequest_UpdateSelectedKHSes = false;
                        return;
                    }

                    var cancelToken = new CancellationToken();

                    var dialog = await DialogCoordinator.ShowProgressAsync(this,
                        Properties.Resources.DIALOG_CMD_KHSRequest_UpdateSelectedKHSes_PROGRESS_Title,
                        Properties.Resources.DIALOG_CMD_KHSRequest_UpdateSelectedKHSes_PROGRESS_Message_Before,
                        true,
                        new MetroDialogSettings()
                        {
                            CancellationToken = cancelToken,
                            DialogResultOnCancel = MessageDialogResult.Canceled
                        });

                    try
                    {
                        List<Exception> errorList;

                        UpdateKHSRequest.Files = Tree.FindAllFilesSelectedForUpdate();
                        using (var con = new GoogleConnector())
                        {
                            var manager = new GSheetsManager(con);

                            manager.UpdateSheets(UpdateKHSRequest, (progress) =>
                            {
                                dialog.SetProgress(Math.Min(1, Math.Max(0, progress.Progress)));
                                dialog.SetMessage(string.Format(Properties.Resources.DIALOG_CMD_KHSRequest_UpdateSelectedKHSes_PROGRESS_Message,
                                    progress.FileIndex,
                                    progress.FilesCount,
                                    progress.FileName));
                            }, cancelToken, out errorList);

                            if (errorList?.Count > 0)
                                await DialogCoordinator.ShowMessageAsync(this,
                                    Properties.Resources.Dialog_ExceptionOccurs_Title,
                                    string.Join(Environment.NewLine, errorList.Select(a => a.Message)),
                                    MessageDialogStyle.Affirmative);
                        }
                    }
                    catch (Exception ex)
                    {
                        await DialogCoordinator.ShowMessageAsync(this,
                            Properties.Resources.Dialog_ExceptionOccurs_Title,
                            ex.Message + Environment.NewLine + Environment.NewLine + ex.ToString() + Environment.NewLine + ex.StackTrace,
                            MessageDialogStyle.Affirmative);
                    }
                    finally
                    {
                        await dialog.CloseAsync();
                        mIsExecutingCMD_KHSRequest_UpdateSelectedKHSes = false;
                    }
                },
                () =>
                {
                    return !mIsExecutingCMD_KHSRequest_UpdateSelectedKHSes
                        && Tree.FindAllFilesSelectedForUpdate().Any();
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
                    Tree.UpdateAllFilesAndFoldersInFolder((MainSelectedItem as GFolder), a => a.IsSelectedForUpdate = checkIt, (f) => f.IsNotGFolder && f.NameContainsKHS);
                },
                (checkIt) => MainSelectedItem?.IsGFolder ?? false);

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

        private ICommand mCMD_SelectMainFolderToggleChanged = null;
        ///<summary>
        /// CMD_SelectMainFolderToggleChanged
        ///</summary>
        public ICommand CMD_SelectMainFolderToggleChanged
        {
            get
            {
                if (mCMD_SelectMainFolderToggleChanged != null) return mCMD_SelectMainFolderToggleChanged;

                mCMD_SelectMainFolderToggleChanged = new RelayCommand(() =>
                {
                    if (!IsFolderLocked)
                    {
                        Tree.UpdateAllFilesAndFolders((a) => a.IsSelectedForUpdate = false);
                    }
                });

                return this.mCMD_SelectMainFolderToggleChanged;
            }
        }

        private ICommand mCMD_GenerateEmptyFolder = null;
        private bool mIsExecutingCMD_GenerateEmptyFolder = false;
        ///<summary>
        /// CMD_GenerateEmptyFolder
        ///</summary>
        public ICommand CMD_GenerateEmptyFolder
        {
            get
            {
                if (mCMD_GenerateEmptyFolder != null) return mCMD_GenerateEmptyFolder;

                mCMD_GenerateEmptyFolder = new RelayAsyncCommand(async () =>
                {
                    mIsExecutingCMD_GenerateEmptyFolder = true;

                    var dialogResult = await DialogCoordinator.ShowMessageAsync(this,
                        Properties.Resources.DIALOG_CMD_GenerateEmptyFolder_BEFORE_Title,
                        Properties.Resources.DIALOG_CMD_GenerateEmptyFolder_BEFORE_Message,
                        MessageDialogStyle.AffirmativeAndNegative,
                        new MetroDialogSettings()
                        {
                            AffirmativeButtonText = Properties.Resources.Button_Yes,
                            NegativeButtonText = Properties.Resources.Button_No
                        });

                    if (dialogResult == MessageDialogResult.Canceled || dialogResult == MessageDialogResult.Negative)
                    {
                        mIsExecutingCMD_GenerateEmptyFolder = false;
                        return;
                    }

                    var cancelToken = new CancellationToken();

                    var dialog = await DialogCoordinator.ShowProgressAsync(this,
                        Properties.Resources.DIALOG_CMD_GenerateEmptyFolder_PROGRESS_Title,
                        Properties.Resources.DIALOG_CMD_GenerateEmptyFolder_PROGRESS_Message_Before,
                        true,
                        new MetroDialogSettings()
                        {
                            CancellationToken = cancelToken,
                            DialogResultOnCancel = MessageDialogResult.Canceled
                        });

                    List<Exception> errorList = new List<Exception>();

                    try
                    {
                        using (var con = new GoogleConnector())
                        {
                            var manager = new GDriveManager(con);
                            var folders = Tree.FindAllFoldersSelectedForUpdate();
                            for (int i = 0; i < folders.Count; i++)
                            {
                                if (cancelToken.IsCancellationRequested || dialog.IsCanceled) break;

                                var folder = folders[i];

                                double progress = i / Convert.ToDouble(folders.Count);
                                dialog.SetProgress(progress);
                                dialog.SetMessage(string.Format(Properties.Resources.DIALOG_CMD_GenerateEmptyFolder_PROGRESS_Message,
                                    i,
                                    folders.Count,
                                    folder.Name));

                                try
                                {
                                    manager.CreateFolder((folder as GFolder), NewFolderName);
                                }
                                catch (Exception ex)
                                {
                                    errorList.Add(ex);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        await DialogCoordinator.ShowMessageAsync(this,
                            Properties.Resources.Dialog_ExceptionOccurs_Title,
                            ex.Message + Environment.NewLine + Environment.NewLine + ex.ToString() + Environment.NewLine + ex.StackTrace,
                            MessageDialogStyle.Affirmative);
                    }
                    finally
                    {
                        if (errorList?.Count > 0)
                            await DialogCoordinator.ShowMessageAsync(this,
                                Properties.Resources.Dialog_ExceptionOccurs_Title,
                                string.Join(Environment.NewLine, errorList.Select(a => a.Message)),
                                MessageDialogStyle.Affirmative);

                        await dialog.CloseAsync();
                        mIsExecutingCMD_GenerateEmptyFolder = false;
                    }
                },
                () =>
                {
                    return !mIsExecutingCMD_GenerateEmptyFolder
                        && (MainSelectedItem?.IsGFolder ?? false)
                        && Tree.FindAllFoldersSelectedForUpdate().Any()
                        && !string.IsNullOrEmpty(NewFolderName);
                }, null);

                return this.mCMD_GenerateEmptyFolder;
            }
        }

        private ICommand mCMD_CheckAllTopLevelFolders = null;
        ///<summary>
        /// CMD_CheckAllTopLevelFolders
        ///</summary>
        public ICommand CMD_CheckAllTopLevelFolders
        {
            get
            {
                if (mCMD_CheckAllTopLevelFolders != null) return mCMD_CheckAllTopLevelFolders;

                mCMD_CheckAllTopLevelFolders = new RelayCommand<bool>((checkIt) =>
                {
                    Tree.UpdateAllFilesAndFoldersInFolder((MainSelectedItem as GFolder), a => a.IsSelectedForUpdate = checkIt, (f) => f.IsGFolder && (f.GParent?.Equals(MainSelectedItem) ?? false));
                },
                (checkIt) => MainSelectedItem?.IsGFolder ?? false);

                return this.mCMD_CheckAllTopLevelFolders;
            }
        }

        private ICommand mCMD_CloseWindow = null;
        ///<summary>
        /// CMD_CloseWindow
        ///</summary>
        public ICommand CMD_CloseWindow
        {
            get
            {
                if (mCMD_CloseWindow != null) return mCMD_CloseWindow;

                mCMD_CloseWindow = new RelayCommand(() =>
                {
                    System.Environment.Exit(0);
                });

                return this.mCMD_CloseWindow;
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

        private void SaveConfigFile()
        {
            Properties.Settings.Default.Save();
        }
        #endregion

        #region [Implementation of INotifyPropertyChanged]
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string aPropertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(aPropertyName));
        #endregion
    }
}
