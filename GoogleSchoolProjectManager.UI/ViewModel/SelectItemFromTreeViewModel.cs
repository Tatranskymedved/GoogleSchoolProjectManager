using GoogleSchoolProjectManager.Lib.Google.Drive;
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
	public interface ISelectItemFromTreeViewModel : IViewModel
	{
		bool? DialogResult { get; }
	}

	public class SelectItemFromTreeViewModel : ISelectItemFromTreeViewModel
	{
		private bool? mDialogResult = null;
		///<summary>
		/// DialogResult
		///</summary>
		public bool? DialogResult
		{
			get { return this.mDialogResult; }
			set
			{
				if (value == this.mDialogResult) return;

				this.mDialogResult = value;
				OnPropertyChanged(nameof(DialogResult));
			}
		}

		private GTree mTree;
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

		private GFile mSelectedItem = null;
		///<summary>
		/// SelectedItem
		///</summary>
		public GFile SelectedItem
		{
			get { return this.mSelectedItem; }
			set
			{
				if (value == this.mSelectedItem) return;

				this.mSelectedItem = value;
				OnPropertyChanged(nameof(SelectedItem));
			}
		}

		public SelectItemFromTreeViewModel(GTree Tree)
        {
			this.Tree = Tree;
		}

		private ICommand mCMD_CancelSelectedItem = null;
		///<summary>
		/// CMD_CancelSelectedItem
		///</summary>
		public ICommand CMD_CancelSelectedItem
		{
			get
			{
				if (mCMD_CancelSelectedItem != null) return mCMD_CancelSelectedItem;

				mCMD_CancelSelectedItem = new RelayCommand<Window>((window) =>
				{
					this.DialogResult = false;
					window?.Close();
				});

				return this.mCMD_CancelSelectedItem;
			}
		}

		private ICommand mCMD_ConfirmSelectedItem = null;
		///<summary>
		/// CMD_ConfirmSelectedItem
		///</summary>
		public ICommand CMD_ConfirmSelectedItem
		{
			get
			{
				if (mCMD_ConfirmSelectedItem != null) return mCMD_ConfirmSelectedItem;

				mCMD_ConfirmSelectedItem = new RelayCommand<Window>((window) =>
				{
					this.DialogResult = true;
					window?.Close();
				});

				return this.mCMD_ConfirmSelectedItem;
			}
		}

		#region [Implementation of INotifyPropertyChanged]
		public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string aPropertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(aPropertyName));
        #endregion
    }
}
