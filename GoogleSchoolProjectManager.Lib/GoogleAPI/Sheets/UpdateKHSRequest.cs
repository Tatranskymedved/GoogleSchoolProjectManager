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

        public string WeekColumn { get; set; } = "B4:B";
        public string SubjectColumn { get; set; } = "C4:C";
        public string GoalColumn { get; set; } = "D4:D";
        public string WeekSubjectGoalColumns { get; set; } = "B4:D";


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

        private ObservableCollection<SubjectGoalPair> mSubjectGoalList = new ObservableCollection<SubjectGoalPair>(
        //    new List<SubjectGoalPair>(){
        //    new SubjectGoalPair("Subject", "Goal"),
        //    new SubjectGoalPair("Subject", "Goal"),
        //    new SubjectGoalPair("Subject", "Goal"),
        //}
            );
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
