using GoogleSchoolProjectManager.Lib.Google;
using GoogleSchoolProjectManager.Lib.Google.Drive;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
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
		
		private string mGFileTemplateSource = null;
		///<summary>
		/// GFolderTemplateSource
		///</summary>
		public string GFileTemplateSource
		{
			get { return this.mGFileTemplateSource; }
			set
			{
				if (value == this.mGFileTemplateSource) return;

				this.mGFileTemplateSource = value;
				OnPropertyChanged(nameof(GFileTemplateSource));
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

		#endregion

		public MainViewModel (IDialogCoordinator dialogCoordinator)
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

				},
				() =>
				{
					return true;
				});

				return this.mCMD_SelectFileTemplateSource;
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

				mCMD_GenerateFilesFromTemplate = new RelayAsyncCommand(() =>
				{
					mIsExecutingCMD_GenerateFilesFromTemplate = true;
					try
					{

					}
					finally
					{
						mIsExecutingCMD_GenerateFilesFromTemplate = false;
					}
				},
				() =>
				{
					return !mIsExecutingCMD_GenerateFilesFromTemplate;
				}, null);

				return this.mCMD_GenerateFilesFromTemplate;
			}
		}

		private ICommand mCMD_UpdateSelectedKHSes = null;
		private bool mIsExecutingCMD_UpdateSelectedKHSes = false;
		///<summary>
		/// CMD_UpdateSelectedKHSes
		///</summary>
		public ICommand CMD_UpdateSelectedKHSes
		{
			get
			{
				if (mCMD_UpdateSelectedKHSes != null) return mCMD_UpdateSelectedKHSes;

				mCMD_UpdateSelectedKHSes = new RelayAsyncCommand(() =>
				{
					mIsExecutingCMD_UpdateSelectedKHSes = true;
					try
					{

					}
					finally
					{
						mIsExecutingCMD_UpdateSelectedKHSes = false;
					}
				},
				() =>
				{
					return !mIsExecutingCMD_UpdateSelectedKHSes;
				}, null);

				return this.mCMD_UpdateSelectedKHSes;
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

		#endregion

		#region [Implementation of INotifyPropertyChanged]
		public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string aPropertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(aPropertyName));
        #endregion
    }
}
