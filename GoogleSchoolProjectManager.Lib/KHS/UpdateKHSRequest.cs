using GoogleSchoolProjectManager.Lib.Google.Drive;
using GoogleSchoolProjectManager.Lib.GoogleAPI.Sheets;
using GoogleSchoolProjectManager.Lib.GoogleAPI.Sheets.POCOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace GoogleSchoolProjectManager.Lib.KHS
{
    public interface IUpdateRequest
    {
        ObservableCollection<SubjectGoalPair> SubjectGoalList { get; }
        List<GFile> Files { get; }
        string SheetName { get; }
        string GetFormattedDateRange();
        GRowList GetGRowList();
    }

    public class UpdateKHSRequest : INotifyPropertyChanged, IUpdateRequest
    {
        private List<GFile> mFiles = new List<GFile>();
        ///<summary>
        /// Files - List of update files
        ///</summary>
        public List<GFile> Files
        {
            get { return this.mFiles; }
            set
            {
                if (value == this.mFiles) return;

                this.mFiles = value;
                OnPropertyChanged(nameof(Files));
            }
        }

        private DateTime? mDateFrom;
        ///<summary>
        /// DatePicker_KHSUpdate_DateFrom
        ///</summary>
        public DateTime? DateFrom
        {
            get { return this.mDateFrom; }
            set
            {
                if (value == this.mDateFrom) return;

                this.mDateFrom = value;
                OnPropertyChanged(nameof(DateFrom));
            }
        }

        private DateTime? mDateTo;
        ///<summary>
        /// DatePicker_KHSUpdate_DateTo
        ///</summary>
        public DateTime? DateTo
        {
            get { return this.mDateTo; }
            set
            {
                if (value == this.mDateTo) return;

                this.mDateTo = value;
                OnPropertyChanged(nameof(DateTo));
            }
        }

        public string GetFormattedDateRange() => DateRange.Generate(DateFrom.Value, DateTo.Value);

        private string mSheetName = "CÍLE";
        ///<summary>
        /// SheetName
        ///</summary>
        public string SheetName
        {
            get { return this.mSheetName; }
            set
            {
                if (value == this.mSheetName) return;

                this.mSheetName = value;
                OnPropertyChanged(nameof(SheetName));
            }
        }

        public UpdateKHSRequest()
        {
            this.mDateFrom = DateRange.GetNextWeekday(DateTime.Now, DayOfWeek.Monday);
            this.mDateTo = DateRange.GetNextWeekday(mDateFrom.Value, DayOfWeek.Friday);
        }

        public static string A1_WeekColumn { get; set; } = "B4:B";
        public static string A1_SubjectColumn { get; set; } = "C4:C";
        public static string A1_GoalColumn { get; set; } = "D4:D";
        public static string A1_WeekSubjectGoalColumns { get; set; } = "B4:D";
        public static string A1_GetRange_WeekSubjectGoalColumns(int startRow, int endRow) => string.Format("B{0}:D{1}", startRow, endRow);
        public static string A1_GetListRange(string list, string range) => string.Format("{0}!{1}", list, range);
        public static string A1_GetListRange(string list, string rangeStart, string rangeEnd) => A1_GetListRange(list, string.Format("{0}:{1}", rangeStart, rangeEnd));


        private SubjectGoalPair mSelectedSubjectGoal = null;
        ///<summary>
        /// SelectedSubjectGoal
        ///</summary>
        public SubjectGoalPair SelectedSubjectGoal
        {
            get { return this.mSelectedSubjectGoal; }
            set
            {
                if (value == this.mSelectedSubjectGoal) return;

                this.mSelectedSubjectGoal = value;
                OnPropertyChanged(nameof(SelectedSubjectGoal));
            }
        }

        private ObservableCollection<SubjectGoalPair> mSubjectGoalList = new ObservableCollection<SubjectGoalPair>();
        ///<summary>
        /// SubjectGoalList
        ///</summary>
        public ObservableCollection<SubjectGoalPair> SubjectGoalList
        {
            get { return this.mSubjectGoalList; }
            set
            {
                if (value == this.mSubjectGoalList) return;

                this.mSubjectGoalList = value;
                OnPropertyChanged(nameof(SubjectGoalList));
            }
        }

        #region [Implementation of INotifyPropertyChanged]
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string aPropertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(aPropertyName));
        #endregion

        public GRowList GetGRowList()
        {
            var result = new GRowList();

            var weekRange = GetFormattedDateRange();

            result.AddRange(SubjectGoalList.Select((b, c) =>
                new GRow()
                {
                    new GCell().AddValue(c == 0 ? weekRange : ""),
                    new GCell().AddValue(b.Subject).AddFormat(GCellFormat.TextFormatBold, true),
                    new GCell().AddValue(b.Goal)
                }));

            return result;
        }
    }
}
