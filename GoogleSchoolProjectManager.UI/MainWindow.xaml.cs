﻿using GoogleSchoolProjectManager.UI.ViewModel;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GoogleSchoolProjectManager.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow(IViewModel viewModel)
        {
            DataContext = viewModel;

            var language = Properties.Settings.Default.Localization;
            var cultureInfo = new System.Globalization.CultureInfo(language);
            System.Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo;

            InitializeComponent();
        }

        private void DatePicker_Loaded_ToChangeWaterMark(object sender, RoutedEventArgs e)
        {
            var datePicker = sender as DatePicker;
            if (datePicker == null) return;

            var toolTipValue = datePicker?.ToolTip?.ToString();
            if (string.IsNullOrEmpty(toolTipValue)) return;

            var datePickerTextBox = FindVisualChild<System.Windows.Controls.Primitives.DatePickerTextBox>(datePicker);
            if (datePickerTextBox != null)
            {
                ContentControl watermark = datePickerTextBox.Template.FindName("PART_Watermark", datePickerTextBox) as ContentControl;
                if (watermark != null)
                {
                    watermark.Content = toolTipValue;
                }
            }
        }

        private T FindVisualChild<T>(DependencyObject depencencyObject) where T : DependencyObject
        {
            if (depencencyObject != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depencencyObject); ++i)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depencencyObject, i);
                    T result = (child as T) ?? FindVisualChild<T>(child);
                    if (result != null)
                        return result;
                }
            }

            return null;
        }

        private void Btn_ExpandAll_Click(object sender, RoutedEventArgs e)
        {
            treeViewSetIsExpended(true);
        }
        private void Btn_ShrinkAll_Click(object sender, RoutedEventArgs e)
        {
            treeViewSetIsExpended(false);
        }

        private void SetIsExpandedForAllItems(ItemsControl items, bool isExpanded)
        {
            foreach (object obj in items?.Items)
            {
                var childControl = items?.ItemContainerGenerator?.ContainerFromItem(obj) as ItemsControl;
                if (childControl != null)
                {
                    SetIsExpandedForAllItems(childControl, isExpanded);
                }
                var item = childControl as TreeViewItem;
                if (item != null)
                    item.IsExpanded = true;
            }
        }

        private void treeViewSetIsExpended(bool isExpanded)
        {
            foreach (object item in mainTreeView?.Items)
            {
                var treeItem = this.mainTreeView?.ItemContainerGenerator?.ContainerFromItem(item) as TreeViewItem;
                if (treeItem != null)
                {
                    SetIsExpandedForAllItems(treeItem, isExpanded);
                    treeItem.IsExpanded = isExpanded;
                }
            }
        }
    }
}
