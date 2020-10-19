using GoogleSchoolProjectManager.Lib.Google.Drive;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace GoogleSchoolProjectManager.Lib.GoogleAPI.Sheets
{
    public class UpdateKHSRequest : INotifyPropertyChanged
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

        private DateTime? mDateFrom = DateTime.Now;
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

        private DateTime? mDateTo = DateTime.Now.AddDays(3);
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
    }

    public class SubjectGoalPair : INotifyPropertyChanged
    {
        private string mSubject = null;
        ///<summary>
        /// Subject
        ///</summary>
        public string Subject
        {
            get { return this.mSubject; }
            set
            {
                if (value == this.mSubject) return;

                this.mSubject = value;
                OnPropertyChanged(nameof(Subject));
            }
        }

        private string mGoal = null;
        ///<summary>
        /// Goal
        ///</summary>
        public string Goal
        {
            get { return this.mGoal; }
            set
            {
                if (value == this.mGoal) return;

                this.mGoal = value;
                OnPropertyChanged(nameof(Goal));
            }
        }

        public SubjectGoalPair() { }
        public SubjectGoalPair(string Subject, string Goal)
        {
            this.Subject = Subject;
            this.Goal = Goal;
        }

        #region [Implementation of INotifyPropertyChanged]
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string aPropertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(aPropertyName));
        #endregion
    }
}
