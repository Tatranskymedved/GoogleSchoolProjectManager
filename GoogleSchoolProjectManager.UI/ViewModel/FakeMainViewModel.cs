using GoogleSchoolProjectManager.Lib.Google;
using GoogleSchoolProjectManager.Lib.Google.Drive;
using GoogleSchoolProjectManager.Lib.GoogleAPI.Test;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GoogleSchoolProjectManager.UI.ViewModel
{
    public class FakeMainViewModel : MainViewModel
    {
		public FakeMainViewModel (IDialogCoordinator dialogCoordinator)
            : base(dialogCoordinator)
        {
            this.Tree = new FakeGTree();
        }
    }
}
