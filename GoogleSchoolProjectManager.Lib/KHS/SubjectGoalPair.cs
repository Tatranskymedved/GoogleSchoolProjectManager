using System.ComponentModel;

namespace GoogleSchoolProjectManager.Lib.KHS
{
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
