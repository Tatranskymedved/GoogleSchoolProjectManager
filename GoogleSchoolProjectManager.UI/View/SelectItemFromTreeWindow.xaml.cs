using GoogleSchoolProjectManager.UI.ViewModel;
using MahApps.Metro.Controls;

namespace GoogleSchoolProjectManager.UI.View
{
    /// <summary>
    /// Interaction logic for SelectItemFromTreeWindow.xaml
    /// </summary>
    public partial class SelectItemFromTreeWindow : MetroWindow
    {
        public SelectItemFromTreeWindow(ISelectItemFromTreeViewModel viewModel)
        {
            this.DataContext = viewModel;

            InitializeComponent();
        }

        private void selectItemFromTreeWindowInstance_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var dc = (this.DataContext as ISelectItemFromTreeViewModel);
            this.DialogResult = dc.DialogResult;
        }
    }
}
